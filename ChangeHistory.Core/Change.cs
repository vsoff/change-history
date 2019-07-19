using System;

namespace ChangeHistory.Core
{
    public class Change
    {
        public Type PropertyType { get; set; }
        public object ValueOld { get; set; }
        public object ValueNew { get; set; }
        public int Tag { get; set; }


        public Change(Type type, object valueOld, object valueNew, int tag)
        {
            PropertyType = type;
            ValueOld = valueOld;
            ValueNew = valueNew;
            Tag = tag;
        }
    }
}
