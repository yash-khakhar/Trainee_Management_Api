# TraineeManagement System  
A scalable, containerized backend system for managing trainees, mentors, learning tasks, and file submissions. The application utilizes an API Gateway for centralized authentication, Redis for caching, and RabbitMQ for asynchronous background processing.

## 🛠 Architecture & Tech Stack  
* **Framework:** C# and ASP.NET Core 10.0
* **API Gateway:** `TraineeManagementGateway.api` (Port `5000` - Central entry point)
* **Database:** MySQL with Entity Framework Core ORM
* **Caching:** Redis (Cache layer for highly requested data)
* **Message Broker:** RabbitMQ (Asynchronous background message processing)
* **Background Worker:** `TraineeManagement.WorkerService` (Consumes submission tasks)
* **Internal Service:** `TrainingDirectory.Api` (Dummy service used for inter-service HTTP communication)  

## System Architecture Diagram  

                        [ Client Requests ]
                                │
                                ▼ (Port 5000)
                     ┌─────────────────────┐
                     │     Auth Gateway    │ (Handles JWT Auth & Routing)
                     └──────────┬──────────┘
                                │
                                ▼
                     ┌─────────────────────┐       ┌───────────────┐
                     │ TraineeManagement   │ ◄───► │  Redis Cache  │ (Fast Reads)
                     │        .api         │       └───────────────┘
                     └────┬────────────┬───┘
                          │            │
      (Async File Tasks)  │            │ (Inter-Service HttpClient)
                          ▼            ▼
                   ┌────────────┐    ┌─────────────────────┐
                   │  RabbitMQ  │    │ Dummy Trainee Service│
                   └─────┬──────┘    └─────────────────────┘
                         │
                         ▼
                   ┌────────────┐       ┌───────────────┐
                   │ RabbitMQ   │ ◄───► │   MySQL DB    │ (Persistent Storage)
                   │  Consumer  │       └───────────────┘
                   └────────────┘

## 🚀 Getting Started (Docker Compose - Recommended)  
The entire ecosystem is containerized. Users only need to interface with the Gateway URL: **`http://localhost:5000`**.

### Prerequisites
* [Docker Desktop](https://www.docker.com/products/docker-desktop) installed.
* Git installed to clone the repository.  

### Quick Start Steps
1. **Clone the Repository:**
   ```bash
   git clone <repository-url>
   cd TraineeManagement  

   1. Spin up the Ecosystem: **`docker-compose up -d --build`**.  
   2. Verify Services are Running: **`docker compose ps`**.  

### 💻 Local Development Setup (Without Docker)  
If you wish to run the main API project locally outside of containers:  
1. MySQL & Redis Setup:  
* Install MySQL Database and MySQL Workbench locally. Ensure you configure your root password as 1234 and create a schema named trainee_management_db (or match your local appsettings.json).  
* Install Redis locally on port 6379.

## Backend setup steps  
step1: Clone the repo  git  
step2: Open Project in visual studio IDE  
step3: Run following commands for database migration .NET CLI  
    command 1: Add-Migration InitialMySQL    
    command 2: update-database  
step4: click on Run application  

## MySQL Setup Steps  
-> Download mysql database and mysql workbench for viewing database using user interface.  
-> While installing mysql database, you will be prompted to enter username and password. Make sure you remember these database crendials which will be helpful in connecting database with application.  
-> For mysql database, you need to install following dotnet packages:  
package: dotnet add package MySql.EntityFrameworkCore --version 10.0.7  

## EF Core Migration commands  
-> Add-Migration [Migration_Name]  
-> update-database  

## Login Credentials  
-> First you need to register user as admin/trainee/mentor for testing the system.  
-> you can use /api/Auth/register api for adding user.  

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

## Security Checklist  
1. Password is stored as hash in database instead of raw string.    
2. Api is protected using role-based jwt tokens.  
3. Cors configuration is done for frontend application  
4. Global Exception is configured to prevent application to crash from unexpected errors.  

## Known Limitations  
1. Entire application is using monolith architecture. So, if one service gets unexpected error entire application can be down.  
2. Secret Key of JWT Token is still used as raw string instead of encrypting the key.  
3. If jwt token of one user is known to another user, then he can use his token to misuse the role.  

## Next Improvement Areas  
1. We can develop micro-service architecture. So, all services are isolated from each other.  
2. We can use queues such as RabbitMQ or Kafka for intraction between two micro services.  
3. We can encrypt/hash the secret key of jwt token, so it is not exposed to other user in any cases.  
4. We can also hash/encrypt the jwt token itself instead of sending raw token. So another user can not use token even it is known.  

## Challenges Faced
1. I Faced challenges while creating the api. To overcome challenges i used help of official documentation of .NET Core and C#  
References:  
1. https://learn.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-10.0  
2. https://learn.microsoft.com/en-us/aspnet/core/mvc/models/validation?view=aspnetcore-10.0  
3. https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-10.0