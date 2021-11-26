using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Yalo.Test.BlogAPI.Core.Data.Entity;

namespace Yalo.Test.BlogAPI.Core.Data
{
    public interface IUserRepository
    {
        Task<List<UserPost>> Get();
    }

    public class UserRepository : IUserRepository
    {
        private readonly string _baseUrl;
        public UserRepository(string baseUrl)
        {
            _baseUrl = baseUrl;
        }
        public async Task<List<UserPost>> Get()
        {
            HttpClient client = new HttpClient();
        
            var request = await client.GetAsync(_baseUrl);
            if (request.IsSuccessStatusCode)
            {
                var jsonResponse  =  await request.Content.ReadAsStringAsync();
           
                return JsonConvert.DeserializeObject<List<UserPost>>(jsonResponse);
            }

            return new List<UserPost>();
        }
    }
}