using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aafeben.Models
{
    public class PartnerModelDto
    {
        public int Id { get; set; }
        public required string UrlLink { get; set; }
        public required string Name { get; set; }
        public required string? Image { get; set; }
    }
}