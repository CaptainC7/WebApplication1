<%@ Page Language="C#" Async="true" AutoEventWireup="true" CodeBehind="InstanceGroup.aspx.cs" Inherits="WebApplication1.InstanceGroup" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Instance Groups</title>
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
    <script src="https://code.jquery.com/jquery-3.5.1.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.9.2/dist/umd/popper.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    <script type="text/javascript">
        function handleAddClick(button) {
            var groupId = $(button).data('groupid');
            var ddl = $(button).siblings('select');
            var selectedValue = ddl.val();
            if (selectedValue === "0") { // Assuming 0 is the value for "Assign to user"
                alert('Please select a user from the dropdown.');
            } else {
                document.getElementById('hiddenGroupId').value = groupId;
                document.getElementById('hiddenAssignedTo').value = selectedValue;
                document.getElementById('btnSave').click();
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container mt-5">
            <h1>Instance Groups</h1>
            <asp:Repeater ID="RepeaterGroups" runat="server">
                <ItemTemplate>
                    <div class="card" style="margin-bottom: 10px;">
                        <div class="card-body">
                            <h5 class="card-title"><%# Eval("groupName") %></h5>
                            <asp:Button ID="btnAdd" runat="server" Text="Add" CommandArgument='<%# Eval("id") %>' OnClick="btnAdd_Click" CssClass="btn btn-primary" />
                            <asp:DropDownList ID="ddlUsers" runat="server" CssClass="form-control" Style="width: 200px;" AutoPostBack="true">
                                <asp:ListItem Text="Assign to user" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:Button ID="btnSave" runat="server" Text="Save" CommandArgument='<%# Eval("id") %>' OnClick="btnSave_Click" CssClass="btn btn-success" Visible="false" />
                            <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>

            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-success" OnClick="btnSave_Click" Style="display:none;" />
            <input type="hidden" id="hiddenGroupId" runat="server" />
            <input type="hidden" id="hiddenAssignedTo" runat="server" />
            <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="false"></asp:Label>
        </div>
    </form>
</body>
</html>
