using NUnit.Framework;
using Restub.Toolbox;

namespace Restub.Tests
{
    [TestFixture]
    public class JsonFormatterTests
    {
        // empty strings
        [TestCase(null, "")]
        [TestCase("", "")]
        [TestCase("    ", "")]

        // scalar values
        [TestCase("1", "1")]
        [TestCase("  1 ", "1")]
        [TestCase(" \"s \"", "\"s \"")]
        [TestCase(" \"s\\t\\r \"", "\"s\\t\\r \"")]

        // object values
        [TestCase("{}", "{\r\n  \r\n}")]
        [TestCase("{\"i\":1,\"s\":\"321\"}", "{\r\n  \"i\": 1,\r\n  \"s\": \"321\"\r\n}")]
        [TestCase("{\"o\":{\"t\":null},\"i\":1,\"s\":\"321\"}", "{\r\n  \"o\": {\r\n    \"t\": null\r\n  },\r\n  \"i\": 1,\r\n  \"s\": \"321\"\r\n}")]

        // array values
        [TestCase("[]", "[\r\n  \r\n]")]
        [TestCase("[\"i\",1,\"s\",321]", "[\r\n  \"i\",\r\n  1,\r\n  \"s\",\r\n  321\r\n]")]
        [TestCase("{\"o\":{\"t\":[]},\"i\":[1,\"s\",\"321\"]}", "{\r\n  \"o\": {\r\n    \"t\": [\r\n      \r\n    ]\r\n  },\r\n  \"i\": [\r\n    1,\r\n    \"s\",\r\n    \"321\"\r\n  ]\r\n}")]

