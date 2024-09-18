using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebApplication1
{
    public partial class ViewGroupHistory : System.Web.UI.Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                await LoadGroupHistoryAsync();
            }
        }

        private async System.Threading.Tasks.Task LoadGroupHistoryAsync()
        {
            string apiUrl = "https://localhost:7089/api/TaskGroupHistory/history";
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    List<GroupHistoryModel> groupHistory = JsonConvert.DeserializeObject<List<GroupHistoryModel>>(jsonResponse);

                    foreach (var history in groupHistory)
                    {
                        string row = $@"
                            <tr>
                                <td>{history.GroupName}</td>
                                <td>{history.ChangedByPerson.FName} {history.ChangedByPerson.LName}</td>
                                <td>{history.ChangedDate.ToString("yyyy-MM-dd")}</td>
                                <td>{history.ChangedType}</td>
                            </tr>";

                        GroupHistoryTableBody.InnerHtml += row;
                    }
                }
            }
        }
    }

    public class GroupHistoryModel
    {
        public string GroupName { get; set; }
        public PersonModel ChangedByPerson { get; set; }
        public DateTime ChangedDate { get; set; }
        public string ChangedType { get; set; }
    }

    public class PersonModel
    {
        public string FName { get; set; }
        public string LName { get; set; }
    }
}
