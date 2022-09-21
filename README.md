# Restub

[![.NET](https://github.com/restub/restub/actions/workflows/dotnet.yml/badge.svg)](https://github.com/restub/restub/actions/workflows/dotnet.yml)
[![CodeFactor](https://www.codefactor.io/repository/github/restub/restub/badge)](https://www.codefactor.io/repository/github/restub/restub)
[![.NET Framework 4.62](https://img.shields.io/badge/.net-v4.62-yellow)](https://dotnet.microsoft.com/en-us/download/dotnet-framework/net462)
[![.NET 6.0](https://img.shields.io/badge/.net-v6.0-orange)](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

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
  "access_token": "eyJhbGciOiJSUzI1NiIsInR5cCI...8yToig",
  "token_type": "bearer",
  "expires_in": 3599,
  "scope": "order:all payment:all",
  "jti": "8d70741f-8776-411c-80f1-f870b608bc52"
}

// GetRegions
-> GET https://api.edu.cdek.ru/v2/location/regions?page=3
headers: {
  X-ApiMethodName = GetRegions
  Authorization = Bearer eyJhbGciOiJSUzI1NiIsInR5cCI...8yToig
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
