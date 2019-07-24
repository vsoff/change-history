using ChangeHistory.Core.Changes;
using ChangeHistory.Core.ChangesManager;
using System;

namespace ChangeHistory.Tests.TestClasses
{
    internal class DogManager : ChangesManager<Dog>
    {
        public override Guid DomainObjectType => new Guid("a9f7e346-e66f-40d8-bfee-c7f64247de4d");
        public DogManager(ChangesSearcher changesSearcher) : base(changesSearcher)
        {
            Add(1, x => x.Name, "Кличка", x => x);
            Add(2, x => x.IsAngry, "Характер", x => x ? "Злая" : "Добрая");
            Add(3, x => x.BirdthDay, "День рождения", x => $"Родился {x}");
        }
    }
}
