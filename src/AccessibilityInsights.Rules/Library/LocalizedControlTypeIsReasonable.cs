// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Linq;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Types;
using Axe.Windows.Rules.Resources;
using static Axe.Windows.Rules.PropertyConditions.ControlType;
using static Axe.Windows.Rules.PropertyConditions.StringProperties;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.LocalizedControlTypeReasonable)]
    class LocalizedControlTypeIsReasonable : Rule
    {
        public LocalizedControlTypeIsReasonable()
        {
            this.Info.Description = Descriptions.LocalizedControlTypeReasonable;
            this.Info.HowToFix = HowToFix.LocalizedControlTypeReasonable;
            this.Info.Standard = A11yCriteriaId.ObjectInformation;
            this.Info.PropertyID = PropertyType.UIA_LocalizedControlTypePropertyId;
        }

        public override EvaluationCode Evaluate(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException("The element is null");

            return HasReasonableLocalizedControlType(e) ? EvaluationCode.Pass : EvaluationCode.Warning;
        }

        private bool HasReasonableLocalizedControlType(IA11yElement e)
        {
            var names = GetExpectedLocalizedControlTypeNames(e.ControlTypeId);

            if (names == null) throw new InvalidOperationException("Could not find potential LocalizedControlType string(s) for the given control type");

            return Array.Exists(names, s => String.Compare(e.LocalizedControlType, s, StringComparison.OrdinalIgnoreCase) == 0);
        }

        private string[] GetExpectedLocalizedControlTypeNames(int controlTypeId)
        {
            string names = null;

            switch(controlTypeId)
            {
                case Axe.Windows.Core.Types.ControlType.UIA_AppBarControlTypeId:
                    names = LocalizedControlTypeNames.AppBar;
                    break;
                case Axe.Windows.Core.Types.ControlType.UIA_ButtonControlTypeId:
                    names = LocalizedControlTypeNames.Button;
                    break;
                case Axe.Windows.Core.Types.ControlType.UIA_CalendarControlTypeId:
                    names = LocalizedControlTypeNames.Calendar;
                    break;
                case Axe.Windows.Core.Types.ControlType.UIA_CheckBoxControlTypeId:
                    names = LocalizedControlTypeNames.CheckBox;
                    break;
                case Axe.Windows.Core.Types.ControlType.UIA_ComboBoxControlTypeId:
                    names = LocalizedControlTypeNames.ComboBox;
                    break;
                case Axe.Windows.Core.Types.ControlType.UIA_EditControlTypeId:
                    names = LocalizedControlTypeNames.Edit;
                    break;
                case Axe.Windows.Core.Types.ControlType.UIA_HyperlinkControlTypeId:
                    names = LocalizedControlTypeNames.Hyperlink;
                    break;
                case Axe.Windows.Core.Types.ControlType.UIA_ImageControlTypeId:
                    names = LocalizedControlTypeNames.Image;
                    break;
                case Axe.Windows.Core.Types.ControlType.UIA_ListItemControlTypeId:
                    names = LocalizedControlTypeNames.ListItem;
                    break;
                case Axe.Windows.Core.Types.ControlType.UIA_ListControlTypeId:
                    names = LocalizedControlTypeNames.List;
                    break;
                case Axe.Windows.Core.Types.ControlType.UIA_MenuControlTypeId:
                    names = LocalizedControlTypeNames.Menu;
                    break;
                case Axe.Windows.Core.Types.ControlType.UIA_MenuBarControlTypeId:
                    names = LocalizedControlTypeNames.MenuBar;
                    break;
                case Axe.Windows.Core.Types.ControlType.UIA_MenuItemControlTypeId:
                    names = LocalizedControlTypeNames.MenuItem;
                    break;
                case Axe.Windows.Core.Types.ControlType.UIA_ProgressBarControlTypeId:
                    names = LocalizedControlTypeNames.ProgressBar;
                    break;
                case Axe.Windows.Core.Types.ControlType.UIA_RadioButtonControlTypeId:
                    names = LocalizedControlTypeNames.RadioButton;
                    break;
                case Axe.Windows.Core.Types.ControlType.UIA_ScrollBarControlTypeId:
                    names = LocalizedControlTypeNames.ScrollBar;
                    break;
                case Axe.Windows.Core.Types.ControlType.UIA_SliderControlTypeId:
                    names = LocalizedControlTypeNames.Slider;
                    break;
                case Axe.Windows.Core.Types.ControlType.UIA_SpinnerControlTypeId:
                    names = LocalizedControlTypeNames.Spinner;
                    break;
                case Axe.Windows.Core.Types.ControlType.UIA_StatusBarControlTypeId:
                    names = LocalizedControlTypeNames.StatusBar;
                    break;
                case Axe.Windows.Core.Types.ControlType.UIA_TabControlTypeId:
                    names = LocalizedControlTypeNames.Tab;
                    break;
                case Axe.Windows.Core.Types.ControlType.UIA_TabItemControlTypeId:
                    names = LocalizedControlTypeNames.TabItem;
                    break;
                case Axe.Windows.Core.Types.ControlType.UIA_TextControlTypeId:
                    names = LocalizedControlTypeNames.Text;
                    break;
                case Axe.Windows.Core.Types.ControlType.UIA_ToolBarControlTypeId:
                    names = LocalizedControlTypeNames.ToolBar;
                    break;
                case Axe.Windows.Core.Types.ControlType.UIA_ToolTipControlTypeId:
                    names = LocalizedControlTypeNames.ToolTip;
                    break;
                case Axe.Windows.Core.Types.ControlType.UIA_TreeControlTypeId:
                    names = LocalizedControlTypeNames.Tree;
                    break;
                case Axe.Windows.Core.Types.ControlType.UIA_TreeItemControlTypeId:
                    names = LocalizedControlTypeNames.TreeItem;
                    break;
                case Axe.Windows.Core.Types.ControlType.UIA_GroupControlTypeId:
                    names = LocalizedControlTypeNames.Group;
                    break;
                case Axe.Windows.Core.Types.ControlType.UIA_ThumbControlTypeId:
                    names = LocalizedControlTypeNames.Thumb;
                    break;
                case Axe.Windows.Core.Types.ControlType.UIA_DataGridControlTypeId:
                    names = LocalizedControlTypeNames.DataGrid;
                    break;
                case Axe.Windows.Core.Types.ControlType.UIA_DataItemControlTypeId:
                    names = LocalizedControlTypeNames.DataItem;
                    break;
                case Axe.Windows.Core.Types.ControlType.UIA_DocumentControlTypeId:
                    names = LocalizedControlTypeNames.Document;
                    break;
                case Axe.Windows.Core.Types.ControlType.UIA_SplitButtonControlTypeId:
                    names = LocalizedControlTypeNames.SplitButton;
                    break;
                case Axe.Windows.Core.Types.ControlType.UIA_WindowControlTypeId:
                    names = LocalizedControlTypeNames.Window;
                    break;
                case Axe.Windows.Core.Types.ControlType.UIA_PaneControlTypeId:
                    names = LocalizedControlTypeNames.Pane;
                    break;
                case Axe.Windows.Core.Types.ControlType.UIA_HeaderControlTypeId:
                    names = LocalizedControlTypeNames.Header;
                    break;
                case Axe.Windows.Core.Types.ControlType.UIA_HeaderItemControlTypeId:
                    names = LocalizedControlTypeNames.HeaderItem;
                    break;
                case Axe.Windows.Core.Types.ControlType.UIA_TableControlTypeId:
                    names = LocalizedControlTypeNames.Table;
                    break;
                case Axe.Windows.Core.Types.ControlType.UIA_TitleBarControlTypeId:
                    names = LocalizedControlTypeNames.TitleBar;
                    break;
                case Axe.Windows.Core.Types.ControlType.UIA_SeparatorControlTypeId:
                    names = LocalizedControlTypeNames.Separator;
                    break;
                case Axe.Windows.Core.Types.ControlType.UIA_SemanticZoomControlTypeId:
                    names = LocalizedControlTypeNames.SemanticZoom;
                    break;
                default:
                    break;
            }

            return names == null ? null : names.Split(',');
        }

        protected override Condition CreateCondition()
        {
            return ~Custom & LocalizedControlType.NotNullOrEmpty & LocalizedControlType.NotWhiteSpace;
        }
    } // class
} // namespace
