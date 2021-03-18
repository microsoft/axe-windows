// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static System.FormattableString;

namespace Axe.Windows.Core.Types
{
    /// <summary>
    /// Base class for all Types(ControlPattern, Event, Property...)
    /// </summary>
    public abstract class TypeBase
    {
        const string DefaultNamePattern = "UIA_";
        protected Dictionary<int, string> Dic { get; private set; }
        private readonly string _namePattern;

        protected TypeBase() : this(DefaultNamePattern) { }

        protected TypeBase(string np)
        {
            _namePattern = np;

            this.Dic = new Dictionary<int, string>();

            PopulateDictionary();
        }

        /// <summary>
        /// Populate dictionary based on const members of inherited class
        /// </summary>
        private void PopulateDictionary()
        {
            var fields =  this.GetType().GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.FlattenHierarchy );

            foreach(var f in fields)
            {
                if (ShouldIncludeFieldInDictionary(f))
                    AddFieldToDictionary(f);
            }
        }

        private bool ShouldIncludeFieldInDictionary(FieldInfo field)
        {
            return field.Name.StartsWith(_namePattern, StringComparison.Ordinal);
        }

        private void AddFieldToDictionary(FieldInfo field)
        {
            int id = (int)field.GetValue(field);
            this.Dic.Add(id, GetNameInProperFormat(field.Name, id));
        }

        /// <summary>
        /// get name in right pattern
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        protected virtual string GetNameInProperFormat(string name,int id)
        {
            return name;
        }

        /// <summary>
        /// check whether the contant should be part of List
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected virtual bool IsPartOfKeyValuePairList(int id)
        {
            return true;
        }

        /// <summary>
        /// Get the full list of known types in List of KeyValuePairs
        /// </summary>
        public IList<KeyValuePair<int, string>> GetKeyValuePairList()
        {
            var list = new List<KeyValuePair<int, string>>();

            foreach (var k in this.Dic.Keys)
            {
                if(IsPartOfKeyValuePairList(k))
                {
                    list.Add(new KeyValuePair<int, string>(k, this.Dic[k]));
                }
            }

            return list;
        }

        /// <summary>
        /// Get Name of Type by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetNameById(int id)
        {
            if (this.Dic.ContainsKey(id))
            {
                return this.Dic[id];
            }

            return Invariant($"Unknown ({id})");
        }

        /// <summary>
        /// Get Name of Type by Id (Long)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetNameById(long id)
        {
            return GetNameById(Convert.ToInt32(id));
        }

        /// <summary>
        /// Check whether Id exist or not. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Exists(int id)
        {
            return this.Dic.ContainsKey(id);
        }

        /// <summary>
        /// the types contained in the inheriting class
        /// </summary>
        public IEnumerable<int> Values => Dic.Keys;
    }
}
