﻿using System;
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
    public class Healthnews1Controller : ApiController
    {
        private ApportiswebscrapperContext db = new ApportiswebscrapperContext();

        // GET: api/Healthnews1
        public IQueryable<Healthnews> GetHealthnews(string id)
        {
            IQueryable<Healthnews> Articles = null;
            try
            {

                Articles = from Healthnews in db.Healthnews
                           where Healthnews.searchkey == id
                           select Healthnews;

                if (!Articles.Any())
                {

                    Scrapeit(id, db);
                    return db.Healthnews;
                }
                else
                {
                    return Articles;
                }
            }
            catch
            {
                Scrapeit(id, db);
                return db.Healthnews;
            }
        }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="id"></param>
      /// <param name="db"></param>
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

                Healthnewssearch(b, id);
                response = b.Post("http://healthlibrary.epnet.com/GetContent.aspx");
                Healthnewsresults(b, response, id, db);

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
        private static void Healthnewssearch(BrowserSession newform, string id)
        {
            newform.FormElements["ContentURL"] = String.Concat("26,31 $", id, "| false | NWS");
            newform.FormElements["search1"] = String.Concat("Title = (", id, ") OR Teaser = (", id, ") OR ArticleBody = (", id, ")");
            newform.FormElements["ep_defaultContentURL"] = " ";


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="resulttable"></param>
        private static void Healthnewsresults(BrowserSession browser, HtmlNode resulttable,string id , ApportiswebscrapperContext db)
        {
            resulttable = resulttable.SelectSingleNode("//table[@class='ep_searchResultTable']");
            var doc = new HtmlDocument();
            doc.LoadHtml(resulttable.InnerHtml);
            var listoflinks = doc.DocumentNode.SelectNodes("//a[@href]");
            int count = 0;
            foreach (var row in listoflinks)
            {

                if(count ==3)
                 {
                    break;
                }

                Healthnews obj = new Healthnews();
                count++;
                var href = row.Attributes["href"].Value;
                HtmlNode DataPageResult = browser.Get((HttpUtility.HtmlDecode(href)));
               

                var doc2 = new HtmlDocument();
                doc2.LoadHtml(DataPageResult.InnerHtml);
                var innernode = doc2.DocumentNode.SelectSingleNode("//div[@id='ep_documentBody']");
                if (innernode != null)
                {
                    obj.ep_documentBody = innernode.InnerText;
                }
                obj.Articleno = count;
                obj.searchkey = id;

                try
                {
                    db.Healthnews.Add(obj);
                    db.SaveChanges();
                }
                catch(Exception e)
                {
                    throw e;
                }
                //Printthedata(innernode);
                var x = 90;

            }
        }
    }
}