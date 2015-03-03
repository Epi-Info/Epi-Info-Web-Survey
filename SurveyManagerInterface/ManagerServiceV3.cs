using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Epi.Web.Common.DTO;
using Epi.Web.Common.Message;
using Epi.Web.Common.MessageBase;
using Epi.Web.Common.Criteria;
using Epi.Web.Common.ObjectMapping;
using Epi.Web.Common.BusinessObject;
using Epi.Web.Common.Exception;
using Epi.Web.BLL;
using System.Xml;
using System.Xml.Linq;
using System.Configuration;
using Epi.Web.Common.Security;

namespace Epi.Web.WCF.SurveyService
{
    public class ManagerServiceV3 : ManagerServiceV2, IManagerServiceV3
    {
       public void UpdateRecordStatus(SurveyAnswerRequest pRequestMessage)
       {
           try
           {
               Epi.Web.Interfaces.DataInterfaces.ISurveyResponseDao SurveyResponseDao = new EF.EntitySurveyResponseDao();
               Epi.Web.BLL.SurveyResponse Implementation = new Epi.Web.BLL.SurveyResponse(SurveyResponseDao);
               foreach (SurveyAnswerDTO DTO in pRequestMessage.SurveyAnswerList)
               {
                   Implementation.UpdateRecordStatus(Mapper.ToBusinessObject(DTO));
               }
                
           }
           catch (Exception ex)
           {

               throw ex;
               
                
           }
       }
    }
}
