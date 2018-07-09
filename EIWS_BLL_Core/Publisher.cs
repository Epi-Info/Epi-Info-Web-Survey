using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Xml;
using Epi.Web.Common.BusinessObject;
using System.Xml;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Xml.XPath;

namespace Epi.Web.BLL
{
    /// <summary>
    /// 
    /// </summary>

    public class Publisher
    {
        private Epi.Web.Interfaces.DataInterfaces.ISurveyInfoDao SurveyInfoDao;
        private Epi.Web.Interfaces.DataInterfaces.IOrganizationDao OrganizationDao;
        Dictionary<int, int> ViewIds = new Dictionary<int, int>();
        #region"Public members"
        /// <summary>
        ///  This class is used to process the object sent from the WCF service “SurveyManager”, 
        ///  save survey info into SurvayMetaData Table 
        ///  and then returns a URL, StatusText and IsPublished indicator.
        /// </summary>
        /// <param name="pRequestMessage"></param>
        /// <returns></returns>
        /// 
        public Publisher(Epi.Web.Interfaces.DataInterfaces.ISurveyInfoDao pSurveyInfoDao, Epi.Web.Interfaces.DataInterfaces.IOrganizationDao pPrganizationDao)
        {
            this.SurveyInfoDao = pSurveyInfoDao;
            this.OrganizationDao = pPrganizationDao;
        }
        public Publisher()
        {

        }
        public SurveyRequestResultBO PublishSurvey(SurveyInfoBO pRequestMessage)
        {
            SurveyRequestResultBO result = new SurveyRequestResultBO();

            if (IsRelatedForm(pRequestMessage.XML))
            {
                result = PublishRelatedFormSurvey(pRequestMessage);
            }
            else
            {
                result = Publish(pRequestMessage);

            }
            return result;
        }




        public SurveyRequestResultBO RePublishSurvey(SurveyInfoBO pRequestMessage)
        {

            SurveyRequestResultBO result = new SurveyRequestResultBO();
            if (IsRelatedForm(pRequestMessage.XML))
            {
                result = RePublishRelatedFormSurvey(pRequestMessage);
            }
            else
            {
                result = RePublish(pRequestMessage);
            }
            return result;
        }
        private SurveyRequestResultBO Publish(SurveyInfoBO pRequestMessage)
        {
            string _Xml = pRequestMessage.XML;
            SurveyRequestResultBO result = new SurveyRequestResultBO();

            var SurveyId = Guid.NewGuid();

            if (pRequestMessage != null)
            {

                //if (! string.IsNullOrEmpty(pRequestMessage.SurveyNumber)  &&  ValidateOrganizationKey(pRequestMessage.OrganizationKey))
                if (ValidateOrganizationKey(pRequestMessage.OrganizationKey))
                {

                    if (ValidateSurveyFields(pRequestMessage))
                    {
                        try
                        {

                            Epi.Web.Common.BusinessObject.SurveyInfoBO BO = new Epi.Web.Common.BusinessObject.SurveyInfoBO();

                            //BO.SurveyId = SurveyId.ToString();
                            //BO.ClosingDate = pRequestMessage.ClosingDate;

                            //BO.IntroductionText = pRequestMessage.IntroductionText;
                            //BO.ExitText = pRequestMessage.ExitText;
                            //BO.DepartmentName = pRequestMessage.DepartmentName;
                            //BO.OrganizationName = pRequestMessage.OrganizationName;

                            //BO.SurveyNumber = pRequestMessage.SurveyNumber;

                            //XDocument xdoc = XDocument.Parse(pRequestMessage.XML);
                            //xdoc.Descendants().Where(e => e.Name == "SourceTable").Remove();
                            //BO.XML = xdoc.ToString();

                            //BO.SurveyName = pRequestMessage.SurveyName;

                            //BO.SurveyType = pRequestMessage.SurveyType;
                            //BO.UserPublishKey = pRequestMessage.UserPublishKey;
                            //BO.OrganizationKey = pRequestMessage.OrganizationKey;
                            //BO.OrganizationKey = pRequestMessage.OrganizationKey;
                            //BO.TemplateXMLSize = pRequestMessage.TemplateXMLSize;
                            //BO.IsDraftMode = pRequestMessage.IsDraftMode;
                            //BO.StartDate = pRequestMessage.StartDate;

                            BO = ToBusinessObject(pRequestMessage, SurveyId);
                            if (pRequestMessage.IsSqlProject)
                            {
                                this.SurveyInfoDao.ValidateServername(pRequestMessage);
                            }

                            BO.IsSqlProject = pRequestMessage.IsSqlProject;

                            this.SurveyInfoDao.InsertSurveyInfo(BO);
                            result.URL = GetURL(pRequestMessage, SurveyId);
                            result.IsPulished = true;
                            //Insert Connection string..
                            if (pRequestMessage.IsSqlProject)
                            {
                                DbConnectionStringBO DbConnectionStringBO = new DbConnectionStringBO();
                                DbConnectionStringBO = GetConnection(pRequestMessage.DBConnectionString);
                                DbConnectionStringBO.SurveyId = SurveyId;
                                this.SurveyInfoDao.InsertConnectionString(DbConnectionStringBO);
                            }
                            //Source Table 
                            SetSourceTable(_Xml, SurveyId.ToString());
                            // View and ID setup
                            Dictionary<int, string> SurveyIdsList = new Dictionary<int, string>();
                            SurveyIdsList.Add(GetViewId(pRequestMessage.XML), SurveyId.ToString());
                            result.ViewIdAndFormIdList = SurveyIdsList;
                        }
                        catch (Exception ex)
                        {
                            System.Console.Write(ex.ToString());
                            //Entities.ObjectStateManager.GetObjectStateEntry(SurveyMetaData).Delete();
                            result.URL = "";
                            result.IsPulished = false;
                            result.StatusText = "An Error has occurred while publishing your survey.";
                        }




                    }
                    else
                    {

                        result.URL = "";
                        result.IsPulished = false;
                        result.StatusText = "One or more survey required fields are missing values.";
                    }

                }
                else
                {

                    result.URL = "";
                    result.IsPulished = false;
                    result.StatusText = "Organization Key is invalid.";

                }
            }
            return result;
        }


