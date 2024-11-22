using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Aafeben.Models
{
    public class ProjectModel
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public string? FeaturedImageUrl {get; set;}
        public required string Language {get; set;}
        public required string Zone {get; set;}
        public required string Duration { get; set; }

        [DataType(DataType.Date)] 
        public DateTime StartDate {get; set;}

        [DataType(DataType.Date)] 
        public DateTime EndDate {get; set;}
    }
}