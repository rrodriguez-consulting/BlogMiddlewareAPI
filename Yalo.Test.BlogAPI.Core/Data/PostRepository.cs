using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Yalo.Test.BlogAPI.Core.Data.Entity;

namespace Yalo.Test.BlogAPI.Core.Data
{
    public interface IPostRepository
    {
        Task<IList<Post>> Get();
    }

    public class PostRepository : IPostRepository
    {
        private readonly string _baseUrl;
        public PostRepository(string baseUrl)
        {
            _baseUrl = baseUrl;
        }
        public async Task<IList<Post>> Get()
        {
            HttpClient client = new HttpClient();
        
            var request = await client.GetAsync(_baseUrl);
            if (request.IsSuccessStatusCode)
            {
               var jsonResponse  =  await request.Content.ReadAsStringAsync();
           
               return JsonConvert.DeserializeObject<List<Post>>(jsonResponse);
            }

            return new List<Post>();

        }
    }
}