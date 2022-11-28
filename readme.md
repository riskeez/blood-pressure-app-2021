# Blood Pressure App
Simple .NET5 API + Xamarin.Forms app 

## API Demo
~~The API is hosted on App Services and available here: https://mybplogapi.azurewebsites.net/swagger/index.html~~   
You can use Swagger UI there to play around with the API.

### Flow
1) Use `POST: /auth/register` endpoint to register a new user
2) Use `POST: /auth` endpoint to get access token
3) Use the access token to access other endpoints


## App
Unfinished Xamarin.Forms app.
Navigation is done well though :) Useful example of clean MVVM approach
All APIs and etc is in place, only UI is missing.

Right now supports only:
1) Login/Register user
2) Save blood pressure (shows warning if too high)