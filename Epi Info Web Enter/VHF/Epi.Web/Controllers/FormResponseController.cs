using System;
using System.Web.Mvc;
using Epi.Web.MVC.Models;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Linq;
using Epi.Core.EnterInterpreter;
using System.Collections.Generic;
using System.Web.Security;
using System.Configuration;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Reflection;
using System.Diagnostics;
using Epi.Web.Enter.Common.Message;
using Epi.Web.MVC.Utility;
using Epi.Web.Enter.Common.DTO;
using System.Web.Configuration;
using System.Text;
namespace Epi.Web.MVC.Controllers
{
	[Authorize]
	public class FormResponseController : Controller
	{
		//
		// GET: /FormResponse/

		private Epi.Web.MVC.Facade.ISurveyFacade _isurveyFacade;
		private IEnumerable<XElement> PageFields;
		private string RequiredList = "";
		List<KeyValuePair<int, string>> Columns = new List<KeyValuePair<int, string>>();
		private int NumberOfPages = -1;
		private int NumberOfResponses = -1;
		private bool IsEditMode;
		private string Sort, SortField;
		private bool IsNewRequest = true; //Added for retain search and sort
		public FormResponseController(Epi.Web.MVC.Facade.ISurveyFacade isurveyFacade)
		{
			_isurveyFacade = isurveyFacade;

		}



		[HttpGet]
		//string responseid,string SurveyId, int ViewId, string CurrentPage
		// View =0 Root form
		public ActionResult Index(string formid, string responseid, int Pagenumber = 1, int ViewId = 0)
		{

			List<FormsHierarchyDTO> FormsHierarchy = GetFormsHierarchy();

			if (ViewId == 0)
			{
				//Following code checks if request is for new or selected form.
				if (Session["RootFormId"]!= null &&
					Session["RootFormId"].ToString() == formid)
				{
					IsNewRequest = false;
				}

				Session["RootFormId"] = formid;
				Session.Remove("RootResponseId");
				Session.Remove("FormValuesHasChanged");
				Session["IsEditMode"] = false;
			}
			if (ViewId == 0)
			{
				var model = new FormResponseInfoModel();
				model.ViewId = ViewId;
				model = GetSurveyResponseInfoModel(formid, Pagenumber);

				return View("Index", model);
			}
			else
			{

				int UserId = SurveyHelper.GetDecryptUserId(Session["UserId"].ToString());

				bool IsMobileDevice = this.Request.Browser.IsMobileDevice;




				int RequestedViewId;
				RequestedViewId = ViewId;

				Session["RequestedViewId"] = RequestedViewId;
				SurveyModel SurveyModel = new SurveyModel();



				SurveyModel.RelateModel = Mapper.ToRelateModel(FormsHierarchy, formid);
				SurveyModel.RequestedViewId = RequestedViewId;


				var RelateSurveyId = FormsHierarchy.Single(x => x.ViewId == ViewId);

				//SurveyAnswerRequest FormResponseReq = new SurveyAnswerRequest();

				if (!string.IsNullOrEmpty(responseid))
				{
					SurveyModel.FormResponseInfoModel = GetFormResponseInfoModel(RelateSurveyId.FormId, responseid);
					SurveyModel.FormResponseInfoModel.NumberOfResponses = SurveyModel.FormResponseInfoModel.ResponsesList.Count();

					SurveyModel.FormResponseInfoModel.ParentResponseId = responseid;
				}

				if (RelateSurveyId.ResponseIds.Count() > 0)
				{

					Epi.Web.Enter.Common.DTO.SurveyAnswerDTO surveyAnswerDTO = GetSurveyAnswer(RelateSurveyId.ResponseIds[0].ResponseId);
					var form = _isurveyFacade.GetSurveyFormData(RelateSurveyId.ResponseIds[0].SurveyId, 1, surveyAnswerDTO, IsMobileDevice, null);
					SurveyModel.Form = form;
					if (string.IsNullOrEmpty(responseid))
					{
						SurveyModel.FormResponseInfoModel = GetFormResponseInfoModel(RelateSurveyId.FormId, RelateSurveyId.ResponseIds[0].RelateParentId);
						SurveyModel.FormResponseInfoModel.ParentResponseId = RelateSurveyId.ResponseIds[0].RelateParentId;

					}

					SurveyModel.FormResponseInfoModel.FormInfoModel.FormName = form.SurveyInfo.SurveyName;
					SurveyModel.FormResponseInfoModel.FormInfoModel.FormId = form.SurveyInfo.SurveyId;
					SurveyModel.FormResponseInfoModel.NumberOfResponses = SurveyModel.FormResponseInfoModel.ResponsesList.Count();


				}
				else
				{
					FormResponseInfoModel ResponseInfoModel = new FormResponseInfoModel();
					if (SurveyModel.FormResponseInfoModel.ResponsesList.Count() > 0)
					{
						Epi.Web.Enter.Common.DTO.SurveyAnswerDTO surveyAnswerDTO = GetSurveyAnswer(SurveyModel.FormResponseInfoModel.ResponsesList[0].Column0, RelateSurveyId.FormId);
						ResponseInfoModel = GetFormResponseInfoModel(RelateSurveyId.FormId, responseid);
						SurveyModel.Form = _isurveyFacade.GetSurveyFormData(surveyAnswerDTO.SurveyId, 1, surveyAnswerDTO, IsMobileDevice, null);
						ResponseInfoModel.FormInfoModel.FormName = SurveyModel.Form.SurveyInfo.SurveyName.ToString();
						ResponseInfoModel.FormInfoModel.FormId = SurveyModel.Form.SurveyInfo.SurveyId.ToString();
						ResponseInfoModel.ParentResponseId = responseid;//SurveyModel.FormResponseInfoModel.ResponsesList[0].Column0;
						ResponseInfoModel.NumberOfResponses = SurveyModel.FormResponseInfoModel.ResponsesList.Count();
					}
					else
					{
						var form1 = _isurveyFacade.GetSurveyInfoModel(RelateSurveyId.FormId);
						ResponseInfoModel.FormInfoModel.FormName = form1.SurveyName.ToString();
						ResponseInfoModel.FormInfoModel.FormId = form1.SurveyId.ToString();
						ResponseInfoModel.ParentResponseId = responseid;
					}
					SurveyModel.FormResponseInfoModel = ResponseInfoModel;

				}
				SurveyModel.FormResponseInfoModel.ViewId = ViewId;

				return View("Index", SurveyModel.FormResponseInfoModel);

			}
		}

