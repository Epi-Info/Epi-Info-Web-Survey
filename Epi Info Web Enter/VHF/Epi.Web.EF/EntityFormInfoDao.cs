using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using Epi.Web.Enter.Common.BusinessObject;
using Epi.Web.Enter.Common.Criteria;
using Epi.Web.Enter.Interfaces.DataInterface;
namespace Epi.Web.EF
    {
    public class EntityFormInfoDao: IFormInfoDao
        {
        public List<FormInfoBO> GetFormInfo(int UserId)
            {
        List<FormInfoBO> FormList = new List<FormInfoBO>();
        FormInfoBO FormInfoBO;
     
            try
                {

                int Id = UserId;

                    using (var Context = DataObjectFactory.CreateContext())
                        {
                    


                      


                       IEnumerable<SurveyMetaData> AllForms = Context.SurveyMetaDatas.Select(x => x);
                       List<string> Assigned = new List<string>();
                       User CurrentUser = Context.Users.Single(x => x.UserID == Id);
                     
                       foreach (var form in AllForms)
                           {
                           if (form.Users.Contains(CurrentUser) )
                               {
                                  Assigned.Add(form.SurveyId.ToString());
                               }
                           
                           }


                       var items = from FormInfo in Context.SurveyMetaDatas
                                   join UserInfo in Context.Users
                                   on FormInfo.OwnerId equals UserInfo.UserID
                                   into temp
                                  
                                   
                                   from UserInfo in temp.DefaultIfEmpty()
                                   select new { FormInfo, UserInfo }; 
                     

                        foreach (var item in items)
                            {
                            
                        FormInfoBO = Mapper.MapToFormInfoBO(item.FormInfo,item.UserInfo,false);
                        if (string.IsNullOrEmpty(FormInfoBO.ParentId))
                            {
                            if(item.UserInfo.UserID == Id)
                                {
                                    FormInfoBO.IsOwner = true;
                                    FormList.Add(FormInfoBO);
                                   
                                }
                            else
                                {
                                if (Assigned.Contains(FormInfoBO.FormId))
                                     {
                                   FormInfoBO.IsOwner = false;
                                   FormList.Add(FormInfoBO);
                                     }
                                   
                                }

                           // FormList.Add(FormInfoBO);
                               }
                            }
                         }
                }
            catch (Exception ex)
                {
                throw (ex);
                }
           



        return FormList;
            }

        public FormInfoBO GetFormByFormId(string FormId, bool GetXml, int UserId)
            {

       
                      FormInfoBO FormInfoBO = new FormInfoBO();

                      try
                          {

                          Guid Id = new Guid(FormId);

                          using (var Context = DataObjectFactory.CreateContext())
                              {



                              var items = from FormInfo in Context.SurveyMetaDatas
                                          join UserInfo in Context.Users
                                          on FormInfo.OwnerId equals UserInfo.UserID
                                          into temp
                                          from UserInfo in temp.DefaultIfEmpty()
                                          where FormInfo.SurveyId == Id
                                          select new { FormInfo, UserInfo };


                              foreach (var item in items)
                                  {
                                  FormInfoBO = Mapper.MapToFormInfoBO(item.FormInfo, item.UserInfo,GetXml);

                                  if (item.UserInfo.UserID == UserId)
                                      {
                                      FormInfoBO.IsOwner = true;
                                      }
                                  else
                                      {
                                      FormInfoBO.IsOwner = false;
                                      }

                                  }
                              }
                          }
                      catch (Exception ex)
                          {
                          throw (ex);
                          }




                      return FormInfoBO;
            
            
            }

        public FormInfoBO GetFormByFormId(string FormId)
        {

        FormInfoBO FormInfoBO = new FormInfoBO();

        try
            {

            Guid Id = new Guid(FormId);

            using (var Context = DataObjectFactory.CreateContext())
                {
                 
                SurveyMetaData SurveyMetaData = Context.SurveyMetaDatas.Single(x => x.SurveyId == Id);
                FormInfoBO = Mapper.ToFormInfoBO(SurveyMetaData);
                }
            }
        catch (Exception ex)
            {
            throw (ex);
            }




        return FormInfoBO;
            
            }
        }
    }
