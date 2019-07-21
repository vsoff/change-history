using System;
using System.Collections.Generic;

namespace ChangeHistory.Core.Entities
{
    public class UserChange : Entity
    {
        public Guid DomainObjectType { get; set; }

        public long DomainObjectId { get; set; }

        public int EditorId { get; set; }

        public string EditorName { get; set; }

        public DateTime ChangeDateUtc { get; set; }

        public virtual ICollection<FieldChange> Changes { get; set; }
    }
}
