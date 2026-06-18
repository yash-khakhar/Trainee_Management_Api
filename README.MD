# ProjectName: TraineeManagement.api (ASP.NET Core Web API Project)

## Technology Used
C# And ASP.NET CORE  

## Database Used  
MYSQL DB with Entity Framework ORM  

## How to Run
step1: Clone the repo  git  
step2: Open Project in visual studio IDE  
step3: Run following commands for database migration .NET CLI  
    command 1: Add-Migration InitialMySQL    
    command 2: update-database  
step4: click on Run application  

## API LIST For Trainee  
1. [GET] /api/Trainee  

2. [GET] /api/Trainee?searchKeyword=string  
      
3. [POST] /api/Trainee    
  
4. [PUT] /api/Trainee    
  
5. [GET] /api/Trainee/{id}  
    Request Path Param: id    
  
6. [DELETE] /api/Trainee/{id}  
    Request Path Param: id  
    Response: 204 No Content  

## API LIST For Users  
1. [POST] /api/Auth/register  

2. [POST] /api/Auth/login  
  
## API LIST For Mentor  
1. [GET] /api/Mentor   
  
2. [POST] /api/Mentor  
  
3. [PUT] /api/Trainee/{id}  
  
4. [GET] /api/Mentor/{id}  
    Request Path Param: id  
  
5. [DELETE] /api/Trainee/{id}  
    Request Path Param: id  
    Response: 204 No Content  

## API LIST For Learning Task    
1. [GET] /api/LearningTask   
  
2. [POST] /api/LearningTask  
  
3. [PUT] /api/LearningTask/{id}  
  
4. [GET] /api/LearningTask/{id}  
    Request Path Param: id  
  
5. [DELETE] /api/LearningTask/{id}  
    Request Path Param: id  
    Response: 204 No Content  

## API LIST For Task Assignment    
1. [GET] /api/TaskAssignment   
  
2. [POST] /api/TaskAssignment  
  
3. [PUT] /api/TaskAssignment/{id}  
  
4. [GET] /api/TaskAssignment/{id}  
    Request Path Param: id  

## API LIST For Submission    
1. [GET] /api/Submissinon   
  
2. [POST] /api/Submission  
  
3. [GET] /api/Submission/{id}  
    Request Path Param: id  

## API LIST For Review      
1. [GET] /api/Review  
  
2. [POST] /api/Review  
  
3. [GET] /api/Review/{id}  
    Request Path Param: id    

## JWT Usage instruction  
-> For accessing any /api/Trainee api, you need jwt token  
-> To get jwt token, you need to login and provide correct username and password.  
-> Login credentials will work if you are registed, if not then you need to register first using /api/Register api.  

## Global Exception Handling  
-> Created centralized exception handling for unexpected errors.  
-> Returning safe, consistent error messages to clients.  

## Challenges Faced
1. I Faced challenges while creating the api. To overcome challenges i used help of official documentation of .NET Core and C#  
References:  
1. https://learn.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-10.0  
2. https://learn.microsoft.com/en-us/aspnet/core/mvc/models/validation?view=aspnetcore-10.0  
3. https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-10.0