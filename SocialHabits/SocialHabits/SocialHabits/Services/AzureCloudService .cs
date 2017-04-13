using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using SocialHabits.Abstractions;

using Xamarin.Forms;

namespace SocialHabits.Services
{
    
    public class AzureCloudService : ICloudService
    {
        readonly MobileServiceClient _client;
        List<AppServiceIdentity> identities = null;

        public AzureCloudService()
        {
            _client = new MobileServiceClient("https://e22be2e6-794c-socialhabitsbackend.azurewebsites.net");
        }

        public ICloudTable<T> GetTable<T>() where T : TableData 
        {
            return new AzureCloudTable<T>(_client);
        }

        public async Task<MobileServiceUser> LoginAsync()
        {
            var loginProvider = DependencyService.Get<ILoginProvider>();

            _client.CurrentUser = loginProvider.RetrieveTokenFromSecureStore();
          

            if (_client.CurrentUser != null && !IsTokenExpired(_client.CurrentUser.MobileServiceAuthenticationToken))
            {
                // User has previously been authenticated, no refresh is required
                return  _client.CurrentUser;
            }

            // We need to ask for credentials
            await loginProvider.LoginAsync(_client);
            if (_client.CurrentUser != null)
            {
                // We were able to successfully log in
                loginProvider.StoreTokenInSecureStore(_client.CurrentUser);
            }
            return _client.CurrentUser;
        }

        

        public async Task<AppServiceIdentity> GetIdentityAsync()
        {
            if (_client.CurrentUser?.MobileServiceAuthenticationToken == null)
            {
                throw new InvalidOperationException("Not Authenticated");
            }

            identities = identities ?? await _client.InvokeApiAsync<List<AppServiceIdentity>>("/.auth/me");
    
            return identities.Count > 0 ? identities[0] : null;
        }

        public async Task LogoutAsync()
        {
            if (_client.CurrentUser?.MobileServiceAuthenticationToken == null)
                return;

            

            // Invalidate the token on the mobile backend
            var authUri = new Uri($"{_client.MobileAppUri}/.auth/logout");
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("X-ZUMO-AUTH", _client.CurrentUser.MobileServiceAuthenticationToken);
                await httpClient.GetAsync(authUri);
            }

            // Remove the token from the cache
            DependencyService.Get<ILoginProvider>().RemoveTokenFromSecureStore();

            // Remove the token from the MobileServiceClient
            await _client.LogoutAsync();
        }





        public bool IsTokenExpired(string token)
        {

            var jwt = token.Split('.')[1];

            jwt = jwt.Replace('-', '+').Replace('_', '/');
            switch (jwt.Length % 4)
            {
                case 0:
                    break;
                case 2:
                    jwt += "==";
                    break;
                case 3:
                    jwt += "=";
                    break;
                default:
                    throw new ArgumentException("The token is not a valid Base64 string.");
            }

            var bytes = Convert.FromBase64String(jwt);
            string jsonString = Encoding.UTF8.GetString(bytes, 0, bytes.Length);


            JObject jsonObj = JObject.Parse(jsonString);
            var exp = Convert.ToDouble(jsonObj["exp"].ToString());

           
            DateTime minTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var expire = minTime.AddSeconds(exp);
            return (expire < DateTime.UtcNow);
        }
    }
}