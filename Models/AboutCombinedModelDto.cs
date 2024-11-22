using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aafeben.Models
{
    public class AboutCombinedModelDto
    {
        public required List<UserModel> Users {get; set;}
        public required List<PartnerModel> Partners {get; set;}
    }
}