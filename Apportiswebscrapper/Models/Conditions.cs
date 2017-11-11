using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Apportiswebscrapper.Models
{
    public class Conditions
    {
        public string CIDintroduction {get; set;}
        public string CIDrisk   {  get; set; }
        public string CIDsymptoms { get; set; }
        public string CIDtreatment { get; set; }
        public string CIDscreening { get; set; }
        public string CIDriskreduction { get; set; }
        public string CIDtalking { get; set; }
        [Key]
        public int Articleno { get; set; }
        public string searchkey { get; set; }
    }
    
}