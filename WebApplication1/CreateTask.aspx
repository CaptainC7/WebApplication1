<%@ Page Title="Create Task" Async="true" Language="C#" AutoEventWireup="true" CodeBehind="CreateTask.aspx.cs" Inherits="WebApplication1.CreateTask" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Create Task</title>
    <link rel="stylesheet" type="text/css" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h1>Create New Task</h1>
            <div class="form-group">
                <label for="txtTaskName">Task Name</label>
                <asp:TextBox ID="txtTaskName" runat="server" CssClass="form-control" />
                <asp:Label ID="lblGroupId" runat="server" Text="GroupID will be displayed here"></asp:Label>
            </div>
            <div class="form-group">
                <label for="txtDescription">Description</label>
                <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" />
            </div>
            <div class="form-group">
                <label for="ddlDependencyTasks">Dependency Task</label>
                <asp:DropDownList ID="ddlDependencyTasks" runat="server" CssClass="form-control">
                    <asp:ListItem Text="Select a task" Value="" />
                </asp:DropDownList>
            </div>
            <asp:Button ID="btnSubmit" runat="server" Text="Create Task" CssClass="btn btn-primary" OnClick="SubmitForm" />
        </div>
    </form>
</body>
</html>
