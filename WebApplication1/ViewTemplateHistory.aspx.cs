using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.UI;

namespace WebApplication1
{
    public partial class ViewTemplateHistory : Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                await LoadTemplateHistoryAsync();
            }
        }

        private async System.Threading.Tasks.Task LoadTemplateHistoryAsync()
        {
            var url = "https://localhost:7089/api/TaskListTemplateHistory";
            var templateHistories = await FetchDataFromUrlAsync(url);

            if (templateHistories == null || templateHistories.Count == 0)
            {
                TemplateHistoryTableBody.InnerHtml = "<tr><td colspan='6'>No history found.</td></tr>";
                return;
            }

            TemplateHistoryTableBody.InnerHtml = GenerateTableRows(templateHistories);
        }

        private async Task<List<TemplateHistoryModel>> FetchDataFromUrlAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(url);
                    var jsonData = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<TemplateHistoryModel>>(jsonData);
                }
                catch
                {
                    return new List<TemplateHistoryModel>();
                }
            }
        }

        private string GenerateTableRows(List<TemplateHistoryModel> templateHistories)
        {
            var rows = new List<string>();

            foreach (var history in templateHistories)
            {
                rows.Add($@"
                <tr>
                    <td>{history.TempName}</td>
                    <td>{history.CreatedByPerson.FName} {history.CreatedByPerson.LName}</td>
                    <td>{history.CreatedDate}</td>
                    <td>{history.ChangedByPerson.FName} {history.ChangedByPerson.LName}</td>
                    <td>{history.ChangedDate}</td>
                    <td>{history.ChangedType}</td>
                </tr>");
            }

            return string.Join(Environment.NewLine, rows);
        }

        public class TemplateHistoryModel
        {
            public string TempName { get; set; }
            public Person CreatedByPerson { get; set; }
            public DateTime CreatedDate { get; set; }
            public Person ChangedByPerson { get; set; }
            public DateTime ChangedDate { get; set; }
            public string ChangedType { get; set; }
        }

        public class Person
        {
            public string FName { get; set; }
            public string LName { get; set; }
        }
    }
}
