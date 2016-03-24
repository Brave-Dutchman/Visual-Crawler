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

        public List<ParentChild> GetByIndex(int index)
        {
            return _list[index];
        }

        public List<List<ParentChildren>> GetParentChildrenList()
        {
            return _list.Select(GetNumbersList).ToList();
        }

        public int GetWightestChild()
        {
            int count = 0;

            foreach (List<ParentChildren> list in GetParentChildrenList())
            {
                foreach (ParentChildren children in list)
                {
                    if (children.VisualLinks.Count > count)
                    {
                        count = children.VisualLinks.Count;
                    }
                }
            }

            return count;
        }

        private List<ParentChildren> GetNumbersList(List<ParentChild> list)
        {
            List<ParentChildren> fors = new List<ParentChildren>();

            foreach (ParentChild s in list)
            {
                ParentChildren obj = fors.FirstOrDefault(x => Equals(x.Parent, s.Parent));

                if (obj == null)
                {
                    fors.Add(new ParentChildren(s.Parent, s.VisualLink));
                }
                else
                {
                    obj.VisualLinks.Add(s.VisualLink);
                }
            }

            return fors; 
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
