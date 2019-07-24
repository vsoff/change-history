using System;
using System.Collections.Generic;

namespace ChangeHistory.Tests.TestClasses
{
    internal class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirdthDay { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public ICollection<Dog> Dogs { get; set; }
        public ICollection<Cat> Cats { get; set; }
    }
}
