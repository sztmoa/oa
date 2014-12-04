using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;

namespace SMT.Portal.Common.SmtForm.Framework
{
    /// <summary>
    /// List of possible state for an entity.
    /// </summary>
    public enum EntityState
    {
        /// <summary>
        /// Entity is unchanged
        /// </summary>
        Unchanged = 0,

        /// <summary>
        /// Entity is new
        /// </summary>
        Added = 1,

        /// <summary>
        /// Entity has been modified
        /// </summary>
        Changed = 2,

        /// <summary>
        /// Entity has been deleted
        /// </summary>
        Deleted = 3
    }

    /// <summary>
    /// FBEntityComparer ??????????
    /// </summary>
    public class EntityComparer<T> : IComparer<T>
    {
        private string _FieldName;
        private bool _IsASC;

        public EntityComparer(string FieldName, bool IsASC)
        {
            _FieldName = FieldName;
            _IsASC = IsASC;
        }

        private int Result(int res)
        {
            if (_IsASC)
            {
                return res;
            }
            return -res;
        }

        #region IComparer

        public int Compare(T x, T y)
        {
            Type t = x.GetType();
            System.Reflection.PropertyInfo prop = t.GetProperty(_FieldName);
            object a = null;
            object b = null;
            if (prop != null)
            {
                a = prop.GetValue(x, null);
                b = prop.GetValue(y, null);
            }
            else
            {
                System.Reflection.FieldInfo field = t.GetField(_FieldName);
                if (field == null)
                {
                    throw new Exception(string.Format("There is no \"{0}\" property or field in {1}.", _FieldName, t.ToString()));
                }
                a = field.GetValue(x);
                b = field.GetValue(y);
            }

            if (a != null && b == null)
            {
                return Result(1);
            }

            if (a == null && b != null)
            {
                return Result(-1);
            }

            if (a == null && b == null)
            {
                return 0;
            }

            return Result(((IComparable)a).CompareTo(b));
        }

        #endregion
    }

    [Serializable]
    public class ListBase<T> : List<T>
    {
        #region Fields
        private Dictionary<string, T> dict = new Dictionary<string, T>();
        private bool isModified = false;
        private PropertyInfo prop;
        private string _keyFieldName = GetDefaultFieldName();
        private string _id;
        #endregion


        private static string GetDefaultFieldName()
        {
            string tempFieldName =
                        ((string)
                         (typeof(T).GetProperty("PrimaryKeyField") != null
                              ? typeof(T).GetProperty("PrimaryKeyField").GetValue(null, null)
                              : ""));
            if (tempFieldName != "")
            {
                tempFieldName = tempFieldName.Substring(0, 1).ToUpper() + tempFieldName.Substring(1);
            }
            return tempFieldName;
        }
        public string ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        public string KeyFieldName
        {
            get
            {
                return _keyFieldName;
            }
            set
            {
                _keyFieldName = value;
                this.prop = typeof(T).GetProperty(_keyFieldName);
            }
        }

        public ListBase() { }

        public ListBase(string keyFieldName)
        {
            _keyFieldName = keyFieldName;
            this.prop = typeof(T).GetProperty(_keyFieldName);
        }

        #region New Base Methods
        public new void Add(T item)
        {
            base.Add(item);
            this.isModified = true;
        }

        public new void AddRange(IEnumerable<T> collection)
        {
            base.AddRange(collection);
            this.isModified = true;
        }

        public new void Insert(int index, T item)
        {
            base.Insert(index, item);
            this.isModified = true;
        }

        public new void InsertRange(int index, IEnumerable<T> collection)
        {
            base.InsertRange(index, collection);
            this.isModified = true;
        }

        public new void Remove(T item)
        {
            base.Remove(item);
            this.isModified = true;
        }

        public new int RemoveAll(Predicate<T> match)
        {
            this.isModified = true;
            return base.RemoveAll(match);
        }

        public new void RemoveAt(int index)
        {
            base.RemoveAt(index);
            this.isModified = true;
        }

        public new void RemoveRange(int index, int count)
        {
            base.RemoveRange(index, count);
            this.isModified = true;
        }
        #endregion

        public T Find(string key)
        {
            this.ReBuildDict();

            if (this.dict.ContainsKey(key))
                return this.dict[key];
            else
                return default(T);
        }

        private void ReBuildDict()
        {
            if (this.isModified)
            {
                if (this.prop == null)
                    throw new Exception("找不到相应的KeyFieldName");

                this.dict.Clear();

                if (this.prop.Name != this._keyFieldName)
                {
                    this.prop = typeof(T).GetProperty(_keyFieldName);
                }

                foreach (T item in this)
                {
                    string keyValue = prop.GetValue(item, null).ToString();

                    if (!dict.ContainsKey(keyValue))
                        this.dict.Add(keyValue, item);
                }

                this.isModified = false;
            }
        }

        public T this[string key]
        {
            get
            {
                return this.Find(key);
            }
        }

        public virtual void Sort(string fieldName, bool isASC)
        {
            EntityComparer<T> comparer = new EntityComparer<T>(fieldName, isASC);
            base.Sort(comparer);
        }
    }
}