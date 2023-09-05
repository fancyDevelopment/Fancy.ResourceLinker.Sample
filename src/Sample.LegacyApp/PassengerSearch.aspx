<%@ Page Title="Passenger Search" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PassengerSearch.aspx.cs" Inherits="Sample.LegacyApp.PassengerSearch" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main aria-labelledby="title">
        <h2 id="title"><%: Title %>.</h2>
        <h3>Just a dummy page for passenger search.</h3>
        <p>This site comes from the legay application.</p>
        <p>You are currently logged in as: <%: Context.User.Identity.IsAuthenticated ? Context.User.Identity.GetUserName() : "Anonymous"  %></p>
    </main>
</asp:Content>
