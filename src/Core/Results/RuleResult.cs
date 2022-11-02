// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.HelpLinks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Axe.Windows.Core.Results
{
    /// <summary>
    /// Class RuleResult contains the result of each individual rule in a Scan.
    /// </summary>
    public class RuleResult
    {
#pragma warning disable CA2227 // Collection properties should be read only
        /// <summary>
        /// Messages through Rule checking stages
        /// for JSON serialization, allow set()
        /// </summary>
        public IList<string> Messages { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
        /// <summary>
        /// Status of Rule check
        /// </summary>
        public ScanStatus Status { get; set; } = ScanStatus.Pass;

        [JsonIgnore]
        public RuleId Rule { get; set; }

        /// <summary>
        /// Name of the Rule
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Source of the rule (ex, A11yCriteria 4.1.2)
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Meta info for query
        /// </summary>
        public ScanMetaInfo MetaInfo { get; set; }

        /// <summary>
        /// URL to a helpful information
        /// it could be a suggested fix or other things such as A11y guidance.
        /// </summary>
        public HelpUrl HelpUrl { get; set; }

        /// <summary>
        /// Returns the display text for the issue.
        /// </summary>
        public string IssueDisplayText { get; set; }

        /// <summary>
        /// The link to fetch / display the issue
        /// </summary>
        public Uri IssueLink { get; set; }

        /// <summary>
        /// The link to a known framework issue that can cause this rule to fail.
        /// This field will be null if no known framework issue exists.
        /// </summary>
        public string FrameworkIssueLink { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="status"></param>
        /// <param name="desc"></param>
        /// <param name="source"></param>
        /// <param name="url"></param>
        /// <param name="meta"></param>
        /// <param name="frameworkIssueLink"></param>
        internal RuleResult(RuleId id, string desc, string source, HelpUrl url, string frameworkIssueLink, ScanMetaInfo meta)
        {
            Rule = id;
            Description = desc;
            Source = source;
            Messages = new List<string>();
            MetaInfo = meta;
            HelpUrl = url;
            FrameworkIssueLink = frameworkIssueLink;
        }

        /// <summary>
        /// Constructor for Deserialization
        /// Do not use for other purpose.
        /// </summary>
        public RuleResult() { }

        /// <summary>
        /// Add Test status
        /// </summary>
        /// <param name="status"></param>
        /// <param name="message"></param>
        public void SetStatus(ScanStatus status, string message)
        {
            if (Status < status)
            {
                Status = status;
            }
            AddMessage(message);
        }

        /// <summary>
        /// Add Message
        /// </summary>
        /// <param name="message"></param>
        public void AddMessage(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                Messages.Add(message);
            }
        }
    }
}
