using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Apportiswebscrapper.Models
{
    public class Drugcs
    {
        public string drgAboutYourTreatment { get; set; }
        public string drgAdministeringYourMed { get; set; }
        public string drgStorage { get; set; }
        public string Whatllow { get; set; }
       
        [Key]
        public int Articleno { get; set; }
        public string searchkey { get; set; }
    }
}