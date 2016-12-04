# Web API Token

## Abstract

For many actions defined by the business logic of the application it is required to know the user who made the request.  
There is the need for recognize the user in the Web API calls.  
Using the out of the box ASP.Net authentication (enabled by default) is not safe because the user is recognized by an ASP session token that is stored in a cookie.  
Running the tests the returned user stored by this mechanism is the user that is running the code and is not linked to a valid user of the application.  
This mechanism is not applicable when the API is not consumed by the web site.  

It is ok to use a GUID as token passed by HTTP header in the API calls.  
It must not be used the User or Account ID in this calls. A Token can be replaced anytime (and can be temporary), 
instead if a Account or Usaer ID is compromised is a problem.

## Storage

There is no way for the web API to see the ASP Session, 
so the token must be stored in the default storage system of the application (database).  
The token should be related to the User/Account.

## API Session Token


