using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using SocialHabits.Abstractions;

namespace SocialHabits.Services
{
    public class AzureCloudTable<T> : ICloudTable<T> where T : TableData
    {
       
        private readonly IMobileServiceTable<T> _table;

        public AzureCloudTable(MobileServiceClient client)
        {
      
            _table = client.GetTable<T>();
        }

        #region ICloudTable implementation
        public async Task<T> CreateItemAsync(T item)
        {
            await _table.InsertAsync(item);
            return item;
        }

        public async Task DeleteItemAsync(T item)
        {
            await _table.DeleteAsync(item);
        }

        public async Task<ICollection<T>> ReadAllItemsAsync()
        {
            var t = await _table.ToListAsync();
            Debug.WriteLine("got the data");
            return t;

        }

        public async Task<T> ReadItemAsync(string id)
        {
            return await _table.LookupAsync(id);
        }

        public async Task<T> UpdateItemAsync(T item)
        {
            await _table.UpdateAsync(item);
            return item;
        }
        #endregion
    }
}
