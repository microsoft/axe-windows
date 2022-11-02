// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Axe.Windows.SystemAbstractions
{
    internal class MicrosoftFactory : IMicrosoftFactory
    {
        private static readonly IMicrosoftFactory MicrosoftFactoryInstance = new MicrosoftFactory();

        private MicrosoftFactory()
        { }

        public static IMicrosoft CreateMicrosoft()
        {
            return new Microsoft(MicrosoftFactoryInstance);
        }

        public IMicrosoftWin32 CreateMicrosoftWin32()
        {
            return new MicrosoftWin32(MicrosoftFactoryInstance);
        }

        public IMicrosoftWin32Registry CreateMicrosoftWin32Registry()
        {
            return new MicrosoftWin32Registry();
        }
    } // class
} // namespace
