## Building ASP.NET Core 7 Minimal APIs by Kevin Dockx

- OVERVIEW:
    - This course will teach you how to build a web API with ASP.NET Core’s minimal API approach.
    - CRUD. Structuring. Exception handling and logging. Secure and document.

- INTRODUCTION TO ASP.NET CORE MINIMAL APIs:
    - ASP.NET Core MVC? Model. View. Controller.
    - ASP.NET Core Minimal API?
        - Supports routing, parameter binding, basic serialization, and basic filters.
        - Intended for lightweight scenarios without all the ceremony that comes with MVC.
            - Microservice APIs. Aggregate/proxy APIs. Relatively small APIs.
        - External packages.
    ```javascript
        add-migration Init
        update-database
    ```

- LEARNING ABOUT CORE CONCEPTS & READING RESOURCES:
    - Routing: Process of matching an HTTP method & URI to a specific route handler.
        - app.MapAction methods: Where Action = HTTP method.
        - Under the covers: IEndpointRouteBuilder build route endpoints.
        - Using HTTP methods as intended improves the overall reliability of the system.
            - e.g.: A cache server depends upon HTTP standards and responses.
        - Route parameters: Gather input via the URI with route parameters.
            - In the route template/pattern, use accolades to signify a route parameter.
        - Route constraints: Allow constraining the matching behavior of a route.
    - DTOs versus entity onjects.
        - A possible object cycle was detected. Due to cycle/object depth.
        - Don't expose entity to the outside world. 
        - Entity model: Representation of DB rows as objects. (Object graphs.)
        - DTO: Representating the data that's sent over the wire.
        - A more robust, reliably evolable code.
    - Parameter Binding: Process of converting equest data into strongly-typed parameters.
    - Parameter binding sources:
        - Route values. [FromRoute]
        - Query string. [FromQuery]
        - Header. [FromHeader]
        - Body (as JSON.) [FromBody]
        - Services provided by DI. [FromServices]
        - Custom.
        - Inferred binding (via HTTP method used and parameter type.)
    - Improving API responses:
    - IoC: Inversion of control: Delegates the function of delegating the comcrete implementation type for a classes dependencies to an external component.
        - Dependency Injection in minima APIs: Handler's dependencies are provided by an external component.
            - Don't new them up. Instances are provided.
        - Dependency Injection (DI)
        - Uses an object, the container, to iniitialize other objects, manage their lifetime, and provide the required (nested) dependencies to objects.
    - Support via ASP.NET Core's built-in container.
    - AddSingle(): Lifetime of the application. One instance across requests.
    - Add Scoped(): Instance reused across the scope of one request.
    - AddTransient(): A new instance is provided when the object is reqiested.
    - Dependency injection:
        - Typical: Constructor injection.
        - Minimal: Common pattern. Dependencies as parameters.
    - Filtering: Limiting a collection resource, taking into account a predicate.
        - A URL does not have a maximum length. The body of a GET request is often ingored.
    - Searching: Matching items in a cleection based on a predefined set of rules.
        - Via query string, pass through a value to search on.
    - Sorting. Via the query string passing through fields and (optional) direction.
        - Allow sorting on DTO properties. Not on entity properties.
    - Paging: Every endpoint that returns a list of data should have paging enabled by default.
        - pageSize. pageNumber. Limit the page size. And return the pagination metadata in a response header.
    - Status codes. Inspected by the consumer to discern the status of the request.
        - As expected? Something wrong? Consumer's fault? Responsibility?
        - Correct ststus code useage is essential.
            - 201 OK. 201 Created. 204 No content.
            - 400 Bad request. 401 Unathorized. 403 Forbidden. 404 Not found. 405 Method not allowed.
            - 500 Internal server error.
    - Responses: Minimal API endpoint return values:
        - String. [A,B,C]
        - Any type but a string. e.g.: DTO.
        - IResult-based types: Results.X, TypedResults.X

- MANIPULATING RESOURCES:
    - Creating a resource. Use nouns in URLS, not verbs. Do not mix plural and singular nouns.
        - Generating links: LinkGenerator. CreatedAtRoute();
            - With parent/child relationships, vaidate whether the parent exists.
            - Don't use one endpoint for creating on item versus a collection of items.
        - Updating a resource:
            - Check if the resource exists. 
            - Be careful when enabling PUT for collection resources. This can be destructive.
            - PUT is intended for full updates. For partial updates, PATCH exists.
            - Change sets for PATCH requests are often described as a list of operations:
                - JsonPatchDocument.
                - There is no support for this with minimal APIs.
        - Deleting a resource:
    - Grouping resources:
        - We can apply metadata, authorization to a provided group.
    - Content negotiation: 
        - Process of selecting the best representation for a given response when there are multiple representations available.
        - Not supported out of the box for minimal API. Nor is it planned.
            - Accept: application/json.
            - Accept: application/xml.
            - Minimal API: JSON out of the box. Accept is ignored.
    - Input validation: Common requirement.
        - Common approach. Annotation.
               - In ASP.NET Core MVC. Automatic validation od incoming request bodies.
          - Not supported out of the box for minimal API. Nor is it planned.
              - Third party. e.g.: FluentValidation.

