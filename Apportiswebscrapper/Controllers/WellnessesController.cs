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
    public class WellnessesController : ApiController
    {
        private ApportiswebscrapperContext db = new ApportiswebscrapperContext();

        // GET: api/Wellnesses
        public IQueryable<Wellness> GetWellnesses(string id)
        {
            IQueryable<Wellness> Articles = null;
            try
            {

                Articles = from Wellness in db.Wellnesses
                           where Wellness.searchkey == id
                           select Wellness;

                if (!Articles.Any())
                {

                    Scrapeit(id, db);
                    return db.Wellnesses;
                }
                else
                {
                    return Articles;
                }
            }
            catch
            {
                Scrapeit(id, db);
                return db.Wellnesses;
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

                Wellnesscenter(b, id);
                response = b.Post("http://healthlibrary.epnet.com/GetContent.aspx");
                Wellnesssearchesults(b, response,id,db);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private static void Wellnesscenter(BrowserSession newform, string id)
        {
            newform.FormElements["ContentURL"] = String.Concat("12,13,15,16,17,18,19,20,21,22,651,656,656,656,656,671,711,715,1039,1081 $", id, "|false|WCT");
            newform.FormElements["search1"] = String.Concat("Title = (", id, ") OR ArticleBody = (", id, ")");
            //newform.Method = HttpVerb.Post;

        }

        private static void Wellnesssearchesults(BrowserSession browser, HtmlNode resulttable, string searchkey, ApportiswebscrapperContext db)
        {
            resulttable = resulttable.SelectSingleNode("//table[@class='ep_searchResultTable']");
            //throw new NotImplementedException();
            var doc = new HtmlDocument();
            doc.LoadHtml(resulttable.InnerHtml);
            var listoflinks = doc.DocumentNode.SelectNodes("//a[@href]");
            int count = 0;
            foreach (var row in listoflinks)
            {
                if( count==3)
                {
                    break;
                }

                Wellness obj = new Wellness();
                count++;
                var href = row.Attributes["href"].Value;
                HtmlNode DataPageResult = browser.Get((HttpUtility.HtmlDecode(href)));

                var doc2 = new HtmlDocument();
                doc2.LoadHtml(DataPageResult.InnerHtml);
                var innernode = doc2.DocumentNode.SelectSingleNode("//div[@id='ep_documentBody']");
                if (innernode != null)
                {
                    obj.ep_documentBody = innernode.InnerText;
                    obj.Articleno = count;
                    obj.searchkey = searchkey;
                   
                }
                try
                {
                    db.Wellnesses.Add(obj);
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    throw ex;
                }
                var x = 80;
                //  Printthedata(innernode);

            }
        }
    }
}