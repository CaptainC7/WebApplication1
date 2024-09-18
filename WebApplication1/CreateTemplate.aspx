<%@ Page Language="C#" Async="true" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreateTemplate.aspx.cs" Inherits="WebApplication1.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-3">
        <div class="d-flex align-items-start">
            <asp:Button ID="btnBack" runat="server" CssClass="btn btn-secondary" Text="Back" OnClick="btnBack_Click" />
        </div>
    </div>

    <div id="templateForm" runat="server" class="mt-4">
        <div class="container">
            <h1>Create a New Template</h1>
            <div class="form-group mb-3">
                <label for="templateName">Template Name</label>
                <asp:TextBox ID="txtTemplateName" runat="server" CssClass="form-control" placeholder="Enter template name"></asp:TextBox>
            </div>
            <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-primary" Text="Submit" OnClick="submitForm" />
        </div>
    </div>
</asp:Content>

