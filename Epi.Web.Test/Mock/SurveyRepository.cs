using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Epi.Web;
using Epi.Web.Models;
using NUnit.Framework;
using Moq;
using NUnit.Mocks;
using Epi.Web.Common.DTO;
using Epi.Web.Repositories;

namespace Epi.Web.Test.Mock
{
    public class SurveyRepository : Epi.Web.Repositories.Core.ISurveyInfoRepository, Epi.Web.Repositories.Core.IRepository<Epi.Web.Common.DTO.SurveyInfoDTO>
    {


        public SurveyRepository()
        {

        }

        /// <summary>
        /// Returning a specific SurveyInfoDTO based on SurveyId
        /// </summary>
        /// <param name="pIid"></param>
        /// <returns></returns>
        SurveyInfoDTO Epi.Web.Repositories.Core.ISurveyInfoRepository.GetSurveyInfoById(string pIid)
        {
            List<SurveyInfoDTO> surveyResponseList = new SurveyResponseCreater().ListOfSurveyResponse();
            SurveyInfoDTO sResponse = surveyResponseList.Find(x => x.SurveyId == pIid);
            return sResponse;
        }

        public List<SurveyInfoDTO> GetList(Repositories.Core.Criterion criterion = null)
        {
            throw new NotImplementedException();
        }

        public SurveyInfoDTO Get(int id)
        {
            throw new NotImplementedException();
        }

        public int GetCount(Repositories.Core.Criterion criterion = null)
        {
            throw new NotImplementedException();
        }

        public void Insert(SurveyInfoDTO t)
        {
            throw new NotImplementedException();
        }

        public void Update(SurveyInfoDTO t)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        List<SurveyInfoDTO> Repositories.Core.IRepository<SurveyInfoDTO>.GetList(Repositories.Core.Criterion criterion)
        {
            throw new NotImplementedException();
        }

        SurveyInfoDTO Repositories.Core.IRepository<SurveyInfoDTO>.Get(int id)
        {
            throw new NotImplementedException();
        }

        int Repositories.Core.IRepository<SurveyInfoDTO>.GetCount(Repositories.Core.Criterion criterion)
        {
            throw new NotImplementedException();
        }

        void Repositories.Core.IRepository<SurveyInfoDTO>.Insert(SurveyInfoDTO t)
        {
            throw new NotImplementedException();
        }

        void Repositories.Core.IRepository<SurveyInfoDTO>.Update(SurveyInfoDTO t)
        {
            throw new NotImplementedException();
        }

        void Repositories.Core.IRepository<SurveyInfoDTO>.Delete(int id)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Creating a SurveyInfo Class
    /// </summary>
    public class SurveyInfo
    {
        private string _SurveyId;
        private string _SurveyNumber;
        private string _SurveyName;
        private string _IntroductionText;
        private string _DepartmentName;
        private string _OrganizationName;
        private string _XML;
        private bool _IsSuccess;
        private DateTime _ClosingDate;


        public string SurveyId
        {
            get { return _SurveyId; }
            set { _SurveyId = value; }
        }

        public string SurveyNumber
        {
            get { return _SurveyNumber; }
            set { _SurveyNumber = value; }
        }


        public string SurveyName
        {
            get { return _SurveyName; }
            set { _SurveyName = value; }
        }


        public string OrganizationName
        {
            get { return _OrganizationName; }
            set { _OrganizationName = value; }
        }


        public string DepartmentName
        {
            get { return _DepartmentName; }
            set { _DepartmentName = value; }
        }



        public string IntroductionText
        {
            get { return _IntroductionText; }
            set { _IntroductionText = value; }
        }

        public string XML
        {
            get { return _XML; }
            set { _XML = value; }
        }

        public bool IsSuccess
        {
            get { return _IsSuccess; }
            set { _IsSuccess = value; }
        }

        public DateTime ClosingDate
        {
            get { return _ClosingDate; }
            set { _ClosingDate = value; }
        }
    }



    /// <summary>
    /// Creating a repository of SurveyInfo objects
    /// </summary>
    public class SurveyResponseRepository : List<SurveyInfo>
    {

        // private SurveyResponse _surveyResponse;
        private List<SurveyInfoDTO> _surveyResponseList;

        public SurveyResponseRepository()
        {
            // _surveyResponse = sResponse;
            _surveyResponseList = new List<SurveyInfoDTO>();
        }

        public void AddSurveyResponse(SurveyInfoDTO sResponse)
        {
            _surveyResponseList.Add(sResponse);
        }

        public List<SurveyInfoDTO> ShowSurveyResponseList()
        {
            return _surveyResponseList;
        }

    }

    /// <summary>
    /// Populating the repository (list of) SurveyInfo with SurveyInfo objects
    /// </summary>
    public class SurveyResponseCreater
    {

        public List<SurveyInfoDTO> ListOfSurveyResponse()
        {

            SurveyResponseRepository srep = new SurveyResponseRepository();

            SurveyInfoDTO sr = new SurveyInfoDTO();
            sr.SurveyId = "1";
            sr.SurveyNumber = "11";
            sr.SurveyName = "Abc Survey";
            sr.XML = "";
            sr.IsSuccess = true;
            sr.ClosingDate = DateTime.Parse("2011-12-05");
            srep.AddSurveyResponse(sr);

            sr = new SurveyInfoDTO();
            sr.SurveyId = "2";
            sr.SurveyNumber = "22";
            sr.SurveyName = "Bbc Survey";
            sr.XML = "";
            sr.IsSuccess = true;
            sr.ClosingDate = DateTime.Parse("2011-12-06");
            srep.AddSurveyResponse(sr);

            sr = new SurveyInfoDTO();
            sr.SurveyId = "3";
            sr.SurveyNumber = "33";
            sr.SurveyName = "Cbc Survey";
            sr.XML = "";
            sr.IsSuccess = true;
            sr.ClosingDate = DateTime.Parse("2011-12-03");
            srep.AddSurveyResponse(sr);

            sr = new SurveyInfoDTO();
            sr.SurveyId = "4";
            sr.SurveyNumber = "44";
            sr.SurveyName = "Dbc Survey";
            sr.XML = "";
            sr.IsSuccess = true;
            sr.ClosingDate = DateTime.Parse("2011-12-08");
            srep.AddSurveyResponse(sr);

            return srep.ShowSurveyResponseList();
        }
    }

}
