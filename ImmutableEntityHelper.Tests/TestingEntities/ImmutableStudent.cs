namespace ImmutableEntityHelper.Tests.Testing.Entities
{
    public class ImmutableStudent : Student
    {
        private ImmutableEntity<ImmutableStudent> _immutableEntity;

        public ImmutableStudent()
        {
            _immutableEntity = new ImmutableEntity<ImmutableStudent>(this);
        }

        public ImmutableStudent SetName(string name)
        {
            return _immutableEntity.SetField((x) => x.Name, name);
        }

        public ImmutableStudent SetAge(int age)
        {
            return _immutableEntity.SetField((x) => x.Age, age);
        }
    }
}
