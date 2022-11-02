// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Misc;
using Axe.Windows.Core.Resources;
using System;

namespace Axe.Windows.Core.Results
{
    /// <summary>
    /// ScanMetaInfo class
    /// this class indicate the meta information of Scan results origin
    /// the meta info would be like below
    /// - Property Name
    /// - UI Framework
    /// - ControlType
    /// </summary>
    public class ScanMetaInfo
    {
        /// <summary>
        /// Property is overridable by scan logic via SetProperty()
        /// </summary>
        public int PropertyId { get; set; }
        public string UIFramework { get; set; }
        public string ControlType { get; set; }

        /// <summary>
        /// Populate data based on ScanMetaInfo
        /// </summary>
        /// <param name="e"></param>
        /// <param name="propertyid"></param>
        public ScanMetaInfo(IA11yElement e, int propertyid) : this(e)
        {
            PropertyId = propertyid;
        }

        /// <summary>
        /// Constructor for the scans which are not using property but to use structure.
        /// </summary>
        /// <param name="e"></param>
        public ScanMetaInfo(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            UIFramework = e.GetUIFramework();
            ControlType = Types.ControlType.GetInstance().GetNameById(e.ControlTypeId).Split('(')[0];
            PropertyId = 0;
        }

        /// <summary>
        /// Get the frameworkID.
        /// if FrameworkId exists in the element, it returns the value from it.
        /// otherwise, search it in ancestry.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        /// <summary>
        /// Constructor for Deserialization.
        /// </summary>
        public ScanMetaInfo() { }

        /// <summary>
        /// Override Property Id value
        /// if Property is already set, it will throw an exception.
        /// </summary>
        /// <param name="id"></param>
        public void SetProperty(int id)
        {
            if (PropertyId == 0)
            {
                PropertyId = id;
            }
            else
            {
                throw new ArgumentException(ErrorMessages.PropertyAlreadySet);
            }
        }

        /// <summary>
        /// Clone the ScanMetaInfo
        /// </summary>
        /// <returns></returns>
        internal ScanMetaInfo Clone()
        {
            var mi = new ScanMetaInfo
            {
                ControlType = ControlType,
                PropertyId = PropertyId,
                UIFramework = UIFramework
            };

            return mi;
        }
    }
}
