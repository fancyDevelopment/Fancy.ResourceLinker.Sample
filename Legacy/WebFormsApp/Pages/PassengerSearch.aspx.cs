using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebFormsApp.Pages
{
    public partial class PassengerSearch : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            string forwardedHost = Request.Headers.Get("X-Forwarded-Host");

            if (forwardedHost == "localhost:5100")
            {
                // Show content only master page
                MasterPageFile = "~/Site.ContentOnly.Master";
            }
        }
    }
}