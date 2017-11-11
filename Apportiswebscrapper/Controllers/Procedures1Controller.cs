using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Apportiswebscrapper.Models;
using HtmlAgilityPack;
using System.Web;

namespace Apportiswebscrapper.Controllers
{
    public class Procedures1Controller : ApiController
    {
        private ApportiswebscrapperContext db = new ApportiswebscrapperContext();

        // GET: api/Procedures1
        public IQueryable<Procedures> GetProcedures(string id)
        {
            IQueryable<Procedures> Articles = null;
            try
            {

                Articles = from Procedures in db.Procedures
                           where Procedures.searchkey == id
                           select Procedures;

                if (!Articles.Any())
                {

                    Scrapeit(id, db);
                    return db.Procedures;
                }
                else
                {
                    return Articles;
                }
            }
            catch
            {
                Scrapeit(id, db);
                return db.Procedures;
            }
        }

       

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static void Scrapeit(string id, ApportiswebscrapperContext db)
        {
            try
            {

                var gurl = "http://healthlibrary.epnet.com/authenticate.aspx?token=0EF30C79-8A3A-42F4-BE9F-2A90807D3B47&ReturnUrl=http%3a%2f%2fhealthlibrary.epnet.com%2fGetContent.aspx%3faccount%3dhlhosp";

                BrowserSession b = new BrowserSession();
                b.Get("http://healthlibrary.epnet.com/authenticate.aspx?token=0EF30C79-8A3A-42F4-BE9F-2A90807D3B47&ReturnUrl=http%3a%2f%2fhealthlibrary.epnet.com%2fGetContent.aspx%3faccount%3dhlhosp");
                b.FormElements["sUserName"] = "hlhosp";
                b.FormElements["sPassword"] = "fitness";

                HtmlNode response = b.Post("http://healthlibrary.epnet.com/authenticate.aspx?token=0EF30C79-8A3A-42F4-BE9F-2A90807D3B47&ReturnUrl=http%3a%2f%2fhealthlibrary.epnet.com%2fGetContent.aspx%3faccount%3dhlhosp");

                prceduressearch(b, id);
                response = b.Post("http://healthlibrary.epnet.com/GetContent.aspx");
                ProcedureResults(b, response,id,db);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private static void prceduressearch(BrowserSession newform, string id)
        {
            newform.FormElements["ep_searchOption"] = "PRC";
            newform.FormElements["search1"] = String.Concat("Title=(", id, ") OR ArticleBody=(", id, ")");
            newform.FormElements["ContentURL"] = String.Concat("3 $", id, "| false | PRC");
            newform.FormElements["ep_defaultContentURL"] = "";
        }
        /// <summary>
        /// /
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="resulttable"></param>
        private static void ProcedureResults(BrowserSession b, HtmlNode resulttable,string id,ApportiswebscrapperContext db)
        {
            // throw new NotImplementedException();
            resulttable = resulttable.SelectSingleNode("//table[@class='ep_searchResultTable']");
            var doc = new HtmlDocument();
            doc.LoadHtml(resulttable.InnerHtml);
            var listoflinks = doc.DocumentNode.SelectNodes("//a[@href]");
            int count = 0;
            foreach (var row in listoflinks)
            {
                if( count==3)
                { break; }

                Procedures obj = new Procedures();
                count++;


                var href = row.Attributes["href"].Value;

                HtmlNode DataPageResult = b.Get(HttpUtility.HtmlDecode(href));


                var innernode = DataPageResult.SelectSingleNode("//div[@id='definition']");
                if (innernode != null)
                    obj.definition = innernode.InnerText;

                // Printthedata(innernode);
                innernode = DataPageResult.SelectSingleNode("//div[@id='reasons']");
                if (innernode != null)
                    obj.reasons = innernode.InnerText;
                //Printthedata(innernode);
                innernode = DataPageResult.SelectSingleNode("//div[@id='risk']");
                if (innernode != null)
                    obj.risk = innernode.InnerText;
                //Printthedata(innernode);
                innernode = DataPageResult.SelectSingleNode("//div[@id='expect']");
                if (innernode != null)
                    obj.expect = innernode.InnerText;
                // Printthedata(innernode);
                innernode = DataPageResult.SelectSingleNode("//div[@id='call']");
                if (innernode != null)
                    obj.call = innernode.InnerText;
                //Printthedata(innernode);

                try
                {
                    db.Procedures.Add(obj);
                    db.SaveChanges();
                }                
                catch(Exception ex)
                {
                    throw ex;
                }



            }

        }
    }
}