
## Rules in Axe.Windows

Name | Severity | Description | Standard referenced
--- | --- | --- | ---
BoundingRectangleNotAllZeros | Error | The BoundingRectangle property must not be defined as [0,0,0,0] | Section 508 502.3.1 ObjectInformation
BoundingRectangleNotNull | Error | An on-screen element must not have a null BoundingRectangle property. | Section 508 502.3.1 ObjectInformation
BoundingRectangleNotValidButOffScreen | NeedsReview | The BoundingRectangle property is not valid, but the element is off-screen. | Section 508 502.3.1 ObjectInformation
BoundingRectangleDataFormatCorrect | Error | The BoundingRectangle property must return a valid rectangle. | Section 508 502.3.1 ObjectInformation
BoundingRectangleCompletelyObscuresContainer | Error | An element's BoundingRectangle must not obscure its container element. | Section 508 502.3.1 ObjectInformation
BoundingRectangleContainedInParent | Warning | An element's BoundingRectangle must be contained within its parent element. | Section 508 502.3.1 ObjectInformation
BoundingRectangleSizeReasonable | Error | The BoundingRectangle property must represent an area of at least 25 pixels. | Section 508 502.3.1 ObjectInformation
BoundingRectangleNotNullListViewXAML | Error | An on-screen element must not have a null BoundingRectangle property. | Section 508 502.3.1 ObjectInformation
BoundingRectangleNotNullTextBlockXAML | Error | An on-screen element must not have a null BoundingRectangle property. | Section 508 502.3.1 ObjectInformation
SplitButtonInvokeAndTogglePatterns | Error | A split button must support exactly one of the Invoke or Toggle patterns. | WCAG 4.1.2 NameRoleValue
ButtonShouldHavePatterns | Error | A button must support one of these patterns: Invoke, Toggle, or ExpandCollapse. | WCAG 4.1.2 NameRoleValue
ButtonInvokeAndTogglePatterns | Error | A button must not support both the Invoke and Toggle patterns. | WCAG 4.1.2 NameRoleValue
ButtonInvokeAndExpandCollapsePatterns | Warning | A button may have the Invoke and ExpandCollapse patterns together, but it is not recommended. If possible, please have only one of them.  | WCAG 1.3.1 InfoAndRelationships
ButtonToggleAndExpandCollapsePatterns | Error | A button must not support both the Toggle and ExpandCollapse patterns. | WCAG 4.1.2 NameRoleValue
SiblingUniqueAndFocusable | Error | Focusable sibling elements must not have the same Name and LocalizedControlType. | WCAG 4.1.2 NameRoleValue
SiblingUniqueAndNotFocusable | NeedsReview | The given element has siblings with the same Name and LocalizedControlType. | WCAG 4.1.2 NameRoleValue
ChildrenNotAllowedInContentView | Error | A separator must not have any children with IsContentElement set to TRUE. | Section 508 502.3.1 ObjectInformation
ContentViewButtonStructure | NeedsReview | The given element is expected to have the following structure: Button and NoChild(IsContentElement). | WCAG 1.3.1 InfoAndRelationships
ContentViewCalendarStructure | NeedsReview | The given element is expected to have the following structure: Calendar and (NoChild(IsContentElement) or AllChildren(not(IsContentElement) or ListItem)). | WCAG 1.3.1 InfoAndRelationships
ContentViewCheckBoxStructure | NeedsReview | The given element is expected to have the following structure: CheckBox and NoChild(IsContentElement). | WCAG 1.3.1 InfoAndRelationships
ContentViewComboBoxStructure | NeedsReview | The given element is expected to have the following structure: ComboBox and (NoChild(IsContentElement) or AllChildren(not(IsContentElement) or ListItem)). | WCAG 1.3.1 InfoAndRelationships
ContentViewDataGridStructure | NeedsReview | The given element is expected to have the following structure: DataGrid and (NoChild(IsContentElement) or AllChildren(not(IsContentElement) or DataItem)). | WCAG 1.3.1 InfoAndRelationships
ContentViewEditStructure | NeedsReview | The given element is expected to have the following structure: Edit and NoChild(IsContentElement). | WCAG 1.3.1 InfoAndRelationships
ContentViewHyperlinkStructure | NeedsReview | The given element is expected to have the following structure: Hyperlink and NoChild(IsContentElement). | WCAG 1.3.1 InfoAndRelationships
ContentViewListStructure | NeedsReview | The given element is expected to have the following structure: List and (NoChild(IsContentElement) or AllChildren(not(IsContentElement) or (DataItem or ListItem or Group))). | WCAG 1.3.1 InfoAndRelationships
ContentViewListItemStructure | NeedsReview | The given element is expected to have the following structure: ListItem and NoChild(IsContentElement). | WCAG 1.3.1 InfoAndRelationships
ContentViewMenuStructure | NeedsReview | The given element is expected to have the following structure: Menu and AnyChild(IsContentElement) and AllChildren(not(IsContentElement) or MenuItem). | WCAG 1.3.1 InfoAndRelationships
ContentViewProgressBarStructure | NeedsReview | The given element is expected to have the following structure: ProgressBar and NoChild(IsContentElement). | WCAG 1.3.1 InfoAndRelationships
ContentViewRadioButtonStructure | NeedsReview | The given element is expected to have the following structure: RadioButton and NoChild(IsContentElement). | WCAG 1.3.1 InfoAndRelationships
ContentViewSliderStructure | NeedsReview | The given element is expected to have the following structure: Slider and (NoChild(IsContentElement) or AllChildren(not(IsContentElement) or ListItem)). | WCAG 1.3.1 InfoAndRelationships
ContentViewSpinnerStructure | NeedsReview | The given element is expected to have the following structure: Spinner and (NoChild(IsContentElement) or AllChildren(not(IsContentElement) or ListItem)). | WCAG 1.3.1 InfoAndRelationships
ContentViewSplitButtonStructure | NeedsReview | The given element is expected to have the following structure: SplitButton and (CountChildren(Button and AnyChild(IsContentElement) and AllChildren(not(IsContentElement) or MenuItem)) == 1 or CountChildren(Button and AnyChild(IsContentElement) and AllChildren(not(IsContentElement) or MenuItem)) == 2) and AllChildren(not(IsContentElement) or Button). | WCAG 1.3.1 InfoAndRelationships
ContentViewStatusBarStructure | NeedsReview | The given element is expected to have the following structure: StatusBar and (NoChild(IsContentElement) or AllChildren(not(IsContentElement) or (Button or Edit or Image or ProgressBar))). | WCAG 1.3.1 InfoAndRelationships
ContentViewTabStructure | NeedsReview | The given element is expected to have the following structure: Tab and AnyChild(IsContentElement) and AllChildren(not(IsContentElement) or (Group or TabItem)). | WCAG 1.3.1 InfoAndRelationships
ContentViewTreeStructure | NeedsReview | The given element is expected to have the following structure: Tree and (NoChild(IsContentElement) or AllChildren(not(IsContentElement) or (DataItem or TreeItem))). | WCAG 1.3.1 InfoAndRelationships
ContentViewTreeItemStructure | NeedsReview | The given element is expected to have the following structure: TreeItem and (NoChild(IsContentElement) or AllChildren(not(IsContentElement) or TreeItem)). | WCAG 1.3.1 InfoAndRelationships
ControlViewButtonStructure | NeedsReview | The given element is expected to have the following structure: Button and (NoChild(IsControlElement) or AllChildren(not(IsControlElement) or (Image or Text))). | WCAG 1.3.1 InfoAndRelationships
ControlViewCalendarStructure | NeedsReview | The given element is expected to have the following structure: Calendar / (DataGrid and NoChild(Header) or CountChildren(Header) == 1 and AnyChild(Header and (NoChild(HeaderItem) or CountChildren(HeaderItem) == 7)) and NoChild(Button) or CountChildren(Button) == 2 and (CountChildren(ListItem) > 0)). | WCAG 1.3.1 InfoAndRelationships
ControlViewComboBoxStructure | NeedsReview | The given element is expected to have the following structure: ComboBox and CountChildren(Button) == 1 and CountChildren(Edit) <= 1 and CountChildren(List) <= 1 and AllChildren(not(IsControlElement) or (Button or Edit or List)). | WCAG 1.3.1 InfoAndRelationships
ControlViewCheckBoxStructure | NeedsReview | The given element is expected to have the following structure: CheckBox and NoChild(IsControlElement). | WCAG 1.3.1 InfoAndRelationships
ControlViewDataGridStructure | NeedsReview | The given element is expected to have the following structure: DataGrid and CountChildren(Header) <= 2 and (NoChild(IsControlElement) or AllChildren(not(IsControlElement) or (Header or DataItem))). | WCAG 1.3.1 InfoAndRelationships
ControlViewEditStructure | NeedsReview | The given element is expected to have the following structure: Edit and NoChild(IsControlElement). | WCAG 1.3.1 InfoAndRelationships
ControlViewHeaderStructure | NeedsReview | The given element is expected to have the following structure: Header and AnyChild(IsControlElement) and AllChildren(not(IsControlElement) or HeaderItem). | WCAG 1.3.1 InfoAndRelationships
ControlViewHeaderItemStructure | NeedsReview | The given element is expected to have the following structure: HeaderItem and NoChild(IsControlElement). | WCAG 1.3.1 InfoAndRelationships
ControlViewHyperlinkStructure | NeedsReview | The given element is expected to have the following structure: Hyperlink and NoChild(IsControlElement). | WCAG 1.3.1 InfoAndRelationships
ControlViewImageStructure | NeedsReview | The given element is expected to have the following structure: Image and NoChild(IsControlElement). | WCAG 1.3.1 InfoAndRelationships
ControlViewListStructure | NeedsReview | The given element is expected to have the following structure: List and (NoChild(IsControlElement) or AllChildren(not(IsControlElement) or (DataItem or ListItem or Group or ScrollBar))). | WCAG 1.3.1 InfoAndRelationships
ControlViewListItemStructure | NeedsReview | The given element is expected to have the following structure: ListItem and (NoChild(IsControlElement) or AllChildren(not(IsControlElement) or (Edit or Image or Text))). | WCAG 1.3.1 InfoAndRelationships
ControlViewMenuStructure | NeedsReview | The given element is expected to have the following structure: Menu and AnyChild(IsControlElement) and AllChildren(not(IsControlElement) or MenuItem). | WCAG 1.3.1 InfoAndRelationships
ControlViewProgressBarStructure | NeedsReview | The given element is expected to have the following structure: ProgressBar and NoChild(IsControlElement). | WCAG 1.3.1 InfoAndRelationships
ControlViewRadioButtonStructure | NeedsReview | The given element is expected to have the following structure: RadioButton and NoChild(IsControlElement). | WCAG 1.3.1 InfoAndRelationships
ControlViewScrollbarStructure | NeedsReview | The given element is expected to have the following structure: ScrollBar and (CountChildren(Button) == 0 or CountChildren(Button) == 2 or CountChildren(Button) == 4) and CountChildren(Thumb) == 0 or CountChildren(Thumb) == 1. | WCAG 1.3.1 InfoAndRelationships
ControlViewSemanticZoomStructure | NeedsReview | The given element is expected to have the following structure: SemanticZoom and (NoChild(IsControlElement) or AllChildren(not(IsControlElement) or (List or ListItem))). | WCAG 1.3.1 InfoAndRelationships
ControlViewSeparatorStructure | NeedsReview | The given element is expected to have the following structure: Separator and NoChild(IsControlElement). | WCAG 1.3.1 InfoAndRelationships
ControlViewSliderStructure | NeedsReview | The given element is expected to have the following structure: Slider and (CountChildren(Button) == 2 or CountChildren(Button) == 4) and CountChildren(Thumb) == 1 and AllChildren(not(IsControlElement) or (Button or Thumb or ListItem)). | WCAG 1.3.1 InfoAndRelationships
ControlViewSpinnerStructure | NeedsReview | The given element is expected to have the following structure: Spinner and CountChildren(Button) == 2 and CountChildren(Edit) <= 1 and AllChildren(not(IsControlElement) or (Button or Edit or ListItem)). | WCAG 1.3.1 InfoAndRelationships
ControlViewSplitButtonStructure | NeedsReview | The given element is expected to have the following structure: SplitButton and (CountChildren(Button and NoChild(IsControlElement) or CountChildren(MenuItem) == 1 and AllChildren(not(IsControlElement) or MenuItem)) == 1 or CountChildren(Button and NoChild(IsControlElement) or CountChildren(MenuItem) == 1 and AllChildren(not(IsControlElement) or MenuItem)) == 2) and CountChildren(Image) <= 1 and CountChildren(Text) <= 1 and AllChildren(not(IsControlElement) or (Button or Image or Text)). | WCAG 1.3.1 InfoAndRelationships
ControlViewStatusBarStructure | NeedsReview | The given element is expected to have the following structure: StatusBar and (NoChild(IsControlElement) or AllChildren(not(IsControlElement) or (Button or Edit or Image or ProgressBar))). | WCAG 1.3.1 InfoAndRelationships
ControlViewTabStructure | NeedsReview | The given element is expected to have the following structure: Tab and AnyChild(IsControlElement) and AllChildren(not(IsControlElement) or (Group or ScrollBar or TabItem)). | WCAG 1.3.1 InfoAndRelationships
ControlViewThumbStructure | NeedsReview | The given element is expected to have the following structure: Thumb and NoChild(IsControlElement). | WCAG 1.3.1 InfoAndRelationships
ControlViewToolTipStructure | NeedsReview | The given element is expected to have the following structure: ToolTip and (NoChild(IsControlElement) or AllChildren(not(IsControlElement) or (Image or Text))). | WCAG 1.3.1 InfoAndRelationships
ControlViewTreeStructure | NeedsReview | The given element is expected to have the following structure: Tree and (NoChild(IsControlElement) or AllChildren(not(IsControlElement) or (DataItem or ScrollBar or TreeItem))). | WCAG 1.3.1 InfoAndRelationships
ControlViewTreeItemStructure | NeedsReview | The given element is expected to have the following structure: TreeItem and (NoChild(IsControlElement) or AllChildren(not(IsControlElement) or (Button or CheckBox or Image or TreeItem))). | WCAG 1.3.1 InfoAndRelationships
ComboBoxShouldNotSupportScrollPattern | Warning | A combo box should not support the Scroll pattern. This rule may be reported as a warning because some platforms have combo boxes that support the Scroll pattern by default, which app developers can't easily fix. | Section 508 502.3.10 AvailableActions
ControlShouldNotSupportInvokePattern | Error | An element of the given ControlType must not support the Invoke pattern. | Section 508 502.3.10 AvailableActions
ControlShouldNotSupportScrollPattern | Error | An element of the given ControlType must not support the Scroll pattern. | Section 508 502.3.10 AvailableActions
ControlShouldNotSupportValuePattern | Warning | An element of the given type should not support the Value pattern. | Section 508 502.3.10 AvailableActions
ControlShouldNotSupportWindowPattern | Warning | An element of the given type should not support the Window pattern. | Section 508 502.3.10 AvailableActions
ControlShouldSupportExpandCollapsePattern | Error | An element of the given ControlType must support the ExpandCollapse pattern. | Section 508 502.3.10 AvailableActions
ControlShouldSupportGridItemPattern | Error | An element whose parent supports the Grid pattern must support the GridItem pattern. | Section 508 502.3.10 AvailableActions
ControlShouldSupportGridPattern | Error | An element of the given ControlType must support the Grid pattern. | Section 508 502.3.10 AvailableActions
ControlShouldSupportInvokePattern | Error | An element of the given ControlType must support the Invoke pattern. | Section 508 502.3.10 AvailableActions
ControlShouldSupportScrollItemPattern | Error | An element whose parent supports the Scroll pattern must support the ScrollItem pattern. | Section 508 502.3.10 AvailableActions
ControlShouldSupportSelectionItemPattern | Error | An element of the given ControlType must support the SelectionItem pattern. | Section 508 502.3.10 AvailableActions
ControlShouldSupportSelectionPattern | Error | An element of the given ControlType must support the Selection pattern. | Section 508 502.3.10 AvailableActions
ControlShouldSupportSetInfoWPF | Error | The element's ControlType requires valid values for SizeOfSet and PositionInSet. | Section 508 502.3.1 ObjectInformation
ControlShouldSupportSetInfoXAML | Error | The element's ControlType requires valid values for SizeOfSet and PositionInSet. | Section 508 502.3.1 ObjectInformation
ControlShouldSupportSpreadsheetItemPattern | Error | An element whose parent supports the Spreadsheet pattern must support the SpreadsheetItem pattern. | Section 508 502.3.10 AvailableActions
ControlShouldSupportTableItemPattern | Error | An element whose parent supports the Table pattern must support the TableItem pattern. | Section 508 502.3.10 AvailableActions
ControlShouldSupportTablePattern | Error | An element of the given ControlType must support the Table pattern. | Section 508 502.3.10 AvailableActions
ControlShouldSupportTogglePattern | Error | An element of the given ControlType must support the Toggle pattern. | Section 508 502.3.10 AvailableActions
ControlShouldSupportTransformPattern | Error | An element that can be resized must support the Transform pattern. | Section 508 502.3.10 AvailableActions
ControlShouldSupportTextPattern | Error | An element of the given ControlType must support the Text pattern. | Section 508 502.3.10 AvailableActions
ControlShouldSupportTextPatternEditWinform | Error | An element of the given ControlType must support the Text pattern. | Section 508 502.3.10 AvailableActions
EditSupportsIncorrectRangeValuePattern | Error | The RangeValue pattern of an edit control must have a null LargeChange property. | Section 508 502.3.10 AvailableActions
HeadingLevelDescendsWhenNested | Error | An element's HeadingLevel must be greater than or equal to that of its ancestors. | WCAG 1.3.1 InfoAndRelationships
LandmarkBannerIsTopLevel | Error | An element with LocalizedLandmarkType "banner" must not descend from another landmark. | WCAG 1.3.1 InfoAndRelationships
LandmarkComplementaryIsTopLevel | Error | An element with LocalizedLandmarkType "complementary" must not descend from another landmark. | WCAG 1.3.1 InfoAndRelationships
LandmarkContentInfoIsTopLevel | Error | An element with LocalizedLandmarkType "contentinfo" must not descend from another landmark. | WCAG 1.3.1 InfoAndRelationships
LandmarkMainIsTopLevel | Error | An element with LocalizedLandmarkType "main" must not descend from another landmark. | WCAG 1.3.1 InfoAndRelationships
LandmarkNoDuplicateBanner | Error | A page must not have multiple elements with LocalizedLandmarkType "banner". | WCAG 1.3.1 InfoAndRelationships
LandmarkNoDuplicateContentInfo | Error | A page must not have multiple elements with LocalizedLandmarkType "contentinfo". | WCAG 1.3.1 InfoAndRelationships
LocalizedLandmarkTypeExcludesPrivateUnicodeCharacters | Error | The LocalizedControlType property must not contain any characters in the private Unicode range. | WCAG 1.3.1 InfoAndRelationships
LocalizedLandmarkTypeIsReasonableLength | Error | The LocalizedLandmarkType property must not be longer than 64 characters. | WCAG 1.3.1 InfoAndRelationships
LocalizedLandmarkTypeNotCustom | Error | The LandmarkType and LocalizedLandmarkType must not both be set to "custom". | WCAG 1.3.1 InfoAndRelationships
LocalizedLandmarkTypeNotEmpty | Error | An element with LandmarkType set must not have an empty LocalizedLandmarkType. | WCAG 1.3.1 InfoAndRelationships
LocalizedLandmarkTypeNotNull | Error | An element with LandmarkType set must not have a null LocalizedLandmarkType. | WCAG 1.3.1 InfoAndRelationships
LocalizedLandmarkTypeNotWhiteSpace | Error | The LocalizedLandmarkType property must not contain only white space. | WCAG 1.3.1 InfoAndRelationships
HelpTextNotEqualToName | Warning | The HelpText property of an element must not be the same as the element's Name property. | Section 508 502.3.1 ObjectInformation
HyperlinkNameShouldBeUnique | Warning | Links with different purposes and destinations should have different names. | WCAG 4.1.2 NameRoleValue
IsControlElementPropertyExists | Error | The given ControlType must have a non-null IsControlElement property. | Section 508 502.3.1 ObjectInformation
IsContentElementPropertyExists | Error | The given ControlType must have a non-null IsContentElement property. | Section 508 502.3.1 ObjectInformation
IsContentElementFalseOptional | NeedsReview | The recommended value of the IsContentElement property for the given control type is false. Please consider if this is an element that should be reported to an assistive technology user as content. | Section 508 502.3.1 ObjectInformation
IsContentElementTrueOptional | NeedsReview | The recommended value of the IsContentElement property for the given control type is true. Please consider if this is an element that should be reported to an assistive technology user as content. | Section 508 502.3.1 ObjectInformation
IsControlElementTrueOptional | NeedsReview | The recommended value of the IsControlElement property for the given control type is true. Please consider if this is an element that should be reported to an assistive technology user as a control. Note that almost all controls are required to have the IsControl Property set to true. | Section 508 502.3.1 ObjectInformation
IsControlElementTrueRequired | Error | The given ControlType must have the IsControlElement property set to TRUE. | Section 508 502.3.1 ObjectInformation
IsControlElementTrueRequiredButtonWPF | Error | The given ControlType must have the IsControlElement property set to TRUE. | Section 508 502.3.1 ObjectInformation
IsControlElementTrueRequiredTextInEditXAML | Error | The given ControlType must have the IsControlElement property set to TRUE. | Section 508 502.3.1 ObjectInformation
IsKeyboardFocusableShouldBeTrue | Warning | The IsKeyboardFocusable property for the given element should be true based on its control type. | WCAG 2.1.1 Keyboard
IsKeyboardFocusableFalseButDisabled | NeedsReview | The IsKeyboardFocusable property is false for an element where it would normally be true. However, the IsEnabled property on the element is also false, so the value of IsKeyboardFocusable may be acceptable. | WCAG 2.1.1 Keyboard
IsKeyboardFocusableForListItemShouldBeTrue | Warning | The IsKeyboardFocusable property for the given list item is false, but the element has children that are focusable. The element should probably be focusable instead of its children. | WCAG 2.1.1 Keyboard
IsKeyboardFocusableFalseButOffscreen | NeedsReview | The IsKeyboardFocusable property for the given element is false for an element where it would normally be true. However, the IsOffscreen property on the element is true, so the value of IsKeyboardFocusable may be acceptable. | WCAG 2.1.1 Keyboard
IsKeyboardFocusableForCustomShouldBeTrue | Warning | The IsKeyboardFocusable property for a custom element should be true when the element supports actionable patterns. | WCAG 2.1.1 Keyboard
IsKeyboardFocusableDescendantTextPattern | NeedsReview | The IsKeyboardFocusable property may be false when the given element supports the Text pattern and is the descendant of an element that also supports the Text pattern. Please consider if the given element should or should not be focusable. | WCAG 2.1.1 Keyboard
IsKeyboardFocusableOnEmptyContainer | NeedsReview | The IsKeyboardFocusable property should be true when you want an empty container to be discoverable by assistive technology users. IsKeyboardFocusable may be false when you want an empty container not to be discoverable by assistive technology users. | WCAG 2.1.1 Keyboard
IsKeyboardFocusableShouldBeFalse | Warning | The IsKeyboardFocusable property for the given element is expected to be false because of the element's control type. | WCAG 2.1.1 Keyboard
IsKeyboardFocusableTopLevelTextPattern | Warning | The IsKeyboardFocusable property should be true for an element that supports the Text pattern, is not a descendant of an element that supports the Text pattern, and which supports text selection. | WCAG 2.1.1 Keyboard
ItemTypeRecommended | NeedsReview | The ItemType property for the given element has no content, and the element has a child image. Please consider including an ItemType so that assistive technology users can obtain the information provided by the image. If this information is already provided in another way, the ItemType may not be necessary. | Section 508 502.3.1 ObjectInformation
LocalizedControlTypeReasonable | Warning | The LocalizedControlType should be reasonable based on the element's ControlTypeId. | Section 508 502.3.1 ObjectInformation
NameNotEmpty | Error | The Name property of a focusable element must not be an empty string. | Section 508 502.3.1 ObjectInformation
NameExcludesControlType | Error | The Name property must not include the element's control type. | Section 508 502.3.1 ObjectInformation
NameExcludesLocalizedControlType | Error | The Name must not include the same text as the LocalizedControlType. | Section 508 502.3.1 ObjectInformation
NameReasonableLength | Error | The Name property must not be longer than 512 characters. | Section 508 502.3.1 ObjectInformation
OrientationPropertyExists | Error | Controls that can be horizontal or vertical must support the Orientation property. | Section 508 502.3.1 ObjectInformation
ProgressBarRangeValue | Error | The RangeValue pattern of a progress bar must have specific Minimum, Maximum, and IsReadOnly values. | Section 508 502.3.1 ObjectInformation
ItemStatusExists | NeedsReview | The ItemStatus property for the given element should exist. | Section 508 502.3.1 ObjectInformation
NameNotNull | Error | The Name property of a focusable element must not be null. | Section 508 502.3.1 ObjectInformation
NameNotWhiteSpace | Error | The Name property must not contain only space characters. | Section 508 502.3.1 ObjectInformation
NameNullButElementNotKeyboardFocusable | NeedsReview | The Name property for the given element is null, but the element isn't focusable. Please consider whether or not the element should have a name. | Section 508 502.3.1 ObjectInformation
NameEmptyButElementNotKeyboardFocusable | NeedsReview | The Name property for the given element is empty, but the element isn't focusable. Please consider whether or not the element should have a name. | Section 508 502.3.1 ObjectInformation
NameWithValidBoundingRectangle | Warning | An interactive element with a valid Name property is usually expected to have a valid BoundingRectangle that is not null and has area. | Section 508 502.3.1 ObjectInformation
NameOnOptionalType | NeedsReview | The Name property for the given element type is optional. | Section 508 502.3.1 ObjectInformation
NameNoSiblingsOfSameType | NeedsReview | The Name property of the given element may be null or empty if the element has no siblings of the same type. | Section 508 502.3.1 ObjectInformation
NameOnCustomWithParentWPFDataItem | NeedsReview | The Name property of a custom control may be empty if the parent is a WPF dataitem. | Section 508 502.3.1 ObjectInformation
NameIsInformative | Error | The Name property of an element should not contain class names like 'Microsoft.*.*' or 'Windows.*.*' as these are not usually informative. | Section 508 502.3.1 ObjectInformation
LocalizedControlTypeNotWhiteSpace | Error | The LocalizedControlType property must not contain only white space. | Section 508 502.3.1 ObjectInformation
LocalizedControlTypeNotEmpty | Error | The LocalizedControlType property must not be an empty string. | Section 508 502.3.1 ObjectInformation
LocalizedControlTypeNotNull | Error | The LocalizedControlType property must not be null. | Section 508 502.3.1 ObjectInformation
LocalizedControlTypeNotCustom | Error | The ControlType and LocalizedControlType must not both be set to "custom." | Section 508 502.3.1 ObjectInformation
LocalizedControlTypeNotCustomWPFGridCell | Error | The ControlType and LocalizedControlType must not both be set to "custom." | Section 508 502.3.1 ObjectInformation
ParentChildShouldNotHaveSameNameAndLocalizedControlType | Error | An element must not have the same Name and LocalizedControlType as its parent. | Section 508 502.3.1 ObjectInformation
SelectionPatternSelectionRequired | Error | An element of the given ControlType must have the IsSelectionRequired property set to TRUE. | Section 508 502.3.10 AvailableActions
SelectionPatternSingleSelection | Error | An element of the given ControlType must not support multiple selection. | Section 508 502.3.10 AvailableActions
SelectionItemPatternSingleSelection | Error | An element whose parent supports single selection must not have selected siblings. | Section 508 502.3.10 AvailableActions
ListItemSiblingsUnique | Warning | The Name property of sibling list items should be unique. | WCAG 4.1.2 NameRoleValue
NameExcludesPrivateUnicodeCharacters | Error | The Name property must not contain any characters in the private Unicode range. | Section 508 502.3.1 ObjectInformation
HelpTextExcludesPrivateUnicodeCharacters | Error | The HelpText property must not contain any characters in the private Unicode range. | Section 508 502.3.1 ObjectInformation
LocalizedControlTypeExcludesPrivateUnicodeCharacters | Error | The LocalizedControlType property must not contain any characters in the private Unicode range. | Section 508 502.3.1 ObjectInformation
ClickablePointOnScreen | Error | An element's IsOffScreen property must be false when its clickable point is on-screen. | Section 508 502.3.1 ObjectInformation
ClickablePointOnScreenWPF | Error | An element's IsOffScreen property must be false when its clickable point is on-screen. | Section 508 502.3.1 ObjectInformation
ClickablePointOffScreen | Warning | An element's IsOffScreen property must be true when its clickable point is off-screen. | Section 508 502.3.1 ObjectInformation
FrameworkDoesNotSupportUIAutomation | Error | The framework used to build this application does not support. | Section 508 502.3.1 ObjectInformation
EdgeBrowserHasBeenDeprecated | Error | The non-Chromium version of Microsoft Edge has been deprecated. | Section 508 502.3.1 ObjectInformation

## Severity descriptions

### Error

The given element did not meet the success criteria of the rule evaluation. The problem likely can be addressed by the developer and the impact to users is significant.

### Warning

The given element did not meet the success criteria of the rule evaluation, but the cause is known to be difficult for developers to fix (as in issues caused by the platform itself) or impact to users has been determined to be low.

### NeedsReview

The rule highlights possible accessibility issues that need to be reviewed and verified by a human.
