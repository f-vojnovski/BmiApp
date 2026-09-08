# BmiApp

A BMI calculator built as a full stack .NET application: an ASP.NET Core Web API with token based authentication, behind an Angular single page client. Visitors can calculate their BMI without an account, and registered users can save their results and review them later.

The sections below walk through each layer of the API, then the client app, then the reasoning behind the way it is put together. Setup instructions are at the end.

This application uses: 
- asp.net core 5.0
- Swagger for simplified API development and testing
- Microsoft Identity platform for login / registration / authentication / authorisation (Along with Authentication OpenId Connect, Authentication Jwt Bearer)
- Microsoft Entity Framework Core for database communication (along with Entity Framework Core Sql Server, Entity Framework Core Design, Entity Framework Core Tools)
- SQL database
- Auto Mapper for mapping entities to DTO and vice versa
- MediatR for less code coupling.
- Angular 13 with Angular Material for the client app

# Api
Here is the API project structure:

```
BmiApp.sln
src/
├── BmiApp.Api/         controllers, Startup, configuration
├── BmiApp.Service/     business logic, DTOs, AutoMapper profile, JWT auth manager
├── BmiApp.Repository/  DbContext, repositories, unit of work, EF Core migrations
└── BmiApp.Data/        entities and the Identity role configuration
client/                 Angular client, outside the solution
```

`BmiApp.Api` depends on `BmiApp.Service` for business logic, and on `BmiApp.Repository` and `BmiApp.Data` for context registration and entity types. `BmiApp.Service` depends on `BmiApp.Repository`, and `BmiApp.Repository` on `BmiApp.Data`. Dependencies only ever point downward, so no layer can reach back up into the one calling it.

## Data layer
In data I have added all of the entities that will be used in the database.
The BmiRecord is used for storing Bmi records in database. It contains an automatically generated version 4 UUID, email, weight and height.
The ApiUser is used for storing data for registered users, extends the IdentityUser class. Currently, this class has nothing, but if we wanted to add more details for each user that uses the app, we would writhe those properties here.
Finally, the RoleConfiguration class automatically adds the IdentityRole user in database. When using the Identity platform for authentication and authorization, every user needs a role. Since this is not a complicated project, I decided to go with one role, user.

## Repository layer

In repository we can find all of the things the API uses for interacting with the database.

### DbContext
The ApplicationDbContext is this project's database context. It is a pretty simple class which contains only one DbSet of BmiRecord, as well as a function to apply the previously mentioned RoleConfiguration.

### Core Folder
The IRepositoryBase interface contains some basic methods for accesing records in the database. This interface is implemented by the RepositoryBase class, which in turn uses the injected context to provide an implementation for the methods that the IRepositoryBase declares.

This project also uses the Unit of Work pattern, this is implemented by the IUnitOfWork interface, and its implementation UnitOfWork. There is not much to talk about here, the most important functionality is the Save function which saves changes to the database, otherwise it is just a standard unit of work pattern implementation.

### Migrations folder
This is just a folder where the migrations for the database are stored.

### Persistence folder
This is a folder for storing all the repository classes. Now, there are an IBmiRepository interface and a BmiRepository class for implementation. This is a simple repository with two methods for viewing and adding BmiRecords to the database.

### Dependency Injection
In the DependencyInjection class for this layer of the project there is an AddRepositoryLayer function, which basically just tells the project what the concrete implementations for the IUnitOfWork and IBmiRepository are.
The ConfigureIdentity function is used for configuring the identity platform to work with our database context and also to tell the user that they must provide an email address.

## Service
This is the service layer of the application, this is where all the business logic of the application should be stored.

### Bmi Service
There is an IBmiService interface and an BmiService implementation. This service "talks" to the BMI Repository. It has two methods: Writing a BMI Record to database and getting all BMI records for a specific user. It maps DTOs to database entities and saves / retrieves them using the Unit of Work

### Dto Folder
This folder contains data transfer objects which the API gets from the client. There is a LoginDto and a UserDto for the account endpoints, and a BmiReadRecordDto and
BmiWriteRecordDto for the BMI endpoints.

