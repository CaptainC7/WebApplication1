<%@ Page Language="C#" Async="true" AutoEventWireup="true" CodeBehind="ViewGroupHistory.aspx.cs" Inherits="WebApplication1.ViewGroupHistory" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Group History</title>
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container" runat="server">
            <h2>Task Group History</h2>
            <table class="table table-bordered">
                <thead>
                    <tr>
                        <th>Group Name</th>
                        <th>Changed By</th>
                        <th>Changed Date</th>
                        <th>Change Type</th>
                    </tr>
                </thead>
                <tbody id="GroupHistoryTableBody" runat="server">
                </tbody>
            </table>
        </div>
    </form>
    <script src="https://code.jquery.com/jquery-3.4.1.min.js"></script>
</body>
</html>
