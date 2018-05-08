# IvantiRestApi
Beginners how-to use the Ivanti Rest-API with c# and RestSharp

## Get startet

To make use of the powerfull Ivanti Rest API you need to follow some simple steps:

* Create an IdentityServer3 client
* Get access token
* Call Rest APIs

### Create an IdentityServer3 client

Navigate to *C:\ProgramData\LANDesk\ServiceDesk\My.IdentityServer\IdentityServer3.Core.Models.Client.json* file and add following code:

```json
{
   "ClientSecrets": [
		{ 
			"Value":"Interchange",
			"Description":"resource only password secret",
			"Type":"SharedSecret",
			"Expiration":null
		}
	],
   "Enabled":true,
   "ClientId":"ResourceOnly",
   "ClientName":"ResourceOnly",
   "ClientUri":null,
   "LogoUri":null,
   "RequireConsent":true,
   "AllowRememberConsent":true,
   "Flow":4,
   "AllowClientCredentialsOnly":false,
   "RedirectUris":[],
   "PostLogoutRedirectUris":[],
   "LogoutUri":null,
   "LogoutSessionRequired":true,
   "RequireSignOutPrompt":false,
   "AllowAccessToAllScopes":true,
   "AllowedScopes":[],
   "IdentityTokenLifetime":300,
   "AccessTokenLifetime":3600,
   "AuthorizationCodeLifetime":300,
   "AbsoluteRefreshTokenLifetime":2592000,
   "SlidingRefreshTokenLifetime":1296000,
   "RefreshTokenUseage":1,
   "UpdateAccessTokenClaimsOnRefresh":false,
   "RefreshTokenExpiration":1,
   "AccessTokenType":0,
   "EnableLocalLogin":true,
   "IdentityProviderRestrictions":[],
   "IncludeJwtId":false,
   "Claims":[],
   "AlwaysSendClientClaims":false,
   "PrefixClientClaims":true,
   "AllowAccessToAllCustomGrantTypes":false,
   "AllowedCustomGrantTypes":[],
   "AllowedCorsOrigins":[],
   "AllowAccessTokensViaBrowser":true
}
```

## Get access token

Download the sample project *IvantiSampleRestApi* :star: and use the IvantiRestController which handle the token authentication for you.

```c#
IvantiRestController controller =
 new IvantiRestController(
    new RestAccessProperties{
       CoreServerName = "EPMCore2018",
       ClientId = "ResourceOnly",
       ClientSecret = "Interchange",
       Username = "domain\\user",
       Password = "Interchange2018$"
    }, ignoreServerCertificateValidation: true);
```

## Call Rest APIs

Once you have created an instance of IvantiRestController, you can implement and call functions of the API

```c#
controller.CreateTaskFromTemplate(templateName: "Template Name", newTaskName: "New Task Tame", packageId: "1087", errorMessage: ref errorMessage);
```
