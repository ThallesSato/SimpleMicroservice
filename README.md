## Microservice README

This repository contains a simple .NET WebAPI microservice built with the following components:

- **Gateway:** Ocelot
- **Authentication / Authorization:** JWT
- **Messaging:** RabbitMQ
- **ORM:** Entity Framework
- **Microservices Design Pattern:** Domain-Driven Design (DDD)

### Functionality:

- All endpoints are centralized under one domain using Ocelot.
- When a user creates a login, a corresponding user will be created via a message broker (RabbitMQ).
- The Login/Register endpoints return a JWT token for authentication.
- Users can perform the following actions:
  - View all users
  - View their own user profile (based on JWT claims)
  - Update/Delete their own user profile

