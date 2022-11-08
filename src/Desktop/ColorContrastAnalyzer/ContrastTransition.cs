// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Axe.Windows.Desktop.ColorContrastAnalyzer
{
    internal class ColorContrastTransition
    {
        private readonly IColorContrastConfig _colorContrastConfig;

        internal bool IsClosed { get; private set; }
        internal bool IsConnecting { get; private set; }

        internal readonly Color StartingColor;

        internal Color MostContrastingColor { get; private set; }

        /**
         * A text transition will increase in contrast from its original color
         * until it has reached a maximum color. Then it will decrease in contrast.
         * These two booleans help us track that, without having to store all the colors
         * in a list.
         */
        private bool _isMountainShaped = true;
        private bool _isIncreasingInContrast = true;

        /**
         * It is useful to track the size of a transition. Especially for debugging purposes,
         * though if performance is an issue, closing large transitions can help significantly.
         */
        private int _size = 1;

        internal ColorContrastTransition(Color color, IColorContrastConfig colorContrastConfig)
        {
            _colorContrastConfig = colorContrastConfig;
            StartingColor = color;
            MostContrastingColor = color;
        }

        internal void AddColor(Color color)
        {
            _size++;

            if (_size > _colorContrastConfig.MaxTextThickness || (_size > 1 && StartingColor.Equals(color)))
            {
                if (StartingColor.Equals(color))
                {
                    IsConnecting = true;
                }

                IsClosed = true;

                return;
            }

            if (MostContrastingColor.Contrast(StartingColor) < color.Contrast(StartingColor))
            {
                MostContrastingColor = color;

                if (!_isIncreasingInContrast)
                {
                    _isMountainShaped = false;
                }
            }
            else
            {
                _isIncreasingInContrast = false;
            }
        }

        /**
         * True if the transition may be a transition involving text.
         */
        public bool IsPotentialForegroundBackgroundPair()
        {
            return IsConsequential() && !ToColorPair().AreVisuallySimilarColors();
        }

        /**
         * Convert the starting color and most contrasting color to a ColorPair object.
         */
        public ColorPair ToColorPair()
        {
            return new ColorPair(StartingColor, MostContrastingColor);
        }

        internal bool IsConsequential()
        {
            return IsConnecting && _size > 2 && _isMountainShaped;
        }

        public override string ToString()
        {
            return ToColorPair().ToString();
        }
    }
}
