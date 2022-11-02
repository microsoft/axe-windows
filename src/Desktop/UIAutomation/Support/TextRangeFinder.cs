// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Desktop.UIAutomation.Patterns;

namespace Axe.Windows.Desktop.UIAutomation.Support
{
    /// <summary>
    /// class TextRangeFinder
    /// helper class to do "FindXXX()" method call with a given TextRange.
    /// it supports next/previous and forward/backward
    /// </summary>
    public class TextRangeFinder
    {
        private readonly TextRange OriginalRange;
        private TextRange FoundRange;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="range">Original range</param>
        public TextRangeFinder(TextRange range)
        {
            OriginalRange = range;
        }

        public TextRange Find(int id, dynamic value, bool backward)
        {
            using (var range = GetRangeForFind(backward))
            {
                FoundRange = range.FindAttribute(id, value, backward);
            } // using

            return FoundRange;
        }

        public TextRange FindText(string value, bool backward, bool ignorecase)
        {
            using (var range = GetRangeForFind(backward))
            {
                FoundRange = range.FindText(value, backward, ignorecase);
            } // using

            return FoundRange;
        }

        private TextRange GetRangeForFind(bool backward)
        {
            var range = OriginalRange.Clone();

            if (FoundRange != null)
            {
                if (backward == true)
                {
                    range.MoveEndpointByRange(UIAutomationClient.TextPatternRangeEndpoint.TextPatternRangeEndpoint_End, FoundRange, UIAutomationClient.TextPatternRangeEndpoint.TextPatternRangeEndpoint_Start);
                }
                else
                {
                    range.MoveEndpointByRange(UIAutomationClient.TextPatternRangeEndpoint.TextPatternRangeEndpoint_Start, FoundRange, UIAutomationClient.TextPatternRangeEndpoint.TextPatternRangeEndpoint_End);
                }
            }

            return range;
        }
    }
}
