# Etcd.Microsoft.Extensions.Configuration

[![NuGet Version](https://img.shields.io/nuget/v/Etcd.Microsoft.Extensions.Configuration)](https://www.nuget.org/packages/Etcd.Microsoft.Extensions.Configuration/)
[![NuGet Download](https://img.shields.io/nuget/dt/Etcd.Microsoft.Extensions.Configuration)](https://www.nuget.org/packages/Etcd.Microsoft.Extensions.Configuration/)
[![Build Package](https://github.com/SimplifyNet/Etcd.Microsoft.Extensions.Configuration/actions/workflows/build.yml/badge.svg)](https://github.com/SimplifyNet/Etcd.Microsoft.Extensions.Configuration/actions/workflows/build.yml)
[![Libraries.io dependency status for latest release](https://img.shields.io/librariesio/release/nuget/Etcd.Microsoft.Extensions.Configuration)](https://libraries.io/nuget/Etcd.Microsoft.Extensions.Configuration)
[![CodeFactor Grade](https://img.shields.io/codefactor/grade/github/SimplifyNet/Etcd.Microsoft.Extensions.Configuration)](https://www.codefactor.io/repository/github/simplifynet/Etcd.Microsoft.Extensions.Configuration)
![Platform](https://img.shields.io/badge/platform-.NET%2010.0%20%7C%20.NET%209.0%20%7C%20.NET%208.0-lightgrey)

`Etcd.Microsoft.Extensions.Configuration` is an etcd-backed configuration provider for `Microsoft.Extensions.Configuration`.

It lets you load configuration values from etcd into the standard .NET configuration pipeline, including support for key prefixes, watch-based refresh, and both direct and configuration-based settings.

> Note: Starting with v2, Windows requires Windows 10 / Windows Server 2016 or later because the internal gRPC client uses HTTP/2.

## Contents

- [Installation](#installation)
- [Setup](#setup)
- [Usage](#usage)
- [HTTPS configuration](#https-configuration)
- [Testing locally](#testing-locally)
- [Contributing](#contributing)
- [License](#license)

## Installation

Install the NuGet package:

```bash
dotnet add package Etcd.Microsoft.Extensions.Configuration
```

This package targets .NET 8, .NET 9, and .NET 10.

## Setup

The library exposes an `AddEtcd(...)` extension method on `ConfigurationBuilder`.

At minimum, you need:

- `Credentials` with an etcd username and password
- `EtcdSettings` with the etcd connection string

### Basic HTTP example

```csharp
using Etcd.Microsoft.Extensions.Configuration;
using Etcd.Microsoft.Extensions.Configuration.Auth;
using Etcd.Microsoft.Extensions.Configuration.Settings;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .AddEtcd(
        new Credentials("MyEtcdUserName", "passw"),
        new EtcdSettings("http://serveraddress:2379"))
    .Build();

var mySection = config.GetSection("MySection");
var myKeyValue = mySection["MyKeyName"];
```

### Using environment variables

You can also create credentials from environment variables:

```csharp
var credentials = Credentials.FromEnvironmentVariables();

var config = new ConfigurationBuilder()
    .AddEtcd(credentials, new EtcdSettings("http://localhost:2379"))
    .Build();
```

The default environment variable names are:

- `ETCD_CLIENT_USER_NAME`
- `ETCD_CLIENT_PASSWORD`

## Usage

### Prefix-based configuration

You can load only a subset of keys by applying a prefix:

```csharp
var config = new ConfigurationBuilder()
    .AddEtcd(
        new Credentials("MyEtcdUserName", "passw"),
        new EtcdSettings("http://localhost:2379"),
        "MyApplication")
    .Build();
```

This is useful when different services share the same etcd instance but use separate namespaces.

### Multiple prefixes

```csharp
var config = new ConfigurationBuilder()
    .AddEtcd(
        new Credentials("MyEtcdUserName", "passw"),
        new EtcdSettings("http://localhost:2379"),
        new[] { "App:Shared", "App:Specific" })
    .Build();
```

### Watching for changes

Watching is enabled by default. You can disable it or control unwatching behavior with the `enableWatch` and `unwatchOnDispose` parameters:

```csharp
var config = new ConfigurationBuilder()
    .AddEtcd(
        new Credentials("MyEtcdUserName", "passw"),
        new EtcdSettings("http://localhost:2379"),
        keyPrefix: "MyApp",
        enableWatch: true,
        unwatchOnDispose: true)
    .Build();
```

## HTTPS configuration

When using HTTPS, a `.crt` CA certificate provided by the etcd administrator should be added to the system trust store.

- Windows: `Trusted Root Certification Authority`
- Linux (for example Arch/Manjaro): place it in `/etc/ca-certificates/trust-source/anchors/` and run:

```bash
sudo trust extract-compat
```

### HTTPS via appsettings.json

`appsettings.json`

```json
{
  "EtcdSettings": {
    "ConnectionString": "https://serveraddress:2379"
  }
}
```

```csharp
using Etcd.Microsoft.Extensions.Configuration;
using Etcd.Microsoft.Extensions.Configuration.Auth;
using Etcd.Microsoft.Extensions.Configuration.Settings;
using Microsoft.Extensions.Configuration;

var jsonConfig = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var config = new ConfigurationBuilder()
    .AddEtcd(
        new Credentials("MyEtcdUserName", "passw"),
        new ConfigurationBasedEtcdSettings(jsonConfig))
    .Build();
```

Settings can be composed from multiple configuration sources.

## Testing locally

To run the integration tests locally, make sure an etcd instance is running on the default port without SSL:

```text
http://localhost:2379
```

On Ubuntu, install the required tools:

```bash
sudo apt-get install etcd-server etcd-client
```

Create a root user and enable authentication:

```bash
etcdctl --endpoints=http://localhost:2379 user add root
etcdctl --endpoints=http://localhost:2379 auth enable
```

The repository includes Terraform files in `tests/terraform/` to provision test keys for the integration setup.

```bash
cd tests/terraform
terraform init
terraform plan
terraform apply
```

After the etcd test data is ready, run the test projects from the `src` folder.

## Contributing

Contributions are welcome. You can help by:

- improving documentation
- reporting bugs
- submitting fixes
- proposing features
- adding tests
- improving code quality

## License

Licensed under the GNU Lesser General Public License.
