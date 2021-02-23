// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Axe.Windows.Core.Bases;
using Axe.Windows.Rules.Resources;

namespace Axe.Windows.Rules.PropertyConditions
{
    class StringProperty
    {
        public Condition Null;
        public Condition NotNull;
        public Condition Empty;
        public Condition NotEmpty;
        public Condition NullOrEmpty;
        public Condition NotNullOrEmpty;
        public Condition WhiteSpace;
        public Condition NotWhiteSpace;
        public Condition NullOrWhiteSpace;
        public Condition NotNullOrWhiteSpace;
        public Condition IncludesPrivateUnicodeCharacters;
        public Condition ExcludesPrivateUnicodeCharacters;
        public ValueCondition<int> Length;
        private Func<IA11yElement, string> GetStringPropertyValue;

        /// <summary>
        /// Represents a string property of an element such as "Name", "LocalizedControlType", etc.
        /// This may also apply to patterns, so please include the pattern name where applicable.
        /// This information may be visible to users.
        /// </summary>
        public readonly string PropertyDescription = ConditionDescriptions.StringPropertyNotSet;

        public StringProperty(Func<IA11yElement, string> valueGetter)
            : this(valueGetter, ConditionDescriptions.StringPropertyNotSet)
        { }

        /// <summary>
        /// StringProperty constructor with property description
        /// </summary>
        /// <param name="valueGetter"></param>
        /// <param name="propertyDescription">
        /// Represents a string property of an element such as "Name", "LocalizedControlType", etc.
        /// This may also apply to patterns, so please include the pattern name where applicable.
        /// This information may be visible to users.
        /// </param>
        public StringProperty(Func<IA11yElement, string> valueGetter, string propertyDescription)
        {
            GetStringPropertyValue = valueGetter;
            PropertyDescription = propertyDescription;
            this.Null = CreateNullCondition();
            this.Empty = CreateEmptyCondition();
            this.NotNull = ~Null;
            this.NotEmpty = ~Empty;
            this.NullOrEmpty = Null | Empty;
            this.NotNullOrEmpty = NotNull & NotEmpty;
            this.WhiteSpace = CreateWhitespaceCondition();
            this.NotWhiteSpace = ~WhiteSpace;
            this.NullOrWhiteSpace = NullOrEmpty | WhiteSpace;
            this.NotNullOrWhiteSpace = ~NullOrWhiteSpace;
            this.IncludesPrivateUnicodeCharacters = CreateIncludesPrivateUnicodeCharactersCondition();
            this.ExcludesPrivateUnicodeCharacters = ~IncludesPrivateUnicodeCharacters;
            this.Length = CreateLengthCondition();
        }

        private Condition CreateNullCondition()
        {
            var condition = Condition.Create(e => GetStringPropertyValue(e) == null);
            return condition;
        }

        private Condition CreateEmptyCondition()
        {
            var condition = Condition.Create(e => GetStringPropertyValue(e)?.Length <= 0);
            return condition;
        }

        private Condition CreateWhitespaceCondition()
        {
            var condition = Condition.Create(e => GetStringPropertyValue(e)?.Trim().Length <= 0);
            return condition;
        }

        private Condition CreateIncludesPrivateUnicodeCharactersCondition()
        {
            return Condition.Create(e => StringIncludesPrivateUnicodeCharacters(GetStringPropertyValue(e)),
                string.Format(CultureInfo.CurrentCulture, ConditionDescriptions.IncludesPrivateUnicodeCharacters, PropertyDescription));
        }

        private static bool StringIncludesPrivateUnicodeCharacters(string s)
        {
            if (s == null) throw new ArgumentNullException(nameof(s));
            if (string.IsNullOrWhiteSpace(s)) throw new ArgumentException(ErrorMessages.StringNullOrWhiteSpace, nameof(s));

            foreach (char c in s)
            {
                if (IsPrivateUnicodeChar(c))
                    return true;
            } // for chars in string

            return false;
        }

        private static bool IsPrivateUnicodeChar(char c)
        {
            return c >= 0xE000
                && c <= 0xF8FF;
        }

        private ValueCondition<int> CreateLengthCondition()
        {
            return new ValueCondition<int>(e =>
            {
                if (e == null) return 0;

                string s = GetStringPropertyValue(e);
                if (s == null) return 0;

                return s.Length;
            }, string.Empty);
        }

        public Condition Is(string s)
        {
            return Condition.Create(e => GetStringPropertyValue(e) == s);
        }

        public Condition IsNoCase(string s)
        {
            return Condition.Create(e => string.Equals(GetStringPropertyValue(e), s, StringComparison.OrdinalIgnoreCase));
        }

        public Condition IsEqualTo(StringProperty that)
        {
            if (that == null) throw new ArgumentNullException(nameof(that));

            return Condition.Create(e => this.IsEqualTo(e, that));
        }

        public Condition IsNotEqualTo(StringProperty that)
        {
            return ~IsEqualTo(that);
        }

        private bool IsEqualTo(IA11yElement e, StringProperty that)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (that == null) throw new ArgumentNullException(nameof(that));

            string s1 = this.GetStringPropertyValue(e);
            if (string.IsNullOrWhiteSpace(s1)) return false;

            string s2 = that.GetStringPropertyValue(e);
            if (string.IsNullOrWhiteSpace(s2)) return false;

            return string.Equals(s1, s2, StringComparison.OrdinalIgnoreCase);
        }

        public Condition MatchesRegEx(string s)
        {
            return Condition.Create(e =>
            {
                var propertyValue = GetStringPropertyValue(e);
                if (propertyValue == null) return false;

                Regex r = new Regex(s);
                return r.IsMatch(propertyValue);
            },
            string.Format(CultureInfo.InvariantCulture, ConditionDescriptions.MatchesRegEx, PropertyDescription, s));
        }

        public Condition MatchesRegEx(string s, RegexOptions options)
        {
            return Condition.Create(e =>
            {
                var propertyValue = GetStringPropertyValue(e);
                if (propertyValue == null) return false;

                Regex r = new Regex(s, options);
                return r.IsMatch(propertyValue);
                },
                string.Format(CultureInfo.InvariantCulture, ConditionDescriptions.MatchesRegExWithOptions, PropertyDescription, s, options.ToString()));
        }
    } // class
} // namespace
