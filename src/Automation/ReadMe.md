When looking at the workflow of this project, please start at
`AutomationFactory.CreateAutomationSessionFactory`.

## Compatibility practices

The set of public interfaces defined in this project constitutes the
requirements to run an AxeWindows automated scan of a given application
and take action on the errors detected by the scan. We expect  the set
of interfaces will grow as the requirements grow. Ideally, customers could
update to new versions of the Axe.Windows package, taking advantage of rule
improvements or updated element properties for example, without having to modify
their code. To that end, the following sections describe types of
changes to the interfaces.

### Considerations

Because this library is a strong-named .Net assembly, we can be sure 
the code that depends on it will always link to the expected 
version of the dll at runtime. The benefit is that we don't need 
to design for the situation where the calling code might unintentionally link to a newer or older version of this assembly. 

### Types of changes

The following sections describe the three types of changes for
[semantic versioning](https://semver.org/)
as they relate to the public set of interfaces in this assembly.

#### Non-breaking changes

As long as the interfaces themselves have not changed, changes to this
assembly **may** be considered non-breaking. It's conceivable  that changes
to code behind the interfaces could be considered a feature or a
breaking change. But to be considered a non-breaking change, the interfaces
**must not** be modified in any way.

#### Features

The following modifications to the interfaces **must** at least be
considered feature changes:

- The addition of new methods or properties on an existing interface
- The addition of a new interface

These changes are additions to existing functionality which will not force
package consumers to update their code. But of course, if these types
of changes are combined with any breaking changes, the changes **must**
be considered breaking.

#### Breaking changes

The following modifications to the interfaces **must** be
considered breaking changes:

- Deleted methods or properties
- Changes to existing method or property signatures.

### Relational Results Data

Scan results data has been organized relationally to minimize the
possible ripple effects of changing any single type of data. This is mainly
done in the `IAutomationScanResult` interface. The interface contains only 
the ids of a rule and an element. Therefore, the `IElementInfo` interface
and the `IRuleInfo` interface can change completely independently.
And of course, unless something drastic happens, the `IAutomationScanResult` interface doesn't need to change.
This has the added benefit of not duplicating data unnecessarily in memory.
