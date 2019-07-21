namespace ChangeHistory.Core.Changes
{
    public interface IChangesSearcher
    {
        Change[] GetChanges<T>(T oldObj, T newObj);
        ChangeSearchBuilder<T> SearchBuilder<T>();
    }
}