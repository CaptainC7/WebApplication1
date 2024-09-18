using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1
{
    public partial class ViewTaskHistory : System.Web.UI.Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                await LoadTaskHistoryAsync();
            }
        }

        private async System.Threading.Tasks.Task LoadTaskHistoryAsync()
        {
            string apiUrl = "https://localhost:7089/api/TaskHistory";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var taskHistoryList = JsonConvert.DeserializeObject<List<TaskHistoryModel>>(json);

                        StringBuilder tableBody = new StringBuilder();
                        foreach (var taskHistory in taskHistoryList)
                        {
                            tableBody.Append($@"
                                <tr>
                                    <td>{taskHistory.TaskName}</td>
                                    <td>{taskHistory.Description}</td>
                                    <td>{taskHistory.TaskGroup.GroupName}</td>
                                    <td>{taskHistory.TaskGroup.TaskListTemplate.TempName}</td>
                                    <td>{taskHistory.ChangedByPerson.FName} {taskHistory.ChangedByPerson.LName}</td>
                                    <td>{taskHistory.ChangedDate.ToString("yyyy-MM-dd")}</td>
                                    <td>{taskHistory.ChangedType}</td>
                                    <td>{taskHistory.DependancyTask?.TaskName ?? "N/A"}</td>
                                </tr>
                            ");
                        }

                        taskHistoryTableBody.InnerHtml = tableBody.ToString();
                    }
                    else
                    {
                        taskHistoryTableBody.InnerHtml = "<tr><td colspan='8' class='text-center'>No task history found.</td></tr>";
                    }
                }
                catch (Exception ex)
                {
                    taskHistoryTableBody.InnerHtml = $"<tr><td colspan='8' class='text-center'>Error: {ex.Message}</td></tr>";
                }
            }
        }
    }

    public class TaskHistoryModel
    {
        public string TaskName { get; set; }
        public string Description { get; set; }
        public TaskGroup TaskGroup { get; set; }
        public ChangedByPerson ChangedByPerson { get; set; }
        public DateTime ChangedDate { get; set; }
        public string ChangedType { get; set; }
        public DependancyTask DependancyTask { get; set; }
    }

    public class TaskGroup
    {
        public string GroupName { get; set; }
        public TaskListTemplate TaskListTemplate { get; set; }
    }

    public class TaskListTemplate
    {
        public string TempName { get; set; }
    }

    public class ChangedByPerson
    {
        public string FName { get; set; }
        public string LName { get; set; }
    }

    public class DependancyTask
    {
        public string TaskName { get; set; }
    }
}