		[HttpPost]
		public ActionResult Index(string surveyid, string AddNewFormId, string EditForm, string Cancel)
		{
			if (!string.IsNullOrEmpty(Cancel))
			{
				int PNumber;
				int.TryParse(Cancel, out PNumber);

				return RedirectToRoute(new { Controller = "Survey", Action = "Index", responseid = this.Request.Form["Parent_Response_Id"].ToString(), PageNumber = PNumber });
			}
			bool IsEditMode = false;
			if (!string.IsNullOrEmpty(EditForm))
			{
				//Session["RootFormId"] = surveyid;
				if (Session["RootResponseId"] == null)
				{
					Session["RootResponseId"] = EditForm;
				}
				Session["IsEditMode"] = true;
				IsEditMode = true;
				Epi.Web.Enter.Common.DTO.SurveyAnswerDTO surveyAnswerDTO = GetSurveyAnswer(EditForm);
				string ChildRecordId = GetChildRecordId(surveyAnswerDTO);
				return RedirectToAction(Epi.Web.MVC.Constants.Constant.INDEX, Epi.Web.MVC.Constants.Constant.SURVEY_CONTROLLER, new { responseid = ChildRecordId, PageNumber = 1, Edit = "Edit" });
			}
            //else
            //{
            //    Session["IsEditMode"] = false;

			//}
			int UserId = SurveyHelper.GetDecryptUserId(Session["UserId"].ToString());
			if (!string.IsNullOrEmpty(EditForm))
			{
				Epi.Web.Enter.Common.DTO.SurveyAnswerDTO surveyAnswerDTO = GetSurveyAnswer(EditForm);
				surveyAnswerDTO.Status = 1;
				string ChildRecordId = GetChildRecordId(surveyAnswerDTO);
				return RedirectToAction(Epi.Web.MVC.Constants.Constant.INDEX, Epi.Web.MVC.Constants.Constant.SURVEY_CONTROLLER, new { responseid = ChildRecordId, PageNumber = 1, Edit = "Edit" });
			}
			bool IsMobileDevice = this.Request.Browser.IsMobileDevice;


			if (IsMobileDevice == false)
			{
				IsMobileDevice = Epi.Web.MVC.Utility.SurveyHelper.IsMobileDevice(this.Request.UserAgent.ToString());
			}

			FormsAuthentication.SetAuthCookie("BeginSurvey", false);

			//create the responseid
			Guid ResponseID = Guid.NewGuid();
			if (Session["RootResponseId"] == null)
			{
				Session["RootResponseId"] = ResponseID;
			}
			TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID] = ResponseID.ToString();

			// create the first survey response
			// Epi.Web.Enter.Common.DTO.SurveyAnswerDTO SurveyAnswer = _isurveyFacade.CreateSurveyAnswer(surveyModel.SurveyId, ResponseID.ToString());
			// Epi.Web.Enter.Common.DTO.SurveyAnswerDTO SurveyAnswer = _isurveyFacade.CreateSurveyAnswer(AddNewFormId, ResponseID.ToString(), UserId);
			Epi.Web.Enter.Common.DTO.SurveyAnswerDTO SurveyAnswer = _isurveyFacade.CreateSurveyAnswer(AddNewFormId, ResponseID.ToString(), UserId, true, this.Request.Form["Parent_Response_Id"].ToString(), IsEditMode);
			SurveyInfoModel surveyInfoModel = GetSurveyInfo(SurveyAnswer.SurveyId);

			// set the survey answer to be production or test 
			SurveyAnswer.IsDraftMode = surveyInfoModel.IsDraftMode;
			XDocument xdoc = XDocument.Parse(surveyInfoModel.XML);

			MvcDynamicForms.Form form = _isurveyFacade.GetSurveyFormData(SurveyAnswer.SurveyId, 1, SurveyAnswer, IsMobileDevice);

			var _FieldsTypeIDs = from _FieldTypeID in
									 xdoc.Descendants("Field")
								 select _FieldTypeID;

			TempData["Width"] = form.Width + 100;

			XDocument xdocResponse = XDocument.Parse(SurveyAnswer.XML);

