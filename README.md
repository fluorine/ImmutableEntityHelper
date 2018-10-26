# ImmutableEntityHelper
A library to create and maniputate immutable objects. 

The composition method allows you to **use existing mutable classes as immutable classes, safe to use**. They are also easy to clone and "modify" using this library.

The inheritance method allows you to **create flexible fluent interfaces** with far less mess.

# Example for the Inheritance Method

If a field is intended to be _modified_ then a new instance of the object is returned with the assigned field value.

This allows the developer to create fluent interfaces with safe immutable entities.

```C#
// The immutable Person class extends ImmutableEntity<Person>
// class and provides the SetName a SetAge methods,
// which return new instances of Person, as invoked.
var person1 = new Person();

var person2 = person1
	.SetName("Joseph")
	.SetAge(26);

// In this case,
// person1 and person2 are totally
// different instances !!!

// person2 came by copying person1
// and "modifying" the Name and Age fields
// using those methods.
```

Following the previous example, the class `Person` extends our class `ImmutableEntity<T>`
and provides the `SetName` and `SetAge` methods in a fluent interface. These methods returns new
immutable instances of the `Person` class, as used.

```C#
class Person : ImmutableEntity<Person>
{
	// Read only properties!
	public string Name { get; }
	public int Age { get; }

	public Person SetName(string name)
	{
		// These fluent interface methods
		// use the SetField method from ImmutableEntity<T>
		// to create and return new immutable instances.
		return SetField((x) => x.Name, name);
	}

	public Person SetAge(int age)
	{
		return SetField((x) => x.Age, age);
	}
}
```

## Example using the Composition Method
Some entities cannot extend `ImmutableEntity<T>`, so this class can also be instantiated
and consumed as an instance in another class.

In this example, we decorate the `Student` class in the class `ImmutableStudent` by adding
some fluent methods like `SetName` or `SetAge`, which return a new class of the the immutable class.

```C#
class ImmutableStudent : Student, IStudent
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
```

To use the previous, just instantiate the `ImmutableStudent` class and use
its instantiation methods for immutable entities. You can also cast `ImmutableStudent`
to `Student`, if required.

This method allows the developer to use mutable classes as immutable objects, preventing side effects.

```C#
var person1 = new ImmutableStudent();

var person2 = person1
	.SetName("Joseph")
	.SetAge(26);

// Different immutable instances,
// but as easy to manipulate
// as strings, which are also immutable.
person1.Should().NotBe(person2);
person2.Name.Should().Be("Joseph");
person2.Age.Should().Be(26);
```

The class used for this example is:

```c#
class Student : IStudent
{
	public string Name { get; }
	public int Age { get; }
}
```

