using Epi.Web.Common.BusinessObject;
using Epi.Web.Interfaces.DataInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epi.Web.EF
{
    public class EntityReportDao : IReportDao
    {
        public void PublishReport(ReportInfoBO ReportInfo)
        {

            try
            {
                var SurveyId = Guid.Parse(ReportInfo.SurveyId);
                //var GadgetId = Guid.Parse(ReportInfo.GadgetId);
                var ReportId = Guid.Parse(ReportInfo.ReportId);
                using (var Context = DataObjectFactory.CreateContext())
                {



                    if (!ReportExist(SurveyId, ReportId))
                    {

                        SurveyReportsInfo ReportEntity = Mapper.ToReportInfoEF(ReportInfo);

                        Context.SurveyReportsInfoes.Add(ReportEntity);

                        Context.SaveChanges();



                    }
                    else
                    {

                        var Query = from SurveyReportInfo in Context.SurveyReportsInfoes
                                    where SurveyReportInfo.ReportId == ReportId
                                    select SurveyReportInfo;

                        var DataRow = Query.Single();
                        DataRow.DateEdited = DateTime.Now;
                        DataRow.RecordCount = ReportInfo.RecordCount;
                        DataRow.ReportVersion = ReportInfo.ReportVersion + 1;
                        DataRow.DataSource = ReportInfo.DataSource;
                        Context.SaveChanges();

                        // update Gadget
                        foreach (var gadget in ReportInfo.Gadgets)
                        {
                            var GadgetId = Guid.Parse(gadget.GadgetId);
                            var GadgetQuery = from SurveyReport in Context.SurveyReports
                                        where SurveyReport.ReportId == ReportId && SurveyReport.GadgetId == GadgetId
                                              select SurveyReport;
                            if (GadgetQuery != null && GadgetQuery.Count()>0)
                            {
                                var GadgetDataRow = GadgetQuery.Single();
                                GadgetDataRow.DateEdited = DateTime.Now;

                                GadgetDataRow.GadgetVersion = gadget.GadgetVersion + 1;
                                GadgetDataRow.ReportHtml = gadget.ReportHtml;
                                Context.SaveChanges();
                            }
                            else
                            {

                                List<SurveyReport> ReportEntity = Mapper.ToGadgetsEF(ReportInfo.Gadgets);
                                foreach (var item in ReportEntity)
                                {
                                    Context.SurveyReports.Add(item);
                                }
                                Context.SaveChanges();


                            }
                        }


                    }


                }

            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }

        public List<ReportInfoBO> GetSurveyReports(string SurveyID,bool IncludHTML)
        {
            try
            {
                var surveyid = Guid.Parse(SurveyID);
                List<ReportInfoBO> List = new List<ReportInfoBO>();
                using (var Context = DataObjectFactory.CreateContext())
                {
                    if (IncludHTML) {
                        var Query = Context.SurveyReportsInfoes.Where(x => x.SurveyId == surveyid);

                        foreach (var item in Query)
                        {
                            List.Add(Mapper.ToReportInfoBO(item));
                        }
                    }
                    else {

                        var NoHtmlQuery = (from response in Context.SurveyReportsInfoes
                                        where response.SurveyId == surveyid
                                        select new { response.ReportId, response.ReportVersion, response.DateCreated, response.DateEdited, response.DataSource,response.RecordCount });
                        foreach (var reportInfo in NoHtmlQuery)
                        {
                            ReportInfoBO ReportInfoBO = new ReportInfoBO();
                            ReportInfoBO.CreatedDate = reportInfo.DateCreated;
                            ReportInfoBO.EditedDate = (DateTime)reportInfo.DateEdited;
                            if (!string.IsNullOrEmpty(reportInfo.ReportId.ToString()))
                            {
                                ReportInfoBO.ReportId = reportInfo.ReportId.ToString();
                            }
                            ReportInfoBO.DataSource = reportInfo.DataSource;
                            ReportInfoBO.RecordCount = reportInfo.RecordCount;
                            ReportInfoBO.ReportVersion = reportInfo.ReportVersion;

                            List.Add (ReportInfoBO);
                        }
                    }
                }
                return List;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public List<ReportInfoBO> GetReport(string ReportID)
        {
            List<ReportInfoBO> ReportInfoBOList = new List<ReportInfoBO>();
            try
            {
                ReportInfoBO ReportInfoBO = new ReportInfoBO();
               
                    var reportid = Guid.Parse(ReportID);
                    
                    using (var Context = DataObjectFactory.CreateContext())
                    {

                    var Query = Context.SurveyReportsInfoes.Where(x => x.ReportId == reportid);//.OrderBy(x=>x.GadgetNumber);

                    foreach (var item in Query) {

                        ReportInfoBO = Mapper.ToReportInfoBO(item);
                        ReportInfoBOList.Add(ReportInfoBO);
                    }
                    
                }
               
                 
                return ReportInfoBOList;

            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private bool ReportExist(Guid surveyid, Guid ReportId) {

            bool HasReport = false;

            using (var Context = DataObjectFactory.CreateContext())
            {


                var Query = Context.SurveyReportsInfoes.Where(x => x.SurveyId == surveyid && x.ReportId == ReportId).Count();


                if (Query>0) {
                    HasReport = true;
                }





            }


            return HasReport;


        }
        private bool ReportExist2(Guid ReportId)
        {

            bool HasReport = false;

            using (var Context = DataObjectFactory.CreateContext())
            {


                var Query = Context.SurveyReportsInfoes.Where(x =>   x.ReportId == ReportId).Count();


                if (Query > 0)
                {
                    HasReport = true;
                }





            }


            return HasReport;


        }


        public void DeleteReport(ReportInfoBO Report)
        {
            if (ReportExist2(Guid.Parse(Report.ReportId)))
            {
                Guid ReportId = Guid.Parse(Report.ReportId);
                using (var Context = DataObjectFactory.CreateContext())
                {


                    var Query = from SurveyReport in Context.SurveyReports
                                where SurveyReport.ReportId == ReportId && SurveyReport.GadgetVersion == Report.ReportVersion
                                select SurveyReport;
                    foreach (var item in Query) {
                        Context.SurveyReports.Remove(item);
                    }
                    Context.SaveChanges();



                }
            }
        }
    }
    }
