namespace ChangeHistory.Core.Changes
{
    public interface IChangeSearchBuilder<TModel>
    {
        ChangeSearchBuilder<TModel> Select(int tag, string propertyName);
        void Build();
    }
}