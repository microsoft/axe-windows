// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Actions.Attributes;
using Axe.Windows.Actions.Contexts;
using Axe.Windows.Actions.Enums;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Misc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Axe.Windows.Actions
{
    /// <summary>
    /// Manager for actions
    /// it handles all internal data
    /// - ElementContext
    /// - DataContext
    /// - Element
    /// - Property
    /// - Pattern
    /// - etc
    /// When Actions needs data(object) to do anything(execute/select and so on), they should get data from Actions.
    /// the Action caller will pass the ID of data to use than actual object.
    /// </summary>
    [InteractionLevel(UxInteractionLevel.NoUxInteraction)]
    public class DataManager : IDisposable
    {
        /// <summary>
        /// ElementContext dictionary
        /// keep the record of all element context added by AddElementContext
        /// </summary>
        readonly Dictionary<Guid, ElementContext> _elementContexts = new Dictionary<Guid, ElementContext>();

        /// <summary>
        /// Get A11yPattern from an indicated element/elementcontext
        /// </summary>
        /// <param name="ecId">Element Context Id</param>
        /// <param name="eId">Element Id</param>
        /// <param name="ptId">Pattern Id</param>
        /// <returns></returns>
        internal A11yPattern GetA11yPattern(Guid ecId, int eId, int ptId)
        {
            return GetA11yElement(ecId, eId).Patterns.ById(ptId);
        }

        /// <summary>
        /// Set element Context
        /// </summary>
        /// <param name="ec"></param>
        internal void AddElementContext(ElementContext ec)
        {
            if (ec != null && _elementContexts.ContainsKey(ec.Id) == false)
            {
                _elementContexts.Add(ec.Id, ec);
            }
        }

        /// <summary>
        /// Get ElementContext
        /// </summary>
        /// <param name="ecId"></param>
        /// <returns></returns>
        internal ElementContext GetElementContext(Guid ecId)
        {
            if (_elementContexts.TryGetValue(ecId, out ElementContext ec))
            {
                return ec;
            }
            return null;
        }

        /// <summary>
        /// Remove Element Context and Dispose
        /// </summary>
        /// <param name="ecId">ElementContext Id</param>
        internal void RemoveElementContext(Guid ecId)
        {
            ElementContext ec = GetElementContext(ecId);
            if (ec != null)
            {
                _elementContexts.Remove(ecId);
                ec.Dispose();
            }
        }

        /// <summary>
        /// Remove DataContext from an ElementContext
        /// </summary>
        /// <param name="ecId">ElementContext Id</param>
        /// <param name="keepMainElement">true, keep it</param>
        internal void RemoveDataContext(Guid ecId, bool keepMainElement = true)
        {
            // check whether key exists. if not, just silently ignore.
            ElementContext ec = GetElementContext(ecId);
            if (ec?.DataContext != null)
            {
                if (keepMainElement)
                {
                    // make sure that selected element is not disposed by removing it from the list in DataContext
                    ec.DataContext.Elements.Remove(ec.Element.UniqueId);
                }

                ec.DataContext.Dispose();
                ec.DataContext = null;
            }
        }

        /// <summary>
        /// Get Element by Id
        /// </summary>
        /// <param name="ecId">ElementContext Id</param>
        /// <param name="eId">Element Id</param>
        /// <returns></returns>
        public A11yElement GetA11yElement(Guid ecId, int eId)
        {
            ElementContext ec = GetElementContext(ecId);
            if (ec != null)
            {
                if (eId == 0)
                {
                    return ec.Element;
                }
                else
                {
                    var es = ec.DataContext.Elements;
                    if (es.TryGetValue(eId, out A11yElement element))
                    {
                        return element;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the element on which the screenshot was taken.
        /// </summary>
        /// <param name="ecId"></param>
        /// <returns></returns>
        public A11yElement GetScreenshotElement(Guid ecId)
        {
            var elementContext = GetElementContext(ecId);
            if (elementContext == null) return null;
            if (elementContext.DataContext == null) return null;

            return GetA11yElement(ecId, elementContext.DataContext.ScreenshotElementId);
        }

        /// <summary>
        /// Gets the most recent screenshot
        /// </summary>
        /// <param name="ecId"></param>
        /// <returns></returns>
        public Bitmap GetScreenshot(Guid ecId)
        {
            var elementContext = GetElementContext(ecId);
            if (elementContext == null) return null;
            if (elementContext.DataContext == null) return null;

            return elementContext.DataContext.Screenshot;
        }

        #region static members

        static DataManager DefaultInstance;
        static readonly object LockObject = new object();

        /// <summary>
        /// Get default Data Manager instance
        /// if it doesn't exist, create it.
        /// </summary>
        public static DataManager GetDefaultInstance()
        {
            if (DefaultInstance == null)
            {
                lock (LockObject)
                {
#pragma warning disable CA1508 // Analyzer doesn't understand check/lock/check pattern
                    if (DefaultInstance == null)
                    {
                        DefaultInstance = CreateInstance();
                    }
#pragma warning restore CA1508 // Analyzer doesn't understand check/lock/check pattern
                }
            }

            return DefaultInstance;
        }

        /// <summary>
        /// Clear the default instance
        /// </summary>
        public static void ClearDefaultInstance()
        {
            if (DefaultInstance != null)
            {
                lock (LockObject)
                {
#pragma warning disable CA1508 // Analyzer doesn't understand check/lock/check pattern
                    if (DefaultInstance != null)
                    {
                        DefaultInstance.Dispose();
                        DefaultInstance = null;
                    }
#pragma warning restore CA1508 // Analyzer doesn't understand check/lock/check pattern
                }
            }
        }

        public static DataManager CreateInstance()
        {
            return new DataManager();
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
                    // We need an immutable copy of the ecId values for cleanup
                    var ecIdList = _elementContexts.Keys.ToList();
                    foreach (Guid ecId in ecIdList)
                    {
                        RemoveDataContext(ecId, false);
                    }

                    _elementContexts.Clear();
                }

                _disposedValue = true;
            }
        }

        ~DataManager()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
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
