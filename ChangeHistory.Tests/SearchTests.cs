using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using ChangeHistory.Core;
using ChangeHistory.Core.Changes;
using ChangeHistory.Core.ChangesManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProtoBuf;
using ProtoBuf.Meta;

namespace ChangeHistory.Tests
{
    [TestClass]
    public class SearchTests
    {
        private class PeopleManager : ChangesManager<Person>
        {
            public PeopleManager(IChangesSearcher changesSearcher) : base(changesSearcher)
            {
                DomainObjectType = new Guid("3134e037-2c98-4288-86a8-139203c2687f");

                Add(1, x => x.FirstName, "Имя", x => x);
                Add(2, x => x.LastName, "Фамилия", x => x);
                Add(3, x => x.Dogs, "Псы", x => string.Join("; ", x.Select(y => y.Name)));
            }
        }

        private class DogManager : ChangesManager<Dog>
        {
            public DogManager(IChangesSearcher changesSearcher) : base(changesSearcher)
            {
                DomainObjectType = new Guid("23c4e037-2c98-4288-86a8-139203c2687a");

                Add(1, x => x.Name, "Кличка", x => x);
            }
        }

        [TestMethod]
        public void ChangesManagerTest()
        {
            // Конфигурируем
            IChangesSearcher changesSearcher = new ChangesSearcher();

            PeopleManager pm = new PeopleManager(changesSearcher);
            pm.Initialize();
            DogManager dm = new DogManager(changesSearcher);
            dm.Initialize();

            // Создаём объекты для сравнения.
            Person oldPerson = new Person
            {
                FirstName = "Maxim",
                LastName = "Solodov",
                Dogs = new[]
                {
                    new Dog() { Name = "Zlobniy Pes 1" },
                    new Dog() { Name = "Zlobniy Pes 2" },
                    new Dog() { Name = "Zlobniy Pes 3" }
                }
            };

            Person newPerson = new Person
            {
                FirstName = "Petr",
                LastName = "Peskov",
                Dogs = new[]
                {
                    new Dog() { Name = "Zlobniy Pes 1" },
                    new Dog() { Name = "Zlobniy Pes 2" },
                   // new Dog() { Name = "Zlobniy Pes 3" }
                }
            };

            // Ищем 
            var changes = changesSearcher.GetChanges(oldPerson, newPerson);

            var fieldChanges = pm.ToData(changes);
            var formattedChanges = pm.GetFormatted(fieldChanges);
        }

        [TestMethod]
        public void ChangesProtobufTest()
        {
            RuntimeTypeModel.Default.Add(typeof(Person), false)
                .Add(1, nameof(Person.FirstName))
                .Add(2, nameof(Person.LastName))
                .Add(3, nameof(Person.Dogs));

            RuntimeTypeModel.Default.Add(typeof(Dog), false)
                .Add(4, nameof(Dog.Name));
            //var a = new SerializationInfo(,);
            //Serializer.Serialize()

            Person person = new Person
            {
                FirstName = "Maxim",
                LastName = "Solodov",
                Dogs = new[]
                {
                    new Dog() { Name = "Zlobniy Pes 1" },
                    new Dog() { Name = "Zlobniy Pes 2" },
                    new Dog() { Name = "Zlobniy Pes 3" }
                }
            };

            var bytes = ProtoSerializerHelper.Serialize(person);
            var data = ProtoSerializerHelper.Deserialize<Person>(bytes);

        }

        [TestMethod]
        public void SearchChangeTest()
        {
            // Создаём объекты для сравнения.
            Person oldPerson = new Person
            {
                FirstName = "Maxim",
                LastName = "Solodov",
                Dogs = new[]
                {
                    new Dog() { Name = "Zlobniy Pes 1" },
                    new Dog() { Name = "Zlobniy Pes 2" },
                    new Dog() { Name = "Zlobniy Pes 3" }
                }
            };

            Person newPerson = new Person
            {
                FirstName = "Maxim",
                LastName = "Peskov",
                Dogs = new[]
                {
                    new Dog() { Name = "Zlobniy Pes 1" },
                    new Dog() { Name = "Zlobniy Pes 2" },
                   // new Dog() { Name = "Zlobniy Pes 3" }
                }
            };

            // Настраиваем контроллер.
            IChangesSearcher controller = new ChangesSearcher();

            //controller.SearchBuilder<Person>()
            //    .Select(x => x.FirstName, 1)
            //    .Select(x => x.LastName, 2)
            //    .Select(x => x.Dogs, 3)
            //    .Build();

            //controller.SearchBuilder<Dog>()
            //    .Select(x => x.Name, 4)
            //    .Build();

            // Получаем изменения.
            var diffs = controller.GetChanges(oldPerson, newPerson);
        }

        private class Person
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public ICollection<Dog> Dogs { get; set; }

            public override bool Equals(object obj)
            {
                return obj is Person person &&
                       FirstName == person.FirstName &&
                       LastName == person.LastName &&
                       EqualityComparer<ICollection<Dog>>.Default.Equals(Dogs, person.Dogs);
            }

            public override int GetHashCode()
            {
                var hashCode = -1411384438;
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FirstName);
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(LastName);
                hashCode = hashCode * -1521134295 + EqualityComparer<ICollection<Dog>>.Default.GetHashCode(Dogs);
                return hashCode;
            }
        }

        private class Dog
        {
            public string Name { get; set; }

            public override bool Equals(object obj)
            {
                return obj is Dog dog &&
                       Name == dog.Name;
            }

            public override int GetHashCode()
            {
                return 539060726 + EqualityComparer<string>.Default.GetHashCode(Name);
            }
        }
    }
}
