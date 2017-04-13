using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace SocialHabits.Abstractions
{
    public interface ILoginProvider
    {
       
        void RemoveTokenFromSecureStore();
        void StoreTokenInSecureStore(MobileServiceUser user);
        Task<MobileServiceUser> LoginAsync(MobileServiceClient client);
        MobileServiceUser RetrieveTokenFromSecureStore();
    }
}