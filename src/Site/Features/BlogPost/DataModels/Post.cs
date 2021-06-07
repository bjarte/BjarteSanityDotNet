using System;
using Sanity.Linq.CommonTypes;

namespace SanityDotNet.Site.Features.BlogPost.DataModels
{
    public class Post : SanityDocument
    {
        public string Title { get; set; }

        public DateTimeOffset? PublishedAt { get; set; }
    }
}