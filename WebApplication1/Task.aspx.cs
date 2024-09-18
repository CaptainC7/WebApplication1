using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace WebApplication1
{
    public partial class Task : Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string groupId = Request.QueryString["id"];
                btnCreateTask.HRef = "/CreateTask?id=" + groupId;

                if (!string.IsNullOrEmpty(groupId))
                {
                    await LoadTasksAsync(groupId);
                }
            }
        }

        private async System.Threading.Tasks.Task LoadTasksAsync(string groupId)
        {
            var url = $"https://localhost:7089/api/Task/GetTasksByTaskGroup/{groupId}";
            var (jsonData, statusCode) = await FetchDataFromUrlAsync(url);

            if (statusCode == HttpStatusCode.NotFound || string.IsNullOrEmpty(jsonData))
            {
                TaskContainer.Text = "<p>No tasks found for this group.</p>";
            }
            else
            {
                try
                {
                    var taskList = JsonConvert.DeserializeObject<List<TaskDTO>>(jsonData);

                    if (taskList == null || !taskList.Any())
                    {
                        TaskContainer.Text = "<p>No tasks found for this group.</p>";
                    }
                    else
                    {
                        TaskContainer.Text = GenerateBootstrapCards(taskList);
                    }
                }
                catch (JsonException)
                {
                    TaskContainer.Text = "<p>Error parsing data. Please try again later.</p>";
                }
            }
        }

        private async Task<(string jsonData, HttpStatusCode statusCode)> FetchDataFromUrlAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(url);
                    var jsonData = await response.Content.ReadAsStringAsync();
                    return (jsonData, response.StatusCode);
                }
                catch
                {
                    return (string.Empty, HttpStatusCode.InternalServerError);
                }
            }
        }

        private string GenerateBootstrapCards(IEnumerable<TaskDTO> tasks)
        {
            StringBuilder cardHtml = new StringBuilder();

            foreach (var task in tasks)
            {
                cardHtml.Append($@"
                    <div class='card mb-3' id='task-{task.Id}'>
                        <div class='card-body'>
                            <h5 class='card-title'>{task.TaskName}</h5>
                            <p class='card-text'>{task.Description}</p>
                            <p class='card-text'><small class='text-muted'>{task.TaskGroup.GroupName}</small></p>
                            <!-- Delete Button -->
                            <button type='button' class='btn btn-danger' onclick='confirmDelete({task.Id})'>Delete</button>
                        </div>
                    </div>");
            }

            return cardHtml.ToString();
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                string eventTarget = Request["__EVENTTARGET"];
                string eventArgument = Request["__EVENTARGUMENT"];

                if (eventTarget == "DeleteTask" && !string.IsNullOrEmpty(eventArgument))
                {
                    DeleteTaskAsync(eventArgument).Wait();
                }
            }
        }

        private async System.Threading.Tasks.Task DeleteTaskAsync(string taskId)
        {
            string apiUrl = $"https://localhost:7089/api/Task/DeleteTask/{taskId}";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.DeleteAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        Response.Redirect(Request.Url.AbsoluteUri);
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Failed to delete task. Please try again.');", true);
                    }
                }
            }
            catch
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('An error occurred while deleting the task.');", true);
            }
        }

        public class TaskDTO
        {
            public int Id { get; set; }
            public string TaskName { get; set; }
            public string Description { get; set; }
            public TaskGroupDTO TaskGroup { get; set; }
        }

        public class TaskGroupDTO
        {
            public string GroupName { get; set; }
        }
    }
}
