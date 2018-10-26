using FluentAssertions;
using ImmutableEntityHelper.Tests.Testing.Entities;
using NUnit.Framework;

namespace ImmutableEntityHelper.Tests
{
    [TestFixture]
    public class ImmutableEntityTests
    {
        [Test]
        public void CreateImmutableEntityDirectlyExtendingEntityTest()
        {
            // The immutable Person class extends ImmutableEntity<Person>
            // class and provides the SetName a SetAge methods to get
            // new immutable instances with modified fields.
            var person1 = new Person();

            var person2 = person1
                .SetName("Joseph")
                .SetAge(26);

            // person1 and person2 are totally different
            // instances of Person, but person2 came by copying person1.
            person1.Should().NotBe(person2);
            person2.Name.Should().Be("Joseph");
            person2.Age.Should().Be(26);
        }

        [Test]
        public void CreateImmutableEntityWithCompositionTest()
        {
            // Class ImmutableStudent extends the class Student
            // and provides .SetName and .SetAge methods to get new
            // ImmutableStudent instances from existing instances.
            var person1 = new ImmutableStudent();

            var person2 = person1
                .SetName("Joseph")
                .SetAge(26);

            person1.Should().NotBe(person2);
            person2.Name.Should().Be("Joseph");
            person2.Age.Should().Be(26);
        }
    }
}
