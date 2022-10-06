// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Actions.Attributes;
using Axe.Windows.Actions.Contexts;
using Axe.Windows.Actions.Enums;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Misc;
using Axe.Windows.Desktop.UIAutomation;
using System;

namespace Axe.Windows.Actions
{
    /// <summary>
    /// GetDataAction class
    /// Get Data from DataManager
    /// </summary>
    [InteractionLevel(UxInteractionLevel.NoUxInteraction)]
    public static class GetDataAction
    {
        /// <summary>
        /// Get element Context from Action DataManager
        /// </summary>
        /// <param name="ecId"></param>
        /// <returns></returns>
        public static ElementContext GetElementContext(Guid ecId)
        {
            return GetElementContext(ecId, DefaultActionContext.GetDefaultInstance());
        }

        internal static ElementContext GetElementContext(Guid ecId, IActionContext actionContext)
        {
            return actionContext.DataManager.GetElementContext(ecId);
        }

        /// <summary>
        /// Checking whether there is EC with the indicated Id
        /// </summary>
        /// <param name="ecId"></param>
        /// <returns></returns>
        public static bool ExistElementContext(Guid ecId)
        {
            return ExistElementContext(ecId, DefaultActionContext.GetDefaultInstance());
        }

        internal static bool ExistElementContext(Guid ecId, IActionContext actionContext)
        {
            return actionContext.DataManager.GetElementContext(ecId) != null;
        }

        /// <summary>
        /// Get the Id of Selected element of ElementContext(Id)
        /// </summary>
        /// <param name="ecId">Element context Id</param>
        /// <returns></returns>
        public static int GetSelectedElementId(Guid ecId)
        {
            return GetSelectedElementId(ecId, DefaultActionContext.GetDefaultInstance());
        }

        internal static int GetSelectedElementId(Guid ecId, IActionContext actionContext)
        {
            return actionContext.DataManager.GetElementContext(ecId).Element.UniqueId;
        }

        /// <summary>
        /// Get data context of ElementContext
        /// </summary>
        /// <param name="ecId"></param>
        /// <returns></returns>
        public static ElementDataContext GetElementDataContext(Guid ecId)
        {
            return GetElementDataContext(ecId, DefaultActionContext.GetDefaultInstance());
        }

        internal static ElementDataContext GetElementDataContext(Guid ecId, IActionContext actionContext)
        {
            return actionContext.DataManager.GetElementContext(ecId)?.DataContext;
        }

        /// <summary>
        /// Get Instance of A11yElement with live data
        /// for now, it updates data in input A11yElement and return it.
        /// ToDo: it will be changed along with ActionDataManager code.
        /// </summary>
        /// <param name="ecId">ElementContext Id</param>
        /// <param name="eId">Element Id</param>
        /// <returns></returns>
        public static A11yElement GetA11yElementWithLiveData(Guid ecId, int eId)
        {
            return GetA11yElementWithLiveData(ecId, eId, DefaultActionContext.GetDefaultInstance());
        }

        internal static A11yElement GetA11yElementWithLiveData(Guid ecId, int eId, IActionContext actionContext)
        {
            var e = actionContext.DataManager.GetA11yElement(ecId, eId);

            e?.PopulateAllPropertiesWithLiveData(actionContext.Registrar);

            return e;
        }

        /// <summary>
        /// Get Instance of A11yElement with data in Data context
        /// </summary>
        /// <param name="ecId">ElementContext Id</param>
        /// <param name="eId">Element Id</param>
        /// <returns></returns>
        public static A11yElement GetA11yElementInDataContext(Guid ecId, int eId)
        {
            return GetA11yElementInDataContext(ecId, eId, DefaultActionContext.GetDefaultInstance());
        }

        internal static A11yElement GetA11yElementInDataContext(Guid ecId, int eId, IActionContext actionContext)
        {
            return actionContext.DataManager.GetA11yElement(ecId, eId);
        }

        /// <summary>
        /// Get Process name and Ui Framework of Element Context in the default context
        /// </summary>
        /// <param name="ecId"></param>
        /// <returns></returns>
        public static Tuple<string, string> GetProcessAndUIFrameworkOfElementContext(Guid ecId)
        {
            return GetProcessAndUIFrameworkOfElementContext(ecId, DefaultActionContext.GetDefaultInstance());
        }

        /// <summary>
        /// Get Process name and Ui Framework of Element Context in the specified context
        /// </summary>
        internal static Tuple<string, string> GetProcessAndUIFrameworkOfElementContext(Guid ecId, IActionContext actionContext)
        {
            var ec = actionContext.DataManager.GetElementContext(ecId);

            return new Tuple<string, string>(ec.ProcessName, ec.Element.GetUIFramework());
        }

        /// <summary>
        /// Get the DataContext Mode of currently selected ElementContext
        /// </summary>
        /// <returns></returns>
        public static DataContextMode GetDataContextMode()
        {
            return GetDataContextMode(DefaultActionContext.GetDefaultInstance());
        }

        internal static DataContextMode GetDataContextMode(IActionContext actionContext)
        {
            var ec = actionContext.DataManager.GetElementContext(actionContext.SelectAction.SelectedElementContextId.Value);

            return ec.DataContext != null ? ec.DataContext.Mode : DataContextMode.Live;
        }
    }
}
