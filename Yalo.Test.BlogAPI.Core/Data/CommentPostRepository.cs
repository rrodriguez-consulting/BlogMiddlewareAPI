using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Yalo.Test.BlogAPI.Core.Data.Entity;

namespace Yalo.Test.BlogAPI.Core.Data
{
    public interface ICommentPostRepository
    {
        Task<List<CommentPost>> Get(int postId);
    }

    public class CommentPostRepository : ICommentPostRepository
    {
        private readonly string _baseUrl;
        public CommentPostRepository(string baseUrl)
        {
            _baseUrl = baseUrl;
        }
        public async Task<List<CommentPost>> Get(int postId)
        {
            if (postId < 1)
            {
                throw new ApplicationException("Post not found");
            }
            HttpClient client = new HttpClient();
        
            var request = await client.GetAsync(string.Format(_baseUrl, postId));
            if (request.IsSuccessStatusCode)
            {
                var jsonResponse  =  await request.Content.ReadAsStringAsync();
           
                return JsonConvert.DeserializeObject<List<CommentPost>>(jsonResponse);
            }

            return new List<CommentPost>();
        }
    }
}