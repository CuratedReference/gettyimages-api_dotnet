﻿using NUnit.Framework;
using TechTalk.SpecFlow;

namespace GettyImages.Connect.Tests
{
    internal class ScenarioCredentialsHelper
    {
        public static ConnectSdk GetCredentials()
        {
            if (ScenarioContext.Current.ContainsKey("apikey")
               && ScenarioContext.Current.ContainsKey("apisecret")
               && ScenarioContext.Current.ContainsKey("refreshtoken"))
            {
                return  ConnectSdk.GetConnectSdkWithRefreshToken(ScenarioContext.Current.Get<string>("apikey"),
                    ScenarioContext.Current.Get<string>("apisecret"),
                    ScenarioContext.Current.Get<string>("refreshtoken"));
            }

            if (ScenarioContext.Current.ContainsKey("apikey")
                && ScenarioContext.Current.ContainsKey("apisecret")
                && !ScenarioContext.Current.ContainsKey("username"))
            {
                return  ConnectSdk.GetConnectSdkWithClientCredentials(
                    ScenarioContext.Current.Get<string>("apikey"),
                    ScenarioContext.Current.Get<string>("apisecret"));
            }

            if (ScenarioContext.Current.ContainsKey("apikey")
                && ScenarioContext.Current.ContainsKey("apisecret")
                && ScenarioContext.Current.ContainsKey("username")
                && ScenarioContext.Current.ContainsKey("userpassword"))
            {
                return  ConnectSdk.GetConnectSdkWithResourceOwnerCredentials(
                    ScenarioContext.Current.Get<string>("apikey"),
                    ScenarioContext.Current.Get<string>("apisecret"),
                    ScenarioContext.Current.Get<string>("username"),
                    ScenarioContext.Current.Get<string>("userpassword"));
            }

            Assert.Fail(
                "Unable to determine which credentials to use. Did you include the proper steps in your scenario?");

            return null;
        }
    }
}