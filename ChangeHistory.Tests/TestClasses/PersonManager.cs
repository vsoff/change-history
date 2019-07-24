using ChangeHistory.Core.Changes;
using ChangeHistory.Core.ChangesManager;
using System;
using System.Linq;

namespace ChangeHistory.Tests.TestClasses
{
    internal class PersonManager : ChangesManager<Person>
    {
        public override Guid DomainObjectType => new Guid("3134e037-2c98-4288-86a8-139203c2687f");
        public PersonManager(ChangesSearcher changesSearcher) : base(changesSearcher)
        {
            Add(1, x => x.FirstName, "Имя", x => x);
            Add(2, x => x.LastName, "Фамилия", x => x);
            Add(3, x => x.BirdthDay, "День рождения", x => $"Родился {x}");
            Add(4, x => x.Height, "Рост", x => $"{x} см.");
            Add(5, x => x.Weight, "Вес", x => $"{x} кг.");
            Add(6, x => x.Dogs, "Собаки", x => string.Join("; ", x.Select(y => y.Name)));
            Add(7, x => x.Cats, "Коты", x => string.Join("; ", x.Select(y => y.Name)));
        }
    }
}