			XElement ViewElement = xdoc.XPathSelectElement("Template/Project/View");
			string checkcode = ViewElement.Attribute("CheckCode").Value.ToString();

			form.FormCheckCodeObj = form.GetCheckCodeObj(xdoc, xdocResponse, checkcode);

			///////////////////////////// Execute - Record Before - start//////////////////////
			Dictionary<string, string> ContextDetailList = new Dictionary<string, string>();
			EnterRule FunctionObject_B = (EnterRule)form.FormCheckCodeObj.GetCommand("level=record&event=before&identifier=");
            SurveyResponseXML SurveyResponseXML = new SurveyResponseXML(PageFields, RequiredList);
			if (FunctionObject_B != null && !FunctionObject_B.IsNull())
			{
				try
				{
                    SurveyAnswer.XML = SurveyResponseXML.CreateResponseDocument(xdoc, SurveyAnswer.XML);
					//SurveyAnswer.XML = Epi.Web.MVC.Utility.SurveyHelper.CreateResponseDocument(xdoc, SurveyAnswer.XML, RequiredList);
                    Session["RequiredList"] = SurveyResponseXML._RequiredList;
                    this.RequiredList = SurveyResponseXML._RequiredList;
					form.RequiredFieldsList = this.RequiredList;
					FunctionObject_B.Context.HiddenFieldList = form.HiddenFieldsList;
					FunctionObject_B.Context.HighlightedFieldList = form.HighlightedFieldsList;
					FunctionObject_B.Context.DisabledFieldList = form.DisabledFieldsList;
					FunctionObject_B.Context.RequiredFieldList = form.RequiredFieldsList;

					FunctionObject_B.Execute();

					// field list
					form.HiddenFieldsList = FunctionObject_B.Context.HiddenFieldList;
					form.HighlightedFieldsList = FunctionObject_B.Context.HighlightedFieldList;
					form.DisabledFieldsList = FunctionObject_B.Context.DisabledFieldList;
					form.RequiredFieldsList = FunctionObject_B.Context.RequiredFieldList;


					ContextDetailList = Epi.Web.MVC.Utility.SurveyHelper.GetContextDetailList(FunctionObject_B);
					form = Epi.Web.MVC.Utility.SurveyHelper.UpdateControlsValuesFromContext(form, ContextDetailList);

					_isurveyFacade.UpdateSurveyResponse(surveyInfoModel, ResponseID.ToString(), form, SurveyAnswer, false, false, 0, UserId);
				}
				catch (Exception ex)
				{
					// do nothing so that processing
					// can continue
				}
			}
			else
			{
				SurveyAnswer.XML = CreateResponseDocument(xdoc, SurveyAnswer.XML);
              
				//SurveyAnswer.XML = Epi.Web.MVC.Utility.SurveyHelper.CreateResponseDocument(xdoc, SurveyAnswer.XML, RequiredList);
				form.RequiredFieldsList = RequiredList;
                Session["RequiredList"] = RequiredList;
				_isurveyFacade.UpdateSurveyResponse(surveyInfoModel, SurveyAnswer.ResponseId, form, SurveyAnswer, false, false, 0, UserId);
			}

			SurveyAnswer = _isurveyFacade.GetSurveyAnswerResponse(SurveyAnswer.ResponseId).SurveyResponseList[0];

			///////////////////////////// Execute - Record Before - End//////////////////////
			//string page;
			// return RedirectToAction(Epi.Web.Models.Constants.Constant.INDEX, Epi.Web.Models.Constants.Constant.SURVEY_CONTROLLER, new {id="page" });
			return RedirectToAction(Epi.Web.MVC.Constants.Constant.INDEX, Epi.Web.MVC.Constants.Constant.SURVEY_CONTROLLER, new { responseid = ResponseID, PageNumber = 1 });
			//}
			//catch (Exception ex)
			//{
			//    //Epi.Web.Utility.ExceptionMessage.SendLogMessage(ex, this.HttpContext);
			//    //return View(Epi.Web.MVC.Constants.Constant.EXCEPTION_PAGE);
			//}

		}


