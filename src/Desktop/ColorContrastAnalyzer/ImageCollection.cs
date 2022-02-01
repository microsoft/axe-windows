// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections;
using System.Collections.Generic;

namespace Axe.Windows.Desktop.ColorContrastAnalyzer
{
    public abstract class ImageCollection : IEnumerable<Pixel>
    {
        private readonly IColorContrastConfig _colorContrastConfig;

        protected ImageCollection(IColorContrastConfig colorContrastConfig)
        {
            if (colorContrastConfig == null)
                throw new ArgumentNullException(nameof(colorContrastConfig));

            _colorContrastConfig = colorContrastConfig;
        }

        public abstract int NumColumns();

        public abstract int NumRows();

        public abstract Color GetColor(int row, int column);

        private static bool IsNewRow(Pixel pixel)
        {
            return pixel.Column == 0;
        }

        private bool IsEndOfRow(Pixel pixel)
        {
            return pixel.Column == (NumColumns() - 1);
        }

        /**
         * Run the Color Contrast calculation on the image.
         */
        public IColorContrastResult RunColorContrastCalculation()
        {
            Func<IColorContrastResult> contastCalculator = GetColorContrastCalculator();
            return contastCalculator();
        }

        internal Func<IColorContrastResult> GetColorContrastCalculator()
        {
            switch (_colorContrastConfig.AnalyzerVersion)
            {
                case AnalyzerVersion.V2:
                    return RunColorContrastCalculationV2;
                default:
                    return RunColorContrastCalculationV1;
            }
        }

        /**
         * Run the Color Contrast calculation on the image.
         */
        internal IColorContrastResult RunColorContrastCalculationV2()
        {
            ColorContrastRunnerV2 runner = new ColorContrastRunnerV2();
            RowResultV2Accumulator accumulator = new RowResultV2Accumulator(_colorContrastConfig);

            foreach (var pixel in GetAllRowsIterator())
            {
                runner.OnPixel(pixel.Color);

                if (IsEndOfRow(pixel))
                {
                    accumulator.AddRowResult(runner.OnRowEnd());
                }
            }

            return accumulator.GetResult();
        }

        /**
         * Run the Color Contrast calculation on the image.
         */
        internal IColorContrastResult RunColorContrastCalculationV1()
        {
            ColorContrastResult result = null;

            ColorContrastRunner runner = new ColorContrastRunner(_colorContrastConfig);

            Color previousColor = null;

            foreach (var pixel in GetBinaryRowSearchIterator())
            {
                if (IsNewRow(pixel)) runner.OnRowBegin();

                runner.OnPixel(pixel.Color, previousColor);
                previousColor = pixel.Color;

                if (IsEndOfRow(pixel))
                {
                    var newResult = runner.OnRowEnd();

                    if (result == null) result = newResult;

                    // Save newResult if it's higher confidence than the current result.
                    // newResult.Confidence is guaranteed to be either High, Mid, or Low
                    if (newResult.Confidence == Confidence.High)
                    {
                        result = newResult;
                        break;
                    }
                    else if (newResult.Confidence == Confidence.Mid)
                    {
                        // The analyzer claims that this condition is always false, but testing
                        // proves that we definitely execute the code inside the if block.
#pragma warning disable CA1508 // Avoid dead conditional code
                        if (result.Confidence != Confidence.Mid)
#pragma warning restore CA1508 // Avoid dead conditional code
                        {
                            result = newResult;
                        }
                    }
                }
            }

            return result;
        }

        public IEnumerable<Pixel> GetAllRowsIterator()
        {
            for (var row = 0; row < NumRows(); row++)
            {
                for (var column = 0; column < NumColumns(); column++)
                {
                    yield return new Pixel(GetColor(row, column), row, column);
                }
            }
        }

        /**
         * A special iterator, that looks at the middle of an image first,
         * followed by recursively looking at the upper half and lower half
         * of the two pieces of image, until the given samples are some
         * distance apart.
         */
        public IEnumerable<Pixel> GetBinaryRowSearchIterator()
        {
            foreach (var pixel in GetRow(0, NumRows()))
            {
                yield return pixel;
            }
        }

        public IEnumerable<Pixel> GetRow(int top, int bottom)
        {
            int middle = (bottom + top) / 2;

            if ((bottom - top) < _colorContrastConfig.MinSpaceBetweenSamples) yield break;

            for (var i = 0; i < NumColumns(); i++)
            {
                yield return new Pixel(GetColor(middle, i), middle, i);
            }

            foreach (var pixel in GetRow(top, middle))
            {
                yield return pixel;
            }

            foreach (var pixel in GetRow(middle, bottom))
            {
                yield return pixel;
            }
        }

        public IEnumerator<Pixel> GetEnumerator()
        {
            foreach (var pixel in GetRow(0, NumRows()))
            {
                yield return pixel;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var pixel in GetRow(0, NumRows()))
            {
                yield return pixel;
            }
        }
    }
}
