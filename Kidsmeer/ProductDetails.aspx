<%@ Page Title="Product Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProductDetails.aspx.cs" Inherits="Kidsmeer.ProductDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:FormView ID="productDetails" runat="server" ItemType="Kidsmeer.Models.Product" RenderOuterTable="False" SelectMethod="GetProduct">
        <ItemTemplate>
            <div>
                <h1><%#:Item.ProductName %></h1>
            </div>
            <br />
            <table>
                <tr>
                    <td>
                        <img src="/Catalog/Images/<%#:Item.ImagePath %>" style="border:solid" height="300" alt="<%#:Item.ProductName %>" />
                    </td>
                    <td>&nbsp</td>
                    <td style="vertical-align:top;text-align:left;">
                        <b>Description:</b><br /><%#: Item.Description %>
                        <br />
                        <span><b>Price:</b>&nbsp;<%#: String.Format("{0:c}",Item.UnitPrice) %></span>
                        <br />
                        <span><b>Product Number:</b>&nbsp;<%#: Item.ProductID %></span>
                    </td>
                </tr>
            </table>
        </ItemTemplate>
    </asp:FormView>
</asp:Content>