		public FormResponseInfoModel GetSurveyResponseInfoModel(string SurveyId, int PageNumber)
		{

			FormResponseInfoModel FormResponseInfoModel = new FormResponseInfoModel();
			SurveyAnswerRequest FormResponseReq = new SurveyAnswerRequest();
			FormSettingRequest FormSettingReq = new Enter.Common.Message.FormSettingRequest();

			//Populating the request
			FormResponseReq.Criteria.SurveyId = SurveyId.ToString();
			FormResponseReq.Criteria.PageNumber = PageNumber;
			FormResponseReq.Criteria.IsMobile = true;
			FormSettingReq.FormInfo.FormId = new Guid(SurveyId).ToString();

			//Getting Column Name  List
			FormSettingResponse FormSettingResponse = _isurveyFacade.GetFormSettings(FormSettingReq);
			Columns = FormSettingResponse.FormSetting.ColumnNameList.ToList();
			Columns.Sort(Compare);
			FormResponseInfoModel.SearchModel = new SearchBoxModel();
			// Setting  Column Name  List
			FormResponseInfoModel.Columns = Columns;
			FormResponseReq.Criteria.IsSqlProject = FormSettingResponse.FormInfo.IsSQLProject;
			Session["IsSqlProject"] = FormSettingResponse.FormInfo.IsSQLProject;
			//if (Session["SearchCriteria"] != null)
			//{
			//    FormResponseInfoModel.SearchModel = (SeachBoxModel)Session["SearchCriteria"];
			//}
			//FormResponseReq.Criteria.SearchCriteria = CreateSearchCriteria(Request.QueryString, FormResponseInfoModel.SearchModel, FormResponseInfoModel);

//Following code retains the search and sort criteria for already selected form. 
			if (!IsNewRequest)
			{
				if (Session["SortOrder"] != null &&
						!string.IsNullOrEmpty(Session["SortOrder"].ToString()) &&
					string.IsNullOrEmpty(Request.QueryString["sort"]))
				{
					Sort = Session["SortOrder"].ToString();
				}
				else
				{
					Sort = Request.QueryString["sort"];
					Session["SortOrder"] = Sort;
				}

				if (Session["SortField"] != null &&
					   !string.IsNullOrEmpty(Session["SortField"].ToString()) &&
				   string.IsNullOrEmpty(Request.QueryString["sortfield"]))
				{
					SortField = Session["SortField"].ToString();
				}
				else
				{
					SortField = Request.QueryString["sortfield"];
					Session["SortField"] = SortField;
				}
			}

			if (    !IsNewRequest && 
					Session["SearchCriteria"] != null && !string.IsNullOrEmpty(Session["SearchCriteria"].ToString()) &&
                    (Request.QueryString["reset"] == null) && Request.QueryString["col1"] == null
				
				)
					//(Request.QueryString["col1"] == null || Request.QueryString["col1"] == "undefined") &&
			{
				FormResponseReq.Criteria.SearchCriteria = Session["SearchCriteria"].ToString();
				FormResponseInfoModel.SearchModel = (SearchBoxModel)Session["SearchModel"];
			}
			else
			{
				FormResponseReq.Criteria.SearchCriteria = CreateSearchCriteria(Request.QueryString, FormResponseInfoModel.SearchModel, FormResponseInfoModel);
				Session["SearchModel"] = FormResponseInfoModel.SearchModel;
				Session["SearchCriteria"] = FormResponseReq.Criteria.SearchCriteria;
			}
			Session["PageNumber"] = PageNumber;
			// Session["SearchCriteria"] = FormResponseInfoModel.SearchModel;
			PopulateDropDownlists(FormResponseInfoModel, FormSettingResponse.FormSetting.FormControlNameList.ToList());

			if (Sort != null && Sort.Length > 0)
			{
				FormResponseReq.Criteria.SortOrder = Sort;
			}

			if (SortField != null && SortField.Length > 0)
			{
				FormResponseReq.Criteria.Sortfield = SortField;
			}


			//Getting Resposes
			SurveyAnswerResponse FormResponseList = _isurveyFacade.GetFormResponseList(FormResponseReq);

			//Setting Resposes List
			List<ResponseModel> ResponseList = new List<ResponseModel>();
			foreach (var item in FormResponseList.SurveyResponseList)
			{
				//ResponseList.Add(ConvertXMLToModel(item, Columns));
				if (item.SqlData != null)
				{
					ResponseList.Add(ConvertRowToModel(item, Columns));
				}
				else
				{
					ResponseList.Add(ConvertXMLToModel(item, Columns));
				}
			}

			FormResponseInfoModel.ResponsesList = ResponseList;
			//Setting Form Info 
			FormResponseInfoModel.FormInfoModel = Mapper.ToFormInfoModel(FormResponseList.FormInfo);
			//Setting Additional Data

			FormResponseInfoModel.NumberOfPages = FormResponseList.NumberOfPages;
			FormResponseInfoModel.PageSize = FormResponseList.PageSize;
			FormResponseInfoModel.NumberOfResponses = FormResponseList.NumberOfResponses;
			FormResponseInfoModel.sortfield = SortField;
			FormResponseInfoModel.sortOrder = Sort;
			FormResponseInfoModel.CurrentPage = PageNumber;
			return FormResponseInfoModel;
		}

		private string CreateSearchCriteria(System.Collections.Specialized.NameValueCollection nameValueCollection, SearchBoxModel SearchModel, FormResponseInfoModel Model)
		{
			FormCollection Collection = new FormCollection(nameValueCollection);



			StringBuilder searchBuilder = new StringBuilder();


			//SortField = Collection[""];


			

			if (ValidateSearchFields(Collection))
			{

				if (Collection["col1"].Length > 0 && Collection["val1"].Length > 0)
				{
					searchBuilder.Append(Collection["col1"] + "='" + Collection["val1"] + "'");
					SearchModel.SearchCol1 = Collection["col1"];
					SearchModel.Value1 = Collection["val1"];
				}
				if (Collection["col2"].Length > 0 && Collection["val2"].Length > 0)
				{
					searchBuilder.Append(" AND " + Collection["col2"] + "='" + Collection["val2"] + "'");
					SearchModel.SearchCol2 = Collection["col2"];
					SearchModel.Value2 = Collection["val2"];
				}
				if (Collection["col3"].Length > 0 && Collection["val3"].Length > 0)
				{
					searchBuilder.Append(" AND " + Collection["col3"] + "='" + Collection["val3"] + "'");
					SearchModel.SearchCol3 = Collection["col3"];
					SearchModel.Value3 = Collection["val3"];
				}
				if (Collection["col4"].Length > 0 && Collection["val4"].Length > 0)
				{
					searchBuilder.Append(" AND " + Collection["col4"] + "='" + Collection["val4"] + "'");
					SearchModel.SearchCol4 = Collection["col4"];
					SearchModel.Value4 = Collection["val4"];
				}
				if (Collection["col5"].Length > 0 && Collection["val5"].Length > 0)
				{
					searchBuilder.Append(" AND " + Collection["col5"] + "='" + Collection["val5"] + "'");
					SearchModel.SearchCol5 = Collection["col5"];
					SearchModel.Value5 = Collection["val5"];
				}
			}

			return searchBuilder.ToString();
		}

