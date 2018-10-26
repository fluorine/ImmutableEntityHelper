using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ImmutableEntityHelper
{
    public class ImmutableEntity<T> where T: class, new()
    {
        public T Entity { get; }

        /// <summary>
        /// Constructor for immutable types that extend this class.
        /// </summary>
        protected ImmutableEntity()
        {
        }

        /// <summary>
        /// Constructor to decorate entities.
        /// </summary>
        /// <param name="entity"></param>
        public ImmutableEntity(T entity)
        {
            Entity = entity;
        }

        /// <summary>
        /// Set a value to a field, but creating a new instance in the process.
        /// </summary>
        /// <typeparam name="U">Field's type</typeparam>
        /// <param name="memberAccessExpression">Expression to locate the member to be assigned.</param>
        /// <param name="value">Value to set in property.</param>
        /// <returns>Instance of <typeparamref name="T"/> with property with value to set.</returns>
        public T SetField<U>(Expression<Func<T, U>> memberAccessExpression, U value)
        {
            var sourceInstance = Entity ?? (this as T);
            var targetInstance = new T();

            var entityType = (Entity == null) 
                ? sourceInstance.GetType() : sourceInstance.GetType().BaseType;

            // Copy all existing properties
            var properties = GetProperties(entityType);
            foreach(var property in properties.Where(p => p.CanRead))
            {
                object propertyValue = property.GetValue(sourceInstance);

                if(!property.CanWrite)
                {
                    var backingField = entityType.GetField($"<{property.Name}>k__BackingField",
                        BindingFlags.Instance | BindingFlags.NonPublic);

                    backingField?.SetValue(targetInstance, propertyValue);
                }
                else
                {
                    property.SetValue(targetInstance, propertyValue);
                }
            }

            // Set the property being "changed"
            var memberInfo = GetMemberInfo(memberAccessExpression);
            var memberName = memberInfo.Name;
            var field = entityType.GetField($"<{memberName}>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
            field.SetValue(targetInstance, value);

            // Return new instance with all fields copied
            return targetInstance;
        }

        private static object GetDefault(Type type)
        {
            if (type.GetTypeInfo().IsValueType)
            {
                return Activator.CreateInstance(type);
            }

            return null;
        }

        private static MemberInfo GetMemberInfo<U>(Expression<Func<T, U>> expression)
        {
            if (expression.Body is MemberExpression member)
                return member.Member;

            throw new ArgumentException("Expression is not a member access", "expression");
        }

        private static PropertyInfo[] GetProperties(Type type)
        {
            return type.GetProperties();
        }
    }
}
