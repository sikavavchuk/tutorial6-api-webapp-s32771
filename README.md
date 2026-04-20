This project is an ASP.NET Core Web API for managing rooms and reservations in a training center. 
I implemented two controllers: RoomsController and ReservationsController, which handle standard CRUD operations using HTTP methods like GET, POST, PUT, and DELETE.
The data is stored in memory using static lists, so no database is used at this stage.
I also implemented filtering using route parameters and query strings, as well as validation with proper HTTP status codes like 200, 201, 404, and 409.
I tested all endpoints using Postman.
