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
    public class ConditionsController : ApiController
    {
        private ApportiswebscrapperContext db = new ApportiswebscrapperContext();

        // GET: api/Conditions1
        public IQueryable<Conditions> GetConditions(string id)
        {
            IQueryable<Conditions> Articles = null;
            try
            {               
   
                Articles = from Conditions in db.Conditions
                            where Conditions.searchkey == id
                            select Conditions;

                if(!Articles.Any())
                {

                    Scrapeit(id, db);
                    return db.Conditions;
                }
                else
                {                   
                    return Articles;
                }
             
            }
            catch
            {
                Scrapeit(id, db);
                return db.Conditions;
            }
            
        }


        public static void Scrapeit(string id, ApportiswebscrapperContext db)
        {
            try
            {

                BrowserSession b = new BrowserSession();
                b.Get("http://healthlibrary.epnet.com/authenticate.aspx?token=0EF30C79-8A3A-42F4-BE9F-2A90807D3B47&ReturnUrl=http%3a%2f%2fhealthlibrary.epnet.com%2fGetContent.aspx%3faccount%3dhlhosp");
                b.FormElements["sUserName"] = "hlhosp";
                b.FormElements["sPassword"] = "fitness";

                HtmlNode response = b.Post("http://healthlibrary.epnet.com/authenticate.aspx?token=0EF30C79-8A3A-42F4-BE9F-2A90807D3B47&ReturnUrl=http%3a%2f%2fhealthlibrary.epnet.com%2fGetContent.aspx%3faccount%3dhlhosp");

                condtionsearch(b, id);
                response = b.Post("http://healthlibrary.epnet.com/GetContent.aspx");
                CondtionResults(b, response, id, db);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// /
        /// </summary>
        /// <param name="b"></param>
        private static void condtionsearch(BrowserSession b, string id)
        {
            b.FormElements["search1"] = String.Concat("Title=(", id, ") OR ArticleBody=(", id, ")");
            b.FormElements["ContentURL"] = String.Concat("2,254 $", id, "| false | CND");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="resulttable"></param>
        private static void CondtionResults(BrowserSession b, HtmlNode resulttable, string id, ApportiswebscrapperContext db)
        {
            resulttable = resulttable.SelectSingleNode("//table[@class='ep_searchResultTable']");
            if (resulttable == null)
                return;

            try
            {

                var doc = new HtmlDocument();
                doc.LoadHtml(resulttable.InnerHtml);
                var listoflinks = doc.DocumentNode.SelectNodes("//a[@href]");
                int count = 0;
                foreach (var row in listoflinks)
                {

                    var href = row.Attributes["href"].Value;
                    HtmlNode getthepage = b.Get(HttpUtility.HtmlDecode(href));


                    var doc2 = new HtmlDocument();
                    doc2.LoadHtml(getthepage.InnerHtml);
                    var Menucontainer = doc2.DocumentNode.SelectSingleNode("//div[@class='ep_groupNavigation']");
                    doc2.LoadHtml(Menucontainer.InnerHtml);
                    var listofpagestogo = doc2.DocumentNode.SelectNodes("//a[@href]");

                   
                    if (count == 3)
                    {
                        break;
                    }

                    Conditions obj = new Conditions();
                    count++;
                    foreach (var links in listofpagestogo)
                    {

                        

                        var linktopage = links.Attributes["href"].Value;
                        var x = (HttpUtility.HtmlDecode(linktopage));
                        linktopage = String.Concat("http://healthlibrary.epnet.com", x.ToString());

                        HtmlNode html = b.Get(linktopage);
                        var doc3 = new HtmlDocument();
                        doc3.LoadHtml(html.InnerHtml);
                        HtmlNode innernode = null;
                        switch (links.InnerHtml.ToString())
                        {
                            case "Main Page":
                                innernode = doc3.DocumentNode.SelectSingleNode("//div[@id='CIDintroduction']");
                                if (innernode != null)
                                    obj.CIDintroduction = innernode.InnerText;
                                break;
                            case "Risk Factors":
                                innernode = doc3.DocumentNode.SelectSingleNode("//div[@id='CIDrisk']");
                                if (innernode != null)
                                    obj.CIDrisk = innernode.InnerText;
                                break;
                            case "Symptoms":
                                innernode = doc3.DocumentNode.SelectSingleNode("//div[@id='CIDsymptoms']");
                                if (innernode != null)
                                    obj.CIDsymptoms = innernode.InnerText;
                                break;
                            case "Treatment":
                                innernode = doc3.DocumentNode.SelectSingleNode("//div[@id='CIDtreatment']");
                                if (innernode != null)
                                    obj.CIDtreatment = innernode.InnerText;
                                break;
                            case "Screening":
                                innernode = doc3.DocumentNode.SelectSingleNode("//div[@id='CIDscreening']");
                                if (innernode != null)
                                    obj.CIDscreening = innernode.InnerText;
                                break;
                            case "Reducing Your Risk":
                                innernode = doc3.DocumentNode.SelectSingleNode("//div[@id='CIDriskreduction']");
                                if (innernode != null)
                                    obj.CIDriskreduction = innernode.InnerText;
                                break;
                            case "Talking to Your Doctor":
                                innernode = doc3.DocumentNode.SelectSingleNode("//div[@id='CIDtalking']");
                                if (innernode != null)
                                    obj.CIDtalking = innernode.InnerText;
                                break;
                            case "Resource Guide": break;


                            default:
                                break;


                        }

                        // Printthedata(innernode);
                      
                    }
                    try
                    {
                        obj.searchkey = id;
                        db.Conditions.Add(obj);
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        throw e;

                    }
                    //var datadefinition = doc.DocumentNode.SelectSingleNode("//div[@id='CIDTreatment']");
                    //Console.WriteLine(datadefinition.InnerText);
                    //var causes = DataPageResult.Html.SelectSingleNode("//div[@id='CIDrisk']");
                    //Console.WriteLine(causes.InnerText);
                    //var risk = DataPageResult.Html.SelectSingleNode("//div[@id='CIDsymptoms']");
                    //Console.WriteLine(risk.InnerText);
                    //var symptoms = DataPageResult.Html.SelectSingleNode("//div[@id='CIDdiagnosis']");
                    //Console.WriteLine(symptoms.InnerText);
                    //var treatment = DataPageResult.Html.SelectSingleNode("//div[@id='CIDTreatment']");
                    //Console.WriteLine(treatment.InnerText);
                    //var prevention = DataPageResult.Html.SelectSingleNode("//div[@id='CIDscreening']");
                    //Console.WriteLine(prevention.InnerText);
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}