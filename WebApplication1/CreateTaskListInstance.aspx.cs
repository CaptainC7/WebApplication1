using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class CreateTaskListInstance : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Load template name and users dropdown only once when the page loads for the first time
                LoadTemplateName();
                LoadUsers();
            }
        }

        private void LoadTemplateName()
        {
            // Assume the template ID is passed via query string (?id=1)
            int templateID = Convert.ToInt32(Request.QueryString["id"]);

            // Get the template name via API
            string apiUrl = $"https://localhost:7089/api/TaskListTemplate/GetTemplate/{templateID}";
            using (HttpClient client = new HttpClient())
            {
                var response = client.GetAsync(apiUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var templateData = response.Content.ReadAsAsync<dynamic>().Result;
                    txtTemplateName.Text = templateData.tempName; // Set the template name in the textbox
                }
            }
        }

        private void LoadUsers()
        {
            string apiUrl = "https://localhost:7089/GetUsers";
            using (HttpClient client = new HttpClient())
            {
                var response = client.GetAsync(apiUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var users = response.Content.ReadAsAsync<List<dynamic>>().Result;
                    foreach (var user in users)
                    {
                        string userName = $"{user.fName} {user.lName}";
                        ddlUsers.Items.Add(new ListItem(userName, user.id.ToString()));
                    }
                }
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            // Collect all input values from the form
            DateTime startDate = DateTime.Parse(txtStartDate.Text);
            DateTime dueDate = DateTime.Parse(txtDueDate.Text);
            int assignedTo = Convert.ToInt32(ddlUsers.SelectedValue);
            int taskListTemplateID = Convert.ToInt32(Request.QueryString["id"]);
            string status = "pending"; // Fixed status value

            // Create the task list instance model
            TaskListInstanceModel taskListInstance = new TaskListInstanceModel
            {
                TaskListTemplateID = taskListTemplateID,
                StartDate = startDate,
                DueDate = dueDate,
                AssignedTo = assignedTo,
                Status = status
            };

            // Send the task list instance data to the API
            string apiUrl = "https://localhost:7089/TaskListInstance/CreateTaskListInstance";
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.PostAsJsonAsync(apiUrl, taskListInstance).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Success logic, for example, redirect to another page or show success message
                    Response.Write("<script>alert('Task List Instance Created Successfully');</script>");
                }
                else
                {
                    // Error handling
                    Response.Write("<script>alert('Error creating Task List Instance');</script>");
                }
            }
        }
    }

    // Model class for Task List Instance data
    public class TaskListInstanceModel
    {
        public int TaskListTemplateID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public int AssignedTo { get; set; }
        public string Status { get; set; }
    }
}
