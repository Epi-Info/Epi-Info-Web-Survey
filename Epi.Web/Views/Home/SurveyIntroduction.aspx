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
            <%: Model.SurveyName %></h1>
    </div>
    <div id="infobox">
        <div id="surveyno">
            <span class="boldlabel">Survey #:</span> <span>
                <%: Model.SurveyNumber%></span></div>
        <div id="orgn">
            <span class="boldlabel">Organization:</span> <span>
                <%: Model.OrganizationName%></span></div>
        <div id="dept">
            <span class="boldlabel">Department:</span> <span>
                <%: Model.DepartmentName%></span></div>
        <div style="clear: both;">
        </div>
    </div>
    <div id="content">
       
            
             <% if (Model.ClosingDate.Date >= DateTime.Now.Date)
       {%>
       <h2>Welcome!</h2><br>
        <div>
            
                <%: Model.IntroductionText%>
            <br />
        </div>
        
        <p>
        </p>
        <p>
            <button id="beginsurvey" class="begin green" type="submit">
                Begin Survey</button>
        </p>
        <%}else{ %>
        <div>
        </div>
        <div id="attention">
            <img src="../Content/images/sign_warning.png" style="vertical-align: middle; padding-right: 10px;">
            This survey is currently closed. Please contact the author of this survey for further
            assistance.
        </div>
        <% }%>
    </div>
    <% } %>
</asp:Content>
