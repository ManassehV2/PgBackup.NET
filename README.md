
# PostgreSQL backup

This package is a simple wrapper of postgresql's pg_dump client tool and can be used to take postgres database backups on demand from your .NET core or .NET framework applications.
Please refer https://www.postgresql.org/docs/current/libpq-pgpass.html for setting up the .pgpass file for the package to use to authentication to the database server, 

## Installation Instructions
Nuget package available (https://www.nuget.org/packages/PgBackup.Net/1.0.0)
```
Install-Package PgBackup.Net -Version 1.1.0
```
dotnet cli:
```
dotnet add package PgBackup.Net --version=1.1.0
```
# Package usage
## 1. Register the service in Startup.cs or Program.cs file
```
services.AddPgBackupServices();
```
## 2. call the BackupDB method with path string(the storage location of the backup file) 
```
using PgBackup.Services;
public class myClass
{
  private readonly IPgDumpService _pgDumpService;
  
  public myClass(IPgDumpService pgDumpService)
  {
    _pgDumpService = pgDumpService;
  }
  public void TakeBackUp()
  {
    // option 1. to output backup file will be stored in the specified path
    _pgDumpService.BackupDB("dbName", "/Users/Documents/", BackupFileFormat format); //Default is Plain(.sql file)
    
    //option 2. to get the byte array of the backup file
    byte[] result = _pgDumpService.BackupDB("dbName", BackupFileFormat format); //Default is Plain(.sql file)
  }
}
```

## MIT License

Copyright (c) 2022 Minase Mekete Mengistu

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
