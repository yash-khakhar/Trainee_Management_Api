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

   1. Spin up the Ecosystem: `docker-compose up -d --build`.  
   2. Verify Services are Running: `docker compose ps`.  
   ```

### 💻 Local Development Setup (Without Docker)  
If you wish to run the main API project locally outside of containers:  
1. MySQL, Redis & RabbitMQ Setup:  
* Install MySQL Database and MySQL Workbench locally. Ensure you configure your root password as 1234 and create a schema named trainee_management_db (or match your local appsettings.json).  
* Install Redis locally on port 6379.  
* Install RabbitMQ locally on port 5672.  

2. Install Required Packages  
Run this command inside your project directories where required:  
```bash  
1. dotnet restore   
```  

## EF Core Migration commands  
Run this command inside your project directories where required:  
```bash  
1. Add-Migration [Migration_Name]  
2. update-database  
```

## Login Credentials  
-> First you need to register user as admin/trainee/mentor for testing the system.  
-> you can use /api/Auth/register api for adding user.  

## 🛣 API Endpoints (Accessed via http://localhost:5000)  

## 🔑 Authentication / Identity  
* [POST] /api/Auth/register - Create a new user account.  
* [POST] /api/Auth/login - Authenticate and obtain JWT token.  

## 🎓 Trainee Management  
* [GET] /api/Trainee - List all trainees (Uses Redis Cache for optimized reads).  
* [GET] /api/Trainee?searchKeyword={string} - Search trainees.  
* [GET] /api/Trainee/{id} - Fetch trainee by ID.  
* [POST] /api/Trainee - Create a trainee profile.  
* [PUT] /api/Trainee - Update trainee profile details.  
* [DELETE] /api/Trainee/{id} - Remove a trainee (Returns 204 No Content).  

## 👔 Mentor Management  
* [GET] /api/Mentor - List all mentors.  
* [GET] /api/Mentor/{id} - Get mentor by ID.  
* [POST] /api/Mentor - Create a mentor profile.  
* [PUT] /api/Trainee/{id} - Update a trainee's details (Mentor Privilege).  
* [DELETE] /api/Trainee/{id} - Delete a trainee profile (Mentor Privilege).  

## 📚 Learning Tasks & Assignments  
* [GET] /api/LearningTask | [GET] /api/LearningTask/{id} - Fetch curriculum tasks.  
* [POST] /api/LearningTask | [PUT] /api/LearningTask/{id} | [DELETE] /api/LearningTask/{id} - Manage tasks.  
* [GET] /api/TaskAssignment | [GET] /api/TaskAssignment/{id} - Track assignments.  
* [POST] /api/TaskAssignment | [PUT] /api/TaskAssignment/{id} - Assign tasks to trainees.  

## 📥 Submissions & Reviews  
* [GET] /api/Submission | [GET] /api/Submission/{id} - Fetch submissions.  
* [POST] /api/Submission - Submit a learning task file (Pushes message payload to RabbitMQ for backend worker processing).  
* [GET] /api/Review | [GET] /api/Review/{id} - View task reviews.  
* [POST] /api/Review - Submit review notes/grades for a trainee's task.  

## 🛡 Security & Resilience Implementation  
1. Gateway Security: The Gateway isolates internal microservices (Dummy Service, Web API) from direct external exposure.  
2. Password Security: Plaintext passwords are never saved; they are safely salted and hashed before storing in MySQL.  
3. Global Exception Handling: A centralized middleware captures unexpected exceptions globally, rendering sanitized, uniform error objects without leaking internal stack traces.  
4. CORS Policies: Configured securely at the Gateway level to permit designated frontend integrations.  
5. Fault Isolation: Decoupled architecture using RabbitMQ guarantees that spike loads or file-processing failures in the Worker Service won't bring down the core Web API.  

## 📈 Next Improvement Areas  
* Token Protection: Encrypt or double-hash the JWT tokens before transit to mitigate token interception or spoofing.  
* Distributed Configuration: Move hardcoded environment strings in docker-compose.yml (like JwtSettings__SecretKey) into secure vaults like HashiCorp Vault or Azure Key Vault.  
* Service Mesh: Implement a comprehensive service registry (e.g., Consul) if inter-service dependencies grow beyond HttpClient bindings.  

## Challenges Faced
1. I Faced challenges while creating the api. To overcome challenges i used help of official documentation of .NET Core and C#  
References:  
1. https://learn.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-10.0  
2. https://learn.microsoft.com/en-us/aspnet/core/mvc/models/validation?view=aspnetcore-10.0  
3. https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-10.0