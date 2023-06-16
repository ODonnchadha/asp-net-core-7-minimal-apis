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
    - Routing:
    - DTOs versus entity onjects.
    - Parameter Binding: Process of converting equest data into strongly-typed parameters.
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