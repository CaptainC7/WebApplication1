<%@ Page Language="C#" Async="true" AutoEventWireup="true" CodeBehind="ViewTaskHistory.aspx.cs" Inherits="WebApplication1.ViewTaskHistory" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>View Task History</title>
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <div class="container mt-5">
        <h2>Task History</h2>
        <table class="table table-bordered">
            <thead>
                <tr>
                    <th>Task Name</th>
                    <th>Description</th>
                    <th>Task Group Name</th>
                    <th>Template Name</th>
                    <th>Changed By</th>
                    <th>Changed Date</th>
                    <th>Change Type</th>
                    <th>Dependency Task Name</th>
                </tr>
            </thead>
            <tbody id="taskHistoryTableBody" runat="server">

            </tbody>
        </table>
    </div>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
</body>
</html>

