# Blood Pressure Log API (BPLog.API)

## Configuration

- `SqliteConnection` - connection string to SQLite db
- `PrivateKey` - a key that used for JWT token generation (must be at least 16 bytes long)

## Database
SQLite database is used to store users and blood pressure measures.  
Connection to the database is read from `appsettings.json` file (see `SqliteConnection` field), so it can be adjusted if there is a need.

### Create new database
> Update-Database -Context BPLogDbContext

### Add migrations
> Add-Migration BPLogMigration_v0001  -Project "BPLog.API" -Context BPLogDbContext -o "Domain/Migrations"

## Authentication

For simplicity, user passwords are stored in db as `SHA256` hashes and they are validated during `Authenticate` operation.

`JWT` is issued as a result of successful auth operation:
- `Sub` - user id

