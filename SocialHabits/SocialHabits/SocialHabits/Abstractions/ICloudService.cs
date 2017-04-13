using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using SocialHabits.Services;


namespace SocialHabits.Abstractions
{
    public interface ICloudService
    {
        ICloudTable<T> GetTable<T>() where T : TableData;
        Task<MobileServiceUser> LoginAsync();
        Task<AppServiceIdentity> GetIdentityAsync();
    }

}