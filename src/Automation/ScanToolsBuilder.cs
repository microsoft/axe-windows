﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;

namespace Axe.Windows.Automation
{
    internal class ScanToolsBuilder : IScanToolsBuilder
    {
        private IFactory _factory;
        private IOutputFileHelper _outputFileHelper;

        public ScanToolsBuilder(IFactory factory)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            _factory = factory;
        }

        public IScanToolsBuilder WithOutputDirectory(string outputDirectory)
        {
            _outputFileHelper = _factory.CreateOutputFileHelper(outputDirectory);
            return this;
        }

        public IScanTools Build()
        {
            return new ScanTools(
                _outputFileHelper ?? _factory.CreateOutputFileHelper(null),
                    _factory.CreateResultsAssembler(),
                    _factory.CreateTargetElementLocator(),
                    _factory.CreateAxeWindowsActions());
        }
    } // class
} // namespace
