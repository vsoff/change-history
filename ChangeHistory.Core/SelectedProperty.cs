using System;

namespace ChangeHistory.Core
{
    internal class SelectedProperty
    {
        public Type Type { get; set; }
        public int Tag { get; set; }
        public Func<object, object> Func { get; set; }

        public SelectedProperty(int tag, Func<object, object> func, Type propertyType)
        {
            Tag = tag;
            Func = func;
            Type = propertyType;
        }
    }
}
