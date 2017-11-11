using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Apportiswebscrapper.Models
{
    public class Essentials
    {
        public string definition { get; set; }
        public string causes { get; set; }
        public string risk { get; set; }
        public string symptoms { get; set; }
        public string treatment { get; set; }

        public string prevention { get; set; }
        public string Searchkeyword { get; set; }

        [Key]
        public int Articleno { get; set; }

    }
}