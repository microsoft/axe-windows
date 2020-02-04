// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace OldFileVersionCompatibilityTests
{
    class Program
    {
        private static string AppName = "OldFileVersionCompatibilityTests";

        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                DoTest(args);
            }
            catch (TestAssertionException ex)
            {
                ExitWithError(ex);
            }
            catch (ProgramException ex)
            {
                ExitWithError(ex);
            }
            catch (Exception ex)
            {
                ExitWithError(ex);
            }

            Exit(ErrorCode.Success);
        }

        private static void DoTest(string[] args)
        {
            if (args.Length != 1) throw new ProgramException("Expected one command line parameter");
            
            var methodName = args[0];

            var t = typeof(TestRunner);
            var method = t.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance);
            if (method == null) throw new ProgramException($"Expected {methodName} to be a public member of {t.Name}");

            method.Invoke(new TestRunner(), null);
        }

        internal static void ExitWithError(TestAssertionException ex)
        {
            var errorCode = ErrorCode.Assertion;
            var errorCodeString = GetErrorCodeString(errorCode);
            Console.WriteLine($"{ex.FileName}({ex.LineNumber}) : {errorCodeString} {AppName} test assertion failed: {ex.Message}");
            Exit(ErrorCode.Assertion);
        }

        internal static void ExitWithError(ProgramException ex)
        {
            var errorCode = ErrorCode.ProgramException;
            var errorCodeString = GetErrorCodeString(errorCode);
            Console.WriteLine($"{AppName} : {errorCodeString} program exception: {ex.Message}");
            Exit(errorCode);
        }

        internal static void ExitWithError(Exception ex)
        {
            var errorCode = ErrorCode.UnhandledException;
            var errorCodeString = GetErrorCodeString(errorCode);
            Console.WriteLine($"{AppName} : {errorCodeString} unhandled exception: {ex.Message}");
            Exit(errorCode);
        }

        private static string GetErrorCodeString(ErrorCode errorCode)
        {
            return $"error OFC{(int)errorCode}:";
        }

        private static void Exit(ErrorCode errorCode)
        {
            Environment.Exit((int)errorCode);
        }
    } // class
} // namespace
