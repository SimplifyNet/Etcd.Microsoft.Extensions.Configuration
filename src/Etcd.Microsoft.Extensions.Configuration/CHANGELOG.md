# Changelog

## [3.1.1] - 2026-02-23

### Dependencies

- dotnet-etcd bump to 8.1.0

## [3.1.0] - 2025-10-22

### Added

- Possibility to load credentials from environment variables

## [3.0.0] - 2025-06-26

### Removed

- .NET 6 support

### Added

- Explicit .NET 8, 9 support
- Configurable Key Prefix separator (#5)

### Changed

- Before if Key Prefixes were used, then the etcd configuration without prefixes were also added to the Configuration Builder, now it will not and should be added separately if required (#7)
- Keys and prefixes are now case-insensitive according to official documentation (#6)
- Improved internal types performance

### Dependencies

- dotnet-etcd bump to 8.0.1
- Microsoft.Extensions.Configuration bump to 9.0.6

## [2.1.0] - 2024-05-23

### Dependencies

- dotnet-etcd bump to 5.2.1
- Microsoft.Extensions.Configuration bump to 8

### Changed

- Full dispose implemented

## [2.0.1] - 2022-12-02

### Fixed

- No options read from ETCD when user has permission to read all keys (#3)

## [2.0.0] - 2022-06-04

### Removed

- .NET Standard 2.0 support
- .NET Framework 4.6.2 support

### Dependencies

- dotnet-etcd bump to 5.2
- Microsoft.Extensions.Configuration bump to 6.0.1

## [1.0.3] - 2022-06-01

### Fixed

- Regression of: IConfigurationSection.GetChildren returns empty result (#2)

## [1.0.2] - 2022-05-29

### Fixed

- IConfigurationSection.GetChildren returns empty result (#2)

### Dependencies

- Grpc.Core bump to 2.46.3 for .NET 4.6.2 target

### Added

- .NET 6 explicit support

### Removed

- .NET 5 incorrect target

## [1.0.1] - 2021-11-15

### Fixed

- Adding key perfix for GetChildKeys

## [1.0] - 2021-10-26

### Added

- Initial version
