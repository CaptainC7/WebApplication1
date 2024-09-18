using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.UI;

namespace WebApplication1
{
    public partial class TaskListInstance : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadTaskListInstances();
            }
        }

        private async void LoadTaskListInstances()
        {
            try
            {
                List<TaskListInstanceDTO> instances = await FetchTaskListInstances();
                if (instances != null)
                {
                    foreach (var instance in instances)
                    {
                        // Creating card for each instance
                        string cardHtml = $@"
                            <div class='card'>
                                <div class='card-body'>
                                    <h5 class='card-title'>Template Name: {instance.TaskListTemplate.TempName}</h5>
                                    <p class='card-text'>Start Date: {instance.StartDate.ToString("yyyy-MM-dd HH:mm")}</p>
                                    <p class='card-text'>Due Date: {instance.DueDate.ToString("yyyy-MM-dd HH:mm")}</p>
                                    <p class='card-text'>Assigned To: {instance.AssignedPerson.FName} {instance.AssignedPerson.LName}</p>
                                    <p class='card-text'>Status: {instance.Status}</p>
                                    <a href='InstanceGroup.aspx?instanceId={instance.Id}&templateId={instance.TaskListTemplateID}' class='btn btn-primary'>View Instance Groups</a>
                                    <button class='btn btn-danger' onclick='deleteInstance({instance.Id})'>Delete</button>
                                </div>
                            </div>";

                        // Adding the card to the container
                        CardContainer.Controls.Add(new LiteralControl(cardHtml));
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle error (optional: display error message to the user)
            }
        }

        private async Task<List<TaskListInstanceDTO>> FetchTaskListInstances()
        {
            string apiUrl = "https://localhost:7089/TaskListInstance";
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<List<TaskListInstanceDTO>>();
                }
                else
                {
                    return null;
                }
            }
        }

        // JavaScript function for delete confirmation
        protected string RegisterDeleteScript()
        {
            return @"
                <script type='text/javascript'>
                    function deleteInstance(instanceId) {
                        if (confirm('Are you sure you want to delete this instance?')) {
                            document.getElementById('deleteInstanceId').value = instanceId;
                            document.getElementById('deleteForm').submit();
                        }
                    }
                </script>
            ";
        }
    }

    // DTOs
    public class TaskListInstanceDTO
    {
        public int Id { get; set; }
        public int TaskListTemplateID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; }
        public TaskListTemplateDTO TaskListTemplate { get; set; }
        public PersonDTO AssignedPerson { get; set; }
    }

    public class TaskListTemplateDTO
    {
        public string TempName { get; set; }
    }

    public class PersonDTO
    {
        public string FName { get; set; }
        public string LName { get; set; }
    }
}
