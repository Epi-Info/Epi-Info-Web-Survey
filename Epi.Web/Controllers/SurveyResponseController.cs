using System;
using System.Web.Mvc;
using Epi.Web.MVC.Facade;
using Epi.Web.MVC.Models;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Web.Http;
using Epi.Web.MVC.Repositories.Core;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using System.Linq;
using Epi.Web.Common.Message;

namespace Epi.Web.Controllers
{
    public class SurveyResponseApiController : ApiController
    {

        private ISurveyResponseApiRepository _isurveyAnswerRepository;

        public SurveyResponseApiController()
        {

        }
        public SurveyResponseApiController(ISurveyResponseApiRepository isurvyeyAnswerepository)
        {
            _isurveyAnswerRepository = isurvyeyAnswerepository;
        }

        /// <summary>
        /// Handle HTTPPOST request coming in from the client after successful Authentication
        /// </summary>
        /// <param name="request"></param>
        /// <returns>HTTPRespose code with succee/failure</returns>
        public HttpResponseMessage PostSurveyAnswer(HttpRequestMessage request)
        {
            Dictionary<string, string> keyvalupair = new Dictionary<string, string>();
            var value = request.Content.ReadAsStringAsync().Result;
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            try
            {
                keyvalupair = JsonConvert.DeserializeObject<Dictionary<string, string>>(value, settings);
            }
            catch (Exception ex)
            {
                var responseexception = Request.CreateResponse(HttpStatusCode.UnsupportedMediaType, ex.Message.ToString());//415 Unsupported media type The endpoint does not support the format of the request body. 
                return responseexception;
            }
            string responseId;
            SurveyResponseApiModel surveyanswerModel = new SurveyResponseApiModel();
            surveyanswerModel.SurveyId = _isurveyAnswerRepository.SurveyId;
            surveyanswerModel.OrgKey = _isurveyAnswerRepository.OrgKey;
            surveyanswerModel.PublisherKey = _isurveyAnswerRepository.PublisherKey;
            surveyanswerModel.SurveyQuestionAnswerListField = keyvalupair;

            var item = keyvalupair.Where(x => x.Key.ToLower() == "responseid" || x.Key.ToLower() == "id").FirstOrDefault(); //  if (keyvalupair.TryGetValue("ResponseId", out ResponseId))
            if (item.Value != null)
            {
                responseId = item.Value;
                surveyanswerModel.SurveyQuestionAnswerListField = keyvalupair;
                var Result = _isurveyAnswerRepository.SetSurveyAnswer(surveyanswerModel);
                if (Result.SurveyResponseID != null)
                {
                    var response = Request.CreateResponse<PreFilledAnswerResponse>(System.Net.HttpStatusCode.Created, Result);//201 Created Success with response body. 
                    string uri = Url.Link("DefaultApi", new { id = Result.SurveyResponseID });
                    response.Headers.Location = new Uri(uri);
                    return response;
                }
                else
                {
                    var response = Request.CreateResponse(HttpStatusCode.Forbidden, "Response not generated");//The requested operation is not permitted for the user. This error can also be caused by ACL failures, or business rule or data policy constraints.
                    return response;
                }
            }
            else
            {
                var response = Request.CreateResponse(HttpStatusCode.UnsupportedMediaType, "Response not generated");//415 Unsupported media type The endpoint does not support the format of the request body. 
                return response;
            }

        }

        /// <summary>
        /// Handle HTTPPUT request coming in from the client after successful Authentication
        /// </summary>
        /// <param name="request"></param>
        /// <returns>HTTPRespose code with succees/failure</returns>