### Mapping folder
This folder contains just one class, MapperInitializer. Since this project uses AutoMapper with dependency injection to map DTOs to entities and vice versa it needs a class which tells the AutoMapper how it should be configured. This is where that happens.

### Auth folder
This folder contains the IAuthManager interface and its implementation, the AuthManager class. This is where we create JWT tokens and validate logged in users. The CreateToken() function generates a JWT token which is later used for authorization in the API.

Alongside them sits the JwtKey class, which reads the signing key out of configuration and checks that it is present and long enough before handing it back. Token creation here and token validation in this layer's dependency injection both go through it, so the key is resolved in one place and a misconfigured key is reported the same way for both.

### Dependency Injection
The DependencyInjection class sets up the dependencies for this layer. It also configures MediatR for the project and it configures the Jwt generation. The ConfigureJwt function builds the bearer
token validation parameters and takes the signing key from configuration through JwtKey, which
means the API refuses to start if the key was never supplied.

## Web API
The Web Api layer is responsible for handling user requests. There are two controllers, one is the AccountsController, which handles requests related to logging in and registering, and the BmiRecordsController, which contains two endpoints for writing and reading BMI records (these endpoints are authenticated so a JWT token from the API is needed to execute these requests successfully). The record owner is read from the claims on that token, so both endpoints act on the caller's own records and take no user identifier from the route or the request body. Both controllers communicate with both the service layer and the client to perform these operations.

# Client App
The client app is built in Angular, it is relatively simple, so here I will list a few examples and a few more interesting classes.

## Modules
This application uses a standard module, a routing module and a module for importing all of the necessary things from Angular Materials.

## JWT Interceptor
The app uses a JWT Interceptor class, which basically just tells the HttpClient that everytime we send a request to add a JWT Token to it if we have one.

## The interfaces folder
In order to make the code strongly typed, I have the interfaces folder which defines how some objects should look like, mainly DTOs.

## Services
This application has two services: the Bmi Service and the Auth Service. 
The Bmi Service is responsible for sending http requests related to reading / writing BMI records. 
The Auth Service provides login / registration functionalities, as well as an observable currentUser which can detect when a user has logged in / logged out. The current user data is stored in local storage.

## Components
All of the components are stored in the components folder, I think it is better to understand how these components work from viewing the code / application rather than explaining them.

# User Experience
The user is shown a BMI calculator when first accesing the web app.

