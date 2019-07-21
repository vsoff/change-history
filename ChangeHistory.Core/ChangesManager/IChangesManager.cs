using ChangeHistory.Core.Changes;
using ChangeHistory.Core.Entities;
using System;

namespace ChangeHistory.Core.ChangesManager
{
    public interface IChangesManager<T> where T : class
    {
        Type ObjectType { get; }
        Guid DomainObjectType { get; }
        FieldChangeModel[] GetFormatted(FieldChange[] fieldChanges);
        FieldChange[] ToData(Change[] changes);
        void Initialize(bool needInitProtobufTypeModel = true);
    }
}