        private SurveyRequestResultBO RePublish(SurveyInfoBO pRequestMessage)
        {

            SurveyRequestResultBO result = new SurveyRequestResultBO();
            string _Xml = pRequestMessage.XML;
            var SurveyId = new Guid(pRequestMessage.SurveyId);

            if (pRequestMessage != null)
            {

                //if (! string.IsNullOrEmpty(pRequestMessage.SurveyNumber)  &&  ValidateOrganizationKey(pRequestMessage.OrganizationKey))
                if (ValidateOrganizationKey(pRequestMessage.OrganizationKey))
                {

                    if (ValidateSurveyFields(pRequestMessage))
                    {
                        try
                        {

                            Epi.Web.Common.BusinessObject.SurveyInfoBO BO = new Epi.Web.Common.BusinessObject.SurveyInfoBO();

                            BO.SurveyId = SurveyId.ToString();
                            BO.ClosingDate = pRequestMessage.ClosingDate;

                            BO.IntroductionText = pRequestMessage.IntroductionText;
                            BO.ExitText = pRequestMessage.ExitText;
                            BO.DepartmentName = pRequestMessage.DepartmentName;
                            BO.OrganizationName = pRequestMessage.OrganizationName;

                            BO.SurveyNumber = pRequestMessage.SurveyNumber;
                            XDocument xdoc = XDocument.Parse(pRequestMessage.XML);
                            xdoc.Descendants().Where(e => e.Name == "SourceTable").Remove();
                            BO.XML = xdoc.ToString();


                            BO.SurveyName = pRequestMessage.SurveyName;

                            BO.SurveyType = pRequestMessage.SurveyType;
                            BO.UserPublishKey = pRequestMessage.UserPublishKey;
                            BO.OrganizationKey = pRequestMessage.OrganizationKey;
                            BO.OrganizationKey = pRequestMessage.OrganizationKey;
                            BO.TemplateXMLSize = pRequestMessage.TemplateXMLSize;
                            BO.IsDraftMode = pRequestMessage.IsDraftMode;
                            BO.StartDate = pRequestMessage.StartDate;
                            if (pRequestMessage.IsSqlProject)
                            {
                                this.SurveyInfoDao.ValidateServername(pRequestMessage);
                            }
                            BO.IsSqlProject = pRequestMessage.IsSqlProject;

                            this.SurveyInfoDao.UpdateSurveyInfo(BO);
                            ReSetSourceTable(_Xml, SurveyId.ToString());
                            result.URL = GetURL(pRequestMessage, SurveyId);
                            result.IsPulished = true;
                            //Updateing Connection string..
                            if (pRequestMessage.IsSqlProject)
                            {
                                DbConnectionStringBO DbConnectionStringBO = new DbConnectionStringBO();
                                DbConnectionStringBO = GetConnection(pRequestMessage.DBConnectionString);
                                DbConnectionStringBO.SurveyId = SurveyId;
                                //  this.SurveyInfoDao.InsertConnectionString(DbConnectionStringBO);
                                this.SurveyInfoDao.UpdateConnectionString(DbConnectionStringBO);
                            }
                            // View and ID setup
                            Dictionary<int, string> SurveyIdsList = new Dictionary<int, string>();
                            SurveyIdsList.Add(GetViewId(pRequestMessage.XML), SurveyId.ToString());
                            result.ViewIdAndFormIdList = SurveyIdsList;

                        }
                        catch (Exception ex)
                        {
                            System.Console.Write(ex.ToString());
                            //Entities.ObjectStateManager.GetObjectStateEntry(SurveyMetaData).Delete();
                            result.URL = "";
                            result.IsPulished = false;
                            result.StatusText = "An Error has occurred while publishing your survey.";
                        }




                    }
                    else
                    {

                        result.URL = "";
                        result.IsPulished = false;
                        result.StatusText = "One or more survey required fields are missing values.";
                    }

                }
                else
                {

                    result.URL = "";
                    result.IsPulished = false;
                    result.StatusText = "Organization Key is invalid.";

                }
            }
            return result;
        }

