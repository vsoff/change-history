using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using ChangeHistory.Core;
using ChangeHistory.Core.Changes;
using ChangeHistory.Core.ChangesManager;
using ChangeHistory.Tests.TestClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProtoBuf;
using ProtoBuf.Meta;

namespace ChangeHistory.Tests
{
    [TestClass]
    public class SearchTests
    {
        private readonly Person _oldPerson;
        private readonly Person _newPerson;

        public SearchTests()
        {
            _oldPerson = new Person
            {
                FirstName = "Maxim",
                LastName = "Solodov",
                Dogs = new[]
                {
                    new Dog() { Name = "Charlik" },
                    new Dog() { Name = "Sharik" },
                    new Dog() { Name = "Sarik" }
                }
            };

            _newPerson = new Person
            {
                FirstName = "Petr",
                LastName = "Peskov",
                Dogs = new[]
                {
                    new Dog() { Name = "Charlik" },
                    new Dog() { Name = "Sharik" },
                    //new Dog() { Name = "Sarik" }
                }
            };
        }

        [TestMethod]
        public void ChangesManagerTest()
        {
            // Конфигурируем
            ChangesSearcher changesSearcher = new ChangesSearcher(RuntimeTypeModel.Create());

            PersonManager pm = new PersonManager(changesSearcher);
            DogManager dm = new DogManager(changesSearcher);
            pm.Initialize();
            dm.Initialize();

            // Ищем 
            var changes = changesSearcher.GetChanges(_oldPerson, _newPerson);

            var fieldChanges = pm.ToData(changes);
            var formattedChanges = pm.GetFormatted(fieldChanges);
        }

        [TestMethod]
        public void ChangesProtobufTest()
        {
            var typeModel = TypeModel.Create();
            typeModel.Add(typeof(Person), false)
                .Add(1, nameof(Person.FirstName))
                .Add(2, nameof(Person.LastName))
                .Add(3, nameof(Person.Dogs));

            typeModel.Add(typeof(Dog), false)
                .Add(4, nameof(Dog.Name));

            Person person = new Person
            {
                FirstName = "Maxim",
                LastName = "Solodov",
                Dogs = new[]
                {
                    new Dog() { Name = "Charlik" },
                    new Dog() { Name = "Sharik" },
                    new Dog() { Name = "Sarik" }
                }
            };

            var bytes = ProtoSerializerHelper.Serialize(person, typeModel);

            var data = ProtoSerializerHelper.Deserialize<Person>(bytes, typeModel);
        }

        [TestMethod]
        public void SearchChangeTest()
        {
            // Настраиваем контроллер.
            IChangesSearcher controller = new ChangesSearcher();

            controller.SearchBuilder<Person>()
                .Select(1, nameof(Person.FirstName))
                .Select(2, nameof(Person.LastName))
                .Select(3, nameof(Person.Dogs))
                .Build();

            controller.SearchBuilder<Dog>()
                .Select(4, nameof(Dog.Name))
                .Build();

            // Получаем изменения.
            var diffs = controller.GetChanges(_oldPerson, _newPerson);
        }
    }
}
