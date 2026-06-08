# ProjectName: TraineeManagement.api (ASP.NET Core Web API Project)

## Technology Used
C# And ASP.NET CORE  

## Database Used  
SQL SERVER with Entity Framework ORM    

## How to Run
step1: Clone the repo  git  
step2: Open Project in visual studio IDE  
step3: Run following commands for database migration .NET CLI  
    command 1: Add-Migration InitialSqlServerMigration  
    command 2: update-database  
step4: click on Run application  

## API LIST  
1. [GET] /api/Health  
    Response: {  
        "status": "string",  
        "applicationName": "string",  
        "time": "2026-06-05T08:50:07.401Z"  
    }  
  
2. [GET] /api/Trainee?searchKeyword=string  
    Response:  
    [  
    {  

        "id": "string",  
        "firstName": "string",  
        "lastName": "string",  
        "email": "string",  
        "techStack": "string",  
        "status": "ACTIVE",  
        "createdAt": "2026-06-05T08:56:33.575Z",  
        "updatedAt": "2026-06-05T08:56:33.575Z"  
    }  
  ]  
  
3. [POST] /api/Trainee  
    Request Body:  
    {  
        "firstName": "string",  
        "lastName": "string",  
        "email": "user@example.com",  
        "techStack": "string",  
        "status": "ACTIVE"  
    }  
  
4. [PUT] /api/Trainee  
    Request Body:  
    {  
        "id": "string",  
        "firstName": "string",  
        "lastName": "string",  
        "email": "user@example.com",  
        "techStack": "string",  
        "status": "ACTIVE"  
    }  
  
5. [GET] /api/Trainee/{id}  
    Request Path Param: id  
    Response:  
     {  
        "id": "string",  
        "firstName": "string",  
        "lastName": "string",  
        "email": "user@example.com",  
        "techStack": "string",  
        "status": "ACTIVE"  
    }  
  
6. [DELETE] /api/Trainee/{id}  
    Request Path Param: id  
    Response: 204 No Content


## Challenges Faced
1. I Faced challenges while creating the api. To overcome challenges i used help of official documentation of .NET Core and C#  
References:  
1. https://learn.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-10.0  
2. https://learn.microsoft.com/en-us/aspnet/core/mvc/models/validation?view=aspnetcore-10.0  
3. https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-10.0