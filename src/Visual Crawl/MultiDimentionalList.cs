using System;
using System.Collections.Generic;

namespace Visual_Crawl
{
    public class MultiDimentionalList
    {
        private readonly List<List<VisualLink>> _list;

        public MultiDimentionalList()
        {
            _list = new List<List<VisualLink>>();
        }

        /// <summary>
        /// Get the VisualLink lsit
        /// </summary>
        /// <returns></returns>
        public List<List<VisualLink>> Get()
        {
            return _list;
        }

        /// <summary>
        /// Get the a VisualLink list
        /// </summary>
        /// <param name="index">the index of the list</param>
        /// <returns>The list of VisualList</returns>
        public List<VisualLink> GetByIndex(int index)
        {
            return _list[index];
        }

        /// <summary>
        /// Removes a VisualLink 
        /// </summary>
        /// <param name="visualLink">the VisualLink to remove</param>
        /// <param name="index">the index of the list</param>
        public void RemoveAt(VisualLink visualLink, int index)
        {
            try
            {
                _list[index].Remove(visualLink);

                if (_list[index].Count == 0)
                {
                    _list.RemoveAt(index);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        /// The total amount of lists
        /// </summary>
        public int Count
        {
            get { return _list.Count; }
        }

        /// <summary>
        /// Adds a new visual link
        /// </summary>
        /// <param name="visualLink">The VisualLink to add</param>
        public void Add(VisualLink visualLink)
        {
            //true this is the root node
            if (visualLink.Parent == null)
            {
                _list.Add(new List<VisualLink>());
                _list[0].Add(visualLink);
                return;
            }

            // Gets the index of the list that contains the parent,
            // Add + 1 to that
            int index = FindParentList(visualLink.Parent) + 1;

            try
            {
                _list[index].Add(visualLink);
            }
            catch (Exception)
            {
                _list.Add(new List<VisualLink>());
                _list[index].Add(visualLink);
            }
        }

        /// <summary>
        /// Finds the list that contains the parent
        /// </summary>
        /// <param name="parent">The parent to find</param>
        /// <returns>the index of the list that contains the parent</returns>
        public int FindParentList(VisualLink parent)
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

            throw new Exception("Parent was not found");
        }
    }
}
