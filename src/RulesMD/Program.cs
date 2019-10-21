// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using CommandLine;
using System;
using System.Collections.Generic;
using System.IO;

namespace RulesMD
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                DoWork(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(-1);
            }
        }

        static void DoWork(string[] args)
        {
            Parser.Default.ParseArguments < CLIOptions>(args)
                .WithParsed<CLIOptions>(Run);
        }

        private static void Run(CLIOptions options)
        {
            var markdownCreator = new MarkdownCreator();
            var markdown = markdownCreator.TransformText();

            var f = File.CreateText(options.OutputPath);
            f.Write(markdown);
            f.Close();
        }
} // class
} // namespace
