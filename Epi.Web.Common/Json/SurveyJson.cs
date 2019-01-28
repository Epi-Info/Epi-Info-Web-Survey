using Epi.Web.Common.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Epi.Web.Common.Json
{
   public class SurveyResponseJson
    {

        public string GetSurveyResponseJson(Epi.Web.Common.DTO.SurveyAnswerDTO surveyAnswer, List<FormsHierarchyDTO> FormsHierarchyDTOList)
        {
            ResponseDetail Responsedetail = new ResponseDetail();

            var ChildFormsHierarchy = FormsHierarchyDTOList.Where(x => x.IsRoot == false);
            Dictionary<string, string> ResponseQA = new Dictionary<string, string>();
            Dictionary<string, string> RootResponseQA = new Dictionary<string, string>();

            XDocument xdoc = XDocument.Parse(surveyAnswer.XML);
            int NumberOfPages = GetNumberOfPags(surveyAnswer.XML);           

            Responsedetail.ResponseId = surveyAnswer.ResponseId;
            Responsedetail.FormId = surveyAnswer.SurveyId;

            for (int i = 1; NumberOfPages + 1 > i; i++)
            {
                var _FieldsTypeIDs = from _FieldTypeID in
               xdoc.Descendants("Page")

                                     where _FieldTypeID.Attribute("PageNumber").Value == (i).ToString()
                                     select _FieldTypeID;

                var _PageFieldsTypeIDs = from _FieldTypeID1 in
                                             _FieldsTypeIDs.Descendants("ResponseDetail")

                                         select _FieldTypeID1;
                foreach (var item in _PageFieldsTypeIDs)
                {
                    RootResponseQA.Add(item.Attribute("QuestionName").Value, item.Value);
                }

            }
            Responsedetail.ResponseQA = RootResponseQA;

            foreach (var child in ChildFormsHierarchy)
            {
                List<SurveyAnswerDTO> childResponses = child.ResponseIds;
                foreach (var childresponse in childResponses)
                {
                    ResponseDetail childresponseDetail = new ResponseDetail();
                    childresponseDetail.FormId = childresponse.SurveyId;
                    childresponseDetail.ResponseId = childresponse.ResponseId;
                    childresponseDetail.ParentResponseId = childresponse.RelateParentId;
                    childresponseDetail.ParentFormId = childresponse.ParentRecordId;
                    ResponseQA = new Dictionary<string, string>();
                    ResponseQA.Add("FKEY", childresponse.RelateParentId);
                    ResponseQA.Add("ResponseId", childresponse.ResponseId);

                    XDocument xdochild = XDocument.Parse(childresponse.XML);
                    int NumberOfPagesChild = GetNumberOfPags(childresponse.XML);
                    for (int i = 1; NumberOfPagesChild + 1 > i; i++)
                    {
                        var _FieldsTypeIDs = from _FieldTypeID in
                       xdochild.Descendants("Page")

                                             where _FieldTypeID.Attribute("PageNumber").Value == (i).ToString()
                                             select _FieldTypeID;

                        var _PageFieldsTypeIDs = from _FieldTypeID1 in
                                                     _FieldsTypeIDs.Descendants("ResponseDetail")

                                                 select _FieldTypeID1;

                        foreach (var item in _PageFieldsTypeIDs)
                        {
                            try
                            {
                                ResponseQA.Add(item.Attribute("QuestionName").Value, item.Value);
                            }
                            catch (System.Exception ex)
                            {

                            }
                        }
                    }

                    childresponseDetail.ResponseQA = ResponseQA;
                    Responsedetail.ChildResponseDetailList.Add(childresponseDetail);
                }

            }

            var json = JsonConvert.SerializeObject(Responsedetail);

            return json;
        }


        public static int GetNumberOfPags(string ResponseXml)
        {

            XDocument xdoc = XDocument.Parse(ResponseXml);
            int PageNumber = 0;
            PageNumber = xdoc.Root.Elements("Page").Count();

            return PageNumber;


        }
    }
}
