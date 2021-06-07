using Microsoft.AspNetCore.Mvc;

namespace SanityDotNet.Site.Features.BlogPost
{
    public class BlogPostController : Controller
    {
        private readonly BlogPostService _blogPostService;

        public BlogPostController(BlogPostService blogPostService)
        {
            _blogPostService = blogPostService;
        }

        [HttpGet("/")]
        public IActionResult Index()
        {
            return View(_blogPostService.GetBlogPosts());
        }
    }
}