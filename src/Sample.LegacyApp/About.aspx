<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="Sample.LegacyApp.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main aria-labelledby="title">
        <h2 id="title"><%: Title %>.</h2>
        <h3>Legacy version of the Flight42 Angular application.</h3>
        <p>
            This application dummy presents a legacy version of the Angular Flight42 application to demonstrate
            how you can integrate a legay application with an angular application using Fancy.ResourceLinkler.Gateway library.
        </p>
    </main>
</asp:Content>
