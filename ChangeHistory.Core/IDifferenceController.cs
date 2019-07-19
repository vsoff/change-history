namespace ChangeHistory.Core
{
    public interface IDifferenceController
    {
        IDifferenceSearchBuilder<T> SearchBuilder<T>();
        PropertyDifference[] GetDifferences<T>(T oldObj, T newObj);
    }
}