        private static bool ValidateSurveyFields(SurveyInfoBO pRequestMessage)
        {

            bool isValid = true;


            if (pRequestMessage.ClosingDate == null)
            {

                isValid = false;

            }

            else if (string.IsNullOrEmpty(pRequestMessage.XML) || string.IsNullOrWhiteSpace(pRequestMessage.XML))
            {

                isValid = false;
            }
            else if (string.IsNullOrEmpty(pRequestMessage.SurveyName))
            {

                isValid = false;
            }

            else if (string.IsNullOrEmpty(pRequestMessage.UserPublishKey.ToString()))
            {

                isValid = false;
            }



            return isValid;
        }

        /// <summary>
        /// validate the Organization key passed with the list of Organization keys retrieved from database 
        /// through EF
        /// </summary>
        /// <param name="OrganizationKey"></param>
        /// <returns></returns>
        private bool ValidateOrganizationKey(Guid gOrganizationKey)
        {
            string strOrgKeyEncrypted = Epi.Web.Common.Security.Cryptography.Encrypt(gOrganizationKey.ToString());
            List<OrganizationBO> OrganizationBoList = this.OrganizationDao.GetOrganizationInfoByOrgKey(strOrgKeyEncrypted);
            if (OrganizationBoList.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }


        }

        #endregion

        #region "Private members"
        private string GetURL(SurveyInfoBO pRequestMessage, Guid SurveyId)
        {
            System.Text.StringBuilder URL = new System.Text.StringBuilder();
            URL.Append(System.Configuration.ConfigurationManager.AppSettings["URL"]);
            // URL.Append("/");
            //URL.Append(pRequestMessage.SurveyNumber.ToString());
            //URL.Append("/");
            URL.Append(SurveyId.ToString());
            return URL.ToString();
        }
        #endregion
        private bool IsRelatedForm(string Xml)
        {

            bool IsRelatedForm = false;
            XDocument xdoc = XDocument.Parse(Xml);


            int NumberOfViews = xdoc.Descendants("View").Count();
            if (NumberOfViews > 1)
            {
                IsRelatedForm = true;

            }

            return IsRelatedForm;

        }
        private int GetViewId(string Xml)
        {

            int ViewId = 0;
            XDocument xdoc = XDocument.Parse(Xml);

            XElement ViewElement = xdoc.XPathSelectElement("Template/Project/View");

            int.TryParse(ViewElement.Attribute("ViewId").Value.ToString(), out ViewId);

            return ViewId;

        }
        //private SurveyRequestResultBO Publish(SurveyInfoBO pRequestMessage)
        //    {
        //    SurveyRequestResultBO result = new SurveyRequestResultBO();
        //    string _Xml = pRequestMessage.XML;

