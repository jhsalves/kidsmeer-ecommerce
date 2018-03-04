﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CheckoutReview.aspx.cs" Inherits="Kidsmeer.Checkout.CheckoutReview" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Order Review</h1>
    <p></p>
    <h3></h3>
    <asp:GridView ID="OrderItemList" runat="server" AutoGenerateColumns="false" CellPadding="10" Width="500px" BorderColor="#efeeef" BorderWidth="33px">
        <Columns>
            <asp:BoundField DataField="ProductId" HeaderText="Product ID"></asp:BoundField>
            <asp:BoundField DataField="Product.ProductName" HeaderText="Product Name"></asp:BoundField>
            <asp:BoundField HeaderText="Price (each)" DataField="Product.UnitPrice" DataFormatString="{0:c}"></asp:BoundField>
            <asp:BoundField DataField="Quantity" HeaderText="Quantity"></asp:BoundField>
        </Columns>
    </asp:GridView>
    <asp:DetailsView ID="ShipInfo" runat="server" AutoGenerateRows="False" GridLines="None" CellPadding="10" BorderStyle="None" CommandRowStyle-BorderStyle="None">

        <Fields>
            <asp:TemplateField>
                <ItemTemplate>
                    <h3>Shipping Adress:</h3>
                    <br />
                    <asp:Label ID="FirstName" runat="server" Text='<%#: Eval("FirstName") %>'></asp:Label>
                    <asp:Label ID="LastName" runat="server" Text='<%#: Eval("LastName") %>'></asp:Label>
                    <br />
                    <asp:Label ID="Address" runat="server" Text='<%#: Eval("Address") %>'></asp:Label>
                    <br />
                    <asp:Label ID="City" runat="server" Text='<%#: Eval("City") %>'></asp:Label>
                    <asp:Label ID="State" runat="server" Text='<%#: Eval("State") %>'></asp:Label>
                    <asp:Label ID="PostalCode" runat="server" Text='<%#: Eval("PostalCode") %>'></asp:Label>
                    <p></p>
                    <h3>Order Total:</h3>
                    <br />
                    <asp:Label ID="Total" runat="server" Text='<%#: Eval("Total", "{0:C}") %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>
        </Fields>
    </asp:DetailsView>
    <p></p>
    <hr />
    <asp:Button ID="CheckoutConfirm" runat="server" Text="Complete Order" OnClick="CheckoutConfirm_Click" />
</asp:Content>
