using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Aafeben.Models
{
    public class PublicationModel
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        [DataType(DataType.Date)]
        public required DateTime PublishedDate { get; set; }
        public required string Language {get; set;}
    }
}