        public HttpResponseMessage Put(HttpRequestMessage request)
        {
            Dictionary<string, string> keyvalupair = new Dictionary<string, string>();
            var value = request.Content.ReadAsStringAsync().Result;
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            try
            {
                keyvalupair = JsonConvert.DeserializeObject<Dictionary<string, string>>(value, settings);
            }
            catch (Exception ex)
            {
                var response = Request.CreateResponse(HttpStatusCode.UnsupportedMediaType, ex.Message.ToString());//415 Unsupported media type The endpoint does not support the format of the request body. 
                return response;
            }
            string responseId;
            SurveyResponseApiModel surveyanswerModel = new SurveyResponseApiModel();
            surveyanswerModel.SurveyId = _isurveyAnswerRepository.SurveyId;
            surveyanswerModel.OrgKey = _isurveyAnswerRepository.OrgKey;
            surveyanswerModel.PublisherKey = _isurveyAnswerRepository.PublisherKey;

            var item = keyvalupair.Where(x => x.Key.ToLower() == "responseid" || x.Key.ToLower() == "id").FirstOrDefault(); //  if (keyvalupair.TryGetValue("ResponseId", out ResponseId))
            if (item.Value != null)
            {
                responseId = item.Value;
                surveyanswerModel.SurveyQuestionAnswerListField = keyvalupair;
                var Result = _isurveyAnswerRepository.Update(surveyanswerModel, responseId);
                if (Result.SurveyResponseID != null)
                {
                    if (Result.Status == "Created")
                    {
                        var response = Request.CreateResponse<PreFilledAnswerResponse>(System.Net.HttpStatusCode.Created, Result);//201 Created Success with response body.
                        string uri = Url.Link("DefaultApi", new { id = Result.SurveyResponseID });
                        response.Headers.Location = new Uri(uri);
                        return response;
                    }
                    else
                    {
                        var response = Request.CreateResponse<PreFilledAnswerResponse>(System.Net.HttpStatusCode.OK, Result);//200 OK Success with response body.
                        string uri = Url.Link("DefaultApi", new { id = Result.SurveyResponseID });
                        response.Headers.Location = new Uri(uri);
                        return response;
                    }
                }
                else
                {
                    var response = Request.CreateResponse(HttpStatusCode.Forbidden, "Response not generated");// The requested operation is not permitted for the user. This error can also be caused by ACL failures, or business rule or data policy constraints.
                    return response;
                }
            }
            else
            {
                var response = Request.CreateResponse(HttpStatusCode.UnsupportedMediaType, "Response not generated");  //415 Unsupported media type The endpoint does not support the format of the request body.             
                return response;
            }
        }


        /// <summary>
        /// Handle HTTPDELETE request coming in from the client after successful Authentication
        /// </summary>
        /// <param name="request"></param>
        /// <returns>HTTPRespose code with succee/failure</returns>
        public HttpResponseMessage Delete(HttpRequestMessage request)
        {
            Dictionary<string, string> keyvalupair = new Dictionary<string, string>();
            var value = request.Content.ReadAsStringAsync().Result;
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            try
            {
                keyvalupair = JsonConvert.DeserializeObject<Dictionary<string, string>>(value, settings);
            }
            catch (Exception ex)
            {
                var response = Request.CreateResponse(HttpStatusCode.UnsupportedMediaType, ex.Message.ToString());//415 Unsupported media type The endpoint does not support the format of the request body. 
                return response;
            }
            string responseId;
            SurveyResponseApiModel surveyanswerModel = new SurveyResponseApiModel();
            surveyanswerModel.SurveyId = _isurveyAnswerRepository.SurveyId;
            surveyanswerModel.OrgKey = _isurveyAnswerRepository.OrgKey;
            surveyanswerModel.PublisherKey = _isurveyAnswerRepository.PublisherKey;
            var item = keyvalupair.Where(x => x.Key.ToLower() == "responseid" || x.Key.ToLower() == "id").FirstOrDefault(); //  if (keyvalupair.TryGetValue("ResponseId", out ResponseId))
            if (item.Value != null)
            {
                responseId = item.Value;
                try
                {
                    _isurveyAnswerRepository.Remove(responseId);
                }
                catch (Exception ex)
                {
                    var exresponse = Request.CreateResponse(HttpStatusCode.Forbidden, "Response does not exist");//The request has not succeeded. The information returned with the response is dependent on the method used in the request.
                    return exresponse;
                }
                var response = Request.CreateResponse(HttpStatusCode.OK, "Response Deleted.");//The request has succeeded. The information returned with the response is dependent on the method used in the request.
                return response;
            }
            else
            {
                var response = Request.CreateResponse(HttpStatusCode.UnsupportedMediaType, "Response not generated");//415 Unsupported media type The endpoint does not support the format of the request body. 
                return response;
            }
        }
    }
}
