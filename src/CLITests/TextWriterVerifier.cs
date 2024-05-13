// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.IO;

namespace AxeWindowsCLITests
{
    internal enum WriteSource
    {
        WriteStringOnly,
        WriteOneParam,
        WriteLineEmpty,
        WriteLineStringOnly,
        WriteLineOneParam,
        WriteLineTwoParams,
    }

    internal class WriteCall
    {
        public string Format { get; }
        public WriteSource Source { get; }

        public WriteCall(string format, WriteSource source)
        {
            if (source == WriteSource.WriteLineEmpty)
            {
                Assert.IsNull(format);
            }
            else
            {
                Assert.IsNotNull(format);
            }
            Format = format;
            Source = source;
        }
    }

    internal class TextWriterVerifier
    {
        private readonly List<WriteCall> _actualWriteCalls = new List<WriteCall>();
        private readonly Mock<TextWriter> _writerMock;
        private readonly IReadOnlyList<WriteCall> _expectedWriteCalls;

        public TextWriterVerifier(Mock<TextWriter> writerMock, IEnumerable<WriteCall> expectedWriteCalls)
        {
            _writerMock = writerMock;
            _expectedWriteCalls = new List<WriteCall>(expectedWriteCalls);

            foreach (WriteCall expectedCall in expectedWriteCalls)
            {
                switch (expectedCall.Source)
                {
                    case WriteSource.WriteLineEmpty:
                        MockWriteLineEmpty();
                        break;

                    case WriteSource.WriteLineStringOnly:
                        MockWriteLineStringOnly();
                        break;

                    case WriteSource.WriteLineOneParam:
                        MockWriteLineOneParam();
                        break;

                    case WriteSource.WriteLineTwoParams:
                        MockWriteLineTwoParams();
                        break;

                    case WriteSource.WriteStringOnly:
                        MockWriteStringOnly();
                        break;

                    case WriteSource.WriteOneParam:
                        MockWriteOneParam();
                        break;

                    default:
                        Assert.Fail("Unknown parameter: " + expectedCall.Source);
                        break;
                }

            }
        }

        public void VerifyAll()
        {
            int verified = 0;

            foreach (WriteCall expectedWriteCall in _expectedWriteCalls)
            {
                WriteCall actualCall = _actualWriteCalls[verified];
                Assert.AreEqual(expectedWriteCall.Source, actualCall.Source, "Actual Format = " + actualCall.Format);
                if (expectedWriteCall.Source == WriteSource.WriteLineEmpty)
                {
                    Assert.IsNull(expectedWriteCall.Format);
                    Assert.IsNull(actualCall.Format);
                }
                else
                {
                    Assert.IsTrue(actualCall.Format.StartsWith(expectedWriteCall.Format), $"Actual Format = '{actualCall.Format}'; Expected Format = '{expectedWriteCall.Format}'");
                }
                verified++;
            }

            Assert.AreEqual(verified, _actualWriteCalls.Count);
        }

        private void AddWriteCall(string format, WriteSource source)
        {
            _actualWriteCalls.Add(new WriteCall(format, source));
        }

        private void MockWriteStringOnly()
        {
            _writerMock.Setup(x => x.Write(It.IsAny<string>()))
                .Callback<string>((s) => AddWriteCall(s, WriteSource.WriteStringOnly));
        }

        private void MockWriteOneParam()
        {
            _writerMock.Setup(x => x.Write(It.IsAny<string>(), It.IsAny<object>()))
                .Callback<string, object>((s, _) => AddWriteCall(s, WriteSource.WriteOneParam));
        }

        private void MockWriteLineEmpty()
        {
            _writerMock.Setup(x => x.WriteLine())
                .Callback(() => AddWriteCall(null, WriteSource.WriteLineEmpty));
        }

        private void MockWriteLineStringOnly()
        {
            _writerMock.Setup(x => x.WriteLine(It.IsAny<string>()))
                .Callback<string>((s) => AddWriteCall(s, WriteSource.WriteLineStringOnly));
        }

        private void MockWriteLineOneParam()
        {
            _writerMock.Setup(x => x.WriteLine(It.IsAny<string>(), It.IsAny<object>()))
                .Callback<string, object>((s, _) => AddWriteCall(s, WriteSource.WriteLineOneParam));
        }

        private void MockWriteLineTwoParams()
        {
            _writerMock.Setup(x => x.WriteLine(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<object>()))
                .Callback<string, object, object>((s, _, __) => AddWriteCall(s, WriteSource.WriteLineTwoParams));
        }

    }
}
