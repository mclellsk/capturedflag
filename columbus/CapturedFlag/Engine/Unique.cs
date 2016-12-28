using System;
using System.Collections.Generic;

namespace CapturedFlag.Engine
{
    public static class ObjectEx
    {
        public static Unique<T> Unique<T>(this T obj, string id)
        {
            var u = new Unique<T>(id);
            u.Value = obj;
            return u;
        }
    }

    public class UniqueList<T> : List<Unique<T>>
    {
        public T GetValue(string id)
        {
            var o = this.Find(p => p.ID == id);
            return o.Value;
        }

        public void SetValue(string id, T v)
        {
            var o = this.Find(p => p.ID == id);
            if (o != null)
            {
                o.Value = v;
            }
            else
            {
                var n = new Unique<T>(id);
                n.Value = v;
                this.Add(n);
            }
        }

        public void Remove(string id)
        {
            this.RemoveAll(p => p.ID == id);
        }
    }

    public class Unique<T>
    {
        string id;
        T v;

        public string ID
        {
            get { return id; }
        }

        public T Value
        {
            get { return v; }
            set { v = value; }
        }
        
        public Unique()
        {
            this.id = Guid.NewGuid().ToString();
        }

        public Unique(string id)
        {
            this.id = id;
        }
    }
}