		private bool ValidateSearchFields(FormCollection Collection)
		{
			if (string.IsNullOrEmpty(Collection["col1"]) || Collection["col1"] == "undefined" ||
			   string.IsNullOrEmpty(Collection["val1"]) || Collection["val1"] == "undefined")
			{
				return false;
			}
			return true;
		}

		private void PopulateDropDownlist(out List<SelectListItem> SearchColumns, string SelectedValue, List<KeyValuePair<int, string>> Columns)
		{
			SearchColumns = new List<SelectListItem>();
			foreach (var item in Columns)
			{
				SelectListItem newSelectListItem = new SelectListItem { Text = item.Value, Value = item.Value, Selected = item.Value == SelectedValue };
				SearchColumns.Add(newSelectListItem);
			}
		}

		private void PopulateDropDownlists(FormResponseInfoModel FormResponseInfoModel, List<KeyValuePair<int, string>> list)
		{
			PopulateDropDownlist(out FormResponseInfoModel.SearchColumns1, FormResponseInfoModel.SearchModel.SearchCol1, list);
			PopulateDropDownlist(out FormResponseInfoModel.SearchColumns2, FormResponseInfoModel.SearchModel.SearchCol2, list);
			PopulateDropDownlist(out FormResponseInfoModel.SearchColumns3, FormResponseInfoModel.SearchModel.SearchCol3, list);
			PopulateDropDownlist(out FormResponseInfoModel.SearchColumns4, FormResponseInfoModel.SearchModel.SearchCol4, list);
			PopulateDropDownlist(out FormResponseInfoModel.SearchColumns5, FormResponseInfoModel.SearchModel.SearchCol5, list);
		}

		private int Compare(KeyValuePair<int, string> a, KeyValuePair<int, string> b)
		{
			return a.Key.CompareTo(b.Key);
		}

		private ResponseModel ConvertXMLToModel(SurveyAnswerDTO item, List<KeyValuePair<int, string>> Columns)
		{
			ResponseModel ResponseModel = new Models.ResponseModel();


			var MetaDataColumns = Epi.Web.MVC.Constants.Constant.MetaDaTaColumnNames();

			try
			{
				ResponseModel.Column0 = item.ResponseId;
				ResponseModel.IsLocked = item.IsLocked;
				IEnumerable<XElement> nodes;
				var document = XDocument.Parse(item.XML);
				if (MetaDataColumns.Contains(Columns[0].Value.ToString()))
				{

					ResponseModel.Column1 = GetColumnValue(item, Columns[0].Value.ToString());
				}
				else
				{
					nodes = document.Descendants().Where(e => e.Name.LocalName.StartsWith("ResponseDetail") && e.Attribute("QuestionName").Value == Columns[0].Value.ToString());
					ResponseModel.Column1 = nodes.First().Value;
				}
				if (Columns.Count >= 2)
				{
					if (MetaDataColumns.Contains(Columns[1].Value.ToString()))
					{

						ResponseModel.Column2 = GetColumnValue(item, Columns[1].Value.ToString());
					}
					else
					{
						nodes = document.Descendants().Where(e => e.Name.LocalName.StartsWith("ResponseDetail") && e.Attribute("QuestionName").Value == Columns[1].Value.ToString());
						ResponseModel.Column2 = nodes.First().Value;
					}
				}


				if (Columns.Count >= 3)
				{
					if (MetaDataColumns.Contains(Columns[2].Value.ToString()))
					{

						ResponseModel.Column3 = GetColumnValue(item, Columns[2].Value.ToString());
					}
					else
					{
						nodes = document.Descendants().Where(e => e.Name.LocalName.StartsWith("ResponseDetail") && e.Attribute("QuestionName").Value == Columns[2].Value.ToString());
						ResponseModel.Column3 = nodes.First().Value;
					}
				}

				if (Columns.Count >= 4)
				{
					if (MetaDataColumns.Contains(Columns[3].Value.ToString()))
					{

						ResponseModel.Column4 = GetColumnValue(item, Columns[3].Value.ToString());
					}
					else
					{
						nodes = document.Descendants().Where(e => e.Name.LocalName.StartsWith("ResponseDetail") && e.Attribute("QuestionName").Value == Columns[3].Value.ToString());
						ResponseModel.Column4 = nodes.First().Value;
					}
				}

				if (Columns.Count >= 5)
				{
					if (MetaDataColumns.Contains(Columns[4].Value.ToString()))
					{

						ResponseModel.Column5 = GetColumnValue(item, Columns[4].Value.ToString());
					}
					else
					{
						nodes = document.Descendants().Where(e => e.Name.LocalName.StartsWith("ResponseDetail") && e.Attribute("QuestionName").Value == Columns[4].Value.ToString());
						ResponseModel.Column5 = nodes.First().Value;
					}
				}


				return ResponseModel;

			}
			catch (Exception Ex)
			{

				throw new Exception(Ex.Message);
			}

		}
		private ResponseModel ConvertRowToModel(SurveyAnswerDTO item, List<KeyValuePair<int, string>> Columns)
		{
			ResponseModel Response = new ResponseModel();

			Response.Column0 = item.SqlData["GlobalRecordId"];
			if (Columns.Count > 0)
			{
				Response.Column1 = item.SqlData[Columns[0].Value];
			}

			if (Columns.Count > 1)
			{
				Response.Column2 = item.SqlData[Columns[1].Value];
			}

			if (Columns.Count > 2)
			{
				Response.Column3 = item.SqlData[Columns[2].Value];
			}
			if (Columns.Count > 3)
			{
				Response.Column4 = item.SqlData[Columns[3].Value];
			}
			if (Columns.Count > 4)
			{
				Response.Column5 = item.SqlData[Columns[4].Value];
			}

			//Response.Column2 = item.SqlData[Columns[2].Value];
			//Response.Column3 = item.SqlData[Columns[3].Value];
			//Response.Column4 = item.SqlData[Columns[4].Value];
			//Response.Column5 = item.SqlData[Columns[5].Value];

			return Response; 
		}
        //private string GetColumnValue(SurveyAnswerDTO item, string columnName)
        //{
        //    string ColumnValue = "";
        //    switch (columnName)
        //    {
        //        case "UserEmail":
        //            ColumnValue = item.UserEmail;
        //            break;
        //        case "DateUpdated":
        //            ColumnValue = item.DateUpdated.ToString();
        //            break;
        //        case "DateCreated":
        //            ColumnValue = item.DateCreated.ToString();
        //            break;
        //        case "IsDraftMode":
        //            ColumnValue = item.IsDraftMode.ToString();
        //            break;
        //    }
        //    return ColumnValue;
        //}
        private string GetColumnValue(SurveyAnswerDTO item, string columnName)
        {
            string ColumnValue = "";
            switch (columnName)
            {
                case "_UserEmail":
                    ColumnValue = item.UserEmail;
                    break;
                case "_DateUpdated":
                    ColumnValue = item.DateUpdated.ToString();
                    break;
                case "_DateCreated":
                    ColumnValue = item.DateCreated.ToString();
                    break;
                case "IsDraftMode":
                case "_Mode":
                    if (item.IsDraftMode.ToString().ToUpper() == "TRUE")
                    {
                        ColumnValue = "Staging";
                    }
                    else
                    {
                        ColumnValue = "Production";

                    }
                    break;
            }
            return ColumnValue;
        }