![image](https://user-images.githubusercontent.com/48998036/154128926-2420158a-1d93-4aa3-b144-d24919ca748a.png)

This is a screenshot of the registration form, but the login form is the same:

![image](https://user-images.githubusercontent.com/48998036/154129111-5f9195a8-8c05-407c-9ead-06d69ca823fa.png)

There are validators in place and if the user enters incorrect data he will be notified.

If the user logs in, the app will look a little different.
Namely, when entering their BMI they will have the option to save the result.
There will also be changes in the header, the user will now see a Log Out button and a View Records button.

![image](https://user-images.githubusercontent.com/48998036/154129415-9468d328-7e6b-43a2-bb8a-c6b1a0bdca45.png)

This button will become invisible if the user saves a result so they don't accidentally save the same result twice.

When the user view their saved results, this is what they see:

![image](https://user-images.githubusercontent.com/48998036/154129624-2e311717-d885-4ff4-996a-62742b5f1351.png)


# Design notes

Data access sits behind the repository and unit of work patterns. `RepositoryBase<T>` carries the query and write operations that every entity needs against its `DbSet<T>`, `BmiRepository` adds the two BMI specific queries on top of it, and `UnitOfWork` owns the single `SaveChanges` call, so a service method commits its work once rather than writing piecemeal.

DTOs are kept separate from entities on purpose. The shape the API exposes and the shape the database stores change for different reasons, and keeping them apart means an entity can gain a column without that column appearing in a response. AutoMapper holds the translation between the two in one profile, `MapperInitializer`, instead of it being spread across the controllers.

Authentication uses Microsoft Identity for user and role storage and JWT bearer tokens for the API itself, which keeps the endpoints stateless: `BmiRecordsController` needs nothing but the token to authorize a request. `AuthManager` validates credentials against Identity and mints the token, and the signing key is read through `JwtKey` so both signing and validation agree on where it comes from and fail the same way when it is absent.

Record ownership follows from that token rather than from the request. `BmiRecordsController` reads the name claim and hands it to the service layer, which means a caller reads and writes their own history and there is no owner field for a request to set. Registration works the same way round: the role is a constant in the controller, and the password goes to `UserManager.CreateAsync` so the configured Identity validators run instead of being bypassed by hashing it by hand.

Migrations live in `BmiApp.Repository` next to the context, while the connection string lives in `BmiApp.Api`. That split is why the `dotnet ef` command under Getting started passes a project and a startup project separately, and it keeps database configuration in the layer that owns the database.

# Roadmap

Natural next steps, in rough order of how much they would add:

## API
- Move the remaining credential handling out of AccountController and into AuthManager, leaving the controller to translate results into HTTP responses.
- Add refresh tokens with a shorter access token lifetime, and server side invalidation so logging out revokes a token rather than only discarding it on the client.
- Return validation problems through a single exception handling middleware, so every endpoint reports failures in the same shape.
- Move to a supported .NET release. The project targets .NET 5, and the upgrade to the current LTS is mostly a matter of the target framework and package versions.

## Angular
- Replace the per message success components with one dialog component that takes its content as an input.
- Add an HTTP error interceptor so a failed request surfaces a real message instead of failing quietly.
- Present login and registration as dialogs, matching the rest of the Material layout.
- Finish the responsive layout for the calculator and the records table.
- Add a metric and imperial toggle on the calculator so height and weight can be entered either way.

# Getting started

## Prerequisites

- .NET 5 SDK
- SQL Server, either LocalDB or a full instance
- Node.js and the Angular CLI, to run the client app

## Supplying the JWT signing key

The API signs its tokens with an HMAC-SHA256 key that it reads from configuration on startup. `appsettings.json` carries an empty `Jwt:Key` placeholder and no key value, so the key has to be supplied from outside the repository. Either of the following does that.

User secrets keep the key in your user profile rather than the working tree, which suits local development:

```
dotnet user-secrets set "Jwt:Key" "<your key>" --project src/BmiApp.Api
```

An environment variable suits CI and hosting. The double underscore is how .NET maps a flat variable name onto the nested `Jwt:Key` entry:

```
setx Jwt__Key "<your key>"         # Windows, applies to new shells
$env:Jwt__Key = "<your key>"       # PowerShell, current shell only
export Jwt__Key="<your key>"       # bash
```

The key has to be at least 32 bytes, matching the 256 bit output that HMAC-SHA256 signs with. Any cryptographically random string of that length works:

```
# PowerShell
$b=[byte[]]::new(32); [System.Security.Cryptography.RandomNumberGenerator]::Create().GetBytes($b); [Convert]::ToBase64String($b)

# bash
openssl rand -base64 32
```

A key that is missing or too short stops the API during startup, with a message naming the setting and how to supply it, instead of surfacing later as a failed login.

## Database

`ConnectionStrings:sqlConnection` in `appsettings.json` points at the local default SQL Server instance and uses integrated security, so it carries no credentials. Change it if your instance differs, then apply the three migrations:

```
dotnet ef database update --project src/BmiApp.Repository --startup-project src/BmiApp.Api
```

The context and the migrations live in `BmiApp.Repository` while the connection string lives in `BmiApp.Api`, which is why the command names both.

## Running the API

```
dotnet run --project src/BmiApp.Api
```

In the Development environment Swagger is served at `/swagger`, which is enough to register a user, log in, and call the authenticated endpoints with the token that comes back.

## Running the client

```
cd client
npm install
npm start
```

The client reads its API base address from `client/src/environments/environment.ts`, which is set to `https://localhost:44332/` to match the IIS Express profile in `launchSettings.json`. Starting the API with `dotnet run` instead serves it on `https://localhost:5001`, so point that setting at whichever address you are using.
