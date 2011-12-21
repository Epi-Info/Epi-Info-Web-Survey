<%@ Page Title="Survey Error" Language="C#" MasterPageFile="~/Views/Shared/EpiSurvey.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Exception
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="attention" style="margin: 10px">
            <img src="../Content/images/sign_warning.png" alt="" style="vertical-align: middle;
                padding-right: 10px;">
           <%: Epi.Web.Models.Constants.Constant.SURVEY_NOT_EXISTS %>
        </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>