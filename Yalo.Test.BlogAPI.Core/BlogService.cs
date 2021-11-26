using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yalo.Test.BlogAPI.Core.Data;

namespace Yalo.Test.BlogAPI.Core
{
    public interface IBlogService
    {
        Task<List<BlogEntry>> Get(int page, int pageSize);
    }
/// <summary>
/// Blog service has responsibility to get data from the repositories data source and logic for the pagination
/// </summary>
    public class BlogService : IBlogService
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICommentPostRepository _commentPostRepository;

        public BlogService(IPostRepository postRepository, IUserRepository userRepository, ICommentPostRepository commentPostRepository)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
            _commentPostRepository = commentPostRepository;
        }
        /// <summary>
        /// Method to get Blog post 
        /// </summary>
        /// <param name="page">page number</param>
        /// <param name="pageSize">page size</param>
        /// <returns>List of Blog Entry</returns>
        /// <exception cref="ApplicationException">When the page or page size is invalid</exception>
        /// <exception cref="IndexOutOfRangeException">When the page is not valid</exception>
        public async Task<List<BlogEntry>> Get(int page, int pageSize)
        {
            List<BlogEntry> results = new List<BlogEntry>();
            
            if(page < 1)
            {
                throw new ApplicationException("Page number is not valid");
            }

            if (pageSize < 1)
            {
                throw new ApplicationException("The page size is not valid");
            }

            var postEntriesTask = _postRepository.Get(); //Task to get post
            var userEntriesTask = _userRepository.Get(); //Task to get User
            
            await Task.WhenAll(postEntriesTask, userEntriesTask); //execute the task in parallels to get post and users
            
            //it could be a performance issue I am calling the both api to get user and post but maybe I don't need call the user API if the post repository is returning empty list
            
            
            var entries = postEntriesTask.Result;
            var users = userEntriesTask.Result;

            int numberOfPages = entries.Count / pageSize;

            if (page > numberOfPages)
            {
                throw new IndexOutOfRangeException("The page size is not valid");
            }

            var records = entries.Skip((page-1) * pageSize).Take(pageSize);

            foreach (var post in records)
            {
                BlogEntry blogEntry = new BlogEntry();
                blogEntry.Body = post.Body;
                blogEntry.Id = post.Id;
                blogEntry.Title = post.Title;
                blogEntry.UserId = post.UserId;
                var postUser = users.FirstOrDefault(c => c.UserId == post.UserId); // I am using Queryable in memory I have all user in memory
                if (postUser != null)
                {
                    blogEntry.Author = new User() { UserId = postUser.UserId, Name = postUser.Name }; // mapping the DAO to DTO
                }
                results.Add(blogEntry);
            }

            //Using parallels call to get the comments all post also we can setup MaxDegree Parallel for example 10 threads
            //I am using just 3 because the data source service started to be unresponsive 
            Parallel.ForEach(results, new ParallelOptions() {MaxDegreeOfParallelism = 3},  r =>
            {
                var comments =  _commentPostRepository.Get(r.Id).GetAwaiter().GetResult(); // run sync mode because it is parallels
                r.Comments = comments.Select(s => new Comment() { Body = s.Body, Title = s.Name }).ToList();

            });

            return results;


        }
    }
}