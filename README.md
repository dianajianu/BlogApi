# BlogApi
REST API to manage a single blog website

After launching Docker and running the docker-compose file, perform the following steps:

For starting the server from ~/Blog.Api/Blog.Api run
   - dotnet start 
You can access the Swagger Api at https://localhost:8090/swagger/index.html

For running the tests from ~/Blog.Api/Blog.Api.Test run
   - dotnet build
   - dotnet test --logger "console;verbosity=detailed"
