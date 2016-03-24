using System;
using System.Collections.Generic;
using System.Linq;

namespace Visual_Crawl
{
    public class MultiDimentionalList
    {
        private readonly List<List<ParentChild>> _list;

        public MultiDimentionalList()
        {
            _list = new List<List<ParentChild>>();
        }

        public List<List<ParentChild>> Get()
        {
            return _list;
        }

        public bool Add(ParentChild child)
        {
            if (child.Parent == null)
            {
                AddRoot(child);
                return true;
            }

            int index = GetParentIndex(child.Parent) + 1;

            try
            {
                _list[index].Add(child);
            }
            catch (Exception)
            {
                _list.Add(new List<ParentChild>());
                _list[index].Add(child);
            }

            return true;
        }

        public void AddRoot(ParentChild root)
        {
            _list.Add(new List<ParentChild> {root});
        }

        private int GetParentIndex(VisualLink parent)
        {
            foreach (List<ParentChild> list in _list)
            {
                foreach (ParentChild child in list)
                {
                    if (Equals(child.VisualLink, parent))
                    {
                        return _list.IndexOf(list);
                    }
                }
            }

            throw new Exception("Parent does not exist, yet :)");
        }
    }
}
