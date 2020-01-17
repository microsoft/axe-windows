﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyProduct("Axe.Windows.Win32")]
[assembly: AssemblyTitle("Axe.Windows.Win32")]
[assembly: AssemblyCopyright("Copyright © 2020")]

// Limit P/Invoke to assemblies located in the System32 folder
[assembly: DefaultDllImportSearchPaths(DllImportSearchPath.System32)]

#if ENABLE_SIGNING
[assembly: InternalsVisibleTo("Axe.Windows.Actions,PublicKey=002400000480000094000000060200000024000052534131000400000100010007d1fa57c4aed9f0a32e84aa0faefd0de9e8fd6aec8f87fb03766c834c99921eb23be79ad9d5dcc1dd9ad236132102900b723cf980957fc4e177108fc607774f29e8320e92ea05ece4e821c0a5efe8f1645c4c0c93c1ab99285d622caa652c1dfad63d745d6f2de5f17e5eaf0fc4963d261c8a12436518206dc093344d5ad293")]
[assembly: InternalsVisibleTo("Axe.Windows.Automation,PublicKey=002400000480000094000000060200000024000052534131000400000100010007d1fa57c4aed9f0a32e84aa0faefd0de9e8fd6aec8f87fb03766c834c99921eb23be79ad9d5dcc1dd9ad236132102900b723cf980957fc4e177108fc607774f29e8320e92ea05ece4e821c0a5efe8f1645c4c0c93c1ab99285d622caa652c1dfad63d745d6f2de5f17e5eaf0fc4963d261c8a12436518206dc093344d5ad293")]
[assembly: InternalsVisibleTo("Axe.Windows.Core,PublicKey=002400000480000094000000060200000024000052534131000400000100010007d1fa57c4aed9f0a32e84aa0faefd0de9e8fd6aec8f87fb03766c834c99921eb23be79ad9d5dcc1dd9ad236132102900b723cf980957fc4e177108fc607774f29e8320e92ea05ece4e821c0a5efe8f1645c4c0c93c1ab99285d622caa652c1dfad63d745d6f2de5f17e5eaf0fc4963d261c8a12436518206dc093344d5ad293")]
[assembly: InternalsVisibleTo("Axe.Windows.Desktop,PublicKey=002400000480000094000000060200000024000052534131000400000100010007d1fa57c4aed9f0a32e84aa0faefd0de9e8fd6aec8f87fb03766c834c99921eb23be79ad9d5dcc1dd9ad236132102900b723cf980957fc4e177108fc607774f29e8320e92ea05ece4e821c0a5efe8f1645c4c0c93c1ab99285d622caa652c1dfad63d745d6f2de5f17e5eaf0fc4963d261c8a12436518206dc093344d5ad293")]
[assembly: InternalsVisibleTo("Win32Tests,PublicKey=002400000480000094000000060200000024000052534131000400000100010007d1fa57c4aed9f0a32e84aa0faefd0de9e8fd6aec8f87fb03766c834c99921eb23be79ad9d5dcc1dd9ad236132102900b723cf980957fc4e177108fc607774f29e8320e92ea05ece4e821c0a5efe8f1645c4c0c93c1ab99285d622caa652c1dfad63d745d6f2de5f17e5eaf0fc4963d261c8a12436518206dc093344d5ad293")]
#else
[assembly: InternalsVisibleTo("Axe.Windows.Actions")]
[assembly: InternalsVisibleTo("Axe.Windows.Automation")]
[assembly: InternalsVisibleTo("Axe.Windows.Core")]
[assembly: InternalsVisibleTo("Axe.Windows.Desktop")]
[assembly: InternalsVisibleTo("Win32Tests")]
#endif
#region FxCop analysis suppressions for entire assembly
[assembly: SuppressMessage("Microsoft.Naming", "CA1707", Justification = "Underscores are allowed to keep the same name as Win32")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1714", Justification = "Keep the same name as Win32")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1717", Justification = "Keep the same name as Win32")]
[assembly: SuppressMessage("Microsoft.Design", "CA1028", Justification = "Keep the same name as Win32")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1815", Justification = "== or != operators are not needed for parity with Win32")]
[assembly: SuppressMessage("Microsoft.Design", "CA1051", Justification = "For Win32 structure parity, allow visible instance fields")]
#endregion
