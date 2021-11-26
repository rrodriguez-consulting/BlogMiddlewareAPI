using Newtonsoft.Json;

namespace Yalo.Test.BlogAPI.Core.Data.Entity
{
    public class UserPost
    {
        [JsonProperty("id")]
        public int UserId { get; set; }
        public string Name { get; set; }
    }
}