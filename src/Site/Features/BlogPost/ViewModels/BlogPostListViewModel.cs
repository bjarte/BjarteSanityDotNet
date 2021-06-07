using System.Collections.Generic;

namespace SanityDotNet.Site.Features.BlogPost.ViewModels
{
    public class BlogPostListViewModel
    {
        public string Title { get; set; }

        public IEnumerable<BlogPostViewModel> BlogPosts { get; set; }
    }
}