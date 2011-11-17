using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SurveyManagerInterface
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class Service : ISurveyManager
    {

        ISurveyRequestResult PublishSurvey(ISurveyRequest pRequestMessage)
        {
            ISurveyRequestResult result = null;

            return result;

        }
    }
}
