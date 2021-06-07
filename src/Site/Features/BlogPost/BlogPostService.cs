using System.Linq;
using SanityDotNet.Site.Features.BlogPost.ViewModels;

namespace SanityDotNet.Site.Features.BlogPost
{
    public class BlogPostService
    {
        private readonly BlogPostRepository _blogPostRepository;

        public BlogPostService(BlogPostRepository blogPostRepository)
        {
            _blogPostRepository = blogPostRepository;
        }

        public BlogPostListViewModel GetBlogPosts()
        {
            var blogPosts = _blogPostRepository
                .GetBlogPosts()
                .Result
                .Select(_ => new BlogPostViewModel
                {
                    Title = _.Title,
                    PublishedAt = _.PublishedAt
                });

            return new BlogPostListViewModel
            {
                Title = "Listetittel!",
                BlogPosts = blogPosts
            };
        }

    }
}
