using System;

namespace ComissionGeneratorUWP
{
    class _NavigationItem
    {
        public readonly int Id;
        public readonly Type PageType;
        public readonly string Tag;

        public _NavigationItem(int id, Type pageType, string tag)
        {
            this.Id = id;
            this.PageType = pageType;
            this.Tag = tag;
        }
    }
}
