Generating Migrations
----------------------
Make sure you are in the project directory where your `DbContext` is defined before running the migration commands. This is typically the project that contains your database context class.
```bash
cd alpr.api
```

To generate a new migration, use the following command in the Package Manager Console:
```bash
dotnet ef migrations add [Migration name] -o Database/Migrations
```
Replace `[Migration name]` with a descriptive name for your migration, such as `AddUserTable` or `UpdateProductSchema`. The `-o Database/Migrations` option specifies the output directory for the generated migration files.

Applying Migrations
--------------------
To apply the generated migrations to your database, use the following command:
```bash
dotnet ef database update
```