        // json examples taken from real-world apis
        [TestCase("{\"login\":\"restub\",\"id\":113724178,\"node_id\":\"O_kgDOBsdLEg\",\"avatar_url\":\"https://avatars.githubusercontent.com/u/113724178?v=4\",\"gravatar_id\":\"\",\"url\":\"https://api.github.com/users/restub\",\"html_url\":\"https://github.com/restub\",\"followers_url\":\"https://api.github.com/users/restub/followers\",\"following_url\":\"https://api.github.com/users/restub/following{/other_user}\",\"gists_url\":\"https://api.github.com/users/restub/gists{/gist_id}\",\"starred_url\":\"https://api.github.com/users/restub/starred{/owner}{/repo}\",\"subscriptions_url\":\"https://api.github.com/users/restub/subscriptions\",\"organizations_url\":\"https://api.github.com/users/restub/orgs\",\"repos_url\":\"https://api.github.com/users/restub/repos\",\"events_url\":\"https://api.github.com/users/restub/events{/privacy}\",\"received_events_url\":\"https://api.github.com/users/restub/received_events\",\"type\":\"Organization\",\"site_admin\":false,\"name\":null,\"company\":null,\"blog\":\"\",\"location\":null,\"email\":null,\"hireable\":null,\"bio\":\"stub rest api client framework\",\"twitter_username\":null,\"public_repos\":2,\"public_gists\":0,\"followers\":0,\"following\":0,\"created_at\":\"2022-09-16T19:51:53Z\",\"updated_at\":\"2022-10-24T21:24:22Z\"}",
            "{\r\n  \"login\": \"restub\",\r\n  \"id\": 113724178,\r\n  \"node_id\": \"O_kgDOBsdLEg\",\r\n  \"avatar_url\": \"https://avatars.githubusercontent.com/u/113724178?v=4\",\r\n  \"gravatar_id\": \"\",\r\n  \"url\": \"https://api.github.com/users/restub\",\r\n  \"html_url\": \"https://github.com/restub\",\r\n  \"followers_url\": \"https://api.github.com/users/restub/followers\",\r\n  \"following_url\": \"https://api.github.com/users/restub/following{/other_user}\",\r\n  \"gists_url\": \"https://api.github.com/users/restub/gists{/gist_id}\",\r\n  \"starred_url\": \"https://api.github.com/users/restub/starred{/owner}{/repo}\",\r\n  \"subscriptions_url\": \"https://api.github.com/users/restub/subscriptions\",\r\n  \"organizations_url\": \"https://api.github.com/users/restub/orgs\",\r\n  \"repos_url\": \"https://api.github.com/users/restub/repos\",\r\n  \"events_url\": \"https://api.github.com/users/restub/events{/privacy}\",\r\n  \"received_events_url\": \"https://api.github.com/users/restub/received_events\",\r\n  \"type\": \"Organization\",\r\n  \"site_admin\": false,\r\n  \"name\": null,\r\n  \"company\": null,\r\n  \"blog\": \"\",\r\n  \"location\": null,\r\n  \"email\": null,\r\n  \"hireable\": null,\r\n  \"bio\": \"stub rest api client framework\",\r\n  \"twitter_username\": null,\r\n  \"public_repos\": 2,\r\n  \"public_gists\": 0,\r\n  \"followers\": 0,\r\n  \"following\": 0,\r\n  \"created_at\": \"2022-09-16T19:51:53Z\",\r\n  \"updated_at\": \"2022-10-24T21:24:22Z\"\r\n}")]
        [TestCase("{\"version_api\":2,\"version\":\"2.14.4.682\",\"caption\":\"Расчет тарифов\",\"id\":2020,\"name\":\"Письмо с объявленной ценностью\",\"mailtype\":2,\"mailctg\":2,\"directctg\":1,\"weight\":20,\"sumoc\":10000,\"date\":20221029,\"time\":20100,\"date-first\":20220101,\"transtype\":1,\"transname\":\"наземно\",\"items\":[{\"id\":\"3168\",\"name\":\"Пересылка письма с объявленной ценностью\",\"serviceon\":[110,11],\"serviceoff\":[53,57],\"tariff\":{\"val\":12100,\"valnds\":14520,\"valmark\":12100}},{\"id\":\"2432\",\"name\":\"Плата за объявленную ценность\",\"serviceon\":[13,110],\"tariff\":{\"val\":300,\"valnds\":360}}],\"ground\":{\"val\":12100,\"valnds\":14520,\"valmark\":12100},\"cover\":{\"val\":300,\"valnds\":360},\"paymark\":12100,\"paymoney\":300,\"paymoneynds\":360,\"pay\":12400,\"paynds\":14880,\"ndsrate\":20,\"nds\":2480,\"place\":\"C5-r00-2\"}",
            "{\r\n  \"version_api\": 2,\r\n  \"version\": \"2.14.4.682\",\r\n  \"caption\": \"Расчет тарифов\",\r\n  \"id\": 2020,\r\n  \"name\": \"Письмо с объявленной ценностью\",\r\n  \"mailtype\": 2,\r\n  \"mailctg\": 2,\r\n  \"directctg\": 1,\r\n  \"weight\": 20,\r\n  \"sumoc\": 10000,\r\n  \"date\": 20221029,\r\n  \"time\": 20100,\r\n  \"date-first\": 20220101,\r\n  \"transtype\": 1,\r\n  \"transname\": \"наземно\",\r\n  \"items\": [\r\n    {\r\n      \"id\": \"3168\",\r\n      \"name\": \"Пересылка письма с объявленной ценностью\",\r\n      \"serviceon\": [\r\n        110,\r\n        11\r\n      ],\r\n      \"serviceoff\": [\r\n        53,\r\n        57\r\n      ],\r\n      \"tariff\": {\r\n        \"val\": 12100,\r\n        \"valnds\": 14520,\r\n        \"valmark\": 12100\r\n      }\r\n    },\r\n    {\r\n      \"id\": \"2432\",\r\n      \"name\": \"Плата за объявленную ценность\",\r\n      \"serviceon\": [\r\n        13,\r\n        110\r\n      ],\r\n      \"tariff\": {\r\n        \"val\": 300,\r\n        \"valnds\": 360\r\n      }\r\n    }\r\n  ],\r\n  \"ground\": {\r\n    \"val\": 12100,\r\n    \"valnds\": 14520,\r\n    \"valmark\": 12100\r\n  },\r\n  \"cover\": {\r\n    \"val\": 300,\r\n    \"valnds\": 360\r\n  },\r\n  \"paymark\": 12100,\r\n  \"paymoney\": 300,\r\n  \"paymoneynds\": 360,\r\n  \"pay\": 12400,\r\n  \"paynds\": 14880,\r\n  \"ndsrate\": 20,\r\n  \"nds\": 2480,\r\n  \"place\": \"C5-r00-2\"\r\n}")]

        [Test]
        public void JsonFormatterTestSuite(string input, string output)
        {
            // test case
            Assert.That(JsonFormatter.FormatJson(input), Is.EqualTo(output));

            // idempotency
            Assert.That(JsonFormatter.FormatJson(output), Is.EqualTo(output));
        }
    }
}
