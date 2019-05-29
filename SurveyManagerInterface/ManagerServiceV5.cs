//using Epi.Web.WCF.SurveyService.Interfaces;
using Epi.Web.Common.Message;
using System;
using System.Collections.Generic;
using System.Configuration;
using Epi.Web.Common.ObjectMapping;

namespace Epi.Web.WCF.SurveyService
{
    class ManagerServiceV5 : ManagerServiceV4, IManagerServiceV5
    {
        public string SetJsonColumn(List<string> SurveyIds, string OrgId)
        {

            return "";

        }

        public string  SetJsonColumnAll(string AdminKey)
        {
            string Key = ConfigurationManager.AppSettings["AdminKey"];
            if (AdminKey== Key)
            {


            }
            return " Done";

        }
        public UserResponse GetLoginToken(UserRequest UserInfo) {

            UserResponse UserResponse = new UserResponse();
            UserRequest UserRequest = new UserRequest();


            try
            {


                Epi.Web.Interfaces.DataInterfaces.IUserDao IUserDao = new EF.EntityUserDao();
                Epi.Web.BLL.User Implementation = new Epi.Web.BLL.User(IUserDao);
                bool UserExist = Implementation.GetExistingUser(Mapper.ToUserBO(UserRequest.User));

                if (UserExist)
                {
                    UserResponse.Message = ConfigurationManager.AppSettings[""];
                    
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return UserResponse;

        }
    }
}
