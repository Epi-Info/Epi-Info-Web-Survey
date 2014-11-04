using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using Epi.Web.Enter.Common;
using Epi.Web.Enter.Common.Criteria;
using Epi.Web.Enter.Interfaces.DataInterface;
using Epi.Web.Enter.Common.BusinessObject;

namespace Epi.Web.EF
{
    public class EntityFormSettingDao : IFormSettingDao
    {
        public FormSettingBO GetFormSettings(string FormId)
        {

            FormSettingBO FormSettingBO = new FormSettingBO();
            Dictionary<int, string> ColumnNameList = new Dictionary<int, string>();
            Dictionary<int, string> AvailableUsers = new Dictionary<int, string>();
            Dictionary<int, string> SelectedUsers = new Dictionary<int, string>();
            try
            {
                Guid id = new Guid(FormId);
                using (var Context = DataObjectFactory.CreateContext())
                {
                    var Query = from response in Context.ResponseDisplaySettings
                                where response.FormId == id
                                select response;

                    var DataRow = Query;

                    foreach (var Row in DataRow)
                    {

                        ColumnNameList.Add(Row.SortOrder, Row.ColumnName);

                    }

                    


                    //var SelectedUserQuery = from FormInfo in Context.SurveyMetaDatas
                    //            join UserInfo in Context.Users
                    //            on FormInfo.OwnerId equals UserInfo.UserID
                    //            into temp
                    //            from UserInfo in temp.DefaultIfEmpty()
                    //            where FormInfo.SurveyId == id
                    //            select new { FormInfo, UserInfo };

                    SurveyMetaData SelectedUserQuery = Context.SurveyMetaDatas.First(x => x.SurveyId == id);

                    var SelectedOrgId = SelectedUserQuery.OrganizationId;

                    var query = (from user in SelectedUserQuery.Users
                                join userorg in Context.UserOrganizations
                            on user.UserID equals userorg.UserID
                                where userorg.Active == true &&
                                userorg.OrganizationID == SelectedOrgId
                               // orderby user.UserName
                                select user).Distinct().OrderBy(user => user.UserName) ;

                    // IEnumerable<User> Users = SelectedUserQuery.Users;
                    foreach (var user in query)
                    {
                        SelectedUsers.Add(user.UserID, user.UserName);
                    }

                    //foreach (var Selecteduser in SelectedUserQuery)
                    //    {
                    //    SelectedUsers.Add(Selecteduser.UserInfo.UserID, Selecteduser.UserInfo.UserName);
                    //    }


                    var UserQuery = (from user in Context.Users
                                    join userorg in Context.UserOrganizations
                                    on user.UserID equals userorg.UserID
                                     where userorg.Active == true &&
                                 userorg.OrganizationID == SelectedOrgId
                                    //orderby user.UserName
                                     select user).Distinct().OrderBy(user => user.UserName);



                    foreach (var user in UserQuery)
                    {
                        if (!SelectedUsers.ContainsValue(user.UserName) && user.UserID != SelectedUserQuery.OwnerId)
                        {
                            AvailableUsers.Add(user.UserID, user.UserName);
                        }
                    }




                    FormSettingBO.ColumnNameList = ColumnNameList;
                    FormSettingBO.UserList = AvailableUsers;
                    FormSettingBO.AssignedUserList = SelectedUsers;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return FormSettingBO;


        }
        public void UpDateColumnNames(FormSettingBO FormSettingBO, string FormId)
        {
            Guid Id = new Guid(FormId);
            try
            {
                using (var Context = DataObjectFactory.CreateContext())
                {

                    IEnumerable<ResponseDisplaySetting> ColumnList = Context.ResponseDisplaySettings.ToList().Where(x => x.FormId == Id);

                    //Delete old columns
                    foreach (var item in ColumnList)
                    {
                        Context.ResponseDisplaySettings.DeleteObject(item);
                    }
                    Context.SaveChanges();

                    //insert new columns

                    ResponseDisplaySetting ResponseDisplaySettingEntity = new ResponseDisplaySetting();
                    foreach (var item in FormSettingBO.ColumnNameList)
                    {

                        ResponseDisplaySettingEntity = Mapper.ToColumnName(item, Id);
                        Context.AddToResponseDisplaySettings(ResponseDisplaySettingEntity);
                        //Context.SaveChanges();
                    }
                    Context.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }


        }
        public void UpDateFormMode(FormInfoBO FormInfoBO)
        {

            try
            {
                Guid Id = new Guid(FormInfoBO.FormId);

                //Update Form Mode
                using (var Context = DataObjectFactory.CreateContext())
                {
                    var Query = from response in Context.SurveyMetaDatas
                                where response.SurveyId == Id
                                select response;

                    var DataRow = Query.Single();
                    DataRow.IsDraftMode = FormInfoBO.IsDraftMode;



                    Context.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }
        public void UpDateAssignedUserList(FormSettingBO FormSettingBO, string FormId)
        {

            Guid Id = new Guid(FormId);
            try
            {
                using (var Context = DataObjectFactory.CreateContext())
                {
                    SurveyMetaData Response = Context.SurveyMetaDatas.First(x => x.SurveyId == Id);
                    //Remove old Users
                    IEnumerable<User> Users = Context.Users;
                    foreach (User user in Users)
                    {

                        Response.Users.Remove(user);

                    }
                    Context.SaveChanges();



                    //insert new users
                    foreach (var item in FormSettingBO.AssignedUserList)
                    {
                        User User = Context.Users.FirstOrDefault(x => x.UserName == item.Value);
                        Response.Users.Add(User);

                    }
                    Context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


        public List<string> GetAllColumnNames(string FormId)
        {
            Guid Id = new Guid(FormId);
            try
            {
                using (var Context = DataObjectFactory.CreateContext())
                {
                    List<string> Columns = (from c in Context.SurveyMetaDataTransforms
                                            where c.SurveyId == Id &&
                                            (c.FieldTypeId != 2 && c.FieldTypeId != 20 && c.FieldTypeId != 3 && c.FieldTypeId != 17 && c.FieldTypeId != 21) //filter non-data fields.
                                            orderby c.FieldName
                                            select c.FieldName).ToList();
                    return Columns;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
