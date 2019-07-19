using System;
using System.Collections.Generic;
using ChangeHistory.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChangeHistory.Tests
{
    [TestClass]
    public class SearchTests
    {
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
                    new Dog() { Name = "Zlobniy Pes 3" }
                }
            };

            // Настраиваем контроллер.
            IChangesSearcher controller = new ChangesSearcher();

            controller.SearchBuilder<Person>()
                .Select(x => x.FirstName, 1)
                .Select(x => x.LastName, 2)
                .Select(x => x.Dogs, 3)
                .Build();

            controller.SearchBuilder<Dog>()
                .Select(x => x.Name, 4)
                .Build();

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
