namespace ImmutableEntityHelper.Tests.Testing.Entities
{
    public class Person : ImmutableEntity<Person>
    {
        public string Name { get; }

        public int Age { get; }


        public Person SetName(string name)
        {
            return SetField((x) => x.Name, name);
        }

        public Person SetAge(int age)
        {
            return SetField((x) => x.Age, age);
        }
    }
}
