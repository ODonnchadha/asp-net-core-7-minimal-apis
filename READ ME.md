## Building ASP.NET Core 7 Minimal APIs by Kevin Dockx

- OVERVIEW:
    - This course will teach you how to build a web API with ASP.NET Core’s minimal API approach.
    - CRUD. Structuring. Exception handling and logging. Secure and document.

- INTRODUCTION TO ASP.NET CORE MINIMAL APIs:
    - ASP.NET Core MVC? Model. View. Controller.
    - ASP.NET Core Minimal API?
        - Supports routing, parameter binding, basic serialization, and basic filters.
        - Intended for lightweight scenarios without all the ceremony yhat comes with MVC.
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
    - 

- HANDLING EXCEPTIONS AND LOGGING:
- IMPPEMENTING BUSINESS LOGIC WITH ENDPOINT FILTERS:
- SECURING YOUR MINIMAL API:
- DOCUMENTING YOUR MINIMAL API: