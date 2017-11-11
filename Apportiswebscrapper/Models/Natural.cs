using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Apportiswebscrapper.Models
{
    public class Natural
    {
        public string ep_documentBody { get; set; }
       
        [Key]
        public int Articleno { get; set; }
        public string searchkey { get; set; }
    }
}