using System;

namespace ChangeHistory.Core
{
    public class PropertyDifference
    {
        public Type PropertyType { get; set; }
        public object ValueOld { get; set; }
        public object ValueNew { get; set; }
        public int Tag { get; set; }


        public PropertyDifference(Type type, object valueOld, object valueNew, int tag)
        {
            PropertyType = type;
            ValueOld = valueOld;
            ValueNew = valueNew;
            Tag = tag;
        }
    }
}
