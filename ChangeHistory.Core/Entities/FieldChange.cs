namespace ChangeHistory.Core.Entities
{
    public class FieldChange : Entity
    {
        public long UserChangeId { get; set; }

        public int Field { get; set; }

        public byte[] OldValue { get; set; }

        public byte[] NewValue { get; set; }

        public virtual UserChange UserChange { get; set; }
    }
}
