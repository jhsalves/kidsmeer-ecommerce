<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminPage.aspx.cs" Inherits="Kidsmeer.Admin.AdminPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Administration</h1>
    <hr />
    <h3>Add Product:</h3>
    <table>
        <tr>
            <td><asp:Label ID="LabelAddCategory" runat="server">Category:</asp:Label></td>
            <td>
                <asp:DropDownList ID="DropDownAddCategory" runat="server"
                    ItemType="Kidsmeer.Models.Category"
                    SelectMethod="GetCategories" DataTextField="CategoryName"
                    DataValueField="CategoryID"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td><asp:Label ID="LabelAddName" runat="server">Name:</asp:Label></td>
            <td>
              <asp:TextBox runat="server" ID="AddProductName"></asp:TextBox>
              <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="RequiredFieldValidator" ControlToValidate="AddProductName" Display="Dynamic" SetFocusOnError="True">* Product name required</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td><asp:Label ID="LabelAddDescription" runat="server">Description:</asp:Label></td>
            <td>
                <asp:TextBox ID="AddProductDescription" runat="server"></asp:TextBox>

                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="AddProductDescription" Display="Dynamic" ErrorMessage="RequiredFieldValidator" SetFocusOnError="True">* Description required.</asp:RequiredFieldValidator>

            </td>
        </tr>
        <tr>
            <td><asp:Label ID="LabelAddPrice" runat="server">Price:</asp:Label></td>
            <td>
            
            
            
                <asp:TextBox ID="AddProductPrice" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="AddProductPrice" Display="Dynamic" ErrorMessage="RequiredFieldValidator" SetFocusOnError="True">* Price required</asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="AddProductPrice" Display="Dynamic" ErrorMessage="RegularExpressionValidator" SetFocusOnError="True" ValidationExpression="^[0-9]*(\.)?[0-9]?[0-9]?$">* Must be a valid price</asp:RegularExpressionValidator>
            
            
            
            </td>
        </tr>
        <tr>
            <td><asp:Label ID="LabelAddImageFile" runat="server">Image File:</asp:Label></td>
            <td>
             
                <asp:FileUpload ID="ProductImage" runat="server" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ProductImage" Display="Dynamic" ErrorMessage="RequiredFieldValidator" SetFocusOnError="True">* Image path is required</asp:RequiredFieldValidator>
             
            </td>
        </tr>
    </table>
    <p></p>
    <p></p>
    <asp:Button ID="AddProductButton" runat="server" Text="Add Product" OnClick="AddProductButton_Click" CausesValidation="true"/>
    <asp:Label ID="LabelAddStatus" runat="server" Text=""></asp:Label>
    <p></p>
    <h3>Remove Product:</h3>
    <table>
        <tr>
            <td><asp:Label ID="LabelRemoveProduct" runat="server">Product:</asp:Label></td>
            <td>
                <asp:DropDownList ID="DropDownRemoveProduct" runat="server" AppendDataBoundItems="True" DataTextField="ProductName" DataValueField="ProductID" ItemType="Kidsmeer.Models.Product" SelectMethod="GetProducts"></asp:DropDownList>
            </td>
        </tr>
    </table>
    <p></p>
    <asp:Button ID="RemoveProductButton" runat="server" Text="Remove Product" OnClick="RemoveProductButton_Click" CausesValidation="false"/>
    <asp:Label ID="LabelRemoveStatus" runat="server" Text=""></asp:Label>
</asp:Content>
