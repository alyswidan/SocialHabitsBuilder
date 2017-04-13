using System;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Microsoft.WindowsAzure.MobileServices;
using SocialHabitsBuilder.Abstractions;
using SocialHabitsBuilder.Droid.Services;
using Xamarin.Auth;


[assembly: Xamarin.Forms.Dependency(typeof(DroidLoginProvider))]

namespace SocialHabitsBuilder.Droid.Services
{
    public class DroidLoginProvider : ILoginProvider
    {
        public Context RootView { get; private set; }

        public AccountStore AccountStore { get; private set; }

        private const string Provider = "facebook";

        public void Init(Context context)
        {
            RootView = context;
            AccountStore = AccountStore.Create(context);
        }


        public MobileServiceUser RetrieveTokenFromSecureStore()
        {
            var accounts = AccountStore.FindAccountsForService("tasklist");
            if (accounts != null)
            {
                foreach (var acct in accounts)
                {
                    string token;

                    if (acct.Properties.TryGetValue("token", out token))
                    {
                        return new MobileServiceUser(acct.Username)
                        {
                            MobileServiceAuthenticationToken = token
                        };
                    }
                }
            }
            return null;
        }

        public void StoreTokenInSecureStore(MobileServiceUser user)
        {
            var account = new Account(user.UserId);
            account.Properties.Add("token", user.MobileServiceAuthenticationToken);
            AccountStore.Save(account, "tasklist");
        }

        public void RemoveTokenFromSecureStore()
        {
            var accounts = AccountStore.FindAccountsForService("tasklist");
            if (accounts != null)
            {
                foreach (var acct in accounts)
                {
                    AccountStore.Delete(acct, "tasklist");
                }
            }
        }

        public async Task<MobileServiceUser> LoginAsync(MobileServiceClient client)=> await client.LoginAsync(RootView, Provider);



    }
}