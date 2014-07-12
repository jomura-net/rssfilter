using System;
using System.Diagnostics;
using System.Net;
using System.Web;
using System.Text;
using Rss;
using System.Configuration;
using System.IO;

namespace RSSFilter
{
    public partial class Feed : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string decodedURL = HttpUtility.HtmlDecode(Request.QueryString["url"]);
            string decodedFilterString = HttpUtility.HtmlDecode(Request.QueryString["q"]);

            string[] filterStrings = decodedFilterString.Split(new char[]{' ', 'Å@'}, StringSplitOptions.RemoveEmptyEntries);

            bool checkDescription = Request.QueryString["Description"] != null;
            bool checkTitle = true;
            if (checkDescription)
            {
                checkTitle = Request.QueryString["title"] != null;
            }
            
            RssFeed feed;
            try
            {
                feed = RssFeed.Read(decodedURL);
            }
            catch (Exception ex)
            {
                Response.ContentType = "text/plain";
                Response.Write(ex.Message);
                Response.End();
                return;
            }

            if (feed.Channels.Count == 0)
            {
                Response.ContentType = "text/plain";
                Response.Write("This feed has no channel.");
                Response.End();
                return;
            }

            //FeedÇìoò^
            lock (Application["feedsDT"])
            {
                FeedsDS.FeedsDTDataTable feedsDT = Application["feedsDT"] as FeedsDS.FeedsDTDataTable;
                FeedsDS.FeedsDTRow row = feedsDT.FindByFilterStringFeedURL(decodedFilterString, decodedURL);
                if (null == row)
                {
                    // insert
                    feedsDT.AddFeedsDTRow(decodedURL, decodedFilterString, checkTitle, checkDescription, Request.Url.AbsoluteUri, DateTime.Now);
                }
                else
                {
                    //update
                    row.TargetTitle = checkTitle;
                    row.TargetDescription = checkDescription;
                    row.ResistDate = DateTime.Now;
                }
                WriteFeedXml(feedsDT);
            }


            //main from here

            foreach (RssChannel channel in feed.Channels)
            {
                RssItemCollection newItems = new RssItemCollection();
                foreach (RssItem item in channel.Items)
                {
                    StringBuilder targetString = new StringBuilder();
                    if (checkTitle)
                    {
                        targetString.Append(item.Title);
                    }
                    if (checkDescription)
                    {
                        targetString.Append(item.Description);
                    }
                    if (!IsFilteredEntry(targetString.ToString(), filterStrings))
                    {
                        newItems.Add(item);
                    }
                }

                if (newItems.Count == 0)
                {
                    Response.ContentType = "text/plain";
                    Response.Write("Channel must contain at least one item.");
                    Response.End();
                    return;
                }

                channel.Items.Clear();
                foreach (RssItem item in newItems)
                {
                    channel.Items.Add(item);
                }
            }

            feed.Encoding = feed.Encoding ?? Encoding.UTF8;
            Response.ContentType = "text/xml";
            feed.Write(Response.OutputStream);
            Response.End();
        }

        bool IsFilteredEntry(string targetString, string[] filterStrings)
        {
            string targetStringLow = targetString.ToLower();
            foreach (string filterString in filterStrings)
            {
                string filterStringLow = filterString.ToLower();
                if (filterStringLow.StartsWith("-"))
                {
                    if (targetStringLow.Contains(filterStringLow.Substring(1))) return true;
                }
                else
                {
                    if (!targetStringLow.Contains(filterStringLow)) return true;
                }
            }
            return false;
        }

        void WriteFeedXml(FeedsDS.FeedsDTDataTable feedsDT)
        {
            string feedsFileName = Path.Combine(Server.MapPath("./"), ConfigurationManager.AppSettings["FeedsFilePath"]);
            if (!File.Exists(feedsFileName))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(feedsFileName));
            }
            feedsDT.WriteXml(feedsFileName);
        }
    } // end of class
} // end of namespace
