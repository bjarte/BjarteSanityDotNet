using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sanity.Linq;
using SanityDotNet.Site.Features.BlogPost.DataModels;

namespace SanityDotNet.Site.Features.BlogPost
{
    public class BlogPostRepository
    {
        private readonly SanityDataContext _sanity;

        public BlogPostRepository()
        {
            var options = new SanityOptions
            {
                ProjectId = "lm55gcuw",
                Dataset = "production",
                Token = "sk1Yk4Hye2rswzEJBy1Wzz6VxLqSCpQRGdTrVTfvsfZbxOEjsGs46lJD1Vtk5fshAbPhMppaxxR1JlOZ9IJVLR7bkj1jSb9kDbRh91Awg7sAtGr6u4I7vqeKWboLgeo44s7DO9a0mUirMspYbfIkodGA9TmEgXwcf0cF0QtcGdjphpaysjJf",
                UseCdn = false
            };

            _sanity = new SanityDataContext(options);
        }

        public async Task<List<Post>> GetBlogPosts()
        {
            var result = _sanity.DocumentSet<Post>();

            return await result
                .OrderByDescending(_ => _.PublishedAt)
                .ToListAsync();
        }
    }
}
