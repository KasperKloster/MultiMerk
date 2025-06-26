## Links:
### Backend:
1. https://learn.microsoft.com/en-us/aspnet/core/fundamentals/openapi/using-openapi-documents?view=aspnetcore-9.0
2. https://medium.com/codex/securing-the-net-9-app-signup-login-jwt-refresh-tokens-and-role-based-access-with-postgresql-43df24fd0ba2
3. https://www.sqlshack.com/setting-up-a-postgresql-database-on-mac/

### Testing:
1. https://dev.to/tkarropoulos/streamlining-your-tests-with-iclassfixture-in-xunit-1kpo


### Frontend:
1. https://v2.tailwindcss.com/docs/guides/vue-3-vite
2. https://flowbite.com/docs/getting-started/vue/
3. https://tailwindcss.com/docs/installation/using-vite
4. https://vueschool.io/articles/vuejs-tutorials/how-to-use-vue-router-a-complete-tutorial/

## In Development
1. http://localhost:5020/scalar/v1

### Postgre Queries
<code>
DELETE FROM "Products" WHERE "Id" BETWEEN 4 AND 37;
</code>

<code>
UPDATE "WeeklistTaskLinks" SET "WeeklistTaskStatusId" = 2 WHERE "WeeklistId" = 2 AND "WeeklistTaskId" = 7;
</code>

### CLI
<code>
dotnet ef database drop --startup-project src/WebAPI/WebAPI.csproj --project src/Infrastructure
dotnet ef migrations <NAME> seeder --startup-project src/WebAPI/WebAPI.csproj --project src/Infrastructure
dotnet ef database update --startup-project src/WebAPI/WebAPI.csproj --project src/Infrastructure
</code>

### Dev Connection
<code>
"default": "Host=127.0.0.1; Port=5432; Database=testing_db; Username=wepack;Password=Password;Include Error Detail=true"
"default": "Host=localhost; Port=5432; Database=mmultimerk; Username=kasperkloster;Password=password"
</code>