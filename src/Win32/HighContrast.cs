// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Windows.Win32.UI.Accessibility;
using Windows.Win32.UI.WindowsAndMessaging;

namespace Axe.Windows.Win32
{
    internal class HighContrast
    {
        public bool IsOn { get; }

        private HighContrast(bool isOn)
        {
            IsOn = isOn;
        }

        public static HighContrast Create()
        {
            bool succeeded;
            HIGHCONTRASTW highContrastInfo;
            unsafe
            {
                highContrastInfo.cbSize = (uint)sizeof(HIGHCONTRASTW);
                succeeded = global::Windows.Win32.PInvoke.SystemParametersInfo(SYSTEM_PARAMETERS_INFO_ACTION.SPI_GETHIGHCONTRAST, highContrastInfo.cbSize, &highContrastInfo, 0);
            }

            bool isHighContrastOn = succeeded && highContrastInfo.dwFlags.HasFlag(HIGHCONTRASTW_FLAGS.HCF_HIGHCONTRASTON);

            return new HighContrast(isHighContrastOn);
        }
    } // struct
} // namespace
