using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Yalo.Test.BlogAPI.Core;

namespace BlogAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }
        [HttpGet]
        //api/blogs?page=1&pageSize=10
        //if the user omit the page and page size it is using the default values 1 and 10
        public async Task<IActionResult> Get(int page = 1, int pageSize = 10)
        {
            try
            {
                //Calling Blog Service to get the blogs 
                var result = await _blogService.Get(page, pageSize);
                return Ok(result);
            }
            catch (ApplicationException e)
            {
                //handle the page size or page are invalid, we can use PaginationException to catch it
                return NotFound(e.Message);
            }
            catch (IndexOutOfRangeException e)
            {
                //handle the page size is invalid regarding number of records returned by the API data sources also could be PaginationException
                return NotFound(e.Message);
            }
            catch (Exception)
            {
                //handle any exception in the process like  and return http 500
                return StatusCode(500);
            }
           
                
            
        }
    }
}