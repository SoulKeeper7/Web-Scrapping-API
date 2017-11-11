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
    public class DrugcsController : ApiController
    {
        private ApportiswebscrapperContext db = new ApportiswebscrapperContext();

        // GET: api/Drugcs
        public IQueryable<Drugcs> GetDrugcs(string id)
        {
            IQueryable<Drugcs> Articles = null;
            try
            {

                Articles = from Drugcs in db.Drugcs
                           where Drugcs.searchkey == id
                           select Drugcs;

                if (!Articles.Any())
                {

                    Scrapeit(id, db);
                    return db.Drugcs;
                }
                else
                {
                    return Articles;
                }
            }
            catch
            {
                Scrapeit(id, db);
                return db.Drugcs;
            }
        }

       


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

                Drugdatabasesearch(b, id);
                response = b.Post("http://healthlibrary.epnet.com/GetContent.aspx");
                Drugdatabase(b, response, id, db);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newform"></param>
        private static void Drugdatabasesearch(BrowserSession newform, string id)
        {
            newform.FormElements["ContentURL"] = String.Concat("1125 $", id, "|false|DRG");
            newform.FormElements["search1"] = String.Concat("Title = (", id, ") OR ArticleBody = (", id, ")");
            //newform.Method = HttpVerb.Post;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="resulttable"></param>
        private static void Drugdatabase(BrowserSession browser, HtmlNode resulttable, string id, ApportiswebscrapperContext db)
        {
            resulttable = resulttable.SelectSingleNode("//table[@class='ep_searchResultTable']");
            var doc = new HtmlDocument();
            doc.LoadHtml(resulttable.InnerHtml);
            var listoflinks = doc.DocumentNode.SelectNodes("//a[@href]");
            int count = 0;
            foreach (var row in listoflinks)
            {
                if (count == 3)
                {
                    break;
                }

                Drugcs obj = new Drugcs();
                count++;

                var href = row.Attributes["href"].Value;
                HtmlNode DataPageResult = browser.Get((HttpUtility.HtmlDecode(href)));


                var innernode = DataPageResult.SelectSingleNode("//div[@id='drgAboutYourTreatment']");
                if (innernode != null)
                {
                    obj.drgAboutYourTreatment = innernode.InnerText; ;
                }
                // Printthedata(innernode);
                innernode = DataPageResult.SelectSingleNode("//div[@id='drgAdministeringYourMed']");
                if( innernode != null)
                 {
                    obj.drgAboutYourTreatment = innernode.InnerText;
                }
                // Printthedata(innernode);
                innernode = DataPageResult.SelectSingleNode("//div[@id='drgStorage']");
                if (innernode != null)
                {
                    obj.drgStorage = innernode.InnerText;
                }
                // Printthedata(innernode);
                innernode = DataPageResult.SelectSingleNode("//a[@name='Whatllow?']");
                if (innernode != null)
                {
                    obj.Whatllow = innernode.InnerText;
                }

                obj.Articleno = count;
                obj.searchkey = id;

                try
                {
                    db.Drugcs.Add(obj);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                // Printthedata(innernode);
                //innernode = DataPageResult.Html.SelectSingleNode("//div[@id='call']");
                //Printthedata(innernode);



            }

        }
    }
}