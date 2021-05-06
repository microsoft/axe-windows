// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using AxeWindowsCLI.Resources;
using System;
using System.Globalization;
using System.IO;

namespace AxeWindowsCLI
{
    class ScanDelay : IScanDelay
    {
        private readonly TextWriter _writer;
        private readonly Action _oneSecondDelay;

        internal ScanDelay(TextWriter writer, Action oneSecondDelay)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));
            if (oneSecondDelay == null) throw new ArgumentNullException(nameof(oneSecondDelay));

            _writer = writer;
            _oneSecondDelay = oneSecondDelay;
        }

        public void DelayWithCountdown(IOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.DelayInSeconds > 0)
            {
                ConditionallyWriteMessageWithCount(options, DisplayStrings.ScanDelayHeaderOneSecond,
                    DisplayStrings.ScanDelayHeaderMoreThanOneSecond, options.DelayInSeconds);
                for (int secondsRemaining = options.DelayInSeconds; secondsRemaining > 0; secondsRemaining--)
                {
                    ConditionallyWriteMessageWithCount(options, DisplayStrings.ScanDelayCountdownOneSecondLeft,
                        DisplayStrings.ScanDelayCountdownMoreThanOneSecondLeft, secondsRemaining);
                    _oneSecondDelay.Invoke();
                }
                ConditionallyWriteMessage(options, () => DisplayStrings.ScanDelayTriggeringScan);
            }
        }

        private void ConditionallyWriteMessageWithCount(IOptions options, string stringIfCountIsOne, string stringIfCountIsNotOne, int count)
        {
            string format = SelectString(stringIfCountIsOne, stringIfCountIsNotOne, count);
            ConditionallyWriteMessage(options, () => string.Format(CultureInfo.InvariantCulture, format, count));
        }

        private void ConditionallyWriteMessage(IOptions options, Func<string> getMessage)
        {
            if (options.VerbosityLevel > VerbosityLevel.Quiet)
            {
                _writer.WriteLine(getMessage());
            }
        }

        private static string SelectString(string stringIfCountIsOne, string stringIfCountIsNotOne, int count)
        {
            return (count == 1) ? stringIfCountIsOne : stringIfCountIsNotOne;
        }
    }
}
