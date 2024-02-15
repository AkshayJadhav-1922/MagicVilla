﻿using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Services.IServices;
using Newtonsoft.Json;
using System.Text;

namespace MagicVilla_Web.Services
{
    public class BaseService : IBaseService
    {
        public APIResponse responseModel { get; set; }
        public IHttpClientFactory httpClient { get; set; }
        public BaseService(IHttpClientFactory httpClient)
        {
            responseModel = new APIResponse();
            this.httpClient = httpClient;

        }

        /// <summary>
        /// step1: Create http clinet using CreateClient method of httpClient
        /// step2: Create HttpRequestMessage object 
        /// step3: Add Headers to request message
        /// step4: Add Uri using its object to request message
        /// step5: Add data as a content to request message using StringContent, remeber to serialize data
        /// step6: Add HttpMethod to request message
        /// step7: Initialize HttpResponseMessage to store response
        /// step8: Make API call using client.SendAsync(message)
        /// step9: Read Content using ReadAsStringAsync
        /// step10: Deserialize it and then return response
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiRequest"></param>
        /// <returns></returns>
        public async Task<T> SendAsync<T>(APIRequest apiRequest)
        {
            try
            {
                var client = httpClient.CreateClient("MagicAPI");

                
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(apiRequest.Url);

                if(apiRequest.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data),
                        Encoding.UTF8, "application/json");
                }
                switch (apiRequest.ApiType)
                {
                    case SD.ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case SD.ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case SD.ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }

                HttpResponseMessage apiResponse = null;
                apiResponse = await client.SendAsync(message);

                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                APIResponse ApiResponse = JsonConvert.DeserializeObject<APIResponse>(apiContent);
                if(ApiResponse != null)
                {
                    if(ApiResponse.StatusCode == System.Net.HttpStatusCode.NotFound ||ApiResponse.StatusCode ==  System.Net.HttpStatusCode.BadRequest)
                    {
                        ApiResponse.IsSuccess = false;
                    }
                    var res = JsonConvert.SerializeObject(ApiResponse);
                    var APIResponse = JsonConvert.DeserializeObject<T>(res);
                    return APIResponse;
                }
                var ApiResp = JsonConvert.DeserializeObject<T>(apiContent);
                return ApiResp;
            }
            catch(Exception ex)
            {
                var dto = new APIResponse
                {
                    ErrorMessages = new List<string> { Convert.ToString(ex.Message) },
                    IsSuccess = false
                };
                var res = JsonConvert.SerializeObject(dto);
                var APIResponse = JsonConvert.DeserializeObject<T>(res);
                return APIResponse;
            }
        }
    }
}
