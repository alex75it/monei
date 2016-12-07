# Web API Token

## Abstract

For many actions defined in the business logic it is required to know the user who request the operation.  
This means it is needed to recognize the user who is making the Web API call.  
Using the out of the box ASP.Net authentication (enabled by default) is not safe because the user is recognized by an ASP session token that is stored in a cookie.  
Running the tests the returned user stored by this mechanism is the user that is running the code and is not related to a valid user of the application.  
This mechanism is not applicable when the API is not consumed by the web site.  

It is ok to use a GUID as token passed in a specific HTTP header in the API calls.  
It should not be used the User or Account ID in this calls. A Token can be replaced anytime (and can be temporary), 
instead if a Account or Usaer ID is compromised it is a problem.

## Storage

There is no way for the web API to see the ASP Session, 
so the token must be stored in the default storage system of the application (actually a database).  
The token should be related to the User/Account.

## API Session Token
When a client want to call a web API method it have to pass a session API token stored in the "api_token" HTP header.  
The value should by a GUID.  
If the client does not have that GUID it can request one calling the a specific method that return the session API token.  

### Token request
That method accept username and password as parameters.  
The call check the credentials and load the Account data, then it creates a record in the storage system containing these information:
UserId, Token, Creation date, Last use date, Expiry date, Client IP.
A boolean parameter can indicate to not generate a new one but use a previous one (if exists).
This is the common case where a previous call return the error for token expired.

### Duration of the token
When the token is generated it is stored with a precise expiry date that is defined by a precise duration based on business rule settings (example, 2 hours).
This time should be sufficient to permit the use of the API during a common web session.
Everytime a token is loaded for validate a Web aPI call the" Last use date" and "Expiry date" are updated.

### Authentication fail
If the username and password sent are not valid a 403 HTTP state is returned with a clear message about the error.

### Expiration and renewal
If the token is expired a specific HTTP state is returned and the client can try to request a new token or renew it.

### Web API calls from Web Application
To make simple for the Web Application page that use the Web API to use this token, it is preloaded and stored in the HTML of the View Page 
to be used from the client library (AngularJs for example).
This poin must be defined better.

### Cleanup expired tokens
A scheduled process cleanup tokens expired from almost 1 day or something like this.