using System.Collections.Generic;
using Newtonsoft.Json;

namespace Yalo.Test.BlogAPI.Core
{
    public class BlogEntry
    {
        public BlogEntry()
        {
            Comments = new List<Comment>();
        }
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        [JsonProperty("user")]
        public User Author { get; set; }
        public List<Comment> Comments { get; set; }

    }

    public class User
    {
        public string Name { get; set; }
        public int UserId { get; set; }
    }

    public class Comment
    {
        public string Title { get; set; }
        public string Body { get; set; }
    }
}