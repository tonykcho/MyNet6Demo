# MyNet6Demo
 
Build Solution
dotnet build /nowarn:CS8618 /nowarn:CS8603 /nowarn:CS8600 /nowarn:CS8602

# Add Migration
dotnet ef migrations add <migration_name> -s ../MyNet6Demo.Api -o ./Migrations --context AppDbContext

# Remove Migration
dotnet ef migrations remove -s ../MyNet6Demo.Api

Testing Pulling