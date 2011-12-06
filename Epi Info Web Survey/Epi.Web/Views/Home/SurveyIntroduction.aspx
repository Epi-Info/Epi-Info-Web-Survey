<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/EpiSurvey.Master"
    Inherits="System.Web.Mvc.ViewPage<Epi.Web.Models.SurveyInfoModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Index
</asp:Content>

<%--<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    
      
</asp:Content>--%>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   
    <% using (Html.BeginForm())
      { %>
     <div id="header">
        
            <h1>
                Survey Name- <%: Model.SurveyName %></h1>
        </div>

    <div id="infobox">
        <div id="surveyno">
            <span class="boldlabel">Survey #:</span> <span><%: Model.SurveyNumber%></span></div>
        <div id="orgn">
            <span class="boldlabel">Organization:</span> <span><%: Model.OrganizationName%></span></div>
        <div id="dept">
            <span class="boldlabel">Department:</span> <span><%: Model.DepartmentName%></span></div>
        <div style="clear: both;">
        </div>
    </div>
    <div>
     <b><%: Model.IntroductionText%></b> <br />
    </div>
    <div id="content">
        <h2>
            Welcome</h2>
        <p>
        </p>
        <p>
            <button id="beginsurvey" class="begin green" type="submit">
                Begin Survey</button>
        </p>
    </div>
    <% } %>
</asp:Content>
