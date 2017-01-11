using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Epi.Web.Common.DTO;
using OfficeOpenXml;
namespace Epi.Web.Common.Xml
{
    public class SurveyXml
    {
        public SurveyXml()
        {
        }

        public static XDocument BuildXml(ExcelWorkbook xlWorkbook)
        {



            SurveyXml BuildXml = new SurveyXml();
            XmlDocument RootXml = new XmlDocument();

            string RootXmlpath = System.Web.HttpContext.Current.Server.MapPath("~\\Content\\Xml\\RootTemplate.xml");
            RootXml.Load(RootXmlpath);
            XDocument XRoot = SurveyXml.ToXDocument(RootXml);
            XDocument NewXmlDoc = new XDocument(XRoot);


            // Get page list from Excel
            List<SurveyPageDTO> PageList = SetPageValues(xlWorkbook);

            // Build xml
            var xml = BuildXml.BuildNewXml(PageList, NewXmlDoc);



            return xml;

        }


        private enum EpiInfoDataTypes
        { 
            Text = 1,
            Numeric = 5,
            YesNo = 11,
            Checkbox = 10,
            Options = 12,
            Dropdown = 17,
            Date = 7,
            Time = 8
        };
        private XDocument BuildNewXml(List<SurveyPageDTO> PageList, XDocument NewXmlDoc)
        {
            //  "C:/WorkSpace/Challenge/PageTemplate.xml"
            foreach (var NewPage in PageList)
            {


                XElement XmlElement = AddPageXml(NewXmlDoc, NewPage);


                if (NewPage.List_Values != null && NewPage.List_Values.Count() > 0 && NewPage.Question_Type == 17)
                {
                    XElement SourceTableElement = AddSourceTableXml(NewPage);
                    XElement TemplaitElement = NewXmlDoc.XPathSelectElement("Template");
                    TemplaitElement.Add(SourceTableElement);
                }
                // Update Check code
                if (!string.IsNullOrEmpty(NewPage.If_Condition) && !string.IsNullOrEmpty(NewPage.Then_Question))
                {
                    XmlElement.SetAttributeValue("CheckCode", GetCheckCode(XmlElement.Attribute("CheckCode").Value, NewPage));
                }
            }

            return NewXmlDoc;

        }