		private string CreateResponseDocument(XDocument pMetaData, string pXML)
		{
			XDocument XmlResponse = new XDocument();
			int NumberOfPages = GetNumberOfPages(pMetaData);
			for (int i = 0; NumberOfPages > i - 1; i++)
			{
				var _FieldsTypeIDs = from _FieldTypeID in
										 pMetaData.Descendants("Field")
									 where _FieldTypeID.Attribute("Position").Value == (i - 1).ToString()
									 select _FieldTypeID;

				PageFields = _FieldsTypeIDs;

				XDocument CurrentPageXml = ToXDocument(CreateResponseXml("", false, i, ""));

				if (i == 0)
				{
					XmlResponse = ToXDocument(CreateResponseXml("", true, i, ""));
				}
				else
				{
					XmlResponse = MergeXml(XmlResponse, CurrentPageXml, i);
				}
			}

			return XmlResponse.ToString();
		}

		private static int GetNumberOfPages(XDocument Xml)
		{
			var _FieldsTypeIDs = from _FieldTypeID in
									 Xml.Descendants("View")
								 select _FieldTypeID;

			return _FieldsTypeIDs.Elements().Count();
		}

		public XmlDocument CreateResponseXml(string SurveyId, bool AddRoot, int CurrentPage, string Pageid)
		{
			XmlDocument xml = new XmlDocument();
			XmlElement root = xml.CreateElement("SurveyResponse");

			if (CurrentPage == 0)
			{
				root.SetAttribute("SurveyId", SurveyId);
				root.SetAttribute("LastPageVisited", "1");
				root.SetAttribute("HiddenFieldsList", "");
				root.SetAttribute("HighlightedFieldsList", "");
				root.SetAttribute("DisabledFieldsList", "");
				root.SetAttribute("RequiredFieldsList", "");

				xml.AppendChild(root);
			}

			XmlElement PageRoot = xml.CreateElement("Page");
			if (CurrentPage != 0)
			{
				PageRoot.SetAttribute("PageNumber", CurrentPage.ToString());
				PageRoot.SetAttribute("PageId", Pageid);//Added PageId Attribute to the page node
				xml.AppendChild(PageRoot);
			}

			foreach (var Field in this.PageFields)
			{
				XmlElement child = xml.CreateElement(Epi.Web.MVC.Constants.Constant.RESPONSE_DETAILS);
				child.SetAttribute("QuestionName", Field.Attribute("Name").Value);
				child.InnerText = Field.Value;
				PageRoot.AppendChild(child);
				//Start Adding required controls to the list
				SetRequiredList(Field);
			}

			return xml;
		}

