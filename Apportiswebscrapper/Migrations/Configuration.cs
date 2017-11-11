namespace Apportiswebscrapper.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Apportiswebscrapper.Models.ApportiswebscrapperContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Apportiswebscrapper.Models.ApportiswebscrapperContext context)
        {
            context.Essentials.AddOrUpdate(x => x.Searchkeyword,
            new Models.Essentials());

            context.Conditions.AddOrUpdate(x => x.searchkey,
            new Models.Conditions());

            context.Procedures.AddOrUpdate(x => x.searchkey,
            new Models.Procedures());

            context.Drugcs.AddOrUpdate(x => x.searchkey,
            new Models.Drugcs());


            context.Healthnews.AddOrUpdate(x => x.searchkey,
            new Models.Healthnews());

            context.Naturals.AddOrUpdate(x => x.searchkey,
            new Models.Natural());

            context.Wellnesses.AddOrUpdate(x => x.searchkey,
            new Models.Wellness());
        }
    }
}
