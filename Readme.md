# Identity Server 4

### Migration command:

```
dotnet ef migrations add InitialIdentityServerPersistedGrantDbMigration -c PersistedGrantDbContext -o Data/Migrations/IdentityServer/PersistedGrantDb --project identity-server
dotnet ef migrations add InitialIdentityServerConfigurationDbMigration -c ConfigurationDbContext -o Data/Migrations/IdentityServer/ConfigurationDb --project identity-server
```

**Configuration Store:** is used for encapsulating the configuration data and tables such as clients, resources and scopes.

**Operational Store:** is keeping the temporary data such as authorization codes and refresh tokens. For a better understanding I do encourage you to read the [Identity Server docs](https://identityserver4.readthedocs.io/).

URL: documention for configuration http://localhost:5000/.well-known/openid-configuration
