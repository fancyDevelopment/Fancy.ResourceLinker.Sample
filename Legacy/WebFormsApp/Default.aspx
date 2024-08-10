<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebFormsApp._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <section class="row" aria-labelledby="aspnetTitle">
            <h1 id="aspnetTitle">Flight42 Legacy</h1>
            <p class="lead">This is the legacy version of the best flight search experience on the planet!</p>
            <p><a runat="server" href="~/Pages/FlightSearch" class="btn btn-primary btn-md">Go to Flight Search &raquo;</a></p>
        </section>

        <hr />

        <div class="row">
            <section class="col-md-4">
                <h5>Flight Count</h5>
                <h1>
                    26
                </h1>
            </section>
            <section class="col-md-4">
                <h5>Departures Count</h5>
                <h1>
                    5
                </h1>
            </section>
            <section class="col-md-4">
                <h5>Destinations Count</h5>
                <h1>
                    8
                </h1>
            </section>
        </div>
    </main>

</asp:Content>
