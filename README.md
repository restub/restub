# Restub

Restub is a framework for implementing REST API clients with built-in tracing.  
Uses RestSharp library and Newtonsoft.Json serializer behind the scenes.

# Getting started

TODO

## Sample REST client usage

```c#
// connect to sandbox API
var client = new CdekClient();

// trace all API calls to console
client.Tracer = Console.WriteLine;

// get 10 regions
var regions = client.GetRegions(size: 10);

// get all Russian and Chinese cities
var cities = client.GetCities(new[] { "ru", "cn" });
```

## Sample REST client implementation

```c#
public CdekRegion[] GetRegions(int? size = null, int? page = null) =>
  Get<CdekRegion[]>("location/regions", r => r.AddQueryString(new { size, page }));

public CdekCity[] GetCities(string[] countries = null, string city = null) =>
  Get<CdekCity[]>("location/cities", r => r.AddQueryString(new
  {
    city,
    country_codes = countries,
  }));
```

<details>
  <summary>A typical trace log looks like this:</summary>
    
```c
// GetAuthToken
-> POST https://api.edu.cdek.ru/v2/oauth/token?parameters
headers: {
  X-ApiMethodName = GetAuthToken
  Accept = application/json, text/json, text/x-json, text/javascript, application/xml, text/xml
  Content-type = application/json
}
body: null

<- OK 200 (OK) https://api.edu.cdek.ru/v2/oauth/token?parameters
timings: {
  started: 2022-08-31 15:30:57
  elapsed: 0:00:00.812
}
headers: {
  Transfer-Encoding = chunked
  Connection = keep-alive
  Keep-Alive = timeout=15
  Vary = Accept-Encoding
  Pragma = no-cache
  X-Content-Type-Options = nosniff
  X-XSS-Protection = 1; mode=block
  X-Frame-Options = DENY
  Content-Encoding = 
  Cache-Control = no-store
  Content-Type = application/json;charset=utf-8
  Date = Wed, 31 Aug 2022 12:30:59 GMT
  Server = QRATOR
}
body: {
  "access_token": "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJzY29wZSI6WyJvcmRlcjphbGwiLCJwYXltZW50OmFsbCJdLCJleHAiOjE2NjE5NTI2NTksImF1dGhvcml0aWVzIjpbInNoYXJkLWlkOnJ1LTAxIiwiY2xpZW50LWNpdHk60J3QvtCy0L7RgdC40LHQuNGA0YHQuiwg0J3QvtCy0L7RgdC40LHQuNGA0YHQutCw0Y8g0L7QsdC70LDRgdGC0YwiLCJmdWxsLW5hbWU60KLQtdGB0YLQuNGA0L7QstCw0L3QuNC1INCY0L3RgtC10LPRgNCw0YbQuNC4INCY0JwsINCe0JHQqdCV0KHQotCS0J4g0KEg0J7Qk9Cg0JDQndCY0KfQldCd0J3QntCZINCe0KLQktCV0KLQodCi0JLQldCd0J3QntCh0KLQrNCuIiwiYWNjb3VudC1sYW5nOnJ1cyIsImNvbnRyYWN0OtCY0Jwt0KDQpC3Qk9Cb0JMtMjIiLCJhY2NvdW50LXV1aWQ6ZTkyNWJkMGYtMDVhNi00YzU2LWI3MzctNGI5OWMxNGY2NjlhIiwiYXBpLXZlcnNpb246MS4xIiwiY2xpZW50LWlkLWVjNTplZDc1ZWNmNC0zMGVkLTQxNTMtYWZlOS1lYjgwYmI1MTJmMjIiLCJjbGllbnQtaWQtZWM0OjE0MzQ4MjMxIiwic29saWQtYWRkcmVzczpmYWxzZSIsImNvbnRyYWdlbnQtdXVpZDplZDc1ZWNmNC0zMGVkLTQxNTMtYWZlOS1lYjgwYmI1MTJmMjIiXSwianRpIjoiOGQ3MDc0MWYtODc3Ni00MTFjLTgwZjEtZjg3MGI2MDhiYzUyIiwiY2xpZW50X2lkIjoiRU1zY2Q2cjlKbkZpUTNiTG95akpZNmVNNzhKckpjZUkifQ.Ksoyu9zJHSc9AKqfytjwURwO3Eba03y0mC2LcN9cHTzKYJ-fSQzsjTk6z0qI4GeFgMHGrhEfrXPGMr19TwvsaTUKxfTObFnKhaN_xOfCDgZarI_Y5X3_rcGlBMxcbSRQKiKLuZ0c1ob6gTrFo4AuxiD5LyaJJJ4WCQRWkJJJu9zGuE_s2rRwpegcB6B2AqvGlfGrTDvaSgvJqWFAYNkFgGAjDYLvzdIrUD-C0Cad7p6eFvfML68Nh73Y4qityvge1PIZvYaQOAGzP_eeoFoDNxK4ygxqm64wem4umx0pYKZaacdYA6WV-ptfEayfd_Dxq00EGA-z8dYtyD6Y8yToig",
  "token_type": "bearer",
  "expires_in": 3599,
  "scope": "order:all payment:all",
  "jti": "8d70741f-8776-411c-80f1-f870b608bc52"
}

// GetRegions
-> GET https://api.edu.cdek.ru/v2/location/regions?page=3
headers: {
  X-ApiMethodName = GetRegions
  Authorization = Bearer eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJzY29wZSI6WyJvcmRlcjphbGwiLCJwYXltZW50OmFsbCJdLCJleHAiOjE2NjMzNzU5MTUsImF1dGhvcml0aWVzIjpbInNoYXJkLWlkOnJ1LTAxIiwiY2xpZW50LWNpdHk60J3QvtCy0L7RgdC40LHQuNGA0YHQuiwg0J3QvtCy0L7RgdC40LHQuNGA0YHQutCw0Y8g0L7QsdC70LDRgdGC0YwiLCJmdWxsLW5hbWU60KLQtdGB0YLQuNGA0L7QstCw0L3QuNC1INCY0L3RgtC10LPRgNCw0YbQuNC4INCY0JwsINCe0JHQqdCV0KHQotCS0J4g0KEg0J7Qk9Cg0JDQndCY0KfQldCd0J3QntCZINCe0KLQktCV0KLQodCi0JLQldCd0J3QntCh0KLQrNCuIiwiY29udHJhY3Q60JjQnC3QoNCkLdCT0JvQky0yMiIsImFjY291bnQtbGFuZzpydXMiLCJhY2NvdW50LXV1aWQ6ZTkyNWJkMGYtMDVhNi00YzU2LWI3MzctNGI5OWMxNGY2NjlhIiwiYXBpLXZlcnNpb246MS4xIiwiY2xpZW50LWlkLWVjNTplZDc1ZWNmNC0zMGVkLTQxNTMtYWZlOS1lYjgwYmI1MTJmMjIiLCJjbGllbnQtaWQtZWM0OjE0MzQ4MjMxIiwic29saWQtYWRkcmVzczpmYWxzZSIsImNvbnRyYWdlbnQtdXVpZDplZDc1ZWNmNC0zMGVkLTQxNTMtYWZlOS1lYjgwYmI1MTJmMjIiXSwianRpIjoiNGUxNzhiOWQtMTEyZC00MWI3LTkxZDYtMjQ3YTNhZmU0YjNiIiwiY2xpZW50X2lkIjoiRU1zY2Q2cjlKbkZpUTNiTG95akpZNmVNNzhKckpjZUkifQ.YZuu2PiWeWfaXFAXHNMwJw6WUMFuw5okZM4_zqsrSzYD1FyodyZKEhcQqUQ-WB3LtCjdxALflIYBbjVWNGhaQqhC27jY0BFQmZFK-YyrGvQwCFrt5OFdd95QUoi9FHASHsG4JiFUALwVF3UO4bFMXPwSF4x3rwGOyjGVXyD929XdC9wiK7hpvuvMWm2A_WwqGcTldKPh5_7EF8M2CNnlrUMsHLsSEG8NiBqoL1fyL0ZwGfg15qqsxlNQoycbgkrQHQl32X8_DR7erh69w5MBH5Lt6u-Xeurc3o7K9zPCzNArIs2QXRYqgSx269dRHsqecE5ZYNHFaEiZxWVaKlWbgQ
  Accept = application/json, text/json, text/x-json, text/javascript, application/xml, text/xml
}

<- ERROR 400 (BadRequest) https://api.edu.cdek.ru/v2/location/regions?page=3
timings: {
  started: 2022-09-17 02:51:56
  elapsed: 0:00:00.063
}
headers: {
  Server = QRATOR
  Date = Fri, 16 Sep 2022 23:51:57 GMT
  Transfer-Encoding = chunked
  Connection = keep-alive
  Keep-Alive = timeout=15
  X-Content-Type-Options = nosniff
  X-XSS-Protection = 1; mode=block
  Cache-Control = no-store, must-revalidate, no-cache, max-age=0
  Pragma = no-cache
  X-Frame-Options = DENY
  Content-Type = application/json
  Expires = 0
}
body: {
  "errors": [
    {
      "code": "v2_field_is_empty",
      "message": "[size] is empty"
    }
  ]
}
```
</details>

# SDK versioning

The project uses [Nerdbank.GitVersioning](https://github.com/dotnet/Nerdbank.GitVersioning) tool to manage versions.  
Each library build can be traced back to the original git commit.

## Preparing and publishing a new release

1. Make sure that `nbgv` dotnet CLI tool is installed and is up to date
2. Run `nbgv prepare-release` to create a stable branch for the upcoming release, i.e. release/v1.0
3. Switch to the release branch: `git checkout release/v1.0`
4. Execute unit tests, update the README, etc. Commit and push your changes.
5. Run `dotnet pack -c Release` and check that it builds Nuget packages with the right version number.
6. Run `nbgv tag release/v1.0` to tag the last commit on the release branch with your current version number, i.e. v1.0.7.
7. Push tags as suggested by nbgv tool: `git push origin v1.0.7`
8. Go to github project page and create a release out of the last tag v1.0.7.
9. Verify that github workflow for publishing the nuget package has completed.
