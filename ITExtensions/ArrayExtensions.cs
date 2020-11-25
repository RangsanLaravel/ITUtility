﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ITUtility
{
    public static class ArrayExtensions
    {
        public enum EnumInsertMode
        {
            Before = 0,
            After = 1
        }

        public static T[] Push<T>(this T[] i, T value)
        {
            if (i == null || i.Length == 0)
                return new T[1] { value };
            List<T> r = new List<T>();
            foreach (T e in i)
                r.Add(e);
            r.Add(value);
            return r.ToArray();
        }

        public static T[] Insert<T>(this T[] i, T value, int index, EnumInsertMode insertmode = EnumInsertMode.Before)
        {
            if (i == null || i.Length == 0)
                return new T[1] { value };
            List<T> r = new List<T>();
            for (int n = 0; n < i.Length; n++)
            {
                if (insertmode == EnumInsertMode.Before && n == 0 && index == 0)
                    r.Add(value);
                r.Add(i[n]);
                if (insertmode == EnumInsertMode.Before)
                    if (n == index - 1)
                        r.Add(value);
                    else
                        continue;
                else if (n == index)
                    r.Add(value);
            }
            return r.ToArray();
        }

        public static T[] Reorder<T>(this T[] i, bool KeepNullValue = false)
        {
            if (i == null || i.Length == 0)
                return new T[0];
            List<T> r = new List<T>();
            foreach (T e in i)
                if (e == null && !KeepNullValue)
                    continue;
                else
                    r.Add(e);
            return r.ToArray();
        }

        public static T[] Pop<T>(this T[] i, out T value, int? index = null)
        {
            value = default(T);
            if (i == null || i.Length == 0)
                return new T[0];
            List<T> r = new List<T>();
            for (int n = 0; n < i.Length; n++)
                if ((index == null && n != i.Length - 1) || (index != null && n != index))
                    r.Add(i[n]);
                else
                    value = i[n];
            return r.ToArray();
        }

        public static T[] Add<T>(this T[] i, T value)
        {
            return Push(i, value);
        }

        public static T[] Remove<T>(this T[] i, int index)
        {
            T t;
            return Pop(i, out t, index);
        }

        public static T[] Remove<T>(this T[] i, int[] index)
        {
            if (i == null || i.Length == 0)
                return new T[0];
            if (index == null || index.Length == 0)
                return i;
            List<T> r = new List<T>();
            for (int n = 0; n < i.Length; n++)
                if (!index.Contains(n))
                    r.Add(i[n]);
            return r.ToArray();
        }

        public static T[] Swap<T>(this T[] i, int firstindex, int secondindex)
        {
            if (i == null || i.Length == 0)
                return new T[0];
            T t = i[firstindex];
            i[firstindex] = i[secondindex];
            i[secondindex] = t;
            return i;
        }

        public static dynamic ToDynamic(this Array value)
        {
            IDictionary<string, object> expando = new System.Dynamic.ExpandoObject();

            foreach (System.ComponentModel.PropertyDescriptor property in System.ComponentModel.TypeDescriptor.GetProperties(value.GetType()))
                switch (property.Name)
                {
                    case "IsFixedSize": continue;
                    case "IsReadOnly": continue;
                    case "IsSynchronized": continue;
                    case "Length": continue;
                    case "LongLength": continue;
                    case "Rank": continue;
                    default:
                        expando.Add(property.Name, property.GetValue(value));
                        break;
                }

            return expando as System.Dynamic.ExpandoObject;
        }
    }
}
