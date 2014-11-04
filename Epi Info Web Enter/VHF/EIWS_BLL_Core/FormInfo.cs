using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Enter.Common.BusinessObject;
using Epi.Web.Enter.Common.Criteria;

using Epi.Web.Enter.Interfaces.DataInterface;
namespace Epi.Web.BLL
    {
   public class FormInfo
        {
       private IFormInfoDao FormInfoDao;

 

        public FormInfo( IFormInfoDao pSurveyInfoDao)
        {
        this.FormInfoDao = pSurveyInfoDao;
        }

        public List<FormInfoBO> GetFormsInfo(int UserId)
            {
            //Owner Forms
            List<FormInfoBO> result = this.FormInfoDao.GetFormInfo(UserId);

           

            return result;
            }

        public FormInfoBO GetFormInfoByFormId(string FormId, bool GetXml, int UserId)
            {
            //Owner Forms
            FormInfoBO result = this.FormInfoDao.GetFormByFormId(FormId,GetXml,UserId);



            return result;
            }
        }
    }
