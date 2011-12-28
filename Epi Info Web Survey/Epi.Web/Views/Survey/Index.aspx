<%@ Page Title="Enter Survey" Language="C#" MasterPageFile="~/Views/Shared/EpiSurvey.Master"
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
	   
	  <div id="exit">
      <button class="exitsurvey" type="submit">Exit Survey</button>
      
      </div>
	  <div style="clear:both;"></div>
	 </div>

	<div id="content">

     <% if (!string.IsNullOrEmpty(Model.GetErrorSummary()))
        {%>
        <div class="errormsg">
            
            <div class="image">
                <img src="../../Content/images/error.png" style="vertical-align: middle; padding-right: 10px;">
            </div>
            <div class="message">
              <span style="font-weight:bold; font-size:10pt;">Please correct the following errors before continuing:</span>
                <br>
               
                <%= Model.GetErrorSummary()%>
            </div>
            <div style="clear: both;">
            </div>

        </div>
        <% }%>
        <%Html.BeginForm(null , null, FormMethod.Post, new { id = "myform" }); %>
      
        <%=Model.RenderHtml(true) %>
        
      
   	</div>
	        <div id="nav">

		        
                  <div id="prev" align="left"><button class="prev"  type="submit" ><img class="button" src="../../Content/images/arrow_left.png"> Previous</button></div>
	 		 <div id="save" align="center"><button class="save" type="submit" onclick="window.location='survey3.html'"><img class="button" src="../../Content/images/save.png"> Save & Close</button></div>
	 		 
          <div id="next" align="right"><button class="submits" type="submit"  name="submit" value="Submit" >
              Submit Survey </button></div> 

            
	        </div>




 <%Html.EndForm(); %>
</asp:Content>
