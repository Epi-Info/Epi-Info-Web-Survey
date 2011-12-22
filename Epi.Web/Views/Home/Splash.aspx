<%@ Page Title="Survey Start" Language="C#" MasterPageFile="~/Views/Shared/EpiSurvey.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Index
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div id="infobox">

    </div>
    <div>
     <b><%: ViewData["Message"] %></b> <br />
    </div>
    <div id="content">
        <h2>
            Welcome to Epi Info Survey. This is the 2nd sprint.</h2>
        <p>
        </p>
       
    </div>
</asp:Content>

