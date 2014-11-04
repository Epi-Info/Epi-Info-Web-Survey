using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Enter.Common.BusinessObject;
using Epi.Web.Enter.Common.Criteria;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
namespace Epi.Web.BLL
{

  public  class SurveyInfo
    {
      private Epi.Web.Enter.Interfaces.DataInterfaces.ISurveyInfoDao SurveyInfoDao;
      Dictionary<int, int> ViewIds = new Dictionary<int, int>();

        public SurveyInfo(Epi.Web.Enter.Interfaces.DataInterfaces.ISurveyInfoDao pSurveyInfoDao)
        {
            this.SurveyInfoDao = pSurveyInfoDao;
        }

        public SurveyInfoBO GetSurveyInfoById(string pId)
        {
            List<string> IdList = new List<string>();
            if (! string.IsNullOrEmpty(pId))
            {
                IdList.Add(pId);
            }
            List<SurveyInfoBO> result = this.SurveyInfoDao.GetSurveyInfo(IdList);
            if (result.Count > 0)
            {
                return result[0];
            }
            else
            {
                return null;
            }
        }


     

        /// <summary>
        /// Gets SurveyInfo based on criteria
        /// </summary>
        /// <param name="SurveyInfoId">Unique SurveyInfo identifier.</param>
        /// <returns>SurveyInfo.</returns>
        public List<SurveyInfoBO> GetSurveyInfoById(List<string> pIdList)
        {
            List<SurveyInfoBO> result = this.SurveyInfoDao.GetSurveyInfo(pIdList);
            return result;
        }

        public PageInfoBO GetSurveySizeInfo(List<string> pIdList,int BandwidthUsageFactor, int pResponseMaxSize = -1)
        { 
            List<SurveyInfoBO> SurveyInfoBOList = this.SurveyInfoDao.GetSurveySizeInfo(pIdList, -1, -1, pResponseMaxSize);

            PageInfoBO result = new PageInfoBO();

            result = Epi.Web.BLL.Common.GetSurveySize(SurveyInfoBOList,BandwidthUsageFactor, pResponseMaxSize);
            return result;


        }


