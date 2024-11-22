using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Aafeben.Models
{
    public class LoginDto
    {
        public required string Password {get; set;}
        public required string Username {get; set;}
    }
}