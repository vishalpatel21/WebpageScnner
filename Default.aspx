<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebPageScanner._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h4>Web Page Scanner</h4>        
    </div>
    <div class="row">
        <asp:Label ID="lblMsg" runat="server" Text="" ForeColor="Red"></asp:Label>
        <asp:TextBox ID="txtUrl" runat="server"></asp:TextBox>        
    </div>
    <br />
    <div class="row">
        <asp:Button ID="BtnGetWordCount" runat="server" Text="Click Me To Get Count" OnClick="BtnGetWordCount_Click" />
    </div>
    <br />
    <div class="row">
        <div class="row">
            Top 10 Single Words
        </div>
        <br />
        <div class="row">
            <asp:GridView ID="grdSingleword" runat="server"></asp:GridView>
        </div>
    </div>
    <br />
    <div class="row">
        <div class="row">
            <b>Top 10 Pair Words</b>
        </div>
        <br />
        <div class="row">
            <asp:GridView ID="grdPairword" runat="server"></asp:GridView>
        </div>
    </div>
</asp:Content>
