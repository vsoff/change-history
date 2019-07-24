using ChangeHistory.Core.Changes;
using ChangeHistory.Core.Entities;
using ProtoBuf;
using ProtoBuf.Meta;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ChangeHistory.Core.ChangesManager
{
    public abstract class ChangesManager<TModel> : IChangesManager<TModel> where TModel : class
    {
        private Dictionary<int, FieldChangeModelPattern<TModel>> _fieldsInfos { get; set; }

        private readonly ChangesSearcher _changesSearcher;

        public Type ObjectType { get; }

        public abstract Guid DomainObjectType { get; }

        public ChangesManager(ChangesSearcher changesSearcher)
        {
            _changesSearcher = changesSearcher ?? throw new ArgumentNullException(nameof(ChangesSearcher));
            _fieldsInfos = new Dictionary<int, FieldChangeModelPattern<TModel>>();
            ObjectType = typeof(TModel);
        }

        public FieldChangeModel[] GetFormatted(FieldChange[] fieldChanges)
        {
            return fieldChanges.Select(x =>
            {
                var fi = _fieldsInfos[x.Field];
                var oldVal = ProtoSerializerHelper.Deserialize(fi.PropertyInfo.PropertyType, x.OldValue, _changesSearcher.ProtobufTypeModel);
                var newVal = ProtoSerializerHelper.Deserialize(fi.PropertyInfo.PropertyType, x.NewValue, _changesSearcher.ProtobufTypeModel);

                return new FieldChangeModel
                {
                    Header = fi.Header,
                    ValueOld = fi.FormatFunc(oldVal),
                    ValueNew = fi.FormatFunc(newVal)
                };
            }).ToArray();
        }

        public FieldChange[] ToData(Change[] changes)
        {
            return changes.Select(x => new FieldChange
            {
                Field = x.Tag,
                NewValue = ProtoSerializerHelper.Serialize(x.ValueNew, _changesSearcher.ProtobufTypeModel),
                OldValue = ProtoSerializerHelper.Serialize(x.ValueOld, _changesSearcher.ProtobufTypeModel)
            }).ToArray();
        }

        public void Initialize(bool isNeedInitProtobufTypeModel = true)
        {
            // Настраиваем поиск по полям.
            var builder = _changesSearcher.SearchBuilder<TModel>();
            foreach (var field in _fieldsInfos)
            {
                builder.Select(field.Key, field.Value.PropertyInfo.Name);
            }
            builder.Build();

            // Настраиваем Protobuf (если необходимо).
            if (isNeedInitProtobufTypeModel)
            {
                MetaType protoBuilder = _changesSearcher.ProtobufTypeModel.Add(typeof(TModel), false);
                foreach (var field in _fieldsInfos)
                {
                    protoBuilder.Add(field.Key, field.Value.PropertyInfo.Name);
                }
            }
        }

        /// <summary>
        /// Добавляет новое поле.
        /// </summary>
        protected void Add<TProp>(int tag, Expression<Func<TModel, TProp>> prop, string header, Func<TProp, string> formatFunc)
        {
            _fieldsInfos.Add(tag, new FieldChangeModelPattern<TModel>()
            {
                Header = header,
                FormatFunc = x => formatFunc((TProp)x),
                PropertyInfo = typeof(TModel).GetProperty(GetPropertyName(prop))
            });
        }

        /// <summary>
        /// Возвращает PropertyName через Expression.
        /// </summary>
        private string GetPropertyName<T, TProp>(Expression<Func<T, TProp>> property)
        {
            LambdaExpression lambda = property;
            MemberExpression memberExpression;

            if (lambda.Body is UnaryExpression)
            {
                UnaryExpression unaryExpression = (UnaryExpression)lambda.Body;
                memberExpression = (MemberExpression)unaryExpression.Operand;
            }
            else
            {
                memberExpression = (MemberExpression)lambda.Body;
            }

            return ((PropertyInfo)memberExpression.Member).Name;
        }
    }
}
