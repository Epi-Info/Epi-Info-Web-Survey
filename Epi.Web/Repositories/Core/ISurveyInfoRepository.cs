using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Epi.Web.Repositories.Core
{
    /// <summary>
    /// SurveyInfo Repository interface.
    /// Derives from standard IRepository. Adds one SurveyInfo specific member.
    /// </summary>
    public interface ISurveyInfoRepository: IRepository<Epi.Web.Common.DTO.SurveyInfoDTO>
    {
        Epi.Web.Common.DTO.SurveyInfoDTO GetSurveyInfoById(string pId);
    }
}
