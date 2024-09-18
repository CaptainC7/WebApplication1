<%@ Page Language="C#" Async="true" AutoEventWireup="true" CodeBehind="TemplateGroup.aspx.cs" Inherits="WebApplication1.TemplateGroup" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Template Groups</title>
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
    <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.9.2/dist/umd/popper.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    <script type="text/javascript">
        function confirmDelete(groupId) {
            if (confirm('Are you sure you want to delete this group? This will also delete all associated tasks.')) {
                document.getElementById('deleteGroupId').value = groupId;
                document.getElementById('deleteGroupForm').submit();
            }
        }
    </script>
</head>
<body>
    <div class="container mt-4">
        <h1>Template Groups</h1>
        <div class="d-flex mb-3 justify-content-between">
            <a id="btnCreateGroup" runat="server" class="btn btn-success btn-lg">Create Group</a>
            <a href="ViewGroupHistory.aspx" id="btnViewGroupHistory" runat="server" class="btn btn-secondary btn-lg">View Group History</a>
        </div>
        <div>
            <h3 id="templateName" runat="server"></h3>
        </div>
        <div id="CardContainer" runat="server"></div>
        <form id="deleteGroupForm" method="post" runat="server">
            <input type="hidden" name="deleteGroupId" id="deleteGroupId" value="" />
        </form>
    </div>
</body>
</html>
