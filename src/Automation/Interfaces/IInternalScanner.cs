﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using System;

namespace Axe.Windows.Automation
{
    delegate T InternalScannerCallback<T>(A11yElement element, Guid elementId);

    /// <summary>
    /// The Interface representing the boundary between this Automation library
    /// and the other parts (in other projects) of AxeWindows which perform the actual scan.
    /// The code behind this interface should be unit tested as part of the projects where it is implemented,
    /// not as part of this (Automation) project.
    /// </summary>
    internal interface IInternalScanner
    {
        /// <summary>
        /// Runs a scan and calls a callback which can transform the results
        /// into another form and/or save output files
        /// </summary>
        /// <typeparam name="T">The type of results object to be returned by the callback</typeparam>
        /// <param name="element">The element from which to start the scan</param>
        /// <param name="resultsCallback">A delegate which can act on results and transform them into a specified type</param>
        /// <returns></returns>
        T Scan<T>(A11yElement element, InternalScannerCallback<T> resultsCallback);

        /// <summary>
        /// Takes a screenshot, highlighting the given element
        /// </summary>
        /// <param name="elementId"></param>
        void CaptureScreenshot(Guid elementId);

        /// <summary>
        /// Saves an a11ytest file to the given path
        /// </summary>
        /// <param name="path"></param>
        /// <param name="element"></param>
        /// <param name="elementId"></param>
        void SaveA11yTestFile(string path, A11yElement element, Guid elementId);
    } // interface
} // namespace
