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
using System.Web;
using HtmlAgilityPack;

namespace Apportiswebscrapper.Controllers
{
    public class EssentialsController : ApiController
    {
        private ApportiswebscrapperContext db = new ApportiswebscrapperContext();

        // GET: api/Essentials
        public IQueryable<Essentials> GetEssentials(string id)
        {
            IQueryable<Essentials> Articles = null;
            try
            {

                Articles = from Essential in db.Essentials
                           where Essential.Searchkeyword== id
                           select Essential;

                if (!Articles.Any())
                {

                    Scrapeit(id, db);
                    return db.Essentials;
                }
                else
                {
                    return Articles;
                }
            }
            catch
            {
                Scrapeit(id, db);
                return db.Essentials;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="search"></param>
        /// <param name="db"></param>
        /// <returns></returns>

        public static string Scrapeit(string search, ApportiswebscrapperContext db)
        {
            try
            {

                var gurl = "http://healthlibrary.epnet.com/authenticate.aspx?token=0EF30C79-8A3A-42F4-BE9F-2A90807D3B47&ReturnUrl=http%3a%2f%2fhealthlibrary.epnet.com%2fGetContent.aspx%3faccount%3dhlhosp";

                BrowserSession b = new BrowserSession();
                b.Get("http://healthlibrary.epnet.com/authenticate.aspx?token=0EF30C79-8A3A-42F4-BE9F-2A90807D3B47&ReturnUrl=http%3a%2f%2fhealthlibrary.epnet.com%2fGetContent.aspx%3faccount%3dhlhosp");
                b.FormElements["sUserName"] = "hlhosp";
                b.FormElements["sPassword"] = "fitness";

                HtmlNode response = b.Post("http://healthlibrary.epnet.com/authenticate.aspx?token=0EF30C79-8A3A-42F4-BE9F-2A90807D3B47&ReturnUrl=http%3a%2f%2fhealthlibrary.epnet.com%2fGetContent.aspx%3faccount%3dhlhosp");

                essentialsearch(b, search);
                response = b.Post("http://healthlibrary.epnet.com/authenticate.aspx?token=0EF30C79-8A3A-42F4-BE9F-2A90807D3B47&ReturnUrl=http%3a%2f%2fhealthlibrary.epnet.com%2fGetContent.aspx%3faccount%3dhlhosp");
                 DefaultResults(b, response, db, search);

            }
            catch (Exception ex)
            {
                throw ex;
                return "false";
            }
            return "true";
        }

        private static void Printthedata(HtmlNode innernode)
        {
            if (innernode != null)
            {
                Console.WriteLine(innernode.InnerText);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="resulttable"></param>
        private static void essentialsearch(BrowserSession newform, String search)
        {
            newform.FormElements["ep_searchTerms"] = search;
            newform.FormElements["searchbutton.x"] = "22";
            newform.FormElements["searchbutton.y"] = "9";
            newform.FormElements["search1"] = String.Concat("Title= (", search, ") OR ArticleBody = (", search, ") NOT HGID = (livingwith)");

            newform.FormElements["ContentURL"] = String.Concat("2 $", search, "| false | essential");
            //newform.Method = HttpVerb.Post;
        }


        private static  void DefaultResults(BrowserSession b, HtmlNode resulttable, ApportiswebscrapperContext db, string search)
        {
            resulttable = resulttable.SelectSingleNode("//table[@class='ep_searchResultTable']");
            var doc = new HtmlDocument();
            doc.LoadHtml(resulttable.InnerHtml);
            var listoflinks = doc.DocumentNode.SelectNodes("//a[@href]");
            int id = 0;
            foreach (var row in listoflinks)
            {
                id++;
                Essentials obj = new Essentials();


                var href = row.Attributes["href"].Value;
                HtmlNode html = b.Get(HttpUtility.HtmlDecode(href));

                var datadefinition = html.SelectSingleNode("//div[@id='definition']");
                if(datadefinition != null)
                obj.definition = datadefinition.InnerText;

                
                var causes = html.SelectSingleNode("//div[@id='causes']");
                if(causes != null)
                obj.causes = causes.InnerText;
               
                var risk = html.SelectSingleNode("//div[@id='risk']");
                if(risk != null)
                obj.risk = risk.InnerText;
                
                var symptoms = html.SelectSingleNode("//div[@id='symptoms']");
                if(symptoms !=null)
                
                obj.symptoms = symptoms.InnerText;
                var treatment = html.SelectSingleNode("//div[@id='treatment']");
                if(treatment != null)
                obj.treatment = treatment.InnerText;
              
                var prevention = html.SelectSingleNode("//div[@id='prevention']");
                if(prevention != null)
                obj.prevention = prevention.InnerText;
                

                obj.Articleno = id;
                obj.Searchkeyword = search;
                //PutEsssomething(obj, id);
                

                try
                {                   
                    db.Essentials.Add(obj);                    
                     db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {

                    throw;

                }
              
            }
        }


       
    }
}