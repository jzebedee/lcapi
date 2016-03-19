﻿using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using LCAPI.JSON;

namespace LCAPI.REST
{
    public class RestClient : IRestClient
    {
        protected HttpClient Client { get; } = new HttpClient();
        protected IDeserializer Deserializer { get; }

        public RestClient(IDeserializer deserializer)
        {
            Deserializer = deserializer;
        }

        public HttpRequestHeaders RequestHeaders => Client.DefaultRequestHeaders;

        public async Task<string> GetAsync(string url)
        {
            var response = await Client.GetAsync(url);
            ValidateResponse(response);

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<T> GetAsync<T>(string url)
        {
            var response = await Client.GetAsync(url);
            ValidateResponse(response);

            var content = await response.Content.ReadAsStringAsync();
            return Deserializer.Deserialize<T>(content);
        }

        private void ValidateResponse(HttpResponseMessage response)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized: //401
                case HttpStatusCode.Forbidden: //403
                    //TODO: add message
                    throw new Exception();
                case HttpStatusCode.BadRequest: //400
                    //TODO: add message
                    throw new Exception();
                case HttpStatusCode.NotFound: //404
                    //TODO: add message
                    throw new Exception();
                case HttpStatusCode.InternalServerError: //500
                    //TODO: add message
                    throw new Exception();
                case HttpStatusCode.OK: //200
                    break;
                default:
                    throw new NotImplementedException($"Response {response.StatusCode} was not expected");
            }
        }
    }
}
