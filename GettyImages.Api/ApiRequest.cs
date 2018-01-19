using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;

namespace GettyImages.Api
{
    public class ApiRequest
    {
        private readonly DelegatingHandler _customHandler;
        private const string SpaceString = " ";
        protected string BaseUrl;
        protected Credentials Credentials;
        protected string Method;
        protected string Path;
        protected internal IDictionary<string, object> QueryParameters;
        protected internal IDictionary<string, object> HeaderParameters;

        public ApiRequest(DelegatingHandler customHandler)
        {
            _customHandler = customHandler;
            QueryParameters = new Dictionary<string, object>();
            HeaderParameters = new Dictionary<string, object>();

        }

        public virtual Task<dynamic> ExecuteAsync()
        {
            var helper = new WebHelper(Credentials, BaseUrl, _customHandler);
            switch (Method)
            {
                case "GET":
                    return helper.Get(BuildQuery(QueryParameters), Path, BuildHeaders(HeaderParameters));
                case "POST":
                    return helper.PostQuery(BuildQuery(QueryParameters), Path, BuildHeaders(HeaderParameters));
                default:
                    throw new SdkException("No appropriate HTTP method found for this request.");
            }
        }

        private static IEnumerable<KeyValuePair<string, string>> BuildQuery(
            IEnumerable<KeyValuePair<string, object>> queryParameters)
        {
            var keyValuePairs = queryParameters as KeyValuePair<string, object>[] ??
                                queryParameters.ToArray();

            return keyValuePairs.Where(v => v.Value is string || v.Value is bool || v.Value is int)
                .Select(
                    d =>
                        new KeyValuePair<string, string>(d.Key,
                            d.Value.ToString()))
                .Union(keyValuePairs
                    .Where(v => v.Value is Enum)
                    .Select(
                        d =>
                            new KeyValuePair<string, string>(d.Key,
                                BuildEnumString((Enum) d.Value))))
                .Union(keyValuePairs
                    .Where(v => v.Value is IEnumerable<string>)
                    .Select(
                        d => new KeyValuePair<string, string>(d.Key, string.Join(",", (IEnumerable<string>) d.Value))))
                .AsEnumerable();
        }

        private static IEnumerable<KeyValuePair<string, string>> BuildHeaders(
            IEnumerable<KeyValuePair<string, object>> headerParameters)
        {
            var keyValuePairs = headerParameters as KeyValuePair<string, object>[] ??
                                headerParameters.ToArray();

            return keyValuePairs.Where(v => v.Value is string)
                .Select(
                    d =>
                        new KeyValuePair<string, string>(d.Key,
                            d.Value.ToString()))
                .Union(keyValuePairs
                    .Where(v => v.Value is Enum)
                    .Select(
                        d =>
                            new KeyValuePair<string, string>(d.Key,
                                BuildEnumString((Enum)d.Value))))
                .Union(keyValuePairs
                    .Where(v => v.Value is IEnumerable<string>)
                    .Select(
                        d => new KeyValuePair<string, string>(d.Key, string.Join(",", (IEnumerable<string>)d.Value))))
                .AsEnumerable();
        }

        private static string BuildEnumString(Enum value)
        {
            return
                value.GetType().GetTypeInfo().CustomAttributes.Where(a => a.AttributeType == typeof (FlagsAttribute)) !=
                null
                    ? string.Join(",", GetFlags(value).Select(GetEnumDescription).ToArray())
                    : GetEnumDescription(value).ToLowerInvariant().Replace(SpaceString, string.Empty);
        }

        private static string GetEnumDescription(Enum value)
        {
            var fi = value.GetType().GetRuntimeFields().FirstOrDefault(f => f.Name == value.ToString());
            if (fi != null)
            {
                var attributes =
                    (DescriptionAttribute[]) fi.GetCustomAttributes(typeof (DescriptionAttribute), false);
                if (attributes != null && attributes.Length > 0) return attributes[0].Description;
            }

            return value.ToString().ToLowerInvariant();
        }

        private static IEnumerable<Enum> GetFlags(Enum input)
        {
            return Enum.GetValues(input.GetType()).Cast<Enum>().Where(input.HasFlag).Where(v => Convert.ToInt64(v) != 0);
        }

        protected void AddQueryParameter(string key, object value)
        {
            if (!key.Equals("ids") && value is string)
            {
                value = value.ToString().ToLowerInvariant();
            }

            if (QueryParameters.ContainsKey(key))
            {
                QueryParameters[key] = value;
            }
            else
            {
                QueryParameters.Add(key, value);
            }
        }

        protected void AddHeaderParameter(string key, object value)
        {
            if (HeaderParameters.ContainsKey(key))
            {
                HeaderParameters[key] = value;
            }
            else
            {
                HeaderParameters.Add(key, value);
            }
        }
    }
}