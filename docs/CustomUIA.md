<!-- Copyright (c) Microsoft Corporation. All rights reserved.
     Licensed under the MIT Licence. -->

## Custom UI Automation properties
Axe Windows supports the inspection of [custom UI Automation properties](https://docs.microsoft.com/en-gb/windows/win32/winauto/uiauto-custompropertieseventscontrolpatterns). The custom properties for which Axe Windows should register are described in a user-supplied text file that is applied to all inspected UI Automation elements.

### Configuration file format
Custom UI Automation configuration data is specified in a [JSON](https://en.wikipedia.org/wiki/JSON#Syntax) formatted text file, with the additional allowance for both C- and C++-style comments. The file consists of one object with the following possible attributes:

#### properties
This attribute contains an array of objects, one per defined custom UI Automation property. Each property object contains the following fields, which should match the implementation of the property by the UI Automation provider:

Attribute | Description
--- | ---
guid|The [RFC4122](https://datatracker.ietf.org/doc/html/rfc4122#section-4.1) globally unique identifier of this property.
programmaticName|A textual description of this property.
uiaType|The data type of this property's value, one of `string`, `int`, `bool`, `double`, `point`, `element`, or `enum`.
values|If this property is an enumeration (`"uiaType": "enum"`), this attribute contains a mapping of integer members of the enumeration, specified as string keys in the file, to human-readable descriptions. If the value of the property matches one of the attributes of this object, `"value from object (<raw value>)"` is presented as the value for this custom property. If not present, `"Unknown (<raw value>)"` is presented instead.

#### Example of a complete custom UI Automation configuration file
This example file contains definitions for various [Excel](https://docs.microsoft.com/en-gb/office/uia/excel/excelcustomproperties) and [PowerPoint](https://docs.microsoft.com/en-gb/office/uia/powerpoint/powerpointcustomproperties) properties.

``` jsonc
{
  "properties": [
    /* Excel properties */
    {
      "guid": "4BB56516-F354-44CF-A5AA-96B52E968CFD",
      "programmaticName": "AreGridlinesVisible",
      "uiaType": "bool"
    },
    {
      "guid": "E244641A-2785-41E9-A4A7-5BE5FE531507",
      "programmaticName": "CellFormula",
      "uiaType": "string"
    },
    {
      "guid": "626CF4A0-A5AE-448B-A157-5EA4D1D057D7",
      "programmaticName": "CellNumberFormat",
      "uiaType": "string"
    },
    {
      "guid": "312F7536-259A-47C7-B192-AA16352522C4",
      "programmaticName": "CommentReplyCount",
      "uiaType": "int"
    },
    {
      "guid": "7AAEE221-E14D-4DA4-83FE-842AAF06A9B7",
      "programmaticName": "DataValidationPrompt",
      "uiaType": "string"
    },
    {
      "guid": "DFEF6BBD-7A50-41BD-971F-B5D741569A2B",
      "programmaticName": "HasConditionalFormatting",
      "uiaType": "bool"
    },
    {
      "guid": "29F2E049-5DE9-4444-8338-6784C5D18ADF",
      "programmaticName": "HasDataValidation",
      "uiaType": "bool"
    },
    {
      "guid": "1B93A5CD-0956-46ED-9BBF-016C1B9FD75F",
      "programmaticName": "HasDataValidationDropdown",
      "uiaType": "bool"
    },
    { // PowerPoint property
      "guid": "F065BAA7-2794-48B6-A927-193DA1540B84",
      "programmaticName": "ViewType",
      "uiaType": "enum",
      "values": {
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
}
```