        public bool IsSurveyInfoValidByOrgKeyAndPublishKey(string SurveyId, string Okey, Guid publishKey)
        {

            string EncryptedKey = Epi.Web.Enter.Common.Security.Cryptography.Encrypt(Okey);
            List<SurveyInfoBO> result = this.SurveyInfoDao.GetSurveyInfoByOrgKeyAndPublishKey(SurveyId, EncryptedKey, publishKey);

             
            if (result != null && result.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        public bool IsSurveyInfoValidByOrgKey(string SurveyId, string pOrganizationKey)
        {

            string EncryptedKey = Epi.Web.Enter.Common.Security.Cryptography.Encrypt(pOrganizationKey);
            List<SurveyInfoBO> result = this.SurveyInfoDao.GetSurveyInfoByOrgKey(SurveyId, EncryptedKey);


            if (result != null && result.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



      /// <summary>
        /// Gets SurveyInfo based on criteria
        /// </summary>
        /// <param name="SurveyInfoId">Unique SurveyInfo identifier.</param>
        /// <returns>SurveyInfo.</returns>
        public List<SurveyInfoBO> GetSurveyInfo(List<string> SurveyInfoIdList, DateTime pClosingDate, string Okey, int pSurveyType = -1, int pPageNumber = -1, int pPageSize = -1)
        {
            string EncryptedKey = Epi.Web.Enter.Common.Security.Cryptography.Encrypt(Okey);
            List<SurveyInfoBO> result = this.SurveyInfoDao.GetSurveyInfo(SurveyInfoIdList, pClosingDate, EncryptedKey, pSurveyType, pPageNumber, pPageSize);
            return result;
        }
        public PageInfoBO GetSurveySizeInfo(List<string> SurveyInfoIdList, DateTime pClosingDate, string Okey, int BandwidthUsageFactor, int pSurveyType = -1, int pPageNumber = -1, int pPageSize = -1, int pResponseMaxSize = -1)
        {

            string EncryptedKey = Epi.Web.Enter.Common.Security.Cryptography.Encrypt(Okey);

            List<SurveyInfoBO> SurveyInfoBOList = this.SurveyInfoDao.GetSurveySizeInfo(SurveyInfoIdList, pClosingDate, EncryptedKey, pSurveyType, pPageNumber, pPageSize, pResponseMaxSize);

            PageInfoBO result = new PageInfoBO();

            result = Epi.Web.BLL.Common.GetSurveySize(SurveyInfoBOList, BandwidthUsageFactor, pResponseMaxSize);
            return result;

        }
      
        public SurveyInfoBO InsertSurveyInfo(SurveyInfoBO pValue)
        {
            SurveyInfoBO result = pValue;
            this.SurveyInfoDao.InsertSurveyInfo(pValue);
            return result;
        }
        public SurveyInfoBO UpdateSurveyInfo(SurveyInfoBO pRequestMessage)
        {
        SurveyInfoBO result = pRequestMessage;
        if (ValidateSurveyFields(pRequestMessage))
            {
            if (this.IsRelatedForm(pRequestMessage.XML))
                    {

                    List<SurveyInfoBO> FormsHierarchyIds = this.GetFormsHierarchyIds(pRequestMessage.SurveyId.ToString());
                    
                    // 1- breck down the xml to n views
                    List<string> XmlList = new List<string>();
                    XmlList = XmlChunking(pRequestMessage.XML);

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

                        SurveyInfoBO pBO = FormsHierarchyIds.Single(x => x.ViewId == ViewId);
                        SurveyInfoBO.SurveyId = pBO.SurveyId;
                        SurveyInfoBO.ParentId = pBO.ParentId;
                        SurveyInfoBO.UserPublishKey = pBO.UserPublishKey;
                        SurveyInfoBO.OwnerId = pRequestMessage.OwnerId;

                        this.SurveyInfoDao.UpdateSurveyInfo(SurveyInfoBO);


                        }
                    }
                else
                    {

                    this.SurveyInfoDao.UpdateSurveyInfo(pRequestMessage);
                    }
                result.StatusText = "Successfully updated survey information.";
            }else{
                result.StatusText = "One or more survey required fields are missing values.";
            
            }
            
            return result;
        }

        public bool DeleteSurveyInfo(SurveyInfoBO pValue)
        {
            bool result = false;

            this.SurveyInfoDao.DeleteSurveyInfo(pValue);
            result = true;

            return result;
        }
        private static bool ValidateSurveyFields(SurveyInfoBO pRequestMessage)
        {

            bool isValid = true;


            if (pRequestMessage.ClosingDate == null)
            {

                isValid = false;

            }

           
            else if (string.IsNullOrEmpty(pRequestMessage.SurveyName))
            {

                isValid = false;
            }
 



            return isValid;
        }


        public List<SurveyInfoBO> GetChildInfoByParentId(Dictionary<string ,int > ParentIdList)
            {
            List<SurveyInfoBO> result = new List<SurveyInfoBO>();
            foreach (KeyValuePair<string, int> item in ParentIdList)
                {
                result = this.SurveyInfoDao.GetChildInfoByParentId(item.Key, item.Value);
                }
            return result;
            }
        public SurveyInfoBO GetParentInfoByChildId(string ChildId)
            {
            SurveyInfoBO result = new SurveyInfoBO();

            result = this.SurveyInfoDao.GetParentInfoByChildId(ChildId);
              
            return result;
            }
        public List<FormsHierarchyBO> GetFormsHierarchyIdsByRootId(string RootId)
            {
            List<SurveyInfoBO> SurveyInfoBOList = new List<SurveyInfoBO>();
            List<FormsHierarchyBO> result = new List<FormsHierarchyBO>();

            SurveyInfoBOList = this.SurveyInfoDao.GetFormsHierarchyIdsByRootId(RootId);
            foreach (var item in SurveyInfoBOList)
                {
                FormsHierarchyBO FormsHierarchyBO = new FormsHierarchyBO();
                FormsHierarchyBO.ViewId = item.ViewId;
                FormsHierarchyBO.FormId = item.SurveyId;
                if (item.SurveyId == RootId)
                    {
                    FormsHierarchyBO.IsRoot = true;
                    }
                result.Add(FormsHierarchyBO);
                }

            return result;

            }
        private List<SurveyInfoBO> GetFormsHierarchyIds(string RootId)
            {
            List<SurveyInfoBO> FormsHierarchyIds = new List<SurveyInfoBO>();
            FormsHierarchyIds = this.SurveyInfoDao.GetFormsHierarchyIdsByRootId(RootId);
            return FormsHierarchyIds;
            }
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

                this.ViewIds.Add(RelateViewId, ViewId);
                }


            }

        private List<string> XmlChunking(string Xml)
            {
            List<string> XmlList = new List<string>();
            XDocument xdoc = XDocument.Parse(Xml);
            XDocument xdoc1 = XDocument.Parse(Xml);

            xdoc.Descendants("View").Remove();

            foreach (XElement Xelement in xdoc1.Descendants("Project").Elements("View"))
                {

                //xdoc.Element("Project").Add(Xelement);
                xdoc.Root.Element("Project").Add(Xelement);
                XmlList.Add(xdoc.ToString());
                xdoc.Descendants("View").Remove();
                }

            return XmlList;
            }
    }
}
