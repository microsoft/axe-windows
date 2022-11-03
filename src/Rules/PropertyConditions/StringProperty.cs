// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Misc;
using Axe.Windows.Rules.Resources;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

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
        private readonly Func<IA11yElement, string> _getStringPropertyValue;

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
            _getStringPropertyValue = valueGetter;
            PropertyDescription = propertyDescription;
            Null = CreateNullCondition();
            Empty = CreateEmptyCondition();
            NotNull = ~Null;
            NotEmpty = ~Empty;
            NullOrEmpty = Null | Empty;
            NotNullOrEmpty = NotNull & NotEmpty;
            WhiteSpace = CreateWhitespaceCondition();
            NotWhiteSpace = ~WhiteSpace;
            NullOrWhiteSpace = NullOrEmpty | WhiteSpace;
            NotNullOrWhiteSpace = ~NullOrWhiteSpace;
            IncludesPrivateUnicodeCharacters = CreateIncludesPrivateUnicodeCharactersCondition();
            ExcludesPrivateUnicodeCharacters = ~IncludesPrivateUnicodeCharacters;
            Length = CreateLengthCondition();
        }

        private Condition CreateNullCondition()
        {
            var condition = Condition.Create(e => _getStringPropertyValue(e) == null);
            return condition;
        }

        private Condition CreateEmptyCondition()
        {
            var condition = Condition.Create(e => _getStringPropertyValue(e)?.Length <= 0);
            return condition;
        }

        private Condition CreateWhitespaceCondition()
        {
            var condition = Condition.Create(e => _getStringPropertyValue(e)?.Trim().Length <= 0);
            return condition;
        }

        private Condition CreateIncludesPrivateUnicodeCharactersCondition()
        {
            return Condition.Create(e => StringIncludesPrivateUnicodeCharacters(_getStringPropertyValue(e)),
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

                string s = _getStringPropertyValue(e);
                if (s == null) return 0;

                return s.Length;
            }, string.Empty);
        }

        public Condition Is(string s)
        {
            return Condition.Create(e => _getStringPropertyValue(e) == s);
        }

        public Condition IsNoCase(string s)
        {
            return Condition.Create(e => string.Equals(_getStringPropertyValue(e), s, StringComparison.OrdinalIgnoreCase));
        }

        public Condition IsEqualTo(StringProperty that)
        {
            if (that == null) throw new ArgumentNullException(nameof(that));

            return Condition.Create(e => IsEqualTo(e, that));
        }

        public Condition IsNotEqualTo(StringProperty that)
        {
            return ~IsEqualTo(that);
        }

        private bool IsEqualTo(IA11yElement e, StringProperty that)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (that == null) throw new ArgumentNullException(nameof(that));

            string s1 = _getStringPropertyValue(e);
            if (string.IsNullOrWhiteSpace(s1)) return false;

            string s2 = that._getStringPropertyValue(e);
            if (string.IsNullOrWhiteSpace(s2)) return false;

            return string.Equals(s1, s2, StringComparison.OrdinalIgnoreCase);
        }

        public Condition MatchesRegEx(string s)
        {
            return Condition.Create(e =>
            {
                var propertyValue = _getStringPropertyValue(e);
                if (propertyValue == null) return false;

                Regex r = new Regex(s);
                return r.IsMatch(propertyValue);
            },
            ConditionDescriptions.MatchesRegEx.WithParameters(PropertyDescription, s));
        }

        public Condition MatchesRegEx(string s, RegexOptions options)
        {
            return Condition.Create(e =>
            {
                var propertyValue = _getStringPropertyValue(e);
                if (propertyValue == null) return false;

                Regex r = new Regex(s, options);
                return r.IsMatch(propertyValue);
            },
            ConditionDescriptions.MatchesRegExWithOptions.WithParameters(PropertyDescription, s, options.ToString()));
        }
    } // class
} // namespace
