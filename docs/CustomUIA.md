<!-- Copyright (c) Microsoft Corporation. All rights reserved.
     Licensed under the MIT Licence. -->

## Custom UI Automation properties
Axe Windows supports the inspection of [custom UI Automation properties](https://docs.microsoft.com/en-gb/windows/win32/winauto/uiauto-custompropertieseventscontrolpatterns). The custom properties for which Axe Windows should register are described in a user-supplied text file that is applied to all inspected UI Automation elements.

### Configuration file format
Custom UI Automation configuration data is specified in a [JSON](https://en.wikipedia.org/wiki/JSON#Syntax) formatted text file. The file consists of one object with the following possible attributes:

#### metadata
This attribute, if specified, can contain values to describe the file as determined by the user. It may be useful, for instance, to store version or authorship information. Example:

```
  "metadata": {
    "version": "1.0",
    "modified": "1970-01-01"
  }
```

#### properties
This attribute contains an array of objects, one per defined custom UI Automation property. Each property object contains the following fields:

##### guid
The globally unique identifier of this property.

##### programmaticName
A textual description of this property.

##### uiaType
The data type of this property's value, one of `"string"`, `"int"`, or `"bool"`.

##### interpretation
If specified, an object mapping values of a property to more friendly descriptions. This is useful, for instance, when the value of a property is an enumeration.

##### Example
An example of a `properties` object:
``` json
  "properties": [
    {
      "guid": "{F065BAA7-2794-48B6-A927-193DA1540B84}",
      "programmaticName": "ViewType",
      "uiaType": "int",
      "interpretation": {
        "1": "ViewSlide",
        "2": "ViewSlideMaster",
        "3": "ViewNotesPage",
        "4": "ViewHandoutMaster",
        "5": "ViewNotesMaster",
        "6": "ViewOutline",
        "7": "ViewSlideSorter",
        "8": "ViewTitleMaster",
        "9": "ViewNormal",
        "10": "ViewPrintPreview",
        "11": "ViewThumbnails",
        "12": "ViewMasterThumbnails",
        "13": "ViewNotesText",
        "14": "ViewOutlineMaster",
        "15": "ViewSlideShow",
        "16": "ViewSlideShowFullScreen",
        "17": "ViewSlideShowBrowse",
        "18": "ViewPresenterSlide",
        "19": "ViewPresenterNotes",
        "20": "ViewPresenterNextStep",
        "21": "ViewPresenterTitle",
        "22": "ViewGridSections",
        "23": "ViewGridThumbnails",
        "24": "ViewGridSectionTitle",
        "25": "ViewGridThumbnailZoom",
        "26": "ViewGridBack",
        "27": "ViewProtected",
        "28": "ViewVisualBasic",
        "29": "ViewNone",
        "30": "ViewUnknown"
      }
    }
  ]
```

#### Example of a complete custom UI Automation configuration file
``` json
{
  "properties": [
    {
      "guid": "{4BB56516-F354-44CF-A5AA-96B52E968CFD}",
      "programmaticName": "AreGridlinesVisible",
      "uiaType": "bool"
    },
    {
      "guid": "{E244641A-2785-41E9-A4A7-5BE5FE531507}",
      "programmaticName": "CellFormula",
      "uiaType": "string"
    },
    {
      "guid": "{626CF4A0-A5AE-448B-A157-5EA4D1D057D7}",
      "programmaticName": "CellNumberFormat",
      "uiaType": "string"
    },
    {
      "guid": "{312F7536-259A-47C7-B192-AA16352522C4}",
      "programmaticName": "CommentReplyCount",
      "uiaType": "int"
    },
    {
      "guid": "{7AAEE221-E14D-4DA4-83FE-842AAF06A9B7}",
      "programmaticName": "DataValidationPrompt",
      "uiaType": "string"
    },
    {
      "guid": "{DFEF6BBD-7A50-41BD-971F-B5D741569A2B}",
      "programmaticName": "HasConditionalFormatting",
      "uiaType": "bool"
    },
    {
      "guid": "{29F2E049-5DE9-4444-8338-6784C5D18ADF}",
      "programmaticName": "HasDataValidation",
      "uiaType": "bool"
    },
    {
      "guid": "{1B93A5CD-0956-46ED-9BBF-016C1B9FD75F}",
      "programmaticName": "HasDataValidationDropdown",
      "uiaType": "bool"
    }
  ]
}
```