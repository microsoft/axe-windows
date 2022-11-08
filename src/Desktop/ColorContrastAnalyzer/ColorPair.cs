// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Axe.Windows.Desktop.ColorContrastAnalyzer
{
    public class ColorPair
    {
        internal readonly Color backgroundColor;
        internal readonly Color foregroundColor;

        public ColorPair(Color potentialBackgroundColor, Color potentialForegroundColor)
        {
            backgroundColor = potentialBackgroundColor;
            foregroundColor = potentialForegroundColor;
        }

        public Color DarkerColor
        {
            get
            {
                double contrast1 = Color.WHITE.Contrast(backgroundColor);
                double contrast2 = Color.WHITE.Contrast(foregroundColor);

                return contrast1 < contrast2 ? foregroundColor : backgroundColor;
            }
        }

        public Color LighterColor
        {
            get
            {
                double contrast1 = Color.WHITE.Contrast(backgroundColor);
                double contrast2 = Color.WHITE.Contrast(foregroundColor);

                return contrast1 < contrast2 ? backgroundColor : foregroundColor;
            }
        }

        public override bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                ColorPair p = (ColorPair)obj;
                return p.ToString().Equals(ToString(), StringComparison.Ordinal);
            }
        }

        /**
         * True when the pair of colors are not visually different.
         */
        public bool AreVisuallySimilarColors()
        {
            return LighterColor.IsSimilarColor(DarkerColor);
        }

        /**
         * True when a pair of colors have visibly similar pairs of colors.
         */
        public bool IsVisiblySimilarTo(ColorPair otherPair)
        {
            if (otherPair == null) throw new ArgumentNullException(nameof(otherPair));

            return LighterColor.IsSimilarColor(otherPair.LighterColor) &&
                DarkerColor.IsSimilarColor(otherPair.DarkerColor);
        }

        /**
         * Calculate the Color Contrast between the pair of colors.
         */
        public double ColorContrast()
        {
            return Math.Round(LighterColor.Contrast(DarkerColor), 2);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            return LighterColor.ToString() + " --" + ColorContrast() + "-> " + DarkerColor.ToString();
        }
    }
}
