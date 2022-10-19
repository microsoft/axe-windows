// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Automation.Resources;
using Axe.Windows.Core.Exceptions;
using Axe.Windows.Core.Misc;
using Axe.Windows.SystemAbstractions;
using System;
using System.Globalization;
using static System.FormattableString;
using Path = System.IO.Path;

namespace Axe.Windows.Automation
{
    internal class OutputFileHelper : IOutputFileHelper
    {
        private readonly string _outputDirectory;
        private readonly ISystemDateTime _dateTime;
        private readonly ISystemIODirectory _directory;

        private string _scanId;

        public const string DefaultOutputDirectoryName = "AxeWindowsOutputFiles";
        public const string DefaultFileNameBase = "AxeWindows";

        public OutputFileHelper(string outputDirectory, ISystem system)
        {
            if (system == null) throw new ArgumentNullException(nameof(system));

            _dateTime = system.DateTime;
            if (_dateTime == null) throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, ErrorMessages.VariableNull, nameof(_dateTime)));

            var environment = system.Environment;
            if (environment == null) throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, ErrorMessages.VariableNull, nameof(environment)));

            _directory = system.IO?.Directory;
            if (_directory == null) throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, ErrorMessages.VariableNull, nameof(_directory)));

            if (string.IsNullOrWhiteSpace(outputDirectory))
            {
                _outputDirectory = Path.Combine(environment.CurrentDirectory, DefaultOutputDirectoryName);
                return;
            }

            VerifyPathOrThrow(outputDirectory);
            _outputDirectory = outputDirectory;
        }

        /// <summary>
        /// Check if the given path is a well-formed absolute path.
        /// If not, throw and <see cref="AxeWindowsAutomationException"/>.
        /// </summary>
        /// <param name="path"></param>
        /// <exception cref="AxeWindowsAutomationException"/>
        private static void VerifyPathOrThrow(string path)
        {
            try
            {
                // the following throws various exceptions if the given path is invalid
                Path.GetFullPath(path);

                if (!Path.IsPathRooted(path))
                    throw new AxeWindowsException(ErrorMessages.ErrorIsNotFullPath);
            }
            catch (Exception ex)
            {
                throw new AxeWindowsAutomationException(ErrorMessages.ErrorDirectoryInvalid.WithParameters(path), ex);
            }
        }

        /// <summary>
        /// Ensures the output directory for the file path returned by <see cref="GetNewA11yTestFilePath"/>
        /// exists.
        /// </summary>
        /// <remarks>
        /// If the directory does not exist, the method attempts to create it.
        /// This method does not catch any exceptions so that file IO errors are reported to the user.
        /// </remarks>
        public void EnsureOutputDirectoryExists()
        {
            if (!_directory.Exists(_outputDirectory))
                _directory.CreateDirectory(_outputDirectory);
        }

        public string GetNewA11yTestFilePath(Func<string, string> decorator)
        {
            Func<string, string> baseFileNameDecorator = decorator ?? ((name) => name);
            return Path.Combine(_outputDirectory,
                baseFileNameDecorator(GetBaseFileName()) + ".a11ytest");
        }

        private string GetBaseFileName()
        {
            if (!string.IsNullOrEmpty(_scanId))
                return _scanId;

            return GenerateScanId();
        }

        private string GenerateScanId()
        {
            var now = _dateTime.Now;

            var nowString = Invariant($"{now:yy-MM-dd_HH-mm-ss.fffffff}");
            return $"{DefaultFileNameBase}_{nowString}";
        }

        public void SetScanId(string scanId)
        {
            _scanId = scanId;
        }
    } // class
} // namespace
