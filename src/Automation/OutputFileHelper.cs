// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Automation.Resources;
using Axe.Windows.SystemAbstractions;
using System;
using System.Globalization;
using Path = System.IO.Path;

using static System.FormattableString;

namespace Axe.Windows.Automation
{
    internal class OutputFileHelper : IOutputFileHelper
    {
        private readonly string _outputDirectory;
        private readonly ISystemDateTime _dateTime;

        private string _outputFileNameWithoutExtension = null;

        public const string DefaultOutputDirectoryName = "AxeWindowsOutputFiles";
        public const string DefaultFileNameBase = "AxeWindows";

        public OutputFileHelper(string outputDirectory, ISystem system)
        {
            if (system == null) throw new ArgumentNullException(nameof(system));

            _dateTime = system.DateTime;
            if (_dateTime == null) throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, ErrorMessages.VariableNull, nameof(_dateTime)));

            var environment = system.Environment;
            if (environment == null) throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, ErrorMessages.VariableNull, nameof(environment)));

            var directory = system.IO.Directory;
            if (directory == null) throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, ErrorMessages.VariableNull, nameof(directory)));

            if (!string.IsNullOrWhiteSpace(outputDirectory))
            {
                VerifyPathOrThrow(outputDirectory);
                _outputDirectory = outputDirectory;
            }
            else
                _outputDirectory = Path.Combine(environment.CurrentDirectory, DefaultOutputDirectoryName);

            if (!directory.Exists(_outputDirectory))
                directory.CreateDirectory(_outputDirectory);
        }

        /// <summary>
        /// Check if the given path is a well-formed absolute path.
        /// If not, throw and <see cref="AxeWindowsAutomationException"/>.
        /// </summary>
        /// <param name="path"></param>
        /// <exception cref="AxeWindowsAutomationException"/>
        private void VerifyPathOrThrow(string path)
        {
            try
            {
                // the following throws various exceptions if the given path is invalid
                Path.GetFullPath(path);

                if (!Path.IsPathRooted(path))
                    throw new Exception(DisplayStrings.ErrorIsNotFullPath);
            }
            catch (Exception ex)
            {
                throw new AxeWindowsAutomationException(string.Format(CultureInfo.InvariantCulture, DisplayStrings.ErrorDirectoryInvalid, path), ex);
            }
        }

        public string GetNewA11yTestFilePath()
        {
            return Path.Combine(_outputDirectory
                , GetOutputFileBaseName()
                + ".a11ytest");
        }

        private string GetOutputFileBaseName()
        {
            if (!string.IsNullOrEmpty(_outputFileNameWithoutExtension))
                return _outputFileNameWithoutExtension;

            return GetNewFileBaseName();
        }

        private string GetNewFileBaseName()
        {
            var now = _dateTime.Now;

            var nowString = Invariant($"{now:yy-MM-dd_HH-mm-ss.fffffff}");
            return $"{DefaultFileNameBase}_{nowString}";
        }

        public void SetOutputFileNameWithoutExtension(string outputFileNameWithoutExtension)
        {
            _outputFileNameWithoutExtension = outputFileNameWithoutExtension;
        }
    } // class
} // namespace