        //        var SurveyId = Guid.NewGuid();

        //        if (pRequestMessage != null)
        //            {

        //           if (! string.IsNullOrEmpty(pRequestMessage.SurveyNumber)  &&  ValidateOrganizationKey(pRequestMessage.OrganizationKey))
        //                //if (ValidateOrganizationKeyByUser(pRequestMessage.OrganizationKey, pRequestMessage.OwnerId))//EW-96
        //                {

        //                if (ValidateSurveyFields(pRequestMessage))
        //                    {
        //                    try
        //                        {
        //                            if (pRequestMessage.IsSqlProject)
        //                                 this.SurveyInfoDao.ValidateServername(pRequestMessage);
        //                            XDocument xdoc = XDocument.Parse(pRequestMessage.XML);
        //                            xdoc.Descendants().Where(e => e.Name == "SourceTable").Remove();
        //                            pRequestMessage.XML = xdoc.ToString();
        //                            var BO = ToBusinessObject(pRequestMessage, SurveyId);
        //                            this.SurveyInfoDao.InsertSurveyInfo(BO);

        //                        //Insert Connection string..
        //                        if (pRequestMessage.IsSqlProject)
        //                            {
        //                        DbConnectionStringBO DbConnectionStringBO = new DbConnectionStringBO();
        //                        DbConnectionStringBO = GetConnection(pRequestMessage.DBConnectionString);
        //                        DbConnectionStringBO.SurveyId = SurveyId;
        //                        this.SurveyInfoDao.InsertConnectionString(DbConnectionStringBO);
        //                            }
        //                        // Set Survey Settings
        //                        this.SurveyInfoDao.InsertFormdefaultSettings(SurveyId.ToString(), pRequestMessage.IsSqlProject,GetSurveyControls(BO));
        //                        Dictionary<int, string> SurveyIdsList = new Dictionary<int, string>();
        //                        SurveyIdsList.Add(GetViewId(pRequestMessage.XML), SurveyId.ToString());
        //                        result.ViewIdAndFormIdList = SurveyIdsList;
        //                        result.URL = GetURL(pRequestMessage, SurveyId);
        //                        result.IsPulished = true;
        //                        }
        //                    catch (Exception ex)
        //                        {
        //                        System.Console.Write(ex.ToString());
        //                        //Entities.ObjectStateManager.GetObjectStateEntry(SurveyMetaData).Delete();
        //                        result.URL = "";
        //                        result.IsPulished = false;
        //                        result.StatusText = "An Error has occurred while publishing your survey.";
        //                        }




        //                    }
        //                else
        //                    {

        //                    result.URL = "";
        //                    result.IsPulished = false;
        //                    result.StatusText = "One or more survey required fields are missing values.";
        //                    }

        //                }
        //            else
        //                {

        //                result.URL = "";
        //                result.IsPulished = false;
        //                result.StatusText = "Organization Key is invalid.";

