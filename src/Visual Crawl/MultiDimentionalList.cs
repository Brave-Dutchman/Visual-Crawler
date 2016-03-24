using System;
using System.Collections.Generic;

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

        public bool Add(VisualLink parent, VisualLink child)
        {
            if (parent == null)
            {
                AddRoot(new ParentChild(null, child, 0));
                return true;
            }

            int index = GetParentListIndex(parent) + 1;

            try
            {
                _list[index].Add(new ParentChild(parent, child, GetParentCHildren(parent)));
            }
            catch (Exception)
            {
                _list.Add(new List<ParentChild>());
                _list[index].Add(new ParentChild(parent, child, 0));
            }

            return true;
        }

        private int GetParentCHildren(VisualLink parent)
        {
            int count = 0;

            foreach (List<ParentChild> list in _list)
            {
                foreach (ParentChild parentChild in list)
                {
                    if (Equals(parentChild.Parent, parent))
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        public void AddRoot(ParentChild root)
        {
            _list.Add(new List<ParentChild> {root});
        }

        public void ResetByParentIndex(VisualLink parent)
        {
            int index = GetParentListIndex(parent);

            bool found = false;
            int childIndex = -1;

            foreach (ParentChild child in _list[index])
            {
                if (found && child.ChildIndex > childIndex)
                {
                    child.VisualLink.Left = (_list[index][child.ChildIndex - 1].VisualLink.Left + MainWindow.DefaultLeftMargin) -50;
                }

                if (Equals(child.VisualLink, parent))
                {
                    childIndex = child.ChildIndex;
                    found = true;
                }
            }
        }

        private int GetParentListIndex(VisualLink parent)
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
