using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Aafeben.Models
{
    public class MediaModel
    {
        public int Id { get; set; }
        public required string EnCaption { get; set; }
        public required string FrCaption { get; set; }
        public required string Type { get; set; }
        public required string URI { get; set; }
        [DataType(DataType.Date)]
        public required DateTime PublishedDate { get; set;}
        
    }
}