        //                }
        //            }
        //        SetSourceTable(_Xml, SurveyId.ToString());
        //    return result;
        //    }
        private static SurveyInfoBO ToBusinessObject(SurveyInfoBO pRequestMessage, Guid SurveyId)
        {
            Epi.Web.Common.BusinessObject.SurveyInfoBO BO = new Epi.Web.Common.BusinessObject.SurveyInfoBO();
            BO.SurveyId = SurveyId.ToString();
            BO.ClosingDate = pRequestMessage.ClosingDate;

            BO.IntroductionText = pRequestMessage.IntroductionText;
            BO.ExitText = pRequestMessage.ExitText;
            BO.DepartmentName = pRequestMessage.DepartmentName;
            BO.OrganizationName = pRequestMessage.OrganizationName;

            BO.SurveyNumber = pRequestMessage.SurveyNumber;

            BO.XML = pRequestMessage.XML;

            BO.SurveyName = pRequestMessage.SurveyName;

            BO.SurveyType = pRequestMessage.SurveyType;
            BO.UserPublishKey = pRequestMessage.UserPublishKey;
            BO.OrganizationKey = pRequestMessage.OrganizationKey;
            BO.OrganizationKey = pRequestMessage.OrganizationKey;
            BO.TemplateXMLSize = pRequestMessage.TemplateXMLSize;
            BO.IsDraftMode = pRequestMessage.IsDraftMode;
            BO.StartDate = pRequestMessage.StartDate;
            BO.ViewId = pRequestMessage.ViewId;
            //BO.ParentId = pRequestMessage.ParentId;// Removed parent Id for the first step of publish
            // BO.OwnerId = pRequestMessage.OwnerId;
            BO.IsSqlProject = pRequestMessage.IsSqlProject;
            //BO.IsShareable = pRequestMessage.IsShareable;
            // BO.DataAccessRuleId = pRequestMessage.DataAccessRuleId;
            //Insert Survey MetaData
            return BO;
        }
        private DbConnectionStringBO GetConnection(string ConnectionString)
        {
            DbConnectionStringBO DbConnectionStringBO = new Epi.Web.Common.BusinessObject.DbConnectionStringBO();
            //string connStr = "Data Source=SERVERx;Initial Catalog=DBx;User ID=u;Password=p";
            // string connStr =  "Data Source=ETIEX-022/SQLEXPRESS;Initial Catalog=TestEpi;Integrated Security=True";
            var csb = new SqlConnectionStringBuilder(ConnectionString);

            DbConnectionStringBO.DatasourceServerName = csb.DataSource;
            DbConnectionStringBO.InitialCatalog = csb.InitialCatalog;
            DbConnectionStringBO.Password = csb.Password;
            DbConnectionStringBO.DatabaseUserID = csb.UserID;
            DbConnectionStringBO.PersistSecurityInfo = csb.IntegratedSecurity.ToString();

            DbConnectionStringBO.DatabaseType = "SQL";
            return DbConnectionStringBO;
        }
        //private SurveyRequestResultBO RePublish(SurveyInfoBO pRequestMessage)
        //    {

        //    SurveyRequestResultBO result = new SurveyRequestResultBO();
        //    string _Xml = pRequestMessage.XML;
        //        var SurveyId = new Guid(pRequestMessage.SurveyId);

        //        if (pRequestMessage != null)
        //            {

        //            if (! string.IsNullOrEmpty(pRequestMessage.SurveyNumber)  &&  ValidateOrganizationKey(pRequestMessage.OrganizationKey))                    
        //               // if (ValidateOrganizationKeyByUser(pRequestMessage.OrganizationKey,pRequestMessage.OwnerId))//EW-96
        //                {

        //                if (ValidateSurveyFields(pRequestMessage))
        //                    {
        //                    try
        //                        {

