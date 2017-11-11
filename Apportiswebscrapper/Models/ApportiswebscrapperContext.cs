using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Apportiswebscrapper.Models
{
    public class ApportiswebscrapperContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public ApportiswebscrapperContext() : base("name=ApportiswebscrapperContext")
        {
        }

        public System.Data.Entity.DbSet<Apportiswebscrapper.Models.Essentials> Essentials { get; set; }

        public System.Data.Entity.DbSet<Apportiswebscrapper.Models.Drugcs> Drugcs { get; set; }

        public System.Data.Entity.DbSet<Apportiswebscrapper.Models.Conditions> Conditions { get; set; }

        public System.Data.Entity.DbSet<Apportiswebscrapper.Models.Healthnews> Healthnews { get; set; }

        public System.Data.Entity.DbSet<Apportiswebscrapper.Models.Natural> Naturals { get; set; }

        public System.Data.Entity.DbSet<Apportiswebscrapper.Models.Procedures> Procedures { get; set; }

        public System.Data.Entity.DbSet<Apportiswebscrapper.Models.Wellness> Wellnesses { get; set; }
    }
}
