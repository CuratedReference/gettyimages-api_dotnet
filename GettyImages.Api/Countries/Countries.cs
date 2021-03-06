﻿using System.Net.Http;
using System.Threading.Tasks;

namespace GettyImages.Api.Countries
{
    public class Countries : ApiRequest
    {
        protected const string V3CountriesPath = "/countries";

        private Countries(Credentials credentials, string baseUrl, DelegatingHandler customHandler) : base(customHandler)
        {
            Credentials = credentials;
            BaseUrl = baseUrl;
        }

        internal static Countries GetInstance(Credentials credentials, string baseUrl, DelegatingHandler customHandler)
        {
            return new Countries(credentials, baseUrl, customHandler);
        }

        public override async Task<dynamic> ExecuteAsync()
        {
            Method = "GET";
            Path = V3CountriesPath;

            return await base.ExecuteAsync();
        }

        public Countries WithAcceptLanguage(string value)
        {
            AddHeaderParameter(Constants.AcceptLanguage, value);
            return this;
        }
    }
}
