<%@ Page Language="C#" Async="true" AutoEventWireup="true" CodeBehind="ViewTemplateHistory.aspx.cs" Inherits="WebApplication1.ViewTemplateHistory" %>

<!DOCTYPE html>
<html>
<head>
    <title>Template History</title>
    <link rel="stylesheet" type="text/css" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css">
</head>
<body>
    <div class="container">
        <h1 class="mt-4">Template History</h1>
        <div class="table-responsive mt-4">
            <table class="table table-bordered">
                <thead>
                    <tr>
                        <th>Template Name</th>
                        <th>Created By</th>
                        <th>Created Date</th>
                        <th>Changed By</th>
                        <th>Changed Date</th>
                        <th>Change Type</th>
                    </tr>
                </thead>
                <tbody id="TemplateHistoryTableBody" runat="server">

                </tbody>
            </table>
        </div>
    </div>
</body>
</html>
