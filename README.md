# Etcd.Microsoft.Extensions.Configuration

[![Nuget Version](https://img.shields.io/nuget/v/Etcd.Microsoft.Extensions.Configuration)](https://www.nuget.org/packages/Etcd.Microsoft.Extensions.Configuration/)
[![Nuget Download](https://img.shields.io/nuget/dt/Etcd.Microsoft.Extensions.Configuration)](https://www.nuget.org/packages/Etcd.Microsoft.Extensions.Configuration/)
[![Build Package](https://github.com/SimplifyNet/Etcd.Microsoft.Extensions.Configuration/actions/workflows/build.yml/badge.svg)](https://github.com/SimplifyNet/Etcd.Microsoft.Extensions.Configuration/actions/workflows/build.yml)
[![Libraries.io dependency status for latest release](https://img.shields.io/librariesio/release/nuget/Etcd.Microsoft.Extensions.Configuration)](https://libraries.io/nuget/Etcd.Microsoft.Extensions.Configuration)
[![CodeFactor Grade](https://img.shields.io/codefactor/grade/github/SimplifyNet/Etcd.Microsoft.Extensions.Configuration)](https://www.codefactor.io/repository/github/simplifynet/Etcd.Microsoft.Extensions.Configuration)
![Platform](https://img.shields.io/badge/platform-.NET%206.0%20%7C%20.NET%20Standard%202.1-lightgrey)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen)](http://makeapullrequest.com)

Etcd based configuration provider for Microsoft.Extensions.Configuration.

> Keep in mind that starting from v2 it will work only under Windows 10 or Windows Server 2016 (or later) because of internal Grpc client uses HTTP/2 protocol for communication (<https://docs.microsoft.com/en-us/answers/questions/310032/2012-r2-net-core-http2.html>).

## Quick start

### HTTP

```csharp
 var config = new ConfigurationBuilder()
  .AddEtcd(
   new Credentials("MyEtcdUserName", "passw"),
   new EtcdSettings("http://serveraddress:2379"))
  .Build();

 var mySection = config.GetSection("MySection");
 var myKeyValue = mySection["MyKeyName"];
```

### HTTPS

When using HTTPS, *.crt CA certificate (should be provided by etcd administrator) should be placed in well known system certificate store (depending on OS).

* For Windows it is `Trusted Root Certification Authority`.
* For Linux, at least for Arch/Manjaro Linux it should be placed in : `/etc/ca-certificates/trust-source/anchors/` followed by `sudo trust extract-compat` command

### HTTPS with settings from local JSON file

appsettings.json

```json
{
  "EtcdSettings":
  {
    "ConnectionString": "https://serveraddress:2379"
  }
}
```

```csharp
var jsonConfig = new ConfigurationBuilder()
 .AddJsonFile("appsettings.json")
 .Build();

 var config = new ConfigurationBuilder()
 .AddEtcd(
  new Credentials("MyEtcdUserName", "passw"),
  new ConfigurationBasedEtcdSettings(jsonConfig))
 .Build();

var mySection = config.GetSection("MySection");
var myKeyValue = mySection["MyKeyName"];
```

Settings can be mixed from different locations.

## Contributing

There are many ways in which you can participate in the project. Like most open-source software projects, contributing code is just one of many outlets where you can help improve. Some of the things that you could help out with are:

* Documentation (both code and features)
* Bug reports
* Bug fixes
* Feature requests
* Feature implementations
* Test coverage
* Code quality
* Sample applications

## License

Licensed under the GNU LESSER GENERAL PUBLIC LICENSE
