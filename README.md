# Imouto.Auditable

This is rewrite of original library without any consideration of backward compatibility, please don't update to this version if you already have working code.

Changes:
* Writer now have access to AuditableEntry model and can use it
* AuditType.Created was added
* Nullable support was added
* AuditId changed to Guid
* Default EnvironmentCollector takes info from IHostEnvironment
* Removed extension-like configuration (just replace implementations of different parts of library with AddService)
* Only net7 is supported
* Aspnet and tests projects are removed
* Overall simplifications and style updates

[![Nuget](https://img.shields.io/badge/nuget-auditable-blue)](https://www.nuget.org/packages/Imouto.Auditable)
```
<PackageReference Include="Imouto.Auditable" Version="2.0.0" />
```


## Features

- `Unit of work style` to auditing changes
- Track `Read`, `Removed`, `Created` and `Modified` instances
- Full `delta` is provided using the `Json Patch` Specification
- `Customise` what you write to the audit log with your own `Parser`
- Write anywhere, `File`, `Console` or bring bring your own if you need
- Changes can be audited as `explicit` or `observed`
- Capture who with the `IPrincipal` or `IClaimsPrincipal`


## Original
Consider using original library: https://github.com/dbones-labs/auditable
