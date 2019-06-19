// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Abstractions;
using System;
using Path = System.IO.Path;

namespace Axe.Windows.Automation
{
    internal class OutputFileHelper : IOutputFileHelper
    {
        private readonly string _outputDirectory;
        private readonly ISystemDateTime _dateTime;

        public const string DefaultOutputDirectoryName = "AxeWindowsOutputFiles";
        public const string DefaultFileNameBase = "AxeWindows";

        public OutputFileHelper(string outputDirectory, ISystemFactory systemFactory)
        {
            if (systemFactory == null) throw new ArgumentNullException(nameof(systemFactory));

            _dateTime = systemFactory.CreateSystemDateTime();
            if (_dateTime == null) throw new InvalidOperationException($"Expected {nameof(_dateTime)} not to be null");

            var environment = systemFactory.CreateSystemEnvironment();
            if (environment == null) throw new InvalidOperationException($"Expected {nameof(environment)} not to be null");

            var directory = systemFactory.CreateSystemIODirectory();
            if (directory == null) throw new InvalidOperationException($"Expected {nameof(directory)} not to be null");

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
                throw new AxeWindowsAutomationException(String.Format(DisplayStrings.ErrorDirectoryInvalid, path), ex);
            }
        }

        public string GetNewA11yTestFilePath()
        {
            return Path.Combine(_outputDirectory
                , GetNewFileName()
                + ".a11ytest");
        }

        private string GetNewFileName()
        {
            var now = _dateTime.Now;

            var nowString = $"{now:yy-MM-dd_HH-mm-ss.FFFFFFF}";
            return $"{DefaultFileNameBase}_{nowString}";
        }
    } // class
} // namespace
