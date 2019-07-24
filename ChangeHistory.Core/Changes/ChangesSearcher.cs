using ProtoBuf.Meta;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChangeHistory.Core.Changes
{
    public class ChangesSearcher : IChangesSearcher
    {
        internal RuntimeTypeModel ProtobufTypeModel { get; set; }

        private readonly Dictionary<Type, SelectedProperty[]> _propertiesByType;

        public ChangesSearcher(RuntimeTypeModel typeModel = null)
        {
            ProtobufTypeModel = typeModel ?? RuntimeTypeModel.Default;
            _propertiesByType = new Dictionary<Type, SelectedProperty[]>();
        }

        internal void SetChangeSearchSetting<T>(ICollection<SelectedProperty> properties)
            => _propertiesByType.Add(typeof(T), properties.Select(x => x).ToArray());

        public ChangeSearchBuilder<T> SearchBuilder<T>() => new ChangeSearchBuilder<T>(this);

        public Change[] GetChanges<T>(T oldObj, T newObj) => GetChanges(typeof(T), oldObj, newObj);

        private Change[] GetChanges(Type type, object oldObj, object newObj)
        {
            List<Change> diffs = new List<Change>();

            foreach (var prop in _propertiesByType[type])
            {
                var oldVal = prop.Info.GetValue(oldObj);
                var newVal = prop.Info.GetValue(newObj);

                if (_propertiesByType.ContainsKey(prop.Type))
                {
                    diffs.AddRange(GetChanges(prop.Type, oldVal, newVal));
                }
                else
                {
                    if (PrimitiveEquals(prop.Type, oldVal, newVal))
                        continue;

                    var diff = new Change(prop.Type, oldVal, newVal, prop.Tag);
                    diffs.Add(diff);
                }
            }

            return diffs.ToArray();
        }

        #region Equals 

        private bool PrimitiveEquals(Type type, object obj1, object obj2)
        {
            if (obj1 == null || obj2 == null)
                return obj1 == null && obj2 == null;

            if (obj1.Equals(obj2))
                return true;

            if (type.IsArray)
                return CompareArrays(obj1 as Array, obj2 as Array);

            if (type.IsGenericType)
            {
                var genericType = type.GetGenericTypeDefinition();
                if (genericType != typeof(ICollection<>))
                    throw new Exception($"Generic type {genericType} is not supported");

                return CompareArrays(obj1 as Array, obj2 as Array);
            }


            return false;
        }

        private bool CompareArrays(Array arr1, Array arr2)
        {
            if (arr1.Length != arr2.Length)
                return false;

            var en1 = arr1.GetEnumerator();
            var en2 = arr2.GetEnumerator();

            while (en1.MoveNext() && en2.MoveNext())
            {
                if (!PrimitiveEquals(en1.Current.GetType(), en1.Current, en2.Current))
                    return false;
            }

            return true;
        }

        #endregion
    }
}
