# Overview
![website](https://user-images.githubusercontent.com/62789796/129213519-4004c525-9601-403b-9dca-91883c50fedf.jpg)

# ProjectWeb 
- Web API: https://apiservicesecommerce.azurewebsites.net/swagger/
- E-commerce site demo: https://ecommercewebapplication.azurewebsites.net/
## Technologies
- C#, .NET 6.0
- Entity Framework 6.0
- Javascript & Ajax/JQuery
- SQL Server
- Redis 
## Description
### Web API
- Restful API
- Authorization (Access Token & Refresh Token)
- Cache with Redis Server
- Unit tests for ProductServices (basic)
### Web for Manager:
- Authentication (Login/Logout) 
- Manager Products, Categories, Users, Roles, Images
### Web E-commerce for User
- Login, Signup & Logout
- View Categories, Products
- Search Product
- Review detail Product 
- Order Product
- Send mail confirm when order
## Design pattern
- Repository and Unit of Work Pattern
- Inversion of Control (Dependency Injection)
## Architecture
![Untitled Diagram (1)](https://user-images.githubusercontent.com/62789796/128381014-f8b99671-b894-4cb2-af99-2567efa272e4.png)

## Errors
- Can't send email on Azure  using smtp gmail, try this: https://stackoverflow.com/questions/20906077/gmail-error-the-smtp-server-requires-a-secure-connection-or-the-client-was-not#:%7E:text=solution%20for%20case%201%3A%20Enter,to%20login%20from%20all%20apps.&text=Use%20that%20newly%20generated%20password%20to%20authenticate%20via%20SMTP
