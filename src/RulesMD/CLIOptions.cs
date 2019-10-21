// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using CommandLine;
using System;

namespace RulesMD
{
    class CLIOptions
    {
        [Option('o', "output", Required = true,
            HelpText = "path to the markdown file to be created")]
        public string OutputPath { get; set; }
    } // class
} // namespace
