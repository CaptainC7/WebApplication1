<%@ Page Language="C#" Async="true" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="WebApplication1.Home" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Home Page</title>
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
    <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.9.2/dist/umd/popper.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    <script type="text/javascript">
        function confirmDelete(templateId) {
            if (confirm('Are you sure you want to delete this template? This will also delete all related groups and tasks.')) {
                document.getElementById('deleteTemplateForm').action = '/Home.aspx?handler=DeleteTemplateWithDependencies&templateId=' + templateId;
                document.getElementById('deleteTemplateForm').submit();
            }
        }
    </script>
</head>
<body>
    <div class="container mt-5">
        <h1>Templates</h1>
        <div class="d-flex mb-3 justify-content-between">
            <a href="/CreateTemplate.aspx" class="btn btn-success btn-lg">Create Template</a>
            <a href="/TaskListInstance.aspx" class="btn btn-secondary btn-lg">View Instances</a>
            <a href="/ViewTemplateHistory.aspx" class="btn btn-secondary btn-lg">View Templates History</a>
        </div>
        <div id="TemplateContainer" runat="server"></div>
        <form id="deleteTemplateForm" method="post" runat="server">
            <input type="hidden" name="deleteTemplateId" id="deleteTemplateId" value="" />
        </form>
    </div>
</body>
</html>
