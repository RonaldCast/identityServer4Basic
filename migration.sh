dotnet ef migrations add InitialIdentityServerPersistedGrantDbMigration -c PersistedGrantDbContext -o Data/Migrations/IdentityServer/PersistedGrantDb --project identity-server

dotnet ef migrations add InitialIdentityServerConfigurationDbMigration -c ConfigurationDbContext -o Data/Migrations/IdentityServer/ConfigurationDb --project identity-server

dotnet ef migrations add initialMigration -o Data/Migrations/App  --project './identity-server' --context AppDbContext

dotnet ef database update initialMigration --project './identity-server' --context AppDbContext 