		public static XDocument ToXDocument(XmlDocument xmlDocument)
		{
			using (var nodeReader = new XmlNodeReader(xmlDocument))
			{
				nodeReader.MoveToContent();
				return XDocument.Load(nodeReader);
			}
		}

		public static XDocument MergeXml(XDocument SavedXml, XDocument CurrentPageResponseXml, int Pagenumber)
		{
			XDocument xdoc = XDocument.Parse(SavedXml.ToString());
			XElement oldXElement = xdoc.XPathSelectElement("SurveyResponse/Page[@PageNumber = '" + Pagenumber.ToString() + "']");

			if (oldXElement == null)
			{
				SavedXml.Root.Add(CurrentPageResponseXml.Elements());
				return SavedXml;
			}

			else
			{
				oldXElement.Remove();
				xdoc.Root.Add(CurrentPageResponseXml.Elements());
				return xdoc;
			}
		}

		public void SetRequiredList(XElement _Fields)
		{
			bool isRequired = false;
			string value = _Fields.Attribute("IsRequired").Value;

			if (bool.TryParse(value, out isRequired))
			{
				if (isRequired)
				{
					if (!RequiredList.Contains(_Fields.Attribute("Name").Value))
					{
						if (RequiredList != "")
						{
							RequiredList = RequiredList + "," + _Fields.Attribute("Name").Value.ToLower();
						}
						else
						{
							RequiredList = _Fields.Attribute("Name").Value.ToLower();
						}
					}
				}
			}
		}

		public SurveyInfoModel GetSurveyInfo(string SurveyId)
		{
			SurveyInfoModel surveyInfoModel = _isurveyFacade.GetSurveyInfoModel(SurveyId);
			return surveyInfoModel;
		}
		private string GetChildRecordId(SurveyAnswerDTO surveyAnswerDTO)
		{
			//SurveyAnswerRequest SurveyAnswerRequest = new SurveyAnswerRequest();
			//SurveyAnswerResponse SurveyAnswerResponse = new SurveyAnswerResponse();
			//string ChildId = Guid.NewGuid().ToString();
			//surveyAnswerDTO.ParentRecordId = surveyAnswerDTO.ResponseId;
			//surveyAnswerDTO.ResponseId = ChildId;
			//SurveyAnswerRequest.SurveyAnswerList.Add(surveyAnswerDTO);
			//string result = ChildId;

			////responseId = TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID].ToString();
			//SurveyAnswerRequest.Criteria.UserId = SurveyHelper.GetDecryptUserId(Session["UserId"].ToString()); ;
			//SurveyAnswerRequest.RequestId = ChildId;
			//SurveyAnswerRequest.Action = "Create";
			//SurveyAnswerResponse = _isurveyFacade.SetChildRecord(SurveyAnswerRequest);

			//return result;
			SurveyAnswerRequest SurveyAnswerRequest = new SurveyAnswerRequest();
			SurveyAnswerResponse SurveyAnswerResponse = new SurveyAnswerResponse();
			string ChildId = Guid.NewGuid().ToString();
			surveyAnswerDTO.ParentRecordId = surveyAnswerDTO.ResponseId;
			surveyAnswerDTO.ResponseId = ChildId;
			surveyAnswerDTO.Status = 1;
			SurveyAnswerRequest.SurveyAnswerList.Add(surveyAnswerDTO);
			string result;

			//responseId = TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID].ToString();
			string Id = Session["UserId"].ToString();
			SurveyAnswerRequest.Criteria.UserId = SurveyHelper.GetDecryptUserId(Id);//_UserId;
			SurveyAnswerRequest.RequestId = ChildId;
			SurveyAnswerRequest.Action = "CreateMulti";
			SurveyAnswerResponse = _isurveyFacade.SetChildRecord(SurveyAnswerRequest);
			result = SurveyAnswerResponse.SurveyResponseList[0].ResponseId.ToString();
			return result;
		}
		private Epi.Web.Enter.Common.DTO.SurveyAnswerDTO GetSurveyAnswer(string responseId, string formid = "")
		{
			Epi.Web.Enter.Common.DTO.SurveyAnswerDTO result = null;

			//responseId = TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID].ToString();
			string FormId = Session["RootFormId"].ToString();
			string Id = Session["UserId"].ToString();
			if (string.IsNullOrEmpty(formid))
			{
				result = _isurveyFacade.GetSurveyAnswerResponse(responseId, FormId, SurveyHelper.GetDecryptUserId(Id)).SurveyResponseList[0];
			}
			else
			{

				result = _isurveyFacade.GetSurveyAnswerResponse(responseId, formid, SurveyHelper.GetDecryptUserId(Id)).SurveyResponseList[0];
			}
			return result;

		}


		/// <summary>
		/// Following Action method takes ResponseId as a parameter and deletes the response.
		/// For now it returns nothing as a confirmation of deletion, we may add some error/success
		/// messages later. TBD
		/// </summary>
		/// <param name="ResponseId"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult Delete(string ResponseId, string surveyid)
		{

			SurveyAnswerRequest SARequest = new SurveyAnswerRequest();
			SARequest.SurveyAnswerList.Add(new SurveyAnswerDTO() { ResponseId = ResponseId });
			string Id = Session["UserId"].ToString();
			SARequest.Criteria.UserId = SurveyHelper.GetDecryptUserId(Id);
			SARequest.Criteria.IsEditMode = false;
			SARequest.Criteria.IsDeleteMode = false;
			SARequest.Criteria.IsSqlProject = (bool)Session["IsSqlProject"];
			SARequest.Criteria.SurveyId = Session["RootFormId"].ToString();

			SurveyAnswerResponse SAResponse = _isurveyFacade.DeleteResponse(SARequest);

			return Json(surveyid);


		}
		[HttpPost]

