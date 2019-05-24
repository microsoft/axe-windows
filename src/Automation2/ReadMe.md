When looking at the workflow of this project, please start at
`AutomationFactory.CreateAutomationSessionFactory`.

### Compatibility

The set of public interfaces defined in this project **should** constitute the
minimum requirements to run an AxeWindows automated scan of a given application
and take action on the errors detected by the scan. We expect  the set
of interfaces will grow as the requirements grow. Ideally, customers could
update to new versions of the Axe.Windows package, taking advantage of rule
improvements or updated element properties for example, without having to modify
their code. To that end, the following sections describe how the interfaces
can grow with updates to the package, without breaking backward compatibility.

#### Interface versioning

As requirements grow for existing concepts, new, versioned interfaces are
created which inherit from the existing concepts' interfaces. For example,
take the following interface:

```c#
    public interface IElementInfo
    {
        int ParentId { get; }
        IReadOnlyDictionary<string, string> Properties { get; }
    }
```

if we want to add more data about an element, we simply create a versioned
interface which inherits from `IElementInfo`.

```c#
    public interface IElementInfo2 : IElementInfo
    {
        IEnumerable<string> AvailablePatterns { get; }
    }
```

This approach prevents breaking code because the original interface used by
package consumers hasn't been modified.

Consumers don't even need to change their code unless they're interested
in the new data. For example, if a new configuration option is added in
`IAutomationConfig2`, consumers of the latest package can simply cast to the newer
interface from the given one, e.g.,

```c#
    IAutomationSessionFactory factory = AutomationFactory.CreateAutomationSessionFactory();

    IAutomationConfig config = factory.CreateAutomationConfig();

    // now we have access to the latest configuration options if we need them
    var config2 = config as IAutomationConfig2

    // IAutomationSessionFactory.CreateAutomationSession does not need to be 
    // modified because it takes an IAutomationConfig object as a parameter
    // and IAutomationConfig2 inherits from IAutomationConfig.
    IAutomationSession = factory.CreateAutomationSession(config2);
```

Internally, the object that implements
`IAutomationSessionFactory.CreateAutomationSession` can cast `IAutomationConfig`
to `IAutomationConfig2`, so the `IAutomationSessionFactory` interface
remains untouched, even though new configuration data is now available.

#### Relational Results Data

Scan results data has been organized relationally to minimize the
possible ripple effects of changing any single type of data. This is mainly
done in the `IAutomationScanResult` interface. The interface contains only 
the ids of a rule and an element. Therefore, the `IElementInfo` interface
and the `IRuleInfo` interface can change completely independenly.
And of course, unless something drastic happens, the `IAutomationScanResult` interface doesn't need to change.
This has the added benefit of not duplicating data unnecessarily in memory.
