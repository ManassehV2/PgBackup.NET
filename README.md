
# PostgreSQL backup

This package can be used to take postgres database backups on demand from your .NEt core or .Net framework applications.
Please refer https://www.postgresql.org/docs/9.3/libpq-pgpass.html for setting up the .pgpass file for the package to use to authentication to the database server, 

## Installation Instructions
Nuget package available (https://www.nuget.org/packages/PgBackup.Net/1.0.0)
```
Install-Package PgBackup.Net -Version 1.0.0
```
dotnet cli:
```
dotnet add package PgBackup.Net --version 1.0.0
```
# Package usage
## 1. Register the service in Startup.cs or Program.cs file
```
services.AddPgBackupServices();
```
## 2. call the BackupDB method with path string(the storage location of the backup tar file) 
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
    _pgDumpService.BackupDB("/Users/minase/Documents/");
  }
}
```

## MIT License

Copyright (c) 2022 Minase Mekete Mengistu

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
