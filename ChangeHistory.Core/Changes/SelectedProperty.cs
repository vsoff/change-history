using System;
using System.Reflection;

namespace ChangeHistory.Core.Changes
{
    internal class SelectedProperty
    {
        public SelectedProperty(PropertyInfo info, int tag)
        {
            Info = info;
            Tag = tag;
        }

        public Type Type => Info.PropertyType;
        public PropertyInfo Info { get; set; }
        public int Tag { get; set; }
    }
}
