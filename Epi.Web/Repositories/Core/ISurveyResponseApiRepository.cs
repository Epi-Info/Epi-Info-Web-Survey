using Epi.Web.Common.BusinessObject;
using Epi.Web.Common.Message;
using Epi.Web.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Epi.Web.MVC.Repositories.Core
{
    public interface ISurveyResponseApiRepository
    {
        Guid SurveyId
        {
            get; set;
        }

        Guid OrgKey
        {
            get; set;
        }

        Guid PublisherKey
        {
            get; set;
        }
        PreFilledAnswerResponse SetSurveyAnswer(SurveyResponseApiModel item);
        void Remove(string id);
        PreFilledAnswerResponse Update(SurveyResponseApiModel item, string ResponseId);
        SurveyInfoBO GetSurveyInfoById(string SurveyId);
    }
}