﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Exceptions;
using Axe.Windows.Core.Misc;
using Axe.Windows.Rules.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Axe.Windows.Rules.PropertyConditions
{
    /// <summary>
    /// Contains commonly used conditions for testing the relationships between IA11yElement objects.
    /// </summary>
    class Relationships
    {
        public static Condition ParentExists = Parent(Condition.True);
        public static Condition NoParentExists = ~ParentExists;
        public static Condition ChildrenExist = AnyChild(Condition.True)[ConditionDescriptions.ChildrenExist];
        public static Condition TreeItemChildrenExist = AnyChild(ControlType.TreeItem)[ConditionDescriptions.TreeItemChildrenExist];
        public static Condition NoChildrenExist = (~ChildrenExist)[ConditionDescriptions.NoChildrenExist];
        public static Condition SiblingsOfSameType = CreateSiblingsOfSameTypeCondition();
        public static Condition SecondChild = CreateSecondChildCondition();
        public static Condition NoSiblingsOfSameType = ~SiblingsOfSameType;
        public static Condition HasSameType = Condition.Create(HasSameTypeAsReferenceElement);

        private static Condition CreateSiblingsOfSameTypeCondition()
        {
            return Condition.Create(HasSiblingsOfSameType);
        }

        private static Condition CreateSecondChildCondition()
        {
            return Condition.Create(e => MatchOrderInSiblings(e, 2));
        }

        private static bool MatchOrderInSiblings(IA11yElement e, int index)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            IEnumerable<IA11yElement> siblings = e.Parent?.Children;
            if (siblings == null) return false;
            if (index < 1 || index > siblings.Count()) return false;

            return siblings.ElementAt(index - 1).RuntimeId == e.RuntimeId;
        }

        private static bool HasSiblingsOfSameType(IA11yElement e)
        {
            IA11yElement parent = e?.Parent;
            if (parent == null) return false;

            var count = (from c in parent.Children
                         where c.ControlTypeId == e.ControlTypeId
                         select c).Count();

            return count > 1;
        }

        public static Condition Parent(Condition c)
        {
            if (c == null) throw new ArgumentNullException(nameof(c));

            return Condition.Create(e => MatchParent(c, e));
        }

        private static bool MatchParent(Condition c, IA11yElement e)
        {
            if (c == null) throw new ArgumentNullException(nameof(c));
            if (e == null) throw new ArgumentNullException(nameof(e));
            IA11yElement parent = e.Parent;
            if (parent == null) return false;

            return c.Matches(parent);
        }

        public static ValueCondition<int> SiblingCount(Condition c)
        {
            var description = ConditionDescriptions.ChildCount.WithParameters(c);
            return new ValueCondition<int>(e => SiblingCount(c, e), description);
        }

        public static ValueCondition<int> SiblingCount()
        {
            return SiblingCount(Condition.True[string.Empty]);
        }

        private static int SiblingCount(Condition c, IA11yElement e)
        {
            if (c == null) throw new ArgumentNullException(nameof(c));
            if (e == null) throw new ArgumentNullException(nameof(e));
            IEnumerable<IA11yElement> children = e.Parent?.Children;
            if (children == null) return -1;

            int count = 0;
            foreach (var child in children)
            {
                if (c.Matches(child))
                    ++count;
            } // foreach

            return count;
        }

        public static Condition NotParent(Condition condition)
        {
            var parents = Parent(condition);
            return new NotCondition(parents);
        }

        /// <summary>
        /// Sets a condition for an ancestor by specified by an index
        /// </summary>
        /// <param name="index">0 = current element, 1 = parent, 2 = grandparent, ...</param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static Condition Ancestor(int index, Condition condition)
        {
            if (index < 0) throw new ArgumentException(ErrorMessages.IntParameterLessThanZero, nameof(index));
            if (index == 0) return condition;

            return Ancestor(index - 1, Parent(condition));
        }

        /// <summary>
        /// Determines if the specified ancestor exists
        /// </summary>
        /// <param name="index">0 = current element, 1 = parent, 2 = grandparent, ...</param>
        /// <returns></returns>
        public static Condition AncestorExists(int index)
        {
            return Ancestor(index, Condition.True);
        }

        public static Condition AnyAncestor(Condition c)
        {
            return AnyAncestor(c, Condition.False);
        }

        public static Condition AnyAncestor(Condition c, Condition stopCondition)
        {
            if (c == null) throw new ArgumentNullException(nameof(c));
            if (stopCondition == null) throw new ArgumentNullException(nameof(stopCondition));

            var recurse = new RecursiveCondition();
            recurse %= Parent(~stopCondition & (c | recurse));
            return recurse;
        }

        public static Condition NoAncestor(Condition c)
        {
            return ~AnyAncestor(c);
        }

        public static Condition AllAncestors(Condition c)
        {
            if (c == null) throw new ArgumentNullException(nameof(c));

            var recurse = new RecursiveCondition();
            recurse %= Parent(c & (recurse | NoParentExists));
            return recurse;
        }

        private static bool HasSameTypeAsReferenceElement(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (!Condition.Context.IsValueCreated) throw new AxeWindowsException(ErrorMessages.ExpectedValidConditionContext);

            var referenceElement = Condition.Context.Value.ReferenceElements.Peek();
            return ControlTypeMatches(e, referenceElement);
        }

        public static Condition AnyChild(Condition c)
        {
            if (c == null) throw new ArgumentNullException(nameof(c));

            var description = ConditionDescriptions.AnyChild.WithParameters(c);

            return AnyChildInternal(c)[description];
        }

        private static Condition AnyChildInternal(Condition c)
        {
            if (c == null) throw new ArgumentNullException(nameof(c));

            var anyChild = Condition.Create(e => MatchAnyChild(c, e));

            // saving the context means saving the element to the Context stack before iterating through its children.
            // this allows conditions analyzing the child elements to compare the children to the parent.
            return Context.Save(anyChild);
        }

        private static bool MatchAnyChild(Condition c, IA11yElement e)
        {
            if (c == null) throw new ArgumentNullException(nameof(c));
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (e.Children == null) return false;

            foreach (var child in e.Children)
            {
                if (c.Matches(child))
                    return true;
            } // foreach

            return false;
        }

        public static Condition NoChild(Condition c)
        {
            if (c == null) throw new ArgumentNullException(nameof(c));

            var description = ConditionDescriptions.NoChild.WithParameters(c);

            return (~AnyChildInternal(c))[description];
        }

        public static Condition AllChildren(Condition c)
        {
            if (c == null) throw new ArgumentNullException(nameof(c));

            var description = ConditionDescriptions.AllChildren.WithParameters(c);

            // we must check for the existence of children because MatchAnyChild will return false if there are no children
            // and the not operator (~) below will cause this function to return true erroneously when no children exist.
            return (ChildrenExist & ~AnyChild(~c))[description];
        }

        public static ValueCondition<int> ChildCount(Condition c)
        {
            var description = ConditionDescriptions.ChildCount.WithParameters(c);
            return new ValueCondition<int>(e => ChildCount(c, e), description);
        }

        public static ValueCondition<int> ChildCount()
        {
            return ChildCount(Condition.True[string.Empty]);
        }

        private static int ChildCount(Condition c, IA11yElement e)
        {
            if (c == null) throw new ArgumentNullException(nameof(c));
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (e.Children == null) return 0;

            int count = 0;
            foreach (var child in e.Children)
            {
                if (c.Matches(child))
                    ++count;
            } // foreach

            return count;
        }

        public static Condition AnyDescendant(Condition c)
        {
            if (c == null) throw new ArgumentNullException(nameof(c));

            return Condition.Create(e => MatchAnyDescendant(c, e));
        }

        private static bool MatchAnyDescendant(Condition c, IA11yElement e)
        {
            if (c == null) throw new ArgumentNullException(nameof(c));
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (e.Children == null) return false;

            foreach (var child in e.Children)
            {
                if (c.Matches(child))
                    return true;

                if (MatchAnyDescendant(c, child))
                    return true;
            } // foreach

            return false;
        }

        public static Condition AllDescendants(Condition c)
        {
            if (c == null) throw new ArgumentNullException(nameof(c));

            return ChildrenExist & ~Condition.Create(e => MatchAnyDescendant(~c, e));
        }

        public static Condition NoDescendant(Condition c)
        {
            return ~AnyDescendant(c);
        }

        public static ValueCondition<int> DescendantCount(Condition c)
        {
            var description = ConditionDescriptions.DescendantCount.WithParameters(c);
            return new ValueCondition<int>(e => DescendantCount(c, e), description);
        }

        public static ValueCondition<int> DescendantCount()
        {
            return DescendantCount(Condition.True[string.Empty]);
        }

        private static int DescendantCount(Condition c, IA11yElement e)
        {
            if (c == null) throw new ArgumentNullException(nameof(c));
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (e.Children == null) return 0;

            int count = 0;
            foreach (var child in e.Children)
            {
                if (c.Matches(child))
                    ++count;

                count += DescendantCount(c, child);
            } // foreach

            return count;
        }

        private static bool ControlTypeMatches(IA11yElement e1, IA11yElement e2)
        {
            if (e1 == null) throw new ArgumentNullException(nameof(e1));
            if (e2 == null) throw new ArgumentNullException(nameof(e2));

            return e1.ControlTypeId == e2.ControlTypeId;
        }

        public static Condition ExactlyOne(params Condition[] conditions)
        {
            var paramsString = ParameterizeConditionStrings(conditions);
            var description = ConditionDescriptions.ExactlyOne.WithParameters(paramsString);

            return Condition.Create(e => MatchExactlyOne(e, conditions), description);
        }

        private static bool MatchExactlyOne(IA11yElement e, params Condition[] conditions)
        {
            int count = 0;
            foreach (var c in conditions)
            {
                if (!c.Matches(e)) continue;

                ++count;

                if (count > 1)
                    return false;
            }

            return count == 1;
        }

        public static Condition Any(params Condition[] conditions)
        {
            var paramsString = ParameterizeConditionStrings(conditions);
            var description = ConditionDescriptions.Any.WithParameters(paramsString);

            return Condition.Create(e => MatchAny(e, conditions), description);
        }

        private static bool MatchAny(IA11yElement e, params Condition[] conditions)
        {
            foreach (var c in conditions)
            {
                if (c.Matches(e)) return true;
            }

            return false;
        }

        public static Condition All(params Condition[] conditions)
        {
            var paramsString = ParameterizeConditionStrings(conditions);
            var description = ConditionDescriptions.All.WithParameters(paramsString);

            return Condition.Create(e => MatchAll(e, conditions), description);
        }

        private static bool MatchAll(IA11yElement e, params Condition[] conditions)
        {
            foreach (var c in conditions)
            {
                if (!c.Matches(e)) return false;
            }

            return true;
        }

        private static string ParameterizeConditionStrings(params Condition[] conditions)
        {
            string text = "";
            foreach (var c in conditions)
            {
                if (!string.IsNullOrEmpty(text))
                    text += ", ";

                text += c.ToString();
            }

            return text;
        }
    } // class
} // namespace
