# BmiApp

Hello, welcome to my BMI Application. I will list all of the things I have implemented and things I intend to improve.

This application uses: 
- asp.net core 5.0
- Swagger for simplified API development and testing
- Microsoft Identity platform for login / registration / authentication / authorisation (Along with Authentication OpenId Connect, Authentication Jwt Bearer)
- Microsoft Entity Framework Core for database communication (along with Entity Framework Core Sql Server, Entity Framework Core Design, Entity Framework Core Tools)
- SQL database
- Auto Mapper for mapping entities to DTO and vice versa
- MediatR for less code coupling.

# Api
Here is the API project structure:

![image](https://user-images.githubusercontent.com/48998036/154116364-be852e4f-c1ae-4039-84e8-d5a945b0a0a2.png)

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
In the DependencyInjection class for this layer of the project there is an AddRepositoryLayer function, which basically just tells the project what the concrete implementations for the IUnitOfWork and IBmiInterface are.
The ConfigureIdentity function is used for configuring the identity platform to work with our database context and also to tell the user that they must provide an email address.

## Service
This is the service layer of the application, this is where all the business logic of the application should be stored.

### Bmi Service
There is an IBmiService interface and an BmiService implementation. This service "talks" to the BMI Repository. It has two methods: Writing a BMI Record to database and getting all BMI records for a specific user. It maps DTOs to database entities and saves / retrieves them using the Unit of Work

### Dto Folder
This folder contains data transfer objects which the API gets from the client. There is a LoginDto, RegisterDto, BmiReadDto and BmiWriteDto.

### Mapping folder
This folder contains just one class, MapperInitializer. Since this project uses AutoMapper with dependency injection to map DTOs to entities and vice versa it needs a class which tells the AutoMapper how it should be configured. This is where that happens.

### Auth folder
This folder contains the IAuthManager interface and its implementation, the AuthManager class. This is where we create JWT tokens and validate logged in users. The CreateToken() function generates a JWT token which is later used for authorization in the API.

### Dependency Injection
The DependencyInjection class sets up the dependencies for this layer. It also configures MediatR for the project and it configures the Jwt generation.

## Web API
The Web Api layer is responsible for handling user requests. There are two controllers, one is the AccountsController, which handles requests related to logging in and registering, and the BmiController, which contains two endpoints for writing and reading BMI records (these endpoints are authenticated so a JWT token from the API is needed to execute these requests successfully). Both controllers communicate with both the service layer and the client to perform these operations.

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


# Development process
I first developed the API and tested it using Swagger and Postman. This process was not hard at all, but I had a couple of issues that really wasted a lot of time for nothing. First, I had a problem where I accidentaly injected a class instead of an interface in one of the services I was writing. This obviously caused an error which was really hard to find, I spent an entire day thinking that something was wrong with how I performed the dependency injection

Second, I had trouble performing the migrations (since I have never done migrations in an Onion Layer Architecture project before). This also killed a lot of time, since I was not sure how to configure the thing to look in the correct place for the startup project.

In overall, sooo much time wasted on configurations :(. I believe I could have done all of this faster if it wasn't for these two things and some bugs which I spent a lot of time fixing.

# Possible improvement
Since I don't want to send this project too late, I will mention these changes I want to do and then implement them if needed.

## API
- The API folder structure could use some work. I was in a hurry and I believe this can be slightly improved in some places.
- You might notice that some dependencies are not declared in the correct project, this is because I have never built a project with this architecture from the ground up before, and I additionally ran into some problems while developing, which resulted in me possibly declaring the dependencies in the wrong place.
- Some of the authentication logic is performed in the controller. It needs to be performed in the AuthManager service. I was kind of in a hurry, this can be easily fixed.
- A lot of unused imports need to be removed.
- The secret for encoding the JWT Tokens must not be placed in the appsettings.json. I did this in order not to make the project too hard for cloning, since the other option was to declare it as an environment variable in my Windows operating system (or find some other complex solution).
- The JWT token service can be made more advanced and slightly more secure.
- The endpoints for BMI records currently do not check who is the current user, again, I did not address this for the sake of time, but it is an easy fix which I can implement if necessary.
- The logout is currently only front end.

## Angular
- I should have created a generic Ok dialog which you can feed information through an Input so I don't have multiple components just for showing different kinds of information.
- The project could really use a http error interceptor for showing correct information when an error happens.
- Perhaps the login and register forms should have been programmed as a dialog.
- Currently, some parts of the app are not responsive, again, for the sake of time.
- I really want to add a toggle button on the home page so the user can enter the parameters in Metric or Imperial.
