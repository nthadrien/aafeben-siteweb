using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Aafeben.Models
{
    public class BlogPostModel
    {
        public int Id { get; set; }
        [MaxLength(340)]
        public required string Title { get; set; }
        public required string Content { get; set; }
        public required string ShortDescription { get; set; }
        [DataType(DataType.Date)] 
        public required DateTime PublishedDate { get; set; }
        public string? FeaturedImageUrl {get; set;}
        public required string Language {get; set;}
    }
}