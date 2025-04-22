# OwtBoats

This solution is made from two primary components:

- Backend based on ASP.NET (WebAPI)
  - EF Core to persist both Identity and Business Entities in PostgreSQL
  - FluentValidation for input validation
  - SwaggerUI to expose API documentation
- Frontend using Angular
  - Angular Material is used for some styles and components
  - Client code for backend API is built using [ng-openapi-gen](https://github.com/cyclosproject/ng-openapi-gen)

## Development
The devcontainer setup spins up a container with development tools for dotnet and Angular as well as a separate container
for the PostgreSQL database server.

I was using JetBrains Rider, but the whole solution should work in VS Code and Github Codespaces as well.

### Start services
Start backend:
```
cd backend/
dotnet run
```

Start frontend:
```
cd frontend/
npm install
npm start
```

### User
Sign up is not yet implemented in Frontend, but you can trigger it conveniently using Swagger UI, available on `/swagger`.
Alternatively there are also some sample requests in ackend/backend.http) which check basic functionality.

### Access UI
To access the frontend, do *not* directly access the port opened by `ng serve` / `npm start` (probably 4200), instead
navigate to `/` on the backend. The requests are proxied to the Angular development server.

### Change API contracts
1. Make desired changes in `backend/Contracts` and relevant logic
2. Have backend running
3. Run `npm run generate:api` in the `frontend` folder
4. Adapt Frontend accordingly

## Some design decisions / other remarks
- Users can only manage their own boats.
- Prevented duplication of API contracts using OpenAPI by generating client code from the spec.
- Used new-ish (.NET 8) `MapIdentityApi` to provide default auth-related endpoints from backend. This exposes quite a
  bit  more functionality than we need, but it still simplifies the backend.
- Manually mapping between exposed contracts and database entities in the backend. I considered adding a mapping library
  (would probably have chosen [Mapperly](https://github.com/riok/mapperly) since it is a source generator), but decided against.
- There are probably many things to improve on the frontend, I have done very little frontend development in the past 8 years.