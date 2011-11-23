<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/EpiSurvey.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Index
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="infobox">
        <div id="surveyno">
            <span class="boldlabel">Survey #:</span> <span>XXXX</span></div>
        <div id="orgn">
            <span class="boldlabel">Organization:</span> <span>OrganizationName</span></div>
        <div id="dept">
            <span class="boldlabel">Department:</span> <span>DepartmentName</span></div>
        <div style="clear: both;">
        </div>
    </div>
    <div>
     <b><%: ViewData["Message"] %></b> <br />
    </div>
    <div id="content">
        <h2>
            Welcome</h2>
        <p>
        </p>
        <p>
            <button id="beginsurvey" class="begin green" type="submit" onclick="window.location.href='survey.html'">
                Begin Survey</button></p>
    </div>
</asp:Content>
