// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
namespace Axe.Windows.Actions.Enums
{
    /// <summary>
    /// Select type
    /// </summary>
    public enum SelectType
    {
        Live,
        Loaded,
    }

    /// <summary>
    /// Indicate the Snapshotmode
    /// </summary>
    public enum DataContextMode
    {
        Live,
        Test,
        Load,
    }

    /// <summary>
    /// Indicate the scope of selection
    /// </summary>
    public enum SelectionScope
    {
        App,
        Element,
    }

    /// <summary>
    /// The level of user ux interaction of an action
    /// </summary>
    public enum UxInteractionLevel
    {
        /// <summary>
        /// The action has no ux component and operates without user input
        /// </summary>
        NoUxInteraction,

        /// <summary>
        /// The action has a potentially interactive ux component
        /// </summary>
        OptionalUxInteraction,

        /// <summary>
        /// The action requires user interaction with ux
        /// </summary>
        RequiredUxInteraction        
    }

    /// <summary>
    /// UIA Tree states
    /// </summary>
    public enum UIATreeState
    {
        Paused,
        Resumed
    }

    /// <summary>
    /// Controls the order in which event handlers are registered.

    /// </summary>
    /// <remarks>
    /// In some cases, registering for property change events after other types of events
    /// causes some property change events not to be received.
    /// </remarks>
    public enum EventRegistrationOrder
    {
        PropertyEventsFirst,
        PropertyEventsLast,
    }
}
