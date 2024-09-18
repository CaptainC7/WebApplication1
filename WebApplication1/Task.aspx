<%@ Page Title="Task" Async="true" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Task.aspx.cs" Inherits="WebApplication1.Task" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        <div class="d-flex mb-3 justify-content-between">
            <a href="/CreateTask" id="btnCreateTask" class="btn btn-success btn-lg ml-auto" role="button" runat="server">Create Task</a>
            <a href="/ViewTaskHistory" id="A1" class="btn btn-secondary btn-lg ml-auto" role="button" runat="server">View Task History</a>
        </div>
        <h1 id="groupName" runat="server" class="mb-2"></h1>
        <asp:Literal ID="TaskContainer" runat="server"></asp:Literal>
    </div>
    <script type="text/javascript">
        function confirmDelete(taskId) {
            if (confirm('Are you sure you want to delete this task?')) {
                __doPostBack('DeleteTask', taskId);
            }
        }
    </script>
</asp:Content>
