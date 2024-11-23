using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Aafeben.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string? Image { get; set; }
        public required string FrName { get; set; }
        public required string EnName { get; set; }
        public required string Contact { get; set; }
        public required string Address { get; set; }
        public required string EnDescription {get; set;}
        public required string FrDescription {get; set;}
        public required string Qty {get; set;}
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }
    }
}