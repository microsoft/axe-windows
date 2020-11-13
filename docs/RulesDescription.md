
## Rules in Axe.Windows

Name | Description | Standard referenced | Type
--- | --- | --- |---
| BoundingRectangleNotAllZeros | The BoundingRectangle property must not be defined as [0,0,0,0] | Section 508 502.3.1 ObjectInformation | Error |
| BoundingRectangleNotNull | An onscreen element must not have a null BoundingRectangle property. | Section 508 502.3.1 ObjectInformation | Error |
| BoundingRectangleNotValidButOffScreen | The BoundingRectangle property is not valid, but the element is off-screen. | Section 508 502.3.1 ObjectInformation | Warning |
BoundingRectangleDataFormatCorrect | The BoundingRectangle property must return a valid rectangle. | Section 508 502.3.1 ObjectInformation | Error
BoundingRectangleCompletely ObscuresContainer | An element's BoundingRectangle must not obscure its container element. | Section 508 502.3.1 ObjectInformation | Error
BoundingRectangleContainedInParent | An element's BoundingRectangle must be contained within its parent element. | Section 508 502.3.1 ObjectInformation | Warning
BoundingRectangleOnUWPMenuBar | The BoundingRectangle property of a menubar in UWP may have a null or empty value. | Section 508 502.3.1 ObjectInformation | Warning
BoundingRectangleOnUWPMenuItem | The BoundingRectangle property of a menu item in UWP may have a null or empty value. | Section 508 502.3.1 ObjectInformation | Warning
BoundingRectangleOnWPFTextParent | The BoundingRectangle property of a given element in the WPF framework whose parent is of type text may have a null or empty value. | Section 508 502.3.1 ObjectInformation | Warning
BoundingRectangleSizeReasonable | The BoundingRectangle property must represent an area of at least 25 pixels. | Section 508 502.3.1 ObjectInformation | Error
SplitButtonInvokeAndTogglePatterns | A split button must not support both the Invoke and Toggle patterns. | WCAG 4.1.2 NameRoleValue | Error
ButtonShouldHavePatterns | A button must support one of these patterns: Invoke, Toggle, or ExpandCollapse. | WCAG 4.1.2 NameRoleValue | Error
ButtonInvokeAndTogglePatterns | A button must not support both the Invoke and Toggle patterns. | WCAG 4.1.2 NameRoleValue | Error
ButtonInvokeAndExpandCollapsePatterns | A button may have the Invoke and ExpandCollapse patterns together, but it is not recommended. If possible, please have only one of them.  | WCAG 1.3.1 InfoAndRelationships | Warning
ButtonToggleAndExpandCollapsePatterns | A button must not support both the Toggle and ExpandCollapse patterns. | WCAG 4.1.2 NameRoleValue | Warning
ClickablePointOnScreen  | An element's IsOffScreen property must be false when its clickable point is on screen. | Section 508 502.3.1 ObjectInformation | Warning
ClickablePointOffScreen  | An element's IsOffScreen property must be true when its clickable point is off screen. | Section 508 502.3.1 ObjectInformation | Warning
SiblingUniqueAndFocusable | Focusable sibling elements must not have the same Name and LocalizedControlType. | WCAG 4.1.2 NameRoleValue | Error
SiblingUniqueAndNotFocusable | The given element has siblings with the same Name and LocalizedControlType. | WCAG 4.1.2 NameRoleValue | Warning
ChildrenNotAllowedInContentView | A separator must not have any children with IsContentElement set to TRUE. | Section 508 502.3.1 ObjectInformation | Error
ContentViewButtonStructure | The given element is expected to have the following structure: Button and NoChild(IsContentElement). | WCAG 1.3.1 InfoAndRelationships | Warning
ContentViewCalendarStructure | The given element is expected to have the following structure: Calendar and (NoChild(IsContentElement) or AllChildren(not(IsContentElement) or ListItem)). | WCAG 1.3.1 InfoAndRelationships | Warning
ContentViewCheckBoxStructure | The given element is expected to have the following structure: CheckBox and NoChild(IsContentElement). | WCAG 1.3.1 InfoAndRelationships | Warning
ContentViewComboBoxStructure | The given element is expected to have the following structure: ComboBox and (NoChild(IsContentElement) or AllChildren(not(IsContentElement) or ListItem)). | WCAG 1.3.1 InfoAndRelationships | Warning
ContentViewDataGridStructure | The given element is expected to have the following structure: DataGrid and (NoChild(IsContentElement) or AllChildren(not(IsContentElement) or DataItem)). | WCAG 1.3.1 InfoAndRelationships | Warning
ContentViewEditStructure | The given element is expected to have the following structure: Edit and NoChild(IsContentElement). | WCAG 1.3.1 InfoAndRelationships | Warning
ContentViewHyperlinkStructure | The given element is expected to have the following structure: Hyperlink and NoChild(IsContentElement). | WCAG 1.3.1 InfoAndRelationships | Warning
ContentViewListStructure | The given element is expected to have the following structure: List and (NoChild(IsContentElement) or AllChildren(not(IsContentElement) or (DataItem or ListItem or Group))). | WCAG 1.3.1 InfoAndRelationships | Warning
ContentViewListItemStructure | The given element is expected to have the following structure: ListItem and NoChild(IsContentElement). | WCAG 1.3.1 InfoAndRelationships | Warning
ContentViewMenuStructure | The given element is expected to have the following structure: Menu and AnyChild(IsContentElement) and AllChildren(not(IsContentElement) or MenuItem). | WCAG 1.3.1 InfoAndRelationships | Warning
ContentViewProgressBarStructure | The given element is expected to have the following structure: ProgressBar and NoChild(IsContentElement). | WCAG 1.3.1 InfoAndRelationships | Warning
ContentViewRadioButtonStructure | The given element is expected to have the following structure: RadioButton and NoChild(IsContentElement). | WCAG 1.3.1 InfoAndRelationships | Warning
ContentViewSliderStructure | The given element is expected to have the following structure: Slider and (NoChild(IsContentElement) or AllChildren(not(IsContentElement) or ListItem)). | WCAG 1.3.1 InfoAndRelationships | Warning
ContentViewSpinnerStructure | The given element is expected to have the following structure: Spinner and (NoChild(IsContentElement) or AllChildren(not(IsContentElement) or ListItem)). | WCAG 1.3.1 InfoAndRelationships | Warning
ContentViewSplitButtonStructure | The given element is expected to have the following structure: SplitButton and (CountChildren(Button and AnyChild(IsContentElement) and AllChildren(not(IsContentElement) or MenuItem)) == 1 or CountChildren(Button and AnyChild(IsContentElement) and AllChildren(not(IsContentElement) or MenuItem)) == 2) and AllChildren(not(IsContentElement) or Button). | WCAG 1.3.1 InfoAndRelationships | Warning
ContentViewStatusBarStructure | The given element is expected to have the following structure: StatusBar and (NoChild(IsContentElement) or AllChildren(not(IsContentElement) or (Button or Edit or Image or ProgressBar))). | WCAG 1.3.1 InfoAndRelationships | Warning
ContentViewTabStructure | The given element is expected to have the following structure: Tab and AnyChild(IsContentElement) and AllChildren(not(IsContentElement) or (Group or TabItem)). | WCAG 1.3.1 InfoAndRelationships | Warning
ContentViewTreeStructure | The given element is expected to have the following structure: Tree and (NoChild(IsContentElement) or AllChildren(not(IsContentElement) or (DataItem or TreeItem))). | WCAG 1.3.1 InfoAndRelationships | Warning
ContentViewTreeItemStructure | The given element is expected to have the following structure: TreeItem and (NoChild(IsContentElement) or AllChildren(not(IsContentElement) or TreeItem)). | WCAG 1.3.1 InfoAndRelationships | Warning
ControlViewButtonStructure | The given element is expected to have the following structure: Button and (NoChild(IsControlElement) or AllChildren(not(IsControlElement) or (Image or Text))). | WCAG 1.3.1 InfoAndRelationships | Warning
ControlViewCalendarStructure | The given element is expected to have the following structure: Calendar / (DataGrid and NoChild(Header) or CountChildren(Header) == 1 and AnyChild(Header and (NoChild(HeaderItem) or CountChildren(HeaderItem) == 7)) and NoChild(Button) or CountChildren(Button) == 2 and (CountChildren(ListItem) > 0)). | WCAG 1.3.1 InfoAndRelationships | Warning
ControlViewComboBoxStructure | The given element is expected to have the following structure: ComboBox and CountChildren(Button) == 1 and CountChildren(Edit) <= 1 and CountChildren(List) <= 1 and AllChildren(not(IsControlElement) or (Button or Edit or List)). | WCAG 1.3.1 InfoAndRelationships | Warning
ControlViewCheckBoxStructure | The given element is expected to have the following structure: CheckBox and NoChild(IsControlElement). | WCAG 1.3.1 InfoAndRelationships | Warning
ControlViewDataGridStructure | The given element is expected to have the following structure: DataGrid and CountChildren(Header) <= 2 and (NoChild(IsControlElement) or AllChildren(not(IsControlElement) or (Header or DataItem))). | WCAG 1.3.1 InfoAndRelationships | Warning
ControlViewEditStructure | The given element is expected to have the following structure: Edit and NoChild(IsControlElement). | WCAG 1.3.1 InfoAndRelationships | Warning
ControlViewHeaderStructure | The given element is expected to have the following structure: Header and AnyChild(IsControlElement) and AllChildren(not(IsControlElement) or HeaderItem). | WCAG 1.3.1 InfoAndRelationships | Warning
ControlViewHeaderItemStructure | The given element is expected to have the following structure: HeaderItem and NoChild(IsControlElement). | WCAG 1.3.1 InfoAndRelationships | Warning
ControlViewHyperlinkStructure | The given element is expected to have the following structure: Hyperlink and NoChild(IsControlElement). | WCAG 1.3.1 InfoAndRelationships | Warning
ControlViewImageStructure | The given element is expected to have the following structure: Image and NoChild(IsControlElement). | WCAG 1.3.1 InfoAndRelationships | Warning
ControlViewListStructure | The given element is expected to have the following structure: List and (NoChild(IsControlElement) or AllChildren(not(IsControlElement) or (DataItem or ListItem or Group or ScrollBar))). | WCAG 1.3.1 InfoAndRelationships | Warning
ControlViewListItemStructure | The given element is expected to have the following structure: ListItem and (NoChild(IsControlElement) or AllChildren(not(IsControlElement) or (Edit or Image or Text))). | WCAG 1.3.1 InfoAndRelationships | Warning
ControlViewMenuStructure | The given element is expected to have the following structure: Menu and AnyChild(IsControlElement) and AllChildren(not(IsControlElement) or MenuItem). | WCAG 1.3.1 InfoAndRelationships | Warning
ControlViewProgressBarStructure | The given element is expected to have the following structure: ProgressBar and NoChild(IsControlElement). | WCAG 1.3.1 InfoAndRelationships | Warning
ControlViewRadioButtonStructure | The given element is expected to have the following structure: RadioButton and NoChild(IsControlElement). | WCAG 1.3.1 InfoAndRelationships | Warning
ControlViewScrollbarStructure | The given element is expected to have the following structure: ScrollBar and (CountChildren(Button) == 0 or CountChildren(Button) == 2 or CountChildren(Button) == 4) and CountChildren(Thumb) == 0 or CountChildren(Thumb) == 1. | WCAG 1.3.1 InfoAndRelationships | Warning
ControlViewSemanticZoomStructure | The given element is expected to have the following structure: SemanticZoom and (NoChild(IsControlElement) or AllChildren(not(IsControlElement) or (List or ListItem))). | WCAG 1.3.1 InfoAndRelationships | Warning
ControlViewSeparatorStructure | The given element is expected to have the following structure: Separator and NoChild(IsControlElement). | WCAG 1.3.1 InfoAndRelationships | Warning
ControlViewSliderStructure | The given element is expected to have the following structure: Slider and (CountChildren(Button) == 2 or CountChildren(Button) == 4) and CountChildren(Thumb) == 1 and AllChildren(not(IsControlElement) or (Button or Thumb or ListItem)). | WCAG 1.3.1 InfoAndRelationships | Warning
ControlViewSpinnerStructure | The given element is expected to have the following structure: Spinner and CountChildren(Button) == 2 and CountChildren(Edit) <= 1 and AllChildren(not(IsControlElement) or (Button or Edit or ListItem)). | WCAG 1.3.1 InfoAndRelationships | Warning
ControlViewSplitButtonStructure | The given element is expected to have the following structure: SplitButton and (CountChildren(Button and NoChild(IsControlElement) or CountChildren(MenuItem) == 1 and AllChildren(not(IsControlElement) or MenuItem)) == 1 or CountChildren(Button and NoChild(IsControlElement) or CountChildren(MenuItem) == 1 and AllChildren(not(IsControlElement) or MenuItem)) == 2) and CountChildren(Image) <= 1 and CountChildren(Text) <= 1 and AllChildren(not(IsControlElement) or (Button or Image or Text)). | WCAG 1.3.1 InfoAndRelationships | Warning
ControlViewStatusBarStructure | The given element is expected to have the following structure: StatusBar and (NoChild(IsControlElement) or AllChildren(not(IsControlElement) or (Button or Edit or Image or ProgressBar))). | WCAG 1.3.1 InfoAndRelationships | Warning
ControlViewTabStructure | The given element is expected to have the following structure: Tab and AnyChild(IsControlElement) and AllChildren(not(IsControlElement) or (Group or ScrollBar or TabItem)). | WCAG 1.3.1 InfoAndRelationships | Warning
ControlViewThumbStructure | The given element is expected to have the following structure: Thumb and NoChild(IsControlElement). | WCAG 1.3.1 InfoAndRelationships | Warning
ControlViewToolTipStructure | The given element is expected to have the following structure: ToolTip and (NoChild(IsControlElement) or AllChildren(not(IsControlElement) or (Image or Text))). | WCAG 1.3.1 InfoAndRelationships | Warning
ControlViewTreeStructure | The given element is expected to have the following structure: Tree and (NoChild(IsControlElement) or AllChildren(not(IsControlElement) or (DataItem or ScrollBar or TreeItem))). | WCAG 1.3.1 InfoAndRelationships | Warning
ControlViewTreeItemStructure | The given element is expected to have the following structure: TreeItem and (NoChild(IsControlElement) or AllChildren(not(IsControlElement) or (Button or CheckBox or Image or TreeItem))). | WCAG 1.3.1 InfoAndRelationships | Warning
ComboBoxShouldNotSupportScrollPattern | A combo box should not support the Scroll pattern. This rule may be reported as a warning because some platforms have combo boxes support the scroll pattern by default, which app developers can't easily fix. | Section 508 502.3.10 AvailableActions | Warning
ControlShouldNotSupportInvokePattern | An element of the given ControlType must not support the Invoke pattern. | Section 508 502.3.10 AvailableActions | Error
ControlShouldNotSupportScrollPattern | An element of the given ControlType must not support the Scroll pattern. | Section 508 502.3.10 AvailableActions | Error
ControlShouldNotSupportTablePattern | An element of the given ControlType must not support the Table pattern. | Section 508 502.3.10 AvailableActions | Error
ControlShouldNotSupportTogglePattern | An element of the given ControlType must not support the Toggle pattern. | Section 508 502.3.10 AvailableActions | Error
ControlShouldNotSupportValuePattern | An element of the given type should not support the Value pattern. | Section 508 502.3.10 AvailableActions | Warning
ControlShouldNotSupportWindowPattern | An element of the given type should not support the Window pattern. | Section 508 502.3.10 AvailableActions | Warning
ControlShouldSupport ExpandCollapsePattern | An element of the given ControlType must support the ExpandCollapse pattern. | Section 508 502.3.10 AvailableActions | Error
ControlShouldSupportGridItemPattern | An element whose parent supports the Grid pattern must support the GridItem pattern. | Section 508 502.3.10 AvailableActions | Error
ControlShouldSupportGridPattern | An element of the given ControlType must support the Grid pattern. | Section 508 502.3.10 AvailableActions | Error
ControlShouldSupportInvokePattern | An element of the given ControlType must support the Invoke pattern. | Section 508 502.3.10 AvailableActions | Error
ControlShouldSupportScrollItemPattern | An element whose parent supports the Scroll pattern must support the ScrollItem pattern. | Section 508 502.3.10 AvailableActions | Error
ControlShouldSupportSelectionItemPattern | An element of the given ControlType must support the SelectionItem pattern. | Section 508 502.3.10 AvailableActions | Error
ControlShouldSupportSelectionPattern | An element of the given ControlType must support the Selection pattern. | Section 508 502.3.10 AvailableActions | Error
ControlShouldSupportSetInfoWPF | The element's ControlType requires valid values for SizeOfSet and PositionInSet. | Section 508 502.3.1 ObjectInformation | Warning
ControlShouldSupportSetInfoXAML | The element's ControlType requires valid values for SizeOfSet and PositionInSet. | Section 508 502.3.1 ObjectInformation | Error
ControlShouldSupport SpreadsheetItemPattern | An element whose parent supports the Spreadsheet pattern must support the SpreadsheetItem pattern. | Section 508 502.3.10 AvailableActions | Error
ControlShouldSupportTableItemPattern | An element whose parent supports the Table pattern must support the TableItem pattern. | Section 508 502.3.10 AvailableActions | Error
ControlShouldSupportTablePattern | An element of the given ControlType must support the Table pattern. | Section 508 502.3.10 AvailableActions | Error
ControlShouldSupportTablePatternInEdge | An element of the given ControlType must support the Table pattern. | Section 508 502.3.10 AvailableActions | Warning
ControlShouldSupportTogglePattern | An element of the given ControlType must support the Toggle pattern. | Section 508 502.3.10 AvailableActions | Error
ControlShouldSupportTransformPattern | An element that can be resized must support the Transform pattern. | Section 508 502.3.10 AvailableActions | Error
ControlShouldSupportTextPattern | An element of the given ControlType must support the Text pattern. | Section 508 502.3.10 AvailableActions | Error
EditSupportsIncorrectRangeValuePattern | The RangeValue pattern of an edit control must have a null LargeChange property. | Section 508 502.3.10 AvailableActions | Error
HeadingLevelDescendsWhenNested | An element's HeadingLevel must be greater than or equal to that of its ancestors. | WCAG 1.3.1 InfoAndRelationships | Error
LandmarkBannerIsTopLevel | An element with LocalizedLandmarkType "banner" must not descend from another landmark. | WCAG 1.3.1 InfoAndRelationships | Error
LandmarkComplementaryIsTopLevel | An element with LocalizedLandmarkType "complementary" must not descend from another landmark. | WCAG 1.3.1 InfoAndRelationships | Error
LandmarkContentInfoIsTopLevel | An element with LocalizedLandmarkType "contentinfo" must not descend from another landmark. | WCAG 1.3.1 InfoAndRelationships | Error
LandmarkMainIsTopLevel | An element with LocalizedLandmarkType "main" must not descend from another landmark. | WCAG 1.3.1 InfoAndRelationships | Error
LandmarkNoDuplicateBanner | A page must not have multiple elements with LocalizedLandmarkType "banner." | WCAG 1.3.1 InfoAndRelationships | Error
LandmarkNoDuplicateContentInfo | A page must not have multiple elements with LocalizedLandmarkType "contentinfo." | WCAG 1.3.1 InfoAndRelationships | Error
LandmarkOneMain | A page must have exactly one element with the LocalizedLandmarkType "main." | WCAG 1.3.1 InfoAndRelationships | Warning
LocalizedLandmarkType ExcludesPrivateUnicodeCharacters | The LocalizedLandmarkType property must not contain any characters in the private Unicode range. | WCAG 1.3.1 InfoAndRelationships | Error
LocalizedLandmarkTypeIsReasonableLength | The LocalizedLandmarkType property must not be longer than 64 characters. | WCAG 1.3.1 InfoAndRelationships | Error
LocalizedLandmarkTypeNotCustom | The LandmarkType and LocalizedLandmarkType must not both be set to "custom." | WCAG 1.3.1 InfoAndRelationships | Error
LocalizedLandmarkTypeNotEmpty | An element with LandmarkType set must not have an empty LocalizedLandmarkType. | WCAG 1.3.1 InfoAndRelationships | Error
LocalizedLandmarkTypeNotNull | An element with LandmarkType set must not have a null LocalizedLandmarkType. | WCAG 1.3.1 InfoAndRelationships | Error
LocalizedLandmarkTypeNotWhiteSpace | The LocalizedLandmarkType property must not contain only white space. | WCAG 1.3.1 InfoAndRelationships | Error
HelpTextNotEqualToName | The HelpText property of an element must not be the same as the element's Name property. | Section 508 502.3.1 ObjectInformation | Warning
HyperlinkNameShouldBeUnique | Links with different purposes and destinations should have different names. | WCAG 4.1.2 NameRoleValue | Warning
IsControlElementPropertyExists | The given ControlType must have a non-null IsControlElement property. | Section 508 502.3.1 ObjectInformation | Error
IsContentElementPropertyExists | The given ControlType must have a non-null IsContentElement property. | Section 508 502.3.1 ObjectInformation | Error
IsContentElementFalseOptional | The recommended value of the IsContentElement property for the given control type is false. Please consider if this is an element that should be reported to an assistive technology user as content. | Section 508 502.3.1 ObjectInformation | Warning
IsContentElementTrueOptional | The recommended value of the IsContentElement property for the given control type is true. Please consider if this is an element that should be reported to an assistive technology user as content. | Section 508 502.3.1 ObjectInformation | Warning
IsControlElementTrueOptional | The recommended value of the IsControlElement property for the given control type is true. Please consider if this is an element that should be reported to an assistive technology user as a control. Note that almost all controls are required to have the IsControl Property set to true. | Section 508 502.3.1 ObjectInformation | Warning
IsControlElementTrueRequired | The given ControlType must have the IsControlElement property set to TRUE. | Section 508 502.3.1 ObjectInformation | Error
IsKeyboardFocusableShouldBeTrue | The IsKeyboardFocusable property for the given element should be true based on its control type. | WCAG 2.1.1 Keyboard | Warning
IsKeyboardFocusableFalseButDisabled | The IsKeyboardFocusable property is false for an element where it would normally be true. However, the IsEnabled property on the element is also false, so the value of IsKeyboardFocusable may be acceptable. | WCAG 2.1.1 Keyboard | Warning
IsKeyboardFocusableFor ListItemShouldBeTrue | The IsKeyboardFocusable property for the given list item is false, but the element has children that are focusable. The element should probably be focusable instead of its children. | WCAG 2.1.1 Keyboard | Warning
IsKeyboardFocusableFalseButOffscreen | The IsKeyboardFocusable property for the given element is false for an element where it would normally be true. However, the IsOffscreen property on the element is true, so the value of IsKeyboardFocusable may be acceptable. | WCAG 2.1.1 Keyboard | Warning
IsKeyboardFocusableFor CustomShouldBeTrue | The IsKeyboardFocusable property for a custome element should be true when the element supports actionable patterns. | WCAG 2.1.1 Keyboard | Warning
IsKeyboardFocusableDescendantTextPattern | The IsKeyboardFocusable property may be false when the given element supports the text pattern and is the descendant of an element that also supports the text pattern. Please consider if the given element should or should not be focusable. | WCAG 2.1.1 Keyboard | Warning
IsKeyboardFocusableOnEmptyContainer | The IsKeyboardFocusable property should be true when you want an empty container to be discoverable by assistive technology users. IsKeyboardFocusable may be false when you want an empty container not to be discoverable by AT users. | WCAG 2.1.1 Keyboard | Warning
IsKeyboardFocusableShouldBeFalse | The IsKeyboardFocusable property for the given element is expected to be false because of the element's control type. | WCAG 2.1.1 Keyboard | Warning
IsKeyboardFocusableTopLevelTextPattern | The IsKeyboardFocusable property should be true for an element that supports the text pattern, is not a descendant of an element that supports the text pattern, and which supports text selection. | WCAG 2.1.1 Keyboard | Warning
ItemTypeRecommended | The ItemType property for the given element has no content, and the element has a child image. Please consider including an item type so that assistive technology users can obtain the information provided by the image. If this information is already provided in another way, the item type may not be necessary. | Section 508 502.3.1 ObjectInformation | Warning
LocalizedControlTypeReasonable | The localized control type should be reasonable based on ControlTypeId. | Section 508 502.3.1 ObjectInformation | Warning
NameNotEmpty | The Name property of a focusable element must not be an empty string. | Section 508 502.3.1 ObjectInformation | Warning
NameExcludesControlType | The Name property must not include the element's control type. | Section 508 502.3.1 ObjectInformation | Error
NameExcludesLocalizedControlType | The Name must not include the same text as the LocalizedControlType. | Section 508 502.3.1 ObjectInformation | Error
NameReasonableLength | The Name property must not be longer than 512 characters. | Section 508 502.3.1 ObjectInformation | Warning
OrientationPropertyExists | Controls that can be horizontal or vertical must support the Orientation property. | Section 508 502.3.1 ObjectInformation | Error
ProgressBarRangeValue | The RangeValue pattern of a progress bar must have specific Minimum, Maximum, and IsReadOnly values. | Section 508 502.3.1 ObjectInformation | Error
ItemStatusExists | The ItemStatus property for the given element should exist. | Section 508 502.3.1 ObjectInformation | Warning
NameNotNull | The Name property of a focusable element must not be null. | Section 508 502.3.1 ObjectInformation | Warning
NameNotWhiteSpace | The Name property must not contain only space characters. | Section 508 502.3.1 ObjectInformation | Warning
NameNullButElementNot KeyboardFocusable | The Name property for the given element is null, but the element isn't focusable. Please consider whether or not the element should have a name. | Section 508 502.3.1 ObjectInformation | Warning
NameEmptyButElementNot KeyboardFocusable | The Name property for the given element is empty, but the element isn't focusable. Please consider whether or not the element should have a name. | Section 508 502.3.1 ObjectInformation | Warning
NameWithValidBoundingRectangle | An interactive element with a valid name property is usually expected to have a valid bounding rectangle that is not null and has area. | Section 508 502.3.1 ObjectInformation | Warning
NameOnOptionalType | The name property for the given element type is optional. | Section 508 502.3.1 ObjectInformation | Warning
NameNoSiblingsOfSameType | The name property of the given element may be null or empty if the element has no siblings of the same type. | Section 508 502.3.1 ObjectInformation | Warning
NameOnCustomWithParentWPFDataItem | The name property of a custom control may be empty if the parent is a wpf dataitem. | Section 508 502.3.1 ObjectInformation | Warning
NameIsInformative | The Name property must not include its class name. | Section 508 502.3.1 ObjectInformation | Error
LocalizedControlTypeNotWhiteSpace | The LocalizedControlType property must not contain only white space. | Section 508 502.3.1 ObjectInformation | Warning
LocalizedControlTypeNotEmpty | The LocalizedControlType property must not be an empty string. | Section 508 502.3.1 ObjectInformation | Warning
LocalizedControlTypeNotNull | The LocalizedControlType property must not be null. | Section 508 502.3.1 ObjectInformation | Warning
LocalizedControlTypeNotCustom | The ControlType and LocalizedControlType must not both be set to "custom." | Section 508 502.3.1 ObjectInformation | Warning
ParentChildShouldNotHaveSameName AndLocalizedControlType | An element must not have the same Name and LocalizedControlType as its parent. | Section 508 502.3.1 ObjectInformation | Error
SelectionPatternSelectionRequired | An element of the given ControlType must have the IsSelectionRequired property set to TRUE. | Section 508 502.3.10 AvailableActions | Error
SelectionPatternSingleSelection | An element of the given ControlType must not support multiple selection. | Section 508 502.3.10 AvailableActions | Error
SelectionItemPatternSingleSelection | An element whose parent supports single selection must not have selected siblings. | Section 508 502.3.10 AvailableActions | Error
ListItemSiblingsUnique | The Name property of sibling list items should be unique. | WCAG 4.1.2 NameRoleValue | Warning
NameExcludesPrivateUnicodeCharacters | The Name property must not contain any characters in the private Unicode range. | Section 508 502.3.1 ObjectInformation | Error
HelpTextExcludesPrivateUnicodeCharacters | The HelpText property must not contain any characters in the private Unicode range. | Section 508 502.3.1 ObjectInformation | Error
LocalizedControlTypeExcludes PrivateUnicodeCharacters | The LocalizedControlType property must not contain any characters in the private Unicode range. | Section 508 502.3.1 ObjectInformation | Error
