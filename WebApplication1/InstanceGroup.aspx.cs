using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

namespace WebApplication1
{
    public partial class InstanceGroup : System.Web.UI.Page
    {
        private int templateId;
        private int instanceId;
        private string apiUrl = "https://localhost:7089";

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Get TemplateID and InstanceID from Query String
                templateId = Convert.ToInt32(Request.QueryString["templateId"]);
                instanceId = Convert.ToInt32(Request.QueryString["instanceId"]);

                // Load groups for the template
                await LoadGroups();
            }
        }

        private async System.Threading.Tasks.Task LoadGroups()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(apiUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync($"/api/TaskGroup/GetTaskGroupsByTemplate/{templateId}");
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var groups = JsonConvert.DeserializeObject<List<TaskGroupCheck>>(jsonResponse);

                RepeaterGroups.DataSource = groups;
                RepeaterGroups.DataBind();

                // Load users for all dropdowns
                await LoadUsers();
            }
        }

        private async System.Threading.Tasks.Task LoadUsers()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(apiUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync("/GetUsers");
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<List<User>>(jsonResponse);

                // Bind users to each dropdown list in the Repeater
                foreach (RepeaterItem item in RepeaterGroups.Items)
                {
                    DropDownList ddlUsers = (DropDownList)item.FindControl("ddlUsers");
                    ddlUsers.DataSource = users;
                    ddlUsers.DataTextField = "FullName"; // Assuming you concatenate fName + lName in FullName property.
                    ddlUsers.DataValueField = "id";
                    ddlUsers.DataBind();
                }
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int groupId = Convert.ToInt32(btn.CommandArgument);

            // Show dropdown and Save button, hide Add button
            Button addButton = (Button)btn.Parent.FindControl("btnAdd");
            addButton.Visible = false;
            DropDownList ddlUsers = (DropDownList)btn.Parent.FindControl("ddlUsers");
            ddlUsers.Visible = true;
            Button saveButton = (Button)btn.Parent.FindControl("btnSave");
            saveButton.Visible = true;
        }

        protected async void btnSave_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int groupId = Convert.ToInt32(btn.CommandArgument);
            DropDownList ddlUsers = (DropDownList)btn.Parent.FindControl("ddlUsers");

            if (ddlUsers.SelectedValue == "0") // "Assign to user" default value check
            {
                lblError.Text = "Please select a user.";
                lblError.Visible = true;
                return;
            }

            int assignedTo = Convert.ToInt32(ddlUsers.SelectedValue);

            // Create the group instance
            await CreateGroupInstance(groupId, assignedTo);

            // After successful creation, hide Save and dropdown, show Add button
            lblError.Visible = false;
            Button saveButton = (Button)btn.Parent.FindControl("btnSave");
            saveButton.Visible = false;
            ddlUsers.Visible = false;
            Button addButton = (Button)btn.Parent.FindControl("btnAdd");
            addButton.Visible = true;
        }

        private async System.Threading.Tasks.Task CreateGroupInstance(int groupId, int assignedTo)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(apiUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var groupInstance = new
            {
                taskGroupID = groupId,
                taskListInstanceID = instanceId,
                assignedTo = assignedTo,
                status = "Pending"
            };

            var content = new StringContent(JsonConvert.SerializeObject(groupInstance), System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("/CreateTaskGroupInstance", content);

            if (!response.IsSuccessStatusCode)
            {
                // Handle error
            }
        }
    }

    // Models
    public class TaskGroupCheck
    {
        public int id { get; set; }
        public string groupName { get; set; }
        public int taskListTemplateID { get; set; }
    }

    public class User
    {
        public int id { get; set; }
        public string fName { get; set; }
        public string lName { get; set; }
        public string FullName { get { return fName + " " + lName; } }
    }
}
