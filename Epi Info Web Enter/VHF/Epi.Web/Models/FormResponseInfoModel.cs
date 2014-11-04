using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Epi.Web.MVC.Models
{
    public class FormResponseInfoModel
    {
        public FormInfoModel FormInfoModel;
        public List<ResponseModel> ResponsesList;
        //public List<string> ColumnNames;
        public List<KeyValuePair<int, string>> Columns;
        public int NumberOfPages;
        public int CurrentPage;
        public int PageSize;
        public int NumberOfResponses;
        public int ViewId;
        public string ParentResponseId;
        public string sortOrder;
        public string sortfield;
        public SearchBoxModel SearchModel;
        public List<SelectListItem> SearchColumns1;
        public List<SelectListItem> SearchColumns2;
        public List<SelectListItem> SearchColumns3;
        public List<SelectListItem> SearchColumns4;
        public List<SelectListItem> SearchColumns5;

        public FormResponseInfoModel()
        {
            FormInfoModel = new FormInfoModel();
            ResponsesList = new List<ResponseModel>();
            //ColumnNames = new List<string>();
            Columns = new List<KeyValuePair<int, string>>();
        }
    }

}