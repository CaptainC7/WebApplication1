<%@ Page Language="C#" Async="true" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreateGroup.aspx.cs" Inherits="WebApplication1.CreateGroup" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div id="templateForm" runat="server">
        <div class="container mt-4">
            <h1>Create a New Group</h1>
            <div class="form-group mb-3">
                <label for="groupName">Group Name</label>
                <asp:TextBox ID="txtGroupName" runat="server" CssClass="form-control" placeholder="Enter Group name"></asp:TextBox>
            </div>
            <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-primary" Text="Submit" OnClick="submitForm" />
        </div>
    </div>
</asp:Content>
