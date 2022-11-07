// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Actions.Attributes;
using Axe.Windows.Actions.Contexts;
using Axe.Windows.Actions.Enums;
using Axe.Windows.Actions.Trackers;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Misc;
using Axe.Windows.Desktop.Settings;
using Axe.Windows.Desktop.UIAutomation;
using System;
using System.Linq;

namespace Axe.Windows.Actions
{
    /// <summary>
    /// Class SelectionAction
    /// this class is to select unelement via focus or keyboard
    /// </summary>
    [InteractionLevel(UxInteractionLevel.NoUxInteraction)]
    public class SelectAction : IDisposable
    {
        /// <summary>
        /// UIATree state
        /// </summary>
        UIATreeState _uiaTreeState = UIATreeState.Resumed;

        /// <summary>
        /// Selector by Focus
        /// </summary>
        FocusTracker _focusTracker;

        /// <summary>
        /// On/Off Focus Select
        /// </summary>
        public bool IsFocusSelectOn { get; set; }

        private DataManager DataManager { get; }

        /// <summary>
        /// Mouse Selector Delay in Milliseconds
        /// </summary>
        public double IntervalMouseSelector
        {
            get
            {
                return _mouseTracker.IntervalMouseSelector;
            }

            set
            {
                _mouseTracker.IntervalMouseSelector = value;
            }
        } // default for the case

        /// <summary>
        /// Selector by Mouse Hover
        /// </summary>
        MouseTracker _mouseTracker;

        /// <summary>
        /// On/Off Mouse Select
        /// </summary>
        public bool IsMouseSelectOn { get; set; }

        public TreeTracker TreeTracker { get; }

