using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Aafeben.Models
{
    public class OpportunityModel
    {
        public int Id { get; set;}
        public required string Job  { get; set; }
        public required string JobDescription  { get; set; }
        public required string Language  { get; set; }
        public required string JobRequirements  { get; set; }
        public string? Doc { get; set; }
        [DataType(DataType.Date)]
        public required DateTime PublishedDate  { get; set; }
    }
}