		public ActionResult DeleteBranch(string ResponseId)//List<FormInfoModel> ModelList, string formid)
		{

			SurveyAnswerRequest SARequest = new SurveyAnswerRequest();
			SARequest.SurveyAnswerList.Add(new SurveyAnswerDTO() { ResponseId = ResponseId });
			SARequest.Criteria.UserId = SurveyHelper.GetDecryptUserId(Session["UserId"].ToString());
			SARequest.Criteria.IsEditMode = false;
			SARequest.Criteria.IsDeleteMode = false;
			SARequest.Criteria.IsSqlProject = (bool)Session["IsSqlProject"];
			SARequest.Criteria.SurveyId = Session["RootFormId"].ToString();
			SurveyAnswerResponse SAResponse = _isurveyFacade.DeleteResponse(SARequest);

			return Json(string.Empty);//string.Empty
			//return RedirectToAction("Index", "Home");
		}
		private List<FormsHierarchyDTO> GetFormsHierarchy()
		{
			FormsHierarchyResponse FormsHierarchyResponse = new FormsHierarchyResponse();
			FormsHierarchyRequest FormsHierarchyRequest = new FormsHierarchyRequest();
			if (Session["RootFormId"] != null && Session["RootResponseId"] != null)
			{
				FormsHierarchyRequest.SurveyInfo.FormId = Session["RootFormId"].ToString();
				FormsHierarchyRequest.SurveyResponseInfo.ResponseId = Session["RootResponseId"].ToString();
				FormsHierarchyResponse = _isurveyFacade.GetFormsHierarchy(FormsHierarchyRequest);
			}
			return FormsHierarchyResponse.FormsHierarchy;
		}

		private FormResponseInfoModel GetFormResponseInfoModel(string SurveyId, string ResponseId)
		{
			int UserId = SurveyHelper.GetDecryptUserId(Session["UserId"].ToString());
			FormResponseInfoModel FormResponseInfoModel = new FormResponseInfoModel();

			SurveyResponseXML SurveyResponseXML = new SurveyResponseXML();
			if (!string.IsNullOrEmpty(SurveyId))
			{
				SurveyAnswerRequest FormResponseReq = new SurveyAnswerRequest();
				FormSettingRequest FormSettingReq = new Enter.Common.Message.FormSettingRequest();

				//Populating the request

				FormSettingReq.FormInfo.FormId = SurveyId;
				FormSettingReq.FormInfo.UserId = UserId;
				//Getting Column Name  List
				FormSettingResponse FormSettingResponse = _isurveyFacade.GetFormSettings(FormSettingReq);
				Columns = FormSettingResponse.FormSetting.ColumnNameList.ToList();
				Columns.Sort(Compare);

				// Setting  Column Name  List
				FormResponseInfoModel.Columns = Columns;

				//Getting Resposes
				FormResponseReq.Criteria.SurveyId = SurveyId.ToString();
				FormResponseReq.Criteria.SurveyAnswerIdList.Add(ResponseId);

				FormResponseReq.Criteria.PageNumber = 1;
				FormResponseReq.Criteria.UserId = UserId;
				FormResponseReq.Criteria.IsMobile = true;
				SurveyAnswerResponse FormResponseList = _isurveyFacade.GetResponsesByRelatedFormId(FormResponseReq);


				//Setting Resposes List
				List<ResponseModel> ResponseList = new List<ResponseModel>();
				foreach (var item in FormResponseList.SurveyResponseList)
				{
					//ResponseList.Add(SurveyResponseXML.ConvertXMLToModel(item, Columns));
					if (item.SqlData != null)
					{
						ResponseList.Add(ConvertRowToModel(item, Columns));
					}
					else
					{
						ResponseList.Add(SurveyResponseXML.ConvertXMLToModel(item, Columns));
					}
				}

				FormResponseInfoModel.ResponsesList = ResponseList;
				//Setting Form Info 
				//  FormResponseInfoModel.FormInfoModel = Mapper.ToFormInfoModel(FormResponseList.FormInfo);
				//Setting Additional Data

				FormResponseInfoModel.NumberOfPages = FormResponseList.NumberOfPages;
				FormResponseInfoModel.PageSize = ReadPageSize();
				FormResponseInfoModel.NumberOfResponses = FormResponseList.SurveyResponseList.Count();//FormResponseList.NumberOfResponses;
				FormResponseInfoModel.CurrentPage = 1;
			}
			return FormResponseInfoModel;
		}

		private int ReadPageSize()
		{
			return Convert.ToInt16(WebConfigurationManager.AppSettings["RESPONSE_PAGE_SIZE"].ToString());
		}

		[HttpGet]
		public ActionResult LogOut()
		{

			FormsAuthentication.SignOut();
			this.Session.Clear();
			return RedirectToAction("Index", "Login");


		}

	}
}