        // Backing object for POIElementContext - is disposed via POIElementContext property
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "_POIElementContext")]
        ElementContext _polElementContext = null;
        private readonly object _elementContextLock = new object();

        /// <summary>
        /// Element Context of Point of Interest(a.k.a. selected)
        /// </summary>
        public ElementContext POIElementContext
        {
            get
            {
                return _polElementContext;
            }

            private set
            {
                var dma = DataManager;
                if (_polElementContext != null)
                {
                    dma.RemoveElementContext(_polElementContext.Id);
                    _polElementContext.Dispose();
                }

                _polElementContext = value;
                dma.AddElementContext(value);
            }
        }

        /// <summary>
        /// context which is not selected yet. but sent by tracker
        /// </summary>
        ElementContext _candidateEC;

        /// <summary>
        /// Set the scope of Selection
        /// </summary>
        public SelectionScope Scope
        {
            get
            {
                return _mouseTracker.Scope;
            }

            set
            {
                _focusTracker.Scope = value;
                _mouseTracker.Scope = value;
            }
        }

        /// <summary>
        /// private Constructor
        /// </summary>
        private SelectAction(DataManager dataManager)
        {
            DataManager = dataManager ?? throw new ArgumentNullException(nameof(dataManager));
            _focusTracker = new FocusTracker(SetCandidateElement);
            _mouseTracker = new MouseTracker(SetCandidateElement);
            TreeTracker = new TreeTracker(SetCandidateElement, this);
        }

        /// <summary>
        /// Stop/Pause selection
        /// </summary>
        public void Stop()
        {
            if (!IsPaused)
            {
                _focusTracker?.Stop();
                _mouseTracker?.Stop();
            }
        }

        /// <summary>
        /// Start/Resume selection
        /// </summary>
        public void Start()
        {
            if (!IsPaused)
            {
                if (IsFocusSelectOn)
                {
                    _focusTracker?.Start();
                }

                if (IsMouseSelectOn)
                {
                    _mouseTracker?.Start();
                }
            }
        }

        /// <summary>
        /// Pause UIA Tree in live mode
        /// </summary>
        public void PauseUIATreeTracker()
        {
            Stop();
            _uiaTreeState = UIATreeState.Paused;
        }

        /// <summary>
        /// Resume UIA Tree
        /// </summary>
        public void ResumeUIATreeTracker()
        {
            _uiaTreeState = UIATreeState.Resumed;
            Start();
        }

        /// <summary>
        /// Set element for Next selection call
        /// it is for internal use.
        /// </summary>
        /// <param name="el"></param>
        public void SetCandidateElement(A11yElement el)
        {
            lock (_elementContextLock)
            {
                if (el != null)
                {
                    if (el.IsRootElement() == false)
                    {
                        _candidateEC?.Dispose();
                        _candidateEC = new ElementContext(el);
                    }
                    else
                    {
                        el.Dispose();
                    }
                }
            }
        }

        /// <summary>
        ///return true if the UIA Tree is paused
        /// </summary>
        public bool IsPaused
        {
            get => _uiaTreeState == UIATreeState.Paused;
        }

        /// <summary>
        /// Set element for Next selection call
        /// ElementContext will be created with the clone of the selected element.
        /// </summary>
        /// <param name="ecId"></param>
        /// <param name="eId"></param>
        public void SetCandidateElement(Guid ecId, int eId)
        {
            SetCandidateElement(ecId, eId, DefaultActionContext.GetDefaultInstance());
        }

        internal void SetCandidateElement(Guid ecId, int eId, IActionContext actionContext)
        {
            var el = DataManager.GetA11yElement(ecId, eId).CloneForSelection(actionContext.DesktopDataContext);

            SetCandidateElement(el);
        }

        /// <summary>
        /// Select the element and return success(true)/failure(false)
        /// </summary>
        /// <returns>true when there is new selection</returns>
        public bool Select()
        {
            lock (_elementContextLock)
            {
                if (_candidateEC != null && (POIElementContext == null || POIElementContext.Element.IsSameUIElement(_candidateEC.Element) == false))
                {
                    POIElementContext = _candidateEC;
                    _candidateEC = null;

                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Select Loaded Data
        /// Load data from path and use the loaded data to set the selected element context.
        /// </summary>
        /// <param name="path"></param>
        /// <returns>Tuple of ElementContextId and SnapshotMetaInfo</returns>
        public Tuple<Guid, SnapshotMetaInfo> SelectLoadedData(string path, int? selectedElementId = null)
        {
            return SelectLoadedData(path, DefaultActionContext.GetDefaultInstance(), selectedElementId);
        }

        internal Tuple<Guid, SnapshotMetaInfo> SelectLoadedData(string path, IActionContext actionContext, int? selectedElementId = null)
        {
            ClearSelectedContext();

            var parts = LoadAction.LoadSnapshotZip(path, actionContext);
            var meta = parts.MetaInfo;
            var ec = new ElementContext(parts.Element.FindPOIElementFromLoadedData());

            SetSelectedContextWithLoadedData(ec);
            CaptureAction.SetTestModeDataContext(ec.Id, DataContextMode.Load, ec.Element.TreeWalkerMode);

            ec.DataContext.Screenshot = parts.Bmp ?? parts.SynthesizedBmp;
            ec.DataContext.ScreenshotElementId = meta.ScreenshotElementId;
            ec.DataContext.FocusedElementUniqueId = selectedElementId ?? (meta.SelectedItems?.First());

            return new Tuple<Guid, SnapshotMetaInfo>(ec.Id, meta);
        }

        /// <summary>
        /// Set Selected context with loaded snapshot data
        /// </summary>
        /// <param name="ec"></param>
        void SetSelectedContextWithLoadedData(ElementContext ec)
        {
            lock (_elementContextLock)
            {
                POIElementContext?.Dispose();
                _candidateEC?.Dispose();
                _candidateEC = null;
                POIElementContext = ec;
            }
        }

        /// <summary>
        /// Clean up selected Context
        /// </summary>
        public void ClearSelectedContext()
        {
            if (!IsPaused)
            {
                lock (_elementContextLock)
                {
                    POIElementContext = null;
                    _candidateEC?.Dispose();
                    _candidateEC = null;

                    _mouseTracker.Clear();
                    _focusTracker.Clear();
                }
            }
        }

        /// <summary>
        /// The selected ElementContextId (null if nothing is selected).
        /// </summary>
        public Guid? SelectedElementContextId
        {
            get
            {
                lock (_elementContextLock)
                {
                    return POIElementContext != null ? POIElementContext.Id : (Guid?)null;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public bool HasPOIElement()
        {
            return POIElementContext != null;
        }

        public TreeViewMode TreeViewMode
        {
            get
            {
                return (TreeTracker != null)
                    ? TreeTracker.TreeViewMode
                    : TreeViewMode.Raw;
            }

            set
            {
                if (_mouseTracker != null)
                    _mouseTracker.TreeViewMode = value;

                if (TreeTracker != null)
                    TreeTracker.TreeViewMode = value;
            }
        }

        #region static methods
        /// <summary>
        /// default instance
        /// </summary>
        private static SelectAction DefaultInstance;

#pragma warning disable CA1024 // Use properties where appropriate
        /// <summary>
        /// Get the default instance of SelectAction
        /// </summary>
        /// <returns></returns>
        public static SelectAction GetDefaultInstance()
        {
            if (DefaultInstance == null)
            {
                DefaultInstance = new SelectAction(DataManager.GetDefaultInstance());
            }

            return DefaultInstance;
        }
#pragma warning restore CA1024 // Use properties where appropriate

        public static SelectAction CreateInstance(DataManager dataManager)
        {
            return new SelectAction(dataManager);
        }

        /// <summary>
        /// Clear default Instance.
        /// </summary>
        public static void ClearDefaultInstance()
        {
            if (DefaultInstance != null)
            {
                DefaultInstance.Dispose();
                DefaultInstance = null;
            }
        }
        #endregion

        #region IDisposable Support
        private bool _disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    if (POIElementContext != null)
                    {
                        POIElementContext.Dispose();
                        POIElementContext = null;
                    }
                    if (_candidateEC != null)
                    {
                        _candidateEC.Dispose();
                        _candidateEC = null;
                    }
                    if (_mouseTracker != null)
                    {
                        _mouseTracker.Dispose();
                        _mouseTracker = null;
                    }
                    if (_focusTracker != null)
                    {
                        _focusTracker.Stop();
                        _focusTracker.Dispose();
                        _focusTracker = null;
                    }
                }

                _disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
