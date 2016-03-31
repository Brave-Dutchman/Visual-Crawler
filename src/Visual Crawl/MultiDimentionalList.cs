using System;
using System.Collections.Generic;
using Core.Objects;

namespace Visual_Crawl
{
    public class MultiDimentionalList
    {
        private readonly List<List<VisualLink>> _list;

        public MultiDimentionalList()
        {
            _list = new List<List<VisualLink>>();
        }

        public List<List<VisualLink>> Get()
        {
            return _list;
        }

        public List<VisualLink> GetByIndex(int index)
        {
            return _list[index];
        }

        public void RemoveAt(VisualLink link, int index)
        {
            _list[index].Remove(link);

            if (_list[index].Count == 0)
            {
                _list.RemoveAt(index);
            }
        }

        public int Count
        {
            get { return _list.Count; }
        }

        public void Add(VisualLink link)
        {
            if (link.Parent == null)
            {
                _list.Add(new List<VisualLink>());
                _list[0].Add(link);
                return;
            }

            int index = FindParent(link.Parent) + 1;

            try
            {
                _list[index].Add(link);
            }
            catch (Exception)
            {
                _list.Add(new List<VisualLink>());
                _list[index].Add(link);
            }
        }

        public int FindParent(VisualLink parent)
        {
            foreach (List<VisualLink> list in _list)
            {
                foreach (VisualLink visualLink in list)
                {
                    if (Equals(parent, visualLink))
                    {
                        return _list.IndexOf(list);
                    }
                }
            }

            throw new Exception("");
        }
    }
}
