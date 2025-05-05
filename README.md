brew services start postgresql@14
dotnet user-secrets init
dotnet user-secrets set "JWT:secret" "your-32-characters-long-super-strong-jwt-secret-key"

dotnet ef database drop --startup-project src/WebAPI/WebAPI.csproj --project src/Infrastructure
dotnet ef database update --startup-project src/WebAPI/WebAPI.csproj --project src/Infrastructure

## Backend:

https://learn.microsoft.com/en-us/aspnet/core/fundamentals/openapi/using-openapi-documents?view=aspnetcore-9.0
https://medium.com/codex/securing-the-net-9-app-signup-login-jwt-refresh-tokens-and-role-based-access-with-postgresql-43df24fd0ba2
https://www.sqlshack.com/setting-up-a-postgresql-database-on-mac/
http://localhost:5020/scalar/v1

## Testing:
https://dev.to/tkarropoulos/streamlining-your-tests-with-iclassfixture-in-xunit-1kpo


## Frontend:

https://v2.tailwindcss.com/docs/guides/vue-3-vite
https://flowbite.com/docs/getting-started/vue/
https://tailwindcss.com/docs/installation/using-vite
https://vueschool.io/articles/vuejs-tutorials/how-to-use-vue-router-a-complete-tutorial/

#### Other notes
1. Freelancer create new weeklist. Upload an xls file, with week number, order number, shipping number
2. Admin gives EAN, from Google Sheet, and uploads an XLS file to drive filename: WEEKNO-SHIPPINGNUMBER-SUPPLIER


#
"default": "Host=127.0.0.1; Port=5432; Database=testing_db; Username=wepack;Password=Luxpack11;Include Error Detail=true"
"default": "Host=localhost; Port=5432; Database=mmultimerk; Username=kasperkloster;Password=password"
##
DELETE FROM "Products" WHERE "Id" BETWEEN 4 AND 37;