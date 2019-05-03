// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Win32;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Axe.Windows.Desktop.Keyboard
{
    /// <summary>
    /// Class for hot key
    /// </summary>
    public class Hotkey
    {
        /// <summary>
        /// this value will be set by Hotkey Handler
        /// </summary>
        public int Id { get; internal set; }

        public Keys Key { get; internal set; }
        public HotkeyModifier Modifier { get; internal set; }

        public Action Action { get; set; }

        public uint ErrorCode { get; private set; }

        /// <summary>
        /// Register hot key
        /// </summary>
        /// <param name="hWnd"></param>
        public bool Register(IntPtr hWnd)
        {
            this.ErrorCode = 0;

            if(NativeMethods.RegisterHotKey(hWnd, Id, (int)this.Modifier, Key.GetHashCode()) == false)
            {
                ErrorCode = NativeMethods.GetLastError();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Unregister HotKey
        /// </summary>
        /// <param name="hWnd"></param>
        public bool Unregister(IntPtr hWnd)
        {
            this.ErrorCode = 0;

            if(NativeMethods.UnregisterHotKey(hWnd, Id) == false)
            {
                ErrorCode = NativeMethods.GetLastError();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Get an instance of HotKey based on given string
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static Hotkey GetInstance(string txt)
        {
            var atoms = from a in txt.Split(new char[] { '+' }, StringSplitOptions.RemoveEmptyEntries)
                        select a.Trim();
            var hk = new Hotkey();

            try
            {
                if (atoms.Count() == 2)
                {
                    hk.Modifier = GetMofidifier(atoms.ElementAt(0));

                    hk.Key = GetKey(atoms.ElementAt(1));
                }
                else
                {
                    throw new ArgumentException($"Hotkey format is not Modifier + Key: {txt}");
                }
            }
            catch
            {
                return null;
            }

            return hk;
        }

        private static Keys GetKey(string a)
        {
            KeysConverter kc = new KeysConverter();
            return (Keys)kc.ConvertFromInvariantString(a.ToUpperInvariant());
        }

        /// <summary>
        /// Get Modifier value from given text
        /// </summary>
        /// <param name="mods">modifiers. comma separated</param>
        /// <returns></returns>
        private static HotkeyModifier GetMofidifier(string mods)
        {
            var fms = from m in mods.Split(',')
                     select $"MOD_{m.ToUpperInvariant()}";

            HotkeyModifier result = HotkeyModifier.MOD_NoModifier;

            foreach (var fm in fms)
            {
                if (Enum.TryParse<HotkeyModifier>(fm, out HotkeyModifier tmp) == false)
                {
                    result = HotkeyModifier.MOD_NoModifier;
                    break;
                }
                else
                {
                    if (result == HotkeyModifier.MOD_NoModifier)
                    {
                        result = tmp;
                    }
                    else
                    {
                        result |= tmp;
                    }

                }
            }

            return result;
        }
    }
}
