using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Apportiswebscrapper.Models
{
    public class Procedures
    {
        public string definition { get; set; }
        public string reasons { get; set; }
        public string risk { get; set; }
        public string expect { get; set; }
        public string call { get; set; }
       
        [Key]
        public int Articleno { get; set; }
        public string searchkey { get; set; }
    }
}