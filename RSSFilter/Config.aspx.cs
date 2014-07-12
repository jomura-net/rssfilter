using System;
using System.Text;
using System.Web;
using Rss;
using System.Web.UI.WebControls;

namespace RSSFilter
{
    public partial class Config : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.GridView1_Bind();
            } 
        }

        private void GridView1_Bind()
        {
            FeedsDS.FeedsDTDataTable feedsDT = this.Application["feedsDT"] as FeedsDS.FeedsDTDataTable;
            this.GridView1.DataSource = feedsDT.Select("", "ResistDate Desc");
            this.GridView1.DataBind();
        }

        protected void btnGetFilterdFeed_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;

            string encordedURL = HttpUtility.UrlEncode(this.tbFeedURL.Text);
            string encordedFilterString = HttpUtility.UrlEncode(this.tbFilterString.Text);
            StringBuilder url = new StringBuilder();
            url.Append("Feed.aspx?url=" + encordedURL + "&q=" + encordedFilterString);
            if (cbTitle.Checked)
            {
                url.Append("&title=");
            }
            if (cbDescription.Checked)
            {
                url.Append("&Description=");
            }

            Response.Redirect(url.ToString());
        }

        protected void cvFilterTarget_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            if (!IsValid) return;
            if (!cbTitle.Checked && !cbDescription.Checked)
            {
                this.cvFilterTarget.Text = "filter targetÇÕ1Ç¬à»è„éwíËÇµÇƒÇ≠ÇæÇ≥Ç¢ÅB";
                args.IsValid = false;
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.GridView1.PageIndex = e.NewPageIndex;
            this.GridView1_Bind();
        }

    } // end of class
} // end of namespace