        //                            if (pRequestMessage.IsSqlProject)
        //                                this.SurveyInfoDao.ValidateServername(pRequestMessage);
        //                            XDocument xdoc = XDocument.Parse(pRequestMessage.XML);
        //                           xdoc.Descendants().Where(e => e.Name == "SourceTable").Remove();
        //                           pRequestMessage.XML = xdoc.ToString();
        //                            this.SurveyInfoDao.UpdateSurveyInfo(ToBusinessObject(pRequestMessage, SurveyId));
        //                        ////Insert Connection string..
        //                        //DbConnectionStringBO DbConnectionStringBO = new DbConnectionStringBO();
        //                        //DbConnectionStringBO = GetConnection(pRequestMessage.DBConnectionString);
        //                        //DbConnectionStringBO.SurveyId = SurveyId;
        //                        //this.SurveyInfoDao.InsertConnectionString(DbConnectionStringBO);
        //                        var BO = ToBusinessObject(pRequestMessage, SurveyId);
        //                        this.SurveyInfoDao.InsertFormdefaultSettings(SurveyId.ToString(), pRequestMessage.IsSqlProject, GetSurveyControls(BO));
        //                        Dictionary<int, string> SurveyIdsList = new Dictionary<int, string>();
        //                        SurveyIdsList.Add(GetViewId(pRequestMessage.XML), SurveyId.ToString());
        //                        result.ViewIdAndFormIdList = SurveyIdsList;
        //                        result.URL = GetURL(pRequestMessage, SurveyId);
        //                        result.IsPulished = true;
        //                        ReSetSourceTable(_Xml, SurveyId.ToString());
        //                        }
        //                    catch (Exception ex)
        //                        {
        //                        System.Console.Write(ex.ToString());
        //                        //Entities.ObjectStateManager.GetObjectStateEntry(SurveyMetaData).Delete();
        //                        result.URL = "";
        //                        result.IsPulished = false;
        //                        result.StatusText = "An Error has occurred while publishing your survey.";
        //                        }





        //                    }
        //                else
        //                    {

        //                    result.URL = "";
        //                    result.IsPulished = false;
        //                    result.StatusText = "One or more survey required fields are missing values.";
        //                    }

        //                }
        //            else
        //                {

        //                result.URL = "";
        //                result.IsPulished = false;
        //                result.StatusText = "Organization Key is invalid.";

        //                }
        //            }

        //    return result;
        //    }
        private SurveyRequestResultBO RePublishRelatedFormSurvey(SurveyInfoBO pRequestMessage)
        {
            SurveyRequestResultBO SurveyRequestResultBO = new Web.Common.BusinessObject.SurveyRequestResultBO();
            Dictionary<int, int> ViewIds = new Dictionary<int, int>();
            Dictionary<int, string> SurveyIds = new Dictionary<int, string>();
            string _Xml = pRequestMessage.XML;
            List<SurveyInfoBO> FormsHierarchyIds = this.GetFormsHierarchyIdsByRootId(pRequestMessage.SurveyId.ToString());
            // 1- breck down the xml to n views
            List<string> XmlList = new List<string>();
            XmlList = XmlChunking(pRequestMessage.XML);
            string ParentId = "";

            // 2- call publish() with each of the views
            foreach (string Xml in XmlList)
            {
                XDocument xdoc = XDocument.Parse(Xml);
                SurveyInfoBO SurveyInfoBO = new SurveyInfoBO();
                XElement ViewElement = xdoc.XPathSelectElement("Template/Project/View");
                int ViewId;
                int.TryParse(ViewElement.Attribute("ViewId").Value.ToString(), out ViewId);

                GetRelateViewIds(ViewElement, ViewId);

                SurveyInfoBO = pRequestMessage;
                SurveyInfoBO.XML = Xml;
                SurveyInfoBO.SurveyName = ViewElement.Attribute("Name").Value.ToString();
                SurveyInfoBO.ViewId = ViewId;
                var ViewExists = FormsHierarchyIds.Where(x => x.ViewId == ViewId);
                if (ViewExists.Count() > 0)
                {
                    SurveyInfoBO pBO = FormsHierarchyIds.Single(x => x.ViewId == ViewId);
                    SurveyInfoBO.SurveyId = pBO.SurveyId;
                    SurveyInfoBO.ParentId = pBO.ParentId;
                    SurveyInfoBO.UserPublishKey = pBO.UserPublishKey;
                    //SurveyInfoBO.OwnerId = pRequestMessage.OwnerId;
                    SurveyInfoBO.IsSqlProject = pRequestMessage.IsSqlProject;
                    // SurveyInfoBO.IsShareable = pRequestMessage.IsShareable;
                    SurveyInfoBO.DBConnectionString = pRequestMessage.DBConnectionString;
                    SurveyRequestResultBO = RePublish(SurveyInfoBO);
                }
                else
                {
                    SurveyInfoBO.XML = Xml;
                    SurveyInfoBO.SurveyName = ViewElement.Attribute("Name").Value.ToString();
                    SurveyInfoBO.ViewId = ViewId;
                    SurveyInfoBO.ParentId = ParentId;
                    //SurveyInfoBO.OwnerId = pRequestMessage.OwnerId;
                    SurveyInfoBO.IsSqlProject = pRequestMessage.IsSqlProject;
                    //  SurveyInfoBO.IsShareable = pRequestMessage.IsShareable;
                    SurveyInfoBO.DBConnectionString = pRequestMessage.DBConnectionString;
                    SurveyRequestResultBO = Publish(SurveyInfoBO);

                }
                ParentId = SurveyRequestResultBO.ViewIdAndFormIdList[ViewId];
                SurveyIds.Add(ViewId, ParentId);

            }
            foreach (var _ViewId in this.ViewIds)
            {

                string PId = SurveyIds[_ViewId.Value].ToString();
                string SId = SurveyIds[_ViewId.Key].ToString();
                this.SurveyInfoDao.UpdateParentId(SId, _ViewId.Key, PId);

            }
            var _SId = SurveyIds.Keys.ElementAt(0);

            ReSetSourceTable(_Xml, FormsHierarchyIds[0].SurveyId);
            SurveyRequestResultBO.URL = GetURL(pRequestMessage, new Guid(FormsHierarchyIds[0].SurveyId));
            return SurveyRequestResultBO;
        }

