using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;

namespace WebApplication1
{
    public partial class TemplateGroup : Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string id = Request.QueryString["id"];

                if (!string.IsNullOrEmpty(id))
                {
                    btnCreateGroup.HRef = "/CreateGroup?id=" + id;

                    var url = "https://localhost:7089/api/TaskGroup/GetTaskGroupsByTemplate/" + id;
                    var (jsonData, statusCode) = await FetchDataFromUrlAsync(url);

                    if (statusCode == HttpStatusCode.NotFound || string.IsNullOrEmpty(jsonData))
                    {
                        CardContainer.InnerHtml = "<div class='alert alert-warning'>No task groups found for the given TaskListTemplate.</div>";
                    }
                    else
                    {
                        try
                        {
                            var templateGroupList = JsonConvert.DeserializeObject<List<TemplateGroupModel>>(jsonData);

                            if (templateGroupList == null || !templateGroupList.Any())
                            {
                                CardContainer.InnerHtml = "<div class='alert alert-warning'>No task groups found for the given TaskListTemplate.</div>";
                            }
                            else
                            {
                                templateName.InnerText = templateGroupList.First().taskListTemplate.tempName;

                                CardContainer.InnerHtml = GenerateBootstrapCards(templateGroupList);
                            }
                        }
                        catch (JsonException)
                        {
                            CardContainer.InnerHtml = "<div class='alert alert-danger'>Error parsing data. Please try again later.</div>";
                        }
                    }
                }
            }
            else if (!string.IsNullOrEmpty(Request.Form["deleteGroupId"]))
            {
                string groupId = Request.Form["deleteGroupId"];
                await DeleteGroupAsync(groupId);
                Response.Redirect(Request.Url.AbsolutePath + "?id=" + Request.QueryString["id"]);
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

        private async System.Threading.Tasks.Task DeleteGroupAsync(string groupId)
        {
            var tasksUrl = $"https://localhost:7089/api/Task/GetTasksByTaskGroup/{groupId}";
            var (tasksJson, tasksStatusCode) = await FetchDataFromUrlAsync(tasksUrl);

            if (tasksStatusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(tasksJson))
            {
                try
                {
                    var taskList = JsonConvert.DeserializeObject<List<TaskDTO>>(tasksJson);

                    if (taskList != null && taskList.Any())
                    {
                        foreach (var task in taskList)
                        {
                            var deleteTaskUrl = $"https://localhost:7089/api/Task/DeleteTask/{task.Id}";
                            using (HttpClient client = new HttpClient())
                            {
                                await client.DeleteAsync(deleteTaskUrl);
                            }
                        }
                    }

                    var deleteGroupUrl = $"https://localhost:7089/api/TaskGroup/DeleteTaskGroup/{groupId}";
                    using (HttpClient client = new HttpClient())
                    {
                        await client.DeleteAsync(deleteGroupUrl);
                    }
                }
                catch
                {

                }
            }
        }

        private string GenerateBootstrapCards(IEnumerable<TemplateGroupModel> templateGroups)
        {
            StringBuilder cardHtml = new StringBuilder();

            foreach (var tempGroup in templateGroups)
            {
                cardHtml.Append($@"
                    <div class='card mb-3'>
                        <div class='card-header'>
                            Group Order: {tempGroup.groupOrder}
                        </div>
                        <div class='card-body'>
                            <h5 class='card-title'>Group: {tempGroup.groupName}</h5>
                            <a href='/Task?id={tempGroup.id}' class='btn btn-primary'>View Tasks</a>
                            <button type='button' class='btn btn-danger' onclick='confirmDelete({tempGroup.id})'>Delete</button>
                        </div>
                    </div>");
            }

            return cardHtml.ToString();
        }

        public class TemplateGroupModel
        {
            public int id { get; set; }
            public string groupName { get; set; }
            public int groupOrder { get; set; }
            public TemplateModel taskListTemplate { get; set; }
        }

        public class TemplateModel
        {
            public string tempName { get; set; }
        }

        public class TaskDTO
        {
            public int Id { get; set; }
        }
    }
}
