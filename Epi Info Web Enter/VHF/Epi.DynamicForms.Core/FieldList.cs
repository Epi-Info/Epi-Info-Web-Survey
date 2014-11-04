using System;
using System.Collections.Generic;
using MvcDynamicForms.Fields;

namespace MvcDynamicForms
{
    /// <summary>
    /// A collection of Field objects.
    /// </summary>
    [Serializable]
    public class FieldList : IList<Field>
    {
        private List<Field> _fields = new List<Field>();
        //public Dictionary<string, int> FieldIndex = new Dictionary<string, int>();
        internal Form Form { get; set; }

        internal FieldList(Form form)
        {
            Form = form;
        }

        #region IList<Field> Members

        public int IndexOf(Field item)
        {
            return _fields.IndexOf(item);
        }

        public void Insert(int index, Field item)
        {
            item.Form = Form;
            _fields.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _fields[index].Form = null;
            _fields.RemoveAt(index);
        }

        public Field this[int index]
        {
            get
            {
                return _fields[index];
            }
            set
            {
                _fields[index] = value;
            }
        }

        #endregion

        #region ICollection<Field> Members

        public void Add(Field item)
        {
            item.Form = Form;
            _fields.Add(item);
        }

        public void Clear()
        {
            _fields.ForEach(x => x.Form = null);
            _fields.Clear();
        }

        public bool Contains(Field item)
        {
            return _fields.Contains(item);
        }

        public void CopyTo(Field[] array, int arrayIndex)
        {
            _fields.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get
            {
                return _fields.Count;
            }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(Field item)
        {
            item.Form = null;
            return _fields.Remove(item);
        }

        #endregion

        #region IEnumerable<Field> Members

        public IEnumerator<Field> GetEnumerator()
        {
            return _fields.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _fields.GetEnumerator();
        }

        #endregion
    }
}