        private List<SurveyInfoBO> GetFormsHierarchyIdsByRootId(string RootId)
        {
            List<SurveyInfoBO> FormsHierarchyIds = new List<SurveyInfoBO>();
            FormsHierarchyIds = this.SurveyInfoDao.GetFormsHierarchyIdsByRootId(RootId);
            return FormsHierarchyIds;
        }
        private SurveyRequestResultBO PublishRelatedFormSurvey(SurveyInfoBO pRequestMessage)
        {
            string _Xml = pRequestMessage.XML;
            SurveyRequestResultBO SurveyRequestResultBO = new Web.Common.BusinessObject.SurveyRequestResultBO();
            // Dictionary<int, int> ViewIds = new Dictionary<int, int>();
            Dictionary<int, string> SurveyIds = new Dictionary<int, string>();
            string ParentId = "";

            // 1- breck down the xml to n views
            List<string> XmlList = new List<string>();
            XmlList = XmlChunking(pRequestMessage.XML);

            // 2- call publish() with each of the views
            foreach (string Xml in XmlList) //Pain Point
            {
                XDocument xdoc = XDocument.Parse(Xml);
                SurveyInfoBO SurveyInfoBO = new SurveyInfoBO();
                XElement ViewElement = xdoc.XPathSelectElement("Template/Project/View");
                int _ViewId;
                int.TryParse(ViewElement.Attribute("ViewId").Value.ToString(), out _ViewId);

                GetRelateViewIds(ViewElement, _ViewId);

                SurveyInfoBO = pRequestMessage;
                SurveyInfoBO.XML = Xml;
                SurveyInfoBO.SurveyName = ViewElement.Attribute("Name").Value.ToString();
                SurveyInfoBO.ViewId = _ViewId;
                SurveyInfoBO.ParentId = ParentId;
                // SurveyInfoBO.OwnerId = pRequestMessage.OwnerId ;
                SurveyInfoBO.IsSqlProject = pRequestMessage.IsSqlProject;
                // SurveyInfoBO.IsShareable = pRequestMessage.IsShareable;
                SurveyInfoBO.DBConnectionString = pRequestMessage.DBConnectionString;
                SurveyRequestResultBO = Publish(SurveyInfoBO); //temp
                                                               // ParentId = SurveyRequestResultBO.URL.Split('/').Last();

                if (SurveyRequestResultBO.ViewIdAndFormIdList != null)
                {
                    ParentId = SurveyRequestResultBO.ViewIdAndFormIdList[_ViewId];
                }
                SurveyIds.Add(_ViewId, ParentId);

            }


            foreach (var ViewId in this.ViewIds)
            {
                try
                {
                    string PId = SurveyIds[ViewId.Value].ToString();
                    string SId = SurveyIds[ViewId.Key].ToString();
                    this.SurveyInfoDao.UpdateParentId(SId, ViewId.Key, PId);
                }
                catch (Exception ex)
                {

                    throw ex;
                }

            }
            SetSourceTable(_Xml, SurveyIds[1].ToString());
            SurveyRequestResultBO.ViewIdAndFormIdList = SurveyIds;
            // SurveyRequestResultBO.URL = SurveyRequestResultBO.URL.Remove(SurveyRequestResultBO.URL.LastIndexOf('/'));
            SurveyRequestResultBO.URL = GetURL(pRequestMessage, new Guid(SurveyIds[1].ToString()));
            return SurveyRequestResultBO;
        }
        private void GetRelateViewIds(XElement ViewElement, int ViewId)
        {

            var _RelateFields = from _Field in
                                    ViewElement.Descendants("Field")
                                where _Field.Attribute("FieldTypeId").Value == "20"
                                select _Field;

            foreach (var Item in _RelateFields)
            {

                int RelateViewId = 0;
                int.TryParse(Item.Attribute("RelatedViewId").Value, out RelateViewId);
                if (!this.ViewIds.ContainsKey(RelateViewId))
                {
                    this.ViewIds.Add(RelateViewId, ViewId);
                }
            }


        }
        private void SetSourceTable(string Xml, string FormId)
        {

            XDocument xdoc1 = XDocument.Parse(Xml);
            foreach (XElement Xelement in xdoc1.Descendants("Template").Elements("SourceTable"))
            {
                //  Xelement.ToString()
                string SourcetableName = Xelement.Attribute("TableName").Value;
                this.SurveyInfoDao.InsertSourceTable(Xelement.ToString(), SourcetableName, FormId);
            }

        }
        private void ReSetSourceTable(string Xml, string FormId)
        {
            var SourceTables = this.SurveyInfoDao.GetSourceTables(FormId);
            XDocument xdoc1 = XDocument.Parse(Xml);
            foreach (XElement Xelement in xdoc1.Descendants("Template").Elements("SourceTable"))
            {
                //  Xelement.ToString()
                string SourcetableName = Xelement.Attribute("TableName").Value;

                this.SurveyInfoDao.UpdateSourceTable(Xelement.ToString(), SourcetableName, FormId);

                var Table = SourceTables.Where(x => x.TableName == SourcetableName);
                if (Table.Count() > 0)
                {
                    this.SurveyInfoDao.UpdateSourceTable(Xelement.ToString(), SourcetableName, FormId);
                }
                else
                {

                    this.SurveyInfoDao.InsertSourceTable(Xelement.ToString(), SourcetableName, FormId);
                }
            }


        }
        private List<string> XmlChunking(string Xml)
        {
            List<string> XmlList = new List<string>();
            XDocument xdoc = XDocument.Parse(Xml);
            XDocument xdoc1 = XDocument.Parse(Xml);

            xdoc.Descendants("View").Remove();

            xdoc.Descendants().Where(e => e.Name == "SourceTable").Remove(); //Remove the recouce table for the new process
            foreach (XElement Xelement in xdoc1.Descendants("Project").Elements("View"))
            {

                //xdoc.Element("Project").Add(Xelement);
                xdoc.Root.Element("Project").Add(Xelement);
                XmlList.Add(xdoc.ToString());
                xdoc.Descendants("View").Remove();

            }

            return XmlList;
        }
        private List<string> GetSurveyControls(SurveyInfoBO SurveyInfoBO)
        {
            List<string> List = new List<string>();

            XDocument xdoc = XDocument.Parse(SurveyInfoBO.XML);

            var _FieldsTypeIDs = from _FieldTypeID in
                                 xdoc.Descendants("Field")
                                 select _FieldTypeID;

            string fieldType = "";
            int counter = 0;
            foreach (var _FieldTypeID in _FieldsTypeIDs)
            {
                fieldType = _FieldTypeID.Attribute("FieldTypeId").Value;

                if (fieldType != "2" && fieldType != "21" && fieldType != "3" && fieldType != "20" && fieldType != "4" && fieldType != "13")
                {
                    List.Add(_FieldTypeID.Attribute("Name").Value.ToString());
                    counter++;
                }
                if (counter == 5)
                {
                    break;
                }
            }
            return List;
        }
    }
    }

