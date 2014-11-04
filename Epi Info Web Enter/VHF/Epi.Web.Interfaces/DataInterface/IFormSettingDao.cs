using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using Epi.Web.Enter.Common.BusinessObject;
using Epi.Web.Enter.Common.Criteria;
using Epi.Web.Enter.Interfaces.DataInterface;

namespace Epi.Web.Enter.Interfaces.DataInterface
    {
   public  interface IFormSettingDao
        {
       FormSettingBO GetFormSettings(string FormId);
       void UpDateColumnNames(FormSettingBO FormSettingBO, string FormId);

       void UpDateFormMode(FormInfoBO FormInfoBO);

       void UpDateAssignedUserList(FormSettingBO FormSettingBO, string FormId);

       List<string> GetAllColumnNames(string FormId);
        }
    }
