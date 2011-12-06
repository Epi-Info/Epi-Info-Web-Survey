<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/EpiSurvey.Master"
    Inherits="System.Web.Mvc.ViewPage<MvcDynamicForms.Form>" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Index
</asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
     <div id="header">
            <h1>
                Survey Name- <%:Model.SurveyInfo.SurveyName %>
                </h1>
        </div>
</asp:Content>
 
 <asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
     
    <div id="infobox">
        <div class="pages">
		<span class="nextprev"><img src="/Content/images/prev_d.png" height="16px" style="vertical-align:text-top;" alt="Previous Page"></span>
		<span class="current">1</span>
		<a href="" title="Go to page 2">2</a>
		<a href="" title="Go to page 3">3</a>
		<a href="" title="Go to page 4">4</a>
		<a href="" title="Go to page 5">5</a>
		<a href="" title="Go to page 6">6</a>
		<a href="" title="Go to page 7">7</a>
		<a href="" title="Go to page 8">8</a>
		<a href="/page9" title="Go to page 9">9</a>
		<a href="/page10" title="Go to page 10">10</a>
	    <span>&#8230;</span><a href="/page230" title="Go to page 15">15</a>
		<a href="/page231" title="Go to page 20">20</a>
		<a href="/page2" class="nextprev" title="Go to Next Page"><img src="/Content/images/next.png" height="16px" style="vertical-align: text-top; text-decoration:none;" border="0" alt="Next Page"></a>
	  </div>
	   
	  <div id="exit"><button class="exitsurvey" type="submit">Exit Survey</button></div>
	  <div style="clear:both;"></div>
	 </div>

	<div id="content">
		<%--Lable--%>
        <ul>
           <%-- <li>SurveyId: <%: (ViewData.Model).SurveyId %></li>
            <li>SurveyName: <%: (ViewData.Model).SurveyName %></li>
            <li>SurveyNumber: <%: (ViewData.Model).SurveyNumber%></li>
            <li>OrganiztionName: <%: (ViewData.Model).OrganizationName%></li>
            <li>DepartmentName: <%: (ViewData.Model).DepartmentName%></li>
            <li>IntroductionText: <%: (ViewData.Model).IntroductionText %></li>
            <li>XML: <%: (ViewData.Model).XML%></li>
            <li>IsSuccess: <%: (ViewData.Model).IsSuccess%></li>--%>
        </ul>
       <%-- <div class="MvcDynamicForm" style="width:893px; height:1200px;display:block; border:1px solid white; position:relative;">--%>

        <%Html.BeginForm(); %>
        <%=Model.RenderHtml(true) %>
       <%-- </div>--%>
   
	        <div id="nav">
		         <div id="prev" align="left">&nbsp;</div>
 		         <div id="save" align="center"><button class="save" type="submit"><img class="button" src="/Content/images/save.png" alt=""> Save & Close</button></div>
 		         <div id="next" align="right"><button class="next" type="submit" onclick="window.location.href='survey2.html'">Continue <img class="button" src="/Content/images/arrow_right.png" alt=""></button></div>
	        </div>
	</div>
  
    
    
 
</asp:Content>