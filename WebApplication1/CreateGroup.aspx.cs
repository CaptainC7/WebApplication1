using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace WebApplication1
{
    public partial class CreateGroup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                
            }
        }

        protected async void submitForm(object sender, EventArgs e)
        {
            string groupName = txtGroupName.Text.Trim();

            if (string.IsNullOrEmpty(groupName))
            {
                Response.Write("<script>alert('Template Name is required.');</script>");
                return;
            }

            string apiUrl = "https://localhost:7089/CreateTaskGroup";
            bool isSuccess = await PostTemplateNameAsync(apiUrl, groupName);

            if (isSuccess)
            {
                Response.Write("<script>alert('Group successfully created with name: " + groupName + "');</script>");
            }
            else
            {
                Response.Write("<script>alert('Failed to create Group. Please try again.');</script>");
            }
        }

        private async Task<bool> PostTemplateNameAsync(string apiUrl, string groupName)
        {
            var payload = new
            {
                groupName = groupName,
                taskListTemplateID = Request.QueryString["id"],
                userID = 1
            };

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var jsonPayload = JsonConvert.SerializeObject(payload);
                    var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else
                    {
                        string errorResponse = await response.Content.ReadAsStringAsync();
                        Response.Write("<script>alert('API Error: " + errorResponse + "');</script>");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
                return false;
            }
        }

    }
}