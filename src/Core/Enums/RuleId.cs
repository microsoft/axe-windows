// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Axe.Windows.Core.Enums
{
    /// <summary>
    /// Strong rule ids. Each rule must have a unique ID.
    /// Unneeded RuleId values should be removed from this enum when the rule is removed.
    /// </summary>
    public enum RuleId
    {
        // this value will be used if the RuleResult.Rule is loaded from disk
        Indecisive = 0,

        BoundingRectangleNotAllZeros,
        BoundingRectangleNotNull,
        BoundingRectangleNotValidButOffScreen,
        BoundingRectangleDataFormatCorrect,
        BoundingRectangleCompletelyObscuresContainer,
        BoundingRectangleContainedInParent,
        BoundingRectangleSizeReasonable,

        // 2 special case rules for known framework issues
        BoundingRectangleNotNullListViewXAML,
        BoundingRectangleNotNullTextBlockXAML,

        SplitButtonInvokeAndTogglePatterns,
        ButtonShouldHavePatterns, // check whether button has at least one of three patterns(Invoke,Toggle,ExpandCollapse)
        ButtonInvokeAndTogglePatterns, // Button should not have Invoke and Toggle patterns together.
        ButtonInvokeAndExpandCollapsePatterns, // Button may have Invoke and ExpandCollapse patterns together. (warning)
        ButtonToggleAndExpandCollapsePatterns, // Button should have have Toggle and ExpandCollapse patterns together.

        SiblingUniqueAndFocusable,
        SiblingUniqueAndNotFocusable,

        ChildrenNotAllowedInContentView,

        ContentViewButtonStructure,
        ContentViewCalendarStructure,
        ContentViewCheckBoxStructure,
        ContentViewComboBoxStructure,
        ContentViewDataGridStructure,
        ContentViewEditStructure,
        ContentViewHyperlinkStructure,
        ContentViewListStructure,
        ContentViewListItemStructure,
        ContentViewMenuStructure,
        ContentViewProgressBarStructure,
        ContentViewRadioButtonStructure,
        ContentViewSliderStructure,
        ContentViewSpinnerStructure,
        ContentViewSplitButtonStructure,
        ContentViewStatusBarStructure,
        ContentViewTabStructure,
        ContentViewTreeStructure,
        ContentViewTreeItemStructure,
        ControlViewButtonStructure,
        ControlViewCalendarStructure,
        ControlViewComboBoxStructure,
        ControlViewCheckBoxStructure,
        ControlViewDataGridStructure,
        ControlViewEditStructure,
        ControlViewHeaderStructure,
        ControlViewHeaderItemStructure,
        ControlViewHyperlinkStructure,
        ControlViewImageStructure,
        ControlViewListStructure,
        ControlViewListItemStructure,
        ControlViewMenuStructure,
        ControlViewProgressBarStructure,
        ControlViewRadioButtonStructure,
        ControlViewScrollbarStructure,
        ControlViewSemanticZoomStructure,
        ControlViewSeparatorStructure,
        ControlViewSliderStructure,
        ControlViewSpinnerStructure,
        ControlViewSplitButtonStructure,
        ControlViewStatusBarStructure,
        ControlViewTabStructure,
        ControlViewThumbStructure,
        ControlViewToolTipStructure,
        ControlViewTreeStructure,
        ControlViewTreeItemStructure,

        ComboBoxShouldNotSupportScrollPattern,
        ControlShouldNotSupportInvokePattern,
        ControlShouldNotSupportScrollPattern,
        ControlShouldNotSupportValuePattern,
        ControlShouldNotSupportWindowPattern,
        ControlShouldSupportExpandCollapsePattern,
        ControlShouldSupportGridItemPattern,
        ControlShouldSupportGridPattern,
        ControlShouldSupportInvokePattern,
        ControlShouldSupportScrollItemPattern,
        ControlShouldSupportSelectionItemPattern,
        ControlShouldSupportSelectionPattern,
        ControlShouldSupportSetInfoWPF,
        ControlShouldSupportSetInfoXAML,
        ControlShouldSupportSpreadsheetItemPattern,
        ControlShouldSupportTableItemPattern,
        ControlShouldSupportTablePattern,
        ControlShouldSupportTogglePattern,
        ControlShouldSupportTransformPattern,
        ControlShouldSupportTextPattern,

        // special case rule for known framework issue
        ControlShouldSupportTextPatternEditWinform,

        EditSupportsIncorrectRangeValuePattern,

        HeadingLevelDescendsWhenNested,

        LandmarkBannerIsTopLevel,
        LandmarkComplementaryIsTopLevel,
        LandmarkContentInfoIsTopLevel,
        LandmarkMainIsTopLevel,
        LandmarkNoDuplicateBanner,
        LandmarkNoDuplicateContentInfo,

        LocalizedLandmarkTypeExcludesPrivateUnicodeCharacters,
        LocalizedLandmarkTypeIsReasonableLength,
        LocalizedLandmarkTypeNotCustom,
        LocalizedLandmarkTypeNotEmpty,
        LocalizedLandmarkTypeNotNull,
        LocalizedLandmarkTypeNotWhiteSpace,

        PatternsSupportedByControlType,

        HelpTextNotEqualToName,

        HyperlinkNameShouldBeUnique,

        IsControlElementPropertyExists,

        IsContentElementPropertyExists,
        IsContentElementFalseOptional,
        IsContentElementTrueOptional,
        IsControlElementTrueOptional,
        IsControlElementTrueRequired,

        IsKeyboardFocusableShouldBeTrue,
        IsKeyboardFocusableFalseButDisabled,
        IsKeyboardFocusableForListItemShouldBeTrue,
        IsKeyboardFocusableFalseButOffscreen,
        IsKeyboardFocusableForCustomShouldBeTrue,
        IsKeyboardFocusableDescendantTextPattern,
        IsKeyboardFocusableOnEmptyContainer,
        IsKeyboardFocusableShouldBeFalse,
        IsKeyboardFocusableTopLevelTextPattern,

        ItemTypeRecommended,

        LocalizedControlTypeReasonable,

        NameNotEmpty,
        NameExcludesControlType,
        NameExcludesLocalizedControlType,
        NameReasonableLength,

        OrientationPropertyExists,
        ProgressBarRangeValue,

        ItemStatusExists,

        // given by ExtensionMethods.GetTreeStructureRule(TreeViewModes)
        TypicalTreeStructureRaw,
        TypicalTreeStructureControl,
        TypicalTreeStructureContent,

        NameNotNull,
        NameNotWhiteSpace,
        NameNullButElementNotKeyboardFocusable,
        NameEmptyButElementNotKeyboardFocusable,
        NameWithValidBoundingRectangle,
        NameOnOptionalType,
        NameNoSiblingsOfSameType,
        NameOnCustomWithParentWPFDataItem,
        NameIsInformative,

        LocalizedControlTypeNotWhiteSpace,
        LocalizedControlTypeNotEmpty,
        LocalizedControlTypeNotNull,
        LocalizedControlTypeNotCustom,
        LocalizedControlTypeNotCustomWPFGridCell,

        ParentChildShouldNotHaveSameNameAndLocalizedControlType,

        SelectionPatternSelectionRequired,
        SelectionPatternSingleSelection,
        SelectionItemPatternSingleSelection,

        ListItemSiblingsUnique,
        NameExcludesPrivateUnicodeCharacters,
        HelpTextExcludesPrivateUnicodeCharacters,
        LocalizedControlTypeExcludesPrivateUnicodeCharacters,

        ClickablePointOnScreen,
        ClickablePointOffScreen,

        FrameworkDoesNotSupportUIAutomation,
        EdgeBrowserHasBeenDeprecated,
    }
}
