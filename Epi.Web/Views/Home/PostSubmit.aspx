<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/EpiSurvey.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    PostSubmit
</asp:Content>


 <asp:Content ID="Content5" ContentPlaceHolderID="HeaderContent" runat="server">
     <div id="header">
            <h1>
                <%: Model.SurveyName %>
                </h1>
        </div>
</asp:Content>
 
 <asp:Content ID="Content6" ContentPlaceHolderID="MainContent" runat="server">

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
     <div class="success">
 <img src="../../Content/images/button_check.png" style="vertical-align:middle; padding-right: 10px;"> Thank you! Your survey has been submitted.
 </div>  
 </asp:Content> 
 
 
 
 