- STRUCTURING YOUR MINIMAL API:
    - Options: 
        1. Using methods instead on lnline handlers. Improves maintainability and testability.
        2. Variation. Seperate out to another class. e.g.:
        ```csharp
            public static class Handler
            {
                public static async Task<Ok<IEnumerable<DTO>>> GetAsync();
            }
        ```
        3. Extending IEndpontRouteBuilder
        ```csharp
            public static class EndpointRouteBuilderExtensions
            {
                public static void RegisterEndpoints(this IEndpontRouteBuilder app)
                {
                    var endpoints = app.MapGroup("/url");
                    endpoints.MapGet("", async Task<Ok<IEnumerable<DTO>>> GetAsync());
                }
            }
        ```
        4. Third-party: e.g.: Carter.

- HANDLING EXCEPTIONS AND LOGGING:
    - Exception handling:
        - Developer exception page middleware. Exposes stack traces for unhandled exceptions. Developing/integration.
            - Enabled by default in development and app is set up with a call into WebApplication.CreateBuilder.
        - Exception handler middleware: Error payload without stack trace. Avoid in development. Logs exceptions.
            - Not enabled by default. Use app.UseExceptionHandler.
        - Improving error responses with problem details:
            ```csharp
                builder.Services.AddProblemDetails() : IProblemDetailsService
            ```
            - Developer exception page middleware will automatically generate a problem/details response.
                - As will exception handler middleware. 
                - And status code page middleware can be configured to generate problem details responses for empty bodies. e.g.: 400, 404.
    - Logging:
        - Same as ASP.NET Core. Default. Inject.
    
- IMPPEMENTING BUSINESS LOGIC WITH ENDPOINT FILTERS:
    - Filters for minimal APIs. Comparison to ASAP.NET Core MVC filters. Common scenarios.
    - A pipeline of filters that runs after MVC has selected the action to execute.
        - These filters allow you to run code before or after specific stages in the request processing pipeline.
            01. Authorization filters.
            02. Resource filters.
            03. (Model binding.)
            04. Action filters.
            05. (Action.)
            06. Exception filters.
            07. Result filters.
            08. (Result executes.) 
            09. Result filters.
            10. Resource filters.
        - There is no comparable filter pipeline for minimal APIs.
            - As an alternative, endpoint filters are supported. 
                - Not as extensive. Running code before/after endpoint handler.
                - Inspecting/modifying parameters provided during endpoint handler invocation.
                - Intercepting the response behavior of an endpoint handler.
            - e.g.: Logging request/response information. Transforming the request/response. 
            - (Dis)allowing a request. Validating the incoming request.
        - A request travels down the list of endpoint filters and then back up again in reverse order.
            - The order in which filters are added is inportant! Filters can short-circuit the pipeline.
        - Validation annotations are ignored. Third party: MiniValidation.

- SECURING YOUR MINIMAL API:
    - Token-based security.
        - Application-level and/or infrastructure level?
        - Same domain or cross-domain?
        - Local or centralized users and credentials?
        - Authentication and/or authorization?
    - Component that generates and provides tokens. Component that requests a token, Component that requires and validates the token.
        - JWTs. "aud" "role" "exp"
        - Our API validates a token. It does not generate it. It does not provide it.
    - Authentication: The process of determining whether someone or something is who or what it says it is.
        - Authentication: The token contains verifiable information on whom or what is accessing it.
        - Delegation: The token allows access on behalf of a user or application.
        ```csharp
            public interface IAuthenticationService {  };
            builder.Services.AddAuthentication().AddJwtBearer();
            public interface IAuthorizationService {  };
            builder.Services.AddAuthorizatio();
            var endpoint = endpointRouteBuilder.MapGroup("/dishes/{id:guid}/ingredients").RequireAuthorization();
        ```
        - Add/configure authentication services. Tie authentication to the JWT authentication handler.
        - Authorization: The process of determining what someone or something is allowed to do.
        - Generating a token: Manually generate:
            - /login endpoint. Use only for simple use cases.
            - OAuth2 and OpenID COnnect are standardized protocols for token-based security. "Token-based security on steroids."
            - Centralized identity providers implement these & generate tokens:
                - Azure AD, IndeitiyServer, Auth0.
            - NOTE: Our API validates a token. It does not: (1) Generate it. (2) Provide it.
            - .NET Core includes a built-in tool to generate tokens which can be used for develpment.
                ```javascript
                    dotnet-user-jwts
                    dotnet user-jwts create --audience menu-api
                    dotnet user-jwts create --audience menu-api --claim city=Duluth --role admin
                    dotnet user-jwts print TOKEN_ID
                ```
            - Creating and applying an authorization policy:

- DOCUMENTING YOUR MINIMAL API:
    - Swagger and OpenAPI: Driven by, or generated from, an OpenAPI specification:
        - A programming language-agnostic standard for documenting HTTP APIs.
        - Generated from this? Tests. Client-side DTOs.
        - Most common: Swashbuckle. Inspects your API and generates a specification from it.
            - Swashbuckle UI wraps swagger-ui, a documentation interface.
        - Support for improving the OpenAPI spcification is provided via:
            ```csharp
                Microsoft.AspNetCore.OpenApi
            ```
        - "/swagger/index.html" (by default.)
        - "/swagger/v1/swagger.json" (specification.)
    - Describing response types and status codes. TypedResults (versus Results) produces beter documentation.
    - Request: Body. Headers. Cookies.
