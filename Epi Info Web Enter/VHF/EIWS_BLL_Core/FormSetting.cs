using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Enter.Common.BusinessObject;
using Epi.Web.Enter.Common.Criteria;
using System.Xml;
using System.Xml.Linq;
using Epi.Web.Enter.Interfaces.DataInterface;
using System.Configuration;
namespace Epi.Web.BLL
{
    public class FormSetting
    {


        private IFormSettingDao FormSettingDao;

        private IUserDao UserDao;
        private IFormInfoDao FormInfoDao;
        public FormSetting(IFormSettingDao pFormSettingDao, IUserDao pUserDao, IFormInfoDao pFormInfoDao)
        {
            this.FormSettingDao = pFormSettingDao;
            this.UserDao = pUserDao;
            this.FormInfoDao = pFormInfoDao;
        }

        public FormSettingBO GetFormSettings(string FormId, string Xml)
        {
            FormSettingBO result = this.FormSettingDao.GetFormSettings(FormId);
            if (!string.IsNullOrEmpty(Xml))
            {
                result.FormControlNameList = GetFormColumnNames(Xml, result.ColumnNameList);
            }
            else
            {
                result.FormControlNameList = new Dictionary<int, string>();
                var Columns = GetAllColumns(FormId);

                for (int i = 0; i < Columns.Count; i++)
                {
                    result.FormControlNameList.Add(i, Columns[i]);
                }
            }
            return result;
        }

        private List<string> GetAllColumns(string FormId)
        {
            return FormSettingDao.GetAllColumnNames(FormId);
        }

        public Dictionary<int, string> GetFormColumnNames(string Xml, Dictionary<int, string> Selected)
        {
            Dictionary<int, string> List = new Dictionary<int, string>();

            XDocument xdoc = XDocument.Parse(Xml);


            var _FieldsTypeIDs = from _FieldTypeID in
                                     xdoc.Descendants("Field")

                                 select _FieldTypeID;
            int Count = 0;
            foreach (var _FieldTypeID in _FieldsTypeIDs)
            {
                if (!Selected.ContainsValue(_FieldTypeID.Attribute("Name").Value.ToString()) && _FieldTypeID.Attribute("FieldTypeId").Value != "2" && _FieldTypeID.Attribute("FieldTypeId").Value != "21" && _FieldTypeID.Attribute("FieldTypeId").Value != "3")
                {
                    List.Add(Count, _FieldTypeID.Attribute("Name").Value.ToString());
                    Count++;
                }
            }
            return List;

        }


        public FormSettingBO SaveSettings(Dictionary<int, string> ColumnList, bool IsDraftMode)
        {
            throw new NotImplementedException();
        }





        public string SaveSettings(bool IsDraftMode, Dictionary<int, string> ColumnNameList, Dictionary<int, string> AssignedUserList, string FormId)
        {
            string Message = "";
            FormSettingBO FormSettingBO = new FormSettingBO();
            FormSettingBO.ColumnNameList = ColumnNameList;
            FormSettingBO.AssignedUserList = AssignedUserList;
            FormInfoBO FormInfoBO = new FormInfoBO();
            FormInfoBO.FormId = FormId;
            FormInfoBO.IsDraftMode = IsDraftMode;
            try
            {
                List<UserBO> FormCurrentUsersList = this.UserDao.GetUserByFormId(FormId);
                this.FormSettingDao.UpDateColumnNames(FormSettingBO, FormId);
                this.FormSettingDao.UpDateFormMode(FormInfoBO);
                this.FormSettingDao.UpDateAssignedUserList(FormSettingBO, FormId);

                if (ConfigurationManager.AppSettings["SEND_EMAIL_TO_ASSIGNED_USERS"].ToUpper() == "TRUE" && AssignedUserList.Count() > 0)
                {
                    SendEmail(AssignedUserList, FormId, FormCurrentUsersList);

                }




                Message = "Success";
            }
            catch (Exception Ex)
            {
                Message = "Error";
                throw Ex;

            }
            return Message;
        }


        private void SendEmail(Dictionary<int, String> AssignedUserList, string FormId, List<UserBO> FormCurrentUsersList)
        {

            try
            {

                FormInfoBO FormInfoBO = this.FormInfoDao.GetFormByFormId(FormId);
                if (!string.IsNullOrEmpty(FormInfoBO.ParentId))
                {
                    return;
                }
                UserBO UserBO = this.UserDao.GetCurrentUser(FormInfoBO.UserId);
                List<string> UsersEmail = new List<string>();
                List<string> CurrentUsersEmail = new List<string>();

                foreach (UserBO User in FormCurrentUsersList)
                {
                    CurrentUsersEmail.Add(User.EmailAddress);
                }


                if (CurrentUsersEmail.Count() > 0)
                {

                    foreach (var User in AssignedUserList)
                    {
                        if (!CurrentUsersEmail.Contains(User.Value))
                        {

                            UsersEmail.Add(User.Value);
                        }

                    }
                }
                else
                {
                    foreach (var User in AssignedUserList)
                    {

                        UsersEmail.Add(User.Value);

                    }
                }
                if (UsersEmail.Count() > 0)
                {

                    Epi.Web.Enter.Common.Email.Email Email = new Web.Enter.Common.Email.Email();
                    Email.Body = UserBO.FirstName + " " + UserBO.LastName + " has assigned the following form  to you in Epi Info™ Web Enter.\n\nTitle: " + FormInfoBO.FormName + " \n \n \nPlease click the link below to launch Epi Info™ Web Enter.";
                    Email.Body = Email.Body.ToString() + " \n \n" + ConfigurationManager.AppSettings["BaseURL"];
                    Email.From = UserBO.EmailAddress;
                    Email.To = UsersEmail;
                    Email.Subject = "An Epi Info Web Enter Form - " + FormInfoBO.FormName + " has been assigned to You";
                    Epi.Web.Enter.Common.Email.EmailHandler.SendMessage(Email);


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
