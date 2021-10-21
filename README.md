# Microsoft.Extensions.Configuration.Etcd

[![Nuget Version](https://img.shields.io/nuget/v/Microsoft.Extensions.Configuration.Etcd)](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Etcd/)
[![Nuget Download](https://img.shields.io/nuget/dt/Microsoft.Extensions.Configuration.Etcd)](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Etcd/)
[![AppVeyor branch](https://img.shields.io/appveyor/ci/i4004/microsoft-extensions-configuration-etcd/master)](https://ci.appveyor.com/project/i4004/microsoft-extensions-configuration-etcd)
[![AppVeyor tests (branch)](https://img.shields.io/appveyor/tests/i4004/microsoft-extensions-configuration-etcd/master)](https://ci.appveyor.com/project/i4004/microsoft-extensions-configuration-etcd)
[![Libraries.io dependency status for latest release](https://img.shields.io/librariesio/release/nuget/Microsoft.Extensions.Configuration.Etcd)](https://libraries.io/nuget/Microsoft.Extensions.Configuration.Etcd)
[![CodeFactor Grade](https://img.shields.io/codefactor/grade/github/SimplifyNet/Microsoft.Extensions.Configuration.Etcd)](https://www.codefactor.io/repository/github/simplifynet/Microsoft.Extensions.Configuration.Etcd)
![Platform](https://img.shields.io/badge/platform-.NET%205.0%20%7C%20.NET%20Standard%202.1%20%7C%20.NET%20Standard%202.0%20%7C%20.NET%204.6.2-lightgrey)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen)](http://makeapullrequest.com)

Etcd based configuration provider for Microsoft.Extensions.Configuration.
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

### HTTPS with certificate in environment variables

1. Add `ETCD_CLIENT_CA_FILE` environment variable with path to etcd CA file (shoule be provided by etcd administrator), for example: `C:\etcd\cert\EtcdCa.crt`

```csharp
var config = new ConfigurationBuilder()
	.AddEtcd(
		new Credentials("MyEtcdUserName", "passw"),
		new EtcdSettings("https://serveraddress:2379"))
	.Build();

var mySection = config.GetSection("MySection");
var myKeyValue = mySection["MyKeyName"];
```

### HTTP with settings from local JSON file

appsettings.json
```json
{
	"EtcdSettings":
	{
		"ConnectionString":, "https://serveraddress:2379",
		"CertificateData":, "asdb,mdfgldfglfdkjglkjl234-dflkdf;xlcvbxmas'd;l'as;dl'2435ro=fdodfgldk",
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

Settings can be mixed from different locations, for example, you can set only CA file in environment variable and only connection string in JSON file.
