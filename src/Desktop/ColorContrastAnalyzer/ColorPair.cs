// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Axe.Windows.Desktop.ColorContrastAnalyzer
{
    public class ColorPair
    {
        internal Color BackgroundColor { get; }
        internal Color ForegroundColor { get; }

        public ColorPair(Color backgroundColor, Color foregroundColor)
        {
            BackgroundColor = backgroundColor;
            ForegroundColor = foregroundColor;
        }

        public Color DarkerColor
        {
            get
            {
                double contrast1 = Color.WHITE.Contrast(BackgroundColor);
                double contrast2 = Color.WHITE.Contrast(ForegroundColor);

                return contrast1 < contrast2 ? ForegroundColor : BackgroundColor;
            }
        }

        public Color LighterColor
        {
            get
            {
                double contrast1 = Color.WHITE.Contrast(BackgroundColor);
                double contrast2 = Color.WHITE.Contrast(ForegroundColor);

                return contrast1 < contrast2 ? BackgroundColor : ForegroundColor;
            }
        }

        public override bool Equals(Object obj)
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
        public Boolean AreVisuallySimilarColors()
        {
            return LighterColor.IsSimilarColor(DarkerColor);
        }

        /**
         * True when a pair of colors have visibly similar pairs of colors.
         */
        public Boolean IsVisiblySimilarTo(ColorPair otherPair)
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
