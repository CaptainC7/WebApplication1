using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class CreateTask : Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string groupId = Request.QueryString["id"];

                if (!string.IsNullOrEmpty(groupId))
                {
                    await LoadDependencyTasksAsync(groupId);
                }
            }
        }

        private async System.Threading.Tasks.Task LoadDependencyTasksAsync(string groupId)
        {
            string apiUrl = $"https://localhost:7089/api/Task/GetTasksByTaskGroup/{groupId}";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonData = await response.Content.ReadAsStringAsync();
                        var tasks = JsonConvert.DeserializeObject<List<TaskDTO>>(jsonData);

                        if (tasks != null)
                        {
                            ddlDependencyTasks.Items.Clear();
                            ddlDependencyTasks.Items.Add(new ListItem("Select a task", ""));

                            foreach (var task in tasks)
                            {
                                System.Diagnostics.Debug.WriteLine($"TaskID: {task.id}, TaskName: {task.TaskName}");
                                ddlDependencyTasks.Items.Add(new ListItem(task.TaskName, task.id.ToString()));
                            }
                        }
                    }
                    else
                    {
                        ddlDependencyTasks.Items.Add(new ListItem("Error loading tasks", ""));
                    }
                }
            }
            catch (Exception ex)
            {
                ddlDependencyTasks.Items.Add(new ListItem("Error loading tasks", ""));
            }
        }

        protected async void SubmitForm(object sender, EventArgs e)
        {
            string taskName = txtTaskName.Text.Trim();
            string description = txtDescription.Text.Trim();
            string taskGroupID = Request.QueryString["id"];
            string dependancyTaskID = string.IsNullOrEmpty(ddlDependencyTasks.SelectedValue) ? null : ddlDependencyTasks.SelectedValue;
            

            if (string.IsNullOrEmpty(taskName) || string.IsNullOrEmpty(description) || string.IsNullOrEmpty(taskGroupID))
            {
                Response.Write("<script>alert('All fields are required.');</script>");
                return;
            }

            var apiUrl = "https://localhost:7089/CreateTask";
            var payload = new
            {
                taskName = taskName,
                description = description,
                taskGroupID = taskGroupID,
                dependancyTaskID = dependancyTaskID,
                userID = 1
            };

            bool isSuccess = await PostTaskAsync(apiUrl, payload);

            if (isSuccess)
            {
                Response.Redirect($"Task.aspx?id={taskGroupID}");
            }
            else
            {
                Response.Write("<script>alert('Failed to create task. Please try again.');</script>");
            }
        }

        private async Task<bool> PostTaskAsync(string apiUrl, object payload)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var jsonPayload = JsonConvert.SerializeObject(payload);
                    var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    if (!response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        System.Diagnostics.Debug.WriteLine($"API Response Status: {response.StatusCode}");
                        System.Diagnostics.Debug.WriteLine($"API Response Content: {responseContent}");
                    }

                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception: " + ex.Message);
                return false;
            }
        }

        public class TaskDTO
        {
            public int id { get; set; }
            public string TaskName { get; set; }
        }
    }
}
