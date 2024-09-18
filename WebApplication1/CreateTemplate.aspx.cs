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

namespace WebApplication1
{
    public partial class About : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
            }
        }

        protected async void submitForm(object sender, EventArgs e)
        {
            string templateName = txtTemplateName.Text.Trim();

            if (string.IsNullOrEmpty(templateName))
            {
                Response.Write("<script>alert('Template Name is required.');</script>");
                return;
            }

            string apiUrl = "https://localhost:7089/api/TaskListTemplate/CreateTemplate"; 
            bool isSuccess = await PostTemplateNameAsync(apiUrl, templateName);

            if (isSuccess)
            {
                Response.Write("<script>alert('Template successfully created with name: " + templateName + "');</script>");
                Response.Redirect("~/Home.aspx");
            }
            else
            {
                Response.Write("<script>alert('Failed to create template. Please try again.');</script>");
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Home.aspx");
        }


        private async Task<bool> PostTemplateNameAsync(string apiUrl, string templateName)
        {
            var payload = new
            {
                tempName = templateName,
                createDate = DateTime.Now,
                createdBy = 1,
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