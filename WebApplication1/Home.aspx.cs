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
    public partial class Home : Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["handler"] == "DeleteTemplateWithDependencies" && Request.QueryString["templateId"] != null)
            {
                string templateId = Request.QueryString["templateId"];
                await DeleteTemplateWithDependenciesAsync(templateId);

                Response.ContentType = "application/json";
                Response.Write("{\"success\": true}");
                Response.End();
            }
            else
            {
                if (!IsPostBack)
                {
                    var url = "https://localhost:7089/api/TaskListTemplate/GetAllTemplates";
                    var (jsonData, statusCode) = await FetchDataFromUrlAsync(url);

                    if (statusCode == HttpStatusCode.NotFound || string.IsNullOrEmpty(jsonData))
                    {
                        TemplateContainer.InnerHtml = "<div class='alert alert-warning'>No templates found.</div>";
                    }
                    else
                    {
                        try
                        {
                            var templateList = JsonConvert.DeserializeObject<List<TemplateModel>>(jsonData);

                            if (templateList == null || !templateList.Any())
                            {
                                TemplateContainer.InnerHtml = "<div class='alert alert-warning'>No templates found.</div>";
                            }
                            else
                            {
                                TemplateContainer.InnerHtml = GenerateTemplateCards(templateList);
                            }
                        }
                        catch (JsonException)
                        {
                            TemplateContainer.InnerHtml = "<div class='alert alert-danger'>Error parsing data. Please try again later.</div>";
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(Request.Form["deleteTemplateId"]))
                {
                    string templateId = Request.Form["deleteTemplateId"];
                    await DeleteTemplateAsync(templateId);
                    Response.Redirect(Request.Url.AbsolutePath + "?id=" + Request.QueryString["id"]);
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

        private string GenerateTemplateCards(List<TemplateModel> templates)
        {
            StringBuilder cardHtml = new StringBuilder();

            foreach (var template in templates)
            {
                cardHtml.Append($@"
            <div class='card mb-3' id='template-{template.id}'>
                <div class='card-header'>
                    Template Name: {template.tempName}
                </div>
                <div class='card-body'>
                    <h5 class='card-title'>Created Date: {template.createdDate}</h5>
                    <p class='card-text'>Created By: {template.createdByPerson.fName} {template.createdByPerson.lName}</p>
                    <a href='/TemplateGroup.aspx?id={template.id}' class='btn btn-primary'>View Groups</a>
                    <a href='/UpdateTemplate.aspx?id={template.id}&name={template.tempName}' class='btn btn-warning'>Edit</a>
                    <button type='button' class='btn btn-danger' onclick='confirmDelete({template.id})'>Delete</button>
                    <a href='/CreateTaskListInstance.aspx?id={template.id}' class='btn btn-primary'>Create Instance</a>
                </div>
            </div>");
            }

            return cardHtml.ToString();
        }

        private async System.Threading.Tasks.Task DeleteTemplateWithDependenciesAsync(string templateId)
        {
            var groupResponse = await GetGroupsByTemplateIdAsync(templateId);

            if (groupResponse != null && groupResponse.Any())
            {
                foreach (var group in groupResponse)
                {
                    var taskResponse = await GetTasksByGroupIdAsync(group.id.ToString());

                    if (taskResponse != null && taskResponse.Any())
                    {
                        foreach (var task in taskResponse)
                        {
                            await DeleteTaskAsync(task.id.ToString());
                        }
                    }

                    await DeleteGroupAsync(group.id.ToString());
                }
            }

            await DeleteTemplateAsync(templateId);
        }

        //private async Task<List<Group>> GetGroupsByTemplateIdAsync(string templateId)
        //{
        //    var groupUrl = $"https://localhost:7089/api/TaskGroup/GetTaskGroupsByTemplate/{templateId}";
        //    using (HttpClient client = new HttpClient())
        //    {
        //        var response = await client.GetStringAsync(groupUrl);
        //        return JsonConvert.DeserializeObject<List<Group>>(response);
        //    }
        //}

        private async Task<List<Group>> GetGroupsByTemplateIdAsync(string templateId)
        {
            var groupUrl = $"https://localhost:7089/api/TaskGroup/GetTaskGroupsByTemplate/{templateId}";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var response = await client.GetStringAsync(groupUrl);

                    // Check if response is empty or contains no valid tasks
                    if (string.IsNullOrEmpty(response))
                    {
                        return null; // Return null if there's no data
                    }

                    var tasks = JsonConvert.DeserializeObject<List<Group>>(response);

                    // Check if deserialization returns null or an empty list
                    if (tasks == null || tasks.Count == 0)
                    {
                        return null; // Return null if no tasks are found
                    }

                    return tasks;
                }
                catch (Exception ex)
                {
                    // Log or handle the exception as needed
                    return null; // Return null in case of an error
                }
            }
        }


        //private async Task<List<Task>> GetTasksByGroupIdAsync(string groupId)
        //{
        //    var taskUrl = $"https://localhost:7089/api/Task/GetTasksByTaskGroup/{groupId}";
        //    using (HttpClient client = new HttpClient())
        //    {
        //        var response = await client.GetStringAsync(taskUrl);
        //        return JsonConvert.DeserializeObject<List<Task>>(response);
        //    }
        //}

        private async Task<List<Task>> GetTasksByGroupIdAsync(string groupId)
        {
            var taskUrl = $"https://localhost:7089/api/Task/GetTasksByTaskGroup/{groupId}";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var response = await client.GetStringAsync(taskUrl);

                    if (string.IsNullOrEmpty(response))
                    {
                        return null;
                    }

                    var tasks = JsonConvert.DeserializeObject<List<Task>>(response);

                    if (tasks == null || tasks.Count == 0)
                    {
                        return null;
                    }

                    return tasks;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }


        private async System.Threading.Tasks.Task DeleteTaskAsync(string taskId)
        {
            var deleteTaskUrl = $"https://localhost:7089/api/Task/DeleteTask/{taskId}";

            using (HttpClient client = new HttpClient())
            {
                var payload = new
                {
                    userID = 1
                };

                var jsonPayload = JsonConvert.SerializeObject(payload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                var response = await client.DeleteAsync(deleteTaskUrl);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Failed to delete task with ID: " + taskId);
                }
            }
        }

        private async System.Threading.Tasks.Task DeleteGroupAsync(string groupId)
        {
            var deleteGroupUrl = $"https://localhost:7089/api/TaskGroup/DeleteTaskGroup/{groupId}";

            using (HttpClient client = new HttpClient())
            {
                var payload = new
                {
                    userID = 1
                };

                var jsonPayload = JsonConvert.SerializeObject(payload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                var response = await client.DeleteAsync(deleteGroupUrl);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Failed to delete group with ID: " + groupId);
                }
            }
        }

        private async System.Threading.Tasks.Task DeleteTemplateAsync(string templateId)
        {
            var deleteTemplateUrl = $"https://localhost:7089/api/TaskListTemplate/DeleteTemplate/{templateId}";

            using (HttpClient client = new HttpClient())
            {
                var payload = new
                {
                    userID = 1
                };

                var jsonPayload = JsonConvert.SerializeObject(payload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                var response = await client.DeleteAsync(deleteTemplateUrl);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Failed to delete template with ID: " + templateId);
                }
            }
        }

        public class TemplateModel
        {
            public int id { get; set; }
            public string tempName { get; set; }
            public DateTime createdDate { get; set; }
            public CreatedByPerson createdByPerson { get; set; }
        }

        public class CreatedByPerson
        {
            public string fName { get; set; }
            public string lName { get; set; }
        }

        public class Group
        {
            public int id { get; set; }
            public string groupName { get; set; }
            public int taskListTemplateID { get; set; }
            public int groupOrder { get; set; }
        }

        public class Task
        {
            public int id { get; set; }
            public string taskName { get; set; }
            public string description { get; set; }
            public int taskGroupID { get; set; }
            public int taskOrder { get; set; }
            public int? dependancyTaskID { get; set; }
        }
    }
}
