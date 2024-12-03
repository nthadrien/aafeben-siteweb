using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Aafeben.Models
{
    
    public class MessageModel
    {
        public int Id {get; set;}
        public required string Name { get; set;}
        public required string Email { get; set;}
        public string? Subject { get; set;}
        public required string Message { get; set;}
        [DataType(DataType.Date)]
        public required DateTime SendOn { get; set;}
    }

}