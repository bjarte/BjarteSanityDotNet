using System;

namespace SanityDotNet.Site.Features.BlogPost.ViewModels
{
    public class BlogPostViewModel
    {
        public string Title { get; set; }

        public DateTimeOffset? PublishedAt { get; set; }
    }
}