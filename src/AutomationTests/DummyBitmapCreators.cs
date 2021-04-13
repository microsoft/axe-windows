// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.SystemAbstractions;
using System;
using System.Drawing;

namespace Axe.Windows.AutomationTests
{
    // These classes provide different scenarios (visibility and derivation) to test
    // the BitmapCreatorLocator
    public class DummyPublicBitmapCreator : IBitmapCreator
    {
        public IBitmap FromScreenRectangle(Rectangle rect)
        {
            throw new NotImplementedException();
        }

        public IBitmap FromSystemDrawingBitmap(Bitmap bitmap)
        {
            throw new NotImplementedException();
        }
    }

    public class DummyPublicDerivedBitmapCreator : DummyPublicBitmapCreator { }

    internal class DummyInternalDerivedBitmapCreator : DummyPublicBitmapCreator { }
}
