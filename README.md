# BlogApi
REST API to manage a single blog website

After launching Docker and running the docker-compose file, perform the following steps:

1. From ~/Blog.Api/Blog.Api Start the server by running
   - dotnet start 
You can access the Swagger Api at https://localhost:8090/swagger/index.html

2. From ~/Blog.Api/Blog.Api.Test run the tests by running
   - dotnet test --logger "console;verbosity=detailed"
