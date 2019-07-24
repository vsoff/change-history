using ChangeHistory.Core.Changes;
using ChangeHistory.Core.ChangesManager;
using System;

namespace ChangeHistory.Tests.TestClasses
{
    internal class CatManager : ChangesManager<Cat>
    {
        public override Guid DomainObjectType => new Guid("1ee5f742-7e61-4f68-97c7-bb3f6689d542");
        public CatManager(ChangesSearcher changesSearcher) : base(changesSearcher)
        {
            Add(1, x => x.Name, "Кличка", x => x);
            Add(2, x => x.IsLovesCucumbers, "Любит огурцы", x => x ? "Очень" : "Нет");
            Add(3, x => x.BirdthDay, "День рождения", x => $"Родился {x}");
        }
    }
}
