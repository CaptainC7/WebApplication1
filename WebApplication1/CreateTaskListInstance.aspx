<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateTaskListInstance.aspx.cs" Inherits="WebApplication1.CreateTaskListInstance" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Create Task List Instance</title>
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container mt-5">
            <h1>Create Task List Instance</h1>

            <div class="form-group">
                <label for="templateName">Template Name</label>
                <asp:TextBox ID="txtTemplateName" CssClass="form-control" ReadOnly="true" runat="server" />
            </div>

            <div class="form-group">
                <label for="startDate">Start Date and Time</label>
                <asp:TextBox ID="txtStartDate" CssClass="form-control" runat="server" TextMode="DateTimeLocal" />
            </div>

            <div class="form-group">
                <label for="dueDate">Due Date and Time</label>
                <asp:TextBox ID="txtDueDate" CssClass="form-control" runat="server" TextMode="DateTimeLocal" />
            </div>

            <div class="form-group">
                <label for="ddlUsers">Assigned To</label>
                <asp:DropDownList ID="ddlUsers" CssClass="form-control" runat="server"></asp:DropDownList>
            </div>

            <div class="form-group">
                <asp:Button ID="btnCreate" CssClass="btn btn-primary" runat="server" Text="Create" OnClick="btnCreate_Click" />
            </div>
        </div>
    </form>

    <script src="https://code.jquery.com/jquery-3.5.1.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.bundle.min.js"></script>
</body>
</html>
