using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.IO;

namespace RSSFilter
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            string feedsFileName = Path.Combine(Server.MapPath("./"), ConfigurationManager.AppSettings["FeedsFilePath"]);

            //feedsをXMLファイルからリストア
            FeedsDS.FeedsDTDataTable feedsDT = new FeedsDS.FeedsDTDataTable();
            if (File.Exists(feedsFileName))
            {
                feedsDT.ReadXml(feedsFileName);
            }
            Application["feedsDT"] = feedsDT;
        }
    } // end of class
} // end of namespace
