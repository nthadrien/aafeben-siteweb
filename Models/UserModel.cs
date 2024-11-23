using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Aafeben.Models
{
    public class UserModel
    {
        public int Id {get; set;}
        public required string Name {get; set;}
        public required string EnRole {get; set;}
        public required string FrRole {get; set;}
        public string? Image { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }
    }
}