        private static XElement AddPageXml(XDocument NewXmlDoc, SurveyPageDTO NewPage)
        {
            XmlDocument PageXml = new XmlDocument();
            string PageXmlpath = System.Web.HttpContext.Current.Server.MapPath("~\\Content\\Xml\\PageTemplate.xml");
            PageXml.Load(PageXmlpath);
            XDocument XPage = ToXDocument(PageXml);

            XElement PageElement = XPage.XPathSelectElement("Page");
            // change Default values
            PageElement.SetAttributeValue("PageId", NewPage.PageId);
            PageElement.SetAttributeValue("Name", NewPage.PageName);
            PageElement.SetAttributeValue("Position", NewPage.PageId - 1);
            XElement FiledElement = null;
            if (NewPage.Question_Type != 10)
            {
              FiledElement = AddControlXml(NewPage);
              PageElement.Add(FiledElement);
            }
            else
            {
                int count = 1;
                var Variable_Name = NewPage.Variable_Name;
            foreach (var checkbox in NewPage.List_Values)
            {

                NewPage.Question = checkbox;
                NewPage.Variable_Name = "checkbox_" + checkbox.Replace(" ","_");
                NewPage.Counter = count;
                FiledElement = AddControlXml(NewPage);
                PageElement.Add(FiledElement);
                count++;
            }
            NewPage.Variable_Name = Variable_Name;
            
            }
            // GroupBox Title
            if (!string.IsNullOrEmpty(NewPage.Title))
            {
            FiledElement = XPage.XPathSelectElement("Page/Field[@Name='Title']");
            FiledElement.SetAttributeValue("Name", NewPage.Variable_Name + "_Title");
            FiledElement.SetAttributeValue("PromptText", NewPage.Title);
            FiledElement.SetAttributeValue("PageId", NewPage.PageId);
            FiledElement.SetAttributeValue("FieldTypeId", 21);
            FiledElement.SetAttributeValue("UniqueId", Guid.NewGuid().ToString());
            FiledElement.SetAttributeValue("PageName", NewPage.PageName);
            FiledElement.SetAttributeValue("Position", NewPage.PageId - 1);
            FiledElement.SetAttributeValue("FieldId", NewPage.PageId + 4);
            if (NewPage.Question_Type == 12  )
            {

                FiledElement.SetAttributeValue("ControlHeightPercentage", GetPositionValue(NewPage.List_Values.Count(), 0.14, 0.04).ToString());
            }
            if ( NewPage.Question_Type == 10)
            {

                FiledElement.SetAttributeValue("ControlHeightPercentage", GetPositionValue(NewPage.List_Values.Count(), 0.14, 0.05).ToString());
            }
            }else{
                FiledElement = XPage.XPathSelectElement("Page/Field[@Name='Title']");
                FiledElement.Remove();
            
            }
            // Description
            if (!string.IsNullOrEmpty(NewPage.Description))
            {
                FiledElement = XPage.XPathSelectElement("Page/Field[@Name='Description']");
                FiledElement.SetAttributeValue("Name", NewPage.Variable_Name + "_Description");
                FiledElement.SetAttributeValue("PromptText", NewPage.Description);
                FiledElement.SetAttributeValue("PageId", NewPage.PageId);
                FiledElement.SetAttributeValue("FieldTypeId", 2);
                FiledElement.SetAttributeValue("UniqueId", Guid.NewGuid().ToString());
                FiledElement.SetAttributeValue("PageName", NewPage.PageName);
                FiledElement.SetAttributeValue("Position", NewPage.PageId - 1);
                FiledElement.SetAttributeValue("FieldId", NewPage.PageId + 5);
                if (NewPage.Question_Type == 12)
                {
                    FiledElement.SetAttributeValue("ControlTopPositionPercentage", GetPositionValue(NewPage.List_Values.Count(), 0.14, 0.04).ToString());
                }
                if (NewPage.Question_Type == 10)
                {

                    FiledElement.SetAttributeValue("ControlTopPositionPercentage", GetPositionValue(NewPage.List_Values.Count(), 0.14, 0.05).ToString());
                }
            }
            else {
                FiledElement = XPage.XPathSelectElement("Page/Field[@Name='Description']");
                FiledElement.Remove();
            
            }
            // Add page element to Xml
            XElement XmlElement = NewXmlDoc.XPathSelectElement("Template/Project/View");
            XmlElement.Add(PageElement);
            return XmlElement;
        }

        private static double GetPositionValue(int ListCount,Double StartValue,Double IncrementValue )
        {

            Double ControlPosition = StartValue;
            for (int i = 0; ListCount > i; i++)
            {
                ControlPosition = ControlPosition + IncrementValue;

            }
            return ControlPosition;
        }

        private static XElement AddControlXml(SurveyPageDTO NewPage)
        {
            // Control
           // XElement FiledElement = XPage.XPathSelectElement("Page/Field[@Name='Controls']");

            XmlDocument FieldXml = new XmlDocument();
            string PageXmlpath = System.Web.HttpContext.Current.Server.MapPath("~\\Content\\Xml\\Control.xml");
            FieldXml.Load(PageXmlpath);
            XDocument XField = ToXDocument(FieldXml);

            XElement FiledElement = XField.XPathSelectElement("Field");

            FiledElement.SetAttributeValue("IsRequired", NewPage.Required.ToString());
            FiledElement.SetAttributeValue("Name", NewPage.Variable_Name);
            FiledElement.SetAttributeValue("PromptText", NewPage.Question);
            FiledElement.SetAttributeValue("PageId", NewPage.PageId);
            FiledElement.SetAttributeValue("FieldTypeId", NewPage.Question_Type);
            FiledElement.SetAttributeValue("UniqueId", Guid.NewGuid().ToString());
            FiledElement.SetAttributeValue("PageName", NewPage.PageName);
            FiledElement.SetAttributeValue("Position", NewPage.PageId - 1);
            FiledElement.SetAttributeValue("FieldId", NewPage.PageId + 3);
            if (NewPage.Question_Type == 17)
            {
                XmlDocument SourceTableXml = new XmlDocument();
                string SourceTableXmlpath = System.Web.HttpContext.Current.Server.MapPath("~\\Content\\Xml\\SourceTable.xml");
                SourceTableXml.Load(SourceTableXmlpath);
                XDocument XSourceTable = ToXDocument(SourceTableXml);
                XElement SourceTableElement = XSourceTable.XPathSelectElement("SourceTable");
                SourceTableElement.SetAttributeValue("TableName", "code" + NewPage.Variable_Name);
                var TableName = SourceTableElement.Attribute("TableName").Value;
                FiledElement.SetAttributeValue("SourceTableName", TableName);
                FiledElement.SetAttributeValue("CodeColumnName", NewPage.Variable_Name);
                FiledElement.SetAttributeValue("TextColumnName", NewPage.Variable_Name);

            }
            if (NewPage.Question_Type == 12)
            {
                string Values = GetOptionsValue(NewPage.List_Values);
                FiledElement.SetAttributeValue("List", Values);
                FiledElement.SetAttributeValue("ShowTextOnRight", "True");
                FiledElement.SetAttributeValue("ControlWidthPercentage", "0.80");
                FiledElement.SetAttributeValue("ControlHeightPercentage", GetPositionValue(NewPage.List_Values.Count(), 0.12,0.03));
                FiledElement.SetAttributeValue("ControlLeftPositionPercentage", "0.07 ");
                FiledElement.SetAttributeValue("ControlTopPositionPercentage", "0.17");
            }
            if (NewPage.Question_Type == 10)
            {
                Double ControlTopPositionPercentage = 0.12 + (0.04 * NewPage.Counter);
                
                FiledElement.SetAttributeValue("ControlTopPositionPercentage", ControlTopPositionPercentage.ToString());
            }
            return FiledElement;
        }

        

        private static string GetOptionsValue(List<string> list)
        {
            StringBuilder Value = new StringBuilder ();
              Value.Append(  string.Join(",", list));
              Value.Append("||");
              var x = ".01231";
              
              for (int i = 1; i < list.Count()+1;i++ )
              {
                  var y = .03000;
                  y = y * i;
                  Value.Append(y.ToString() + ":" + x);
                  if (i < list.Count() )
                  {
                  Value.Append(",");
                  }
              }
            return Value.ToString();
        }

        private static XElement AddSourceTableXml(SurveyPageDTO NewPage)
        {
            XmlDocument SourceTableXml = new XmlDocument();
            string SourceTableXmlpath = System.Web.HttpContext.Current.Server.MapPath("~\\Content\\Xml\\SourceTable.xml");
            SourceTableXml.Load(SourceTableXmlpath);
            XDocument XSourceTable = ToXDocument(SourceTableXml);
            XElement SourceTableElement = XSourceTable.XPathSelectElement("SourceTable");
            SourceTableElement.SetAttributeValue("TableName", "code" + NewPage.Variable_Name);
            // SourceTableElement.SetAttributeValue("TableName", NewPage.List_Values[0]);
            if (NewPage.Question_Type == 17)
            {
                for (int i = 1; NewPage.List_Values.Count() > i; i++)
                {
                    XElement ItemElement = new XElement("Item");
                    ItemElement.SetAttributeValue(NewPage.List_Values[0], NewPage.List_Values[i]);
                    SourceTableElement.Add(ItemElement);
                }
            }
            else
            {
            for (int i = 0; NewPage.List_Values.Count()-1 > i; i++)
            {
                XElement ItemElement = new XElement("Item");
                var AttributeName = NewPage.List_Values[0].Replace(" ", "_");
                ItemElement.SetAttributeValue(AttributeName, NewPage.List_Values[i]);
                SourceTableElement.Add(ItemElement);
            }
            
            }
            return SourceTableElement;
        }

        private string GetCheckCode(string CheckCode, SurveyPageDTO NewPage)
        {
            StringBuilder _CheckCode = new StringBuilder();
            _CheckCode.Append(CheckCode);
            string text;
            string IFElsepath = System.Web.HttpContext.Current.Server.MapPath("~\\Content\\CheckCode\\IFElse.txt");

            using (var streamReader = new StreamReader(@""+IFElsepath, Encoding.UTF8))
            {
                text = streamReader.ReadToEnd();
            }
            text = string.Format(text, NewPage.PageName, NewPage.Variable_Name, NewPage.If_Condition, NewPage.Then_Question, NewPage.Else_Question);
            _CheckCode.Append("\n" + text);
            return _CheckCode.ToString();
        }
        private static XDocument ToXDocument(XmlDocument xmlDocument)
        {
            using (var nodeReader = new XmlNodeReader(xmlDocument))
            {
                nodeReader.MoveToContent();
                return XDocument.Load(nodeReader);
            }
        }
        private static List<SurveyPageDTO> SetPageValues(ExcelWorkbook xlWorkbook)
        {
            List<SurveyPageDTO> PageList = new List<SurveyPageDTO>();
            ExcelWorksheet xlWorksheet = xlWorkbook.Worksheets[1];

            var start = xlWorksheet.Dimension.Start;
            var end = xlWorksheet.Dimension.End;

            for (int row = 2; row <= end.Row; row++)
            { // Row by row...

                SurveyPageDTO Page = new SurveyPageDTO();
                if (xlWorksheet.Cells[row, 1] != null && xlWorksheet.Cells[row, 1].Text != "")
                {
                    Page.Question = xlWorksheet.Cells[row, 1].Text;
                }
                //
                if (xlWorksheet.Cells[row, 2] != null && xlWorksheet.Cells[row, 2].Text != "")
                {
                    Page.Title = xlWorksheet.Cells[row, 2].Text;
                }
                if (xlWorksheet.Cells[row, 3] != null && xlWorksheet.Cells[row, 3].Text != "")
                {
                    Page.Description = xlWorksheet.Cells[row, 3].Text;
                }
                //
                if (xlWorksheet.Cells[row, 4] != null && xlWorksheet.Cells[row, 4].Text != "")
                {

                    Page.Variable_Name = xlWorksheet.Cells[row, 4].Text.Replace(" ","_");
                }
                if (xlWorksheet.Cells[row, 5] != null && xlWorksheet.Cells[row, 5].Text != "")
                {
                    Page.Question_Type = GetDataType(xlWorksheet.Cells[row, 5].Text);
                }
                if (xlWorksheet.Cells[row, 6] != null && xlWorksheet.Cells[row, 6].Text != "")
                {
                    Page.Required = GetBool(xlWorksheet.Cells[row, 6].Text);
                }
                if (xlWorksheet.Cells[row, 7] != null && xlWorksheet.Cells[row, 7].Text != "")
                {
                    Page.List_Values = GetDropDownList(xlWorksheet.Cells[row, 7].Text, xlWorkbook);
                }
                if (xlWorksheet.Cells[row, 8] != null && xlWorksheet.Cells[row, 8].Text != "")
                {
                    Page.If_Condition = xlWorksheet.Cells[row, 8].Text;
                }
                if (xlWorksheet.Cells[row, 9] != null && xlWorksheet.Cells[row, 9].Text != "")
                {
                    Page.Then_Question = xlWorksheet.Cells[row, 9].Text;
                }
                if (xlWorksheet.Cells[row, 10] != null && xlWorksheet.Cells[row, 10].Text != "")
                {
                    Page.Else_Question = xlWorksheet.Cells[row, 10].Text;
                }
                Page.PageName = "Page " + (row - 1).ToString();
                Page.PageId = row - 1;
                PageList.Add(Page);

            }
            return PageList;
        }

        private static bool GetBool(string p)
        {
            if (p == "1")
            { return true; }
            else { return false; }
        }

        private static int GetDataType(string _DataType)
        {
            int type = 0;

            switch (_DataType)
            {
                case "Checkbox":
                    type = (int) EpiInfoDataTypes.Checkbox;
                    break;
                case "Text":
                    type = (int) EpiInfoDataTypes.Text;
                    break;
                case "Numeric":
                    type = (int) EpiInfoDataTypes.Numeric;
                    break;
                case "Yes/No":
                    type = (int) EpiInfoDataTypes.YesNo;
                    break;
                case "Options":
                    type = (int) EpiInfoDataTypes.Options;
                    break;
                case "Dropdown":
                    type = (int) EpiInfoDataTypes.Dropdown;
                    break;
                case "Date":
                    type = (int) EpiInfoDataTypes.Date;
                    break;
                case "Time":
                    type = (int)EpiInfoDataTypes.Time;
                    break;
                default:
                    type = 0;
                    break;
            }

            return type;
        }

        private static List<string> GetDropDownList(string DropDownValue, ExcelWorkbook xlWorkbook)
        {
            List<string> ValueList = new List<string>();

            ExcelWorksheet xlWorksheet = xlWorkbook.Worksheets[DropDownValue];

            var start = xlWorksheet.Dimension.Start;
            var end = xlWorksheet.Dimension.End;

            for (int row = 1; row <= end.Row; row++)
            { // Row by row...
                if (xlWorksheet.Cells[row, 1] != null && xlWorksheet.Cells[row, 1].Text != "")
                {
                    ValueList.Add(xlWorksheet.Cells[row, 1].Text);
                }
            }
            return ValueList;
        }
    }
}
