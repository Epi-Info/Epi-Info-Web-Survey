﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Epi.Web.MVC;
namespace Epi.Web.MVC.Models
{
    /// <summary>
    /// Maps DTO object to Model object or Model object to DTO object
    /// </summary>
    public static class Mapper
    {

        /// <summary>
        /// Maps SurveyInfo DTO to SurveyInfo Model.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>

       
        public static SurveyInfoModel ToSurveyInfoModel(this Epi.Web.Common.DTO.SurveyInfoDTO SurveyInfoDTO)
        {
            return new SurveyInfoModel
            {
                SurveyId = SurveyInfoDTO.SurveyId,
                SurveyNumber = SurveyInfoDTO.SurveyNumber,
                SurveyName = SurveyInfoDTO.SurveyName,
                OrganizationName = SurveyInfoDTO.OrganizationName,
                DepartmentName = SurveyInfoDTO.DepartmentName,
                IntroductionText = SurveyInfoDTO.IntroductionText,
                ExitText = SurveyInfoDTO.ExitText,
                XML = SurveyInfoDTO.XML,
                IsSuccess = SurveyInfoDTO.IsSuccess,
                SurveyType = SurveyInfoDTO.SurveyType,
                ClosingDate = SurveyInfoDTO.ClosingDate,
                UserPublishKey = SurveyInfoDTO.UserPublishKey,
                IsDraftMode = SurveyInfoDTO.IsDraftMode,
                StartDate = SurveyInfoDTO.StartDate,
               
            };

        }

        /// <summary>
        /// Maps SurveyInfo Model to SurveyInfo DTO.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>

        public static Epi.Web.Common.DTO.SurveyInfoDTO ToSurveyInfoDTO(SurveyInfoModel SurveyInfoModel)
        {
            return new Epi.Web.Common.DTO.SurveyInfoDTO
            {
                SurveyId = SurveyInfoModel.SurveyId,
                SurveyNumber = SurveyInfoModel.SurveyNumber,
                SurveyName = SurveyInfoModel.SurveyName,
                OrganizationName = SurveyInfoModel.OrganizationName,
                DepartmentName = SurveyInfoModel.DepartmentName,
                IntroductionText = SurveyInfoModel.IntroductionText,
                ExitText = SurveyInfoModel.ExitText,
                XML = SurveyInfoModel.XML,
                IsSuccess = SurveyInfoModel.IsSuccess,
                ClosingDate = SurveyInfoModel.ClosingDate,
                UserPublishKey = SurveyInfoModel.UserPublishKey,
                IsDraftMode = SurveyInfoModel.IsDraftMode,
                StartDate = SurveyInfoModel.StartDate,
                
            };
        }


        public static SurveyAnswerModel ToSurveyAnswerModel(this Epi.Web.Common.DTO.SurveyAnswerDTO SurveyAnswerDTO)
        {
            return new SurveyAnswerModel
            {
                ResponseId = SurveyAnswerDTO.ResponseId,
                SurveyId = SurveyAnswerDTO.SurveyId,
                DateUpdated = SurveyAnswerDTO.DateUpdated,
                DateCompleted = SurveyAnswerDTO.DateCompleted,
                Status = SurveyAnswerDTO.Status,
                XML = SurveyAnswerDTO.XML
            };
        }

        /// <summary>
        /// Maps SurveyInfo Model to SurveyInfo DTO.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>

        public static Epi.Web.Common.DTO.SurveyAnswerDTO ToSurveyAnswerDTO(SurveyAnswerModel SurveyAnswerModel)
        {
            return new Epi.Web.Common.DTO.SurveyAnswerDTO
            {
                ResponseId = SurveyAnswerModel.ResponseId,
                SurveyId = SurveyAnswerModel.SurveyId,
                DateUpdated = SurveyAnswerModel.DateUpdated,
                DateCompleted = SurveyAnswerModel.DateCompleted,
                Status = SurveyAnswerModel.Status,
                XML = SurveyAnswerModel.XML
            };
        }

        public static Epi.Web.Common.Message.UserAuthenticationRequest ToUserAuthenticationObj(Epi.Web.Common.DTO.PassCodeDTO pDTO)
        {
            return new Epi.Web.Common.Message.UserAuthenticationRequest
            {
                SurveyResponseId =pDTO.ResponseId,
                PassCode =pDTO.PassCode

            };
        }

    }
}