using System.ComponentModel;
using System.Reflection;

namespace GitHubActionDemo.Primitive
{
    public abstract class Enumeration<TEnum> : IEquatable<Enumeration<TEnum>>
        where TEnum : Enumeration<TEnum>
    {
        protected Enumeration()
        {
        }

        private static readonly Dictionary<int, TEnum> Enumerations = CreateEnumeration();

        protected Enumeration(int value, string name)
        {
            Id = value;
            Name = name;
        }

        public int Id { get; protected init; }
        public string Name { get; protected init; } = string.Empty;

        public static TEnum? FromValue(int value)
        {
            return Enumerations.TryGetValue(
                value, out TEnum? enumeration) ? enumeration : default;
        }

        public static TEnum? FromName(string name)
        {
            return Enumerations
                .Values
                .SingleOrDefault(e => e.Name == name);
        }

        public bool Equals(Enumeration<TEnum>? other)
        {
            if (other is null) return false;

            return GetType() == other.GetType() && Id == other.Id;
        }

        public override bool Equals(object? obj)
        {
            return obj is Enumeration<TEnum> other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            var type = GetType();
            var memberInfo = type.GetMember(Name); // Use the Name property instead of ToString()

            if (memberInfo.Length > 0)
            {
                var attributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attributes.Length > 0)
                {
                    return ((DescriptionAttribute)attributes[0]).Description;
                }
            }
            return Name; // or Value.ToString() if you prefer to return the integer value
        }

        public static IEnumerable<TEnum> GetAll()
        {
            return Enumerations.Values;
        }

        private static Dictionary<int, TEnum> CreateEnumeration()
        {
            var enumerationType = typeof(TEnum);

            var fieldsForType = enumerationType.GetFields(
                BindingFlags.Public |
                BindingFlags.Static |
                BindingFlags.FlattenHierarchy)
                .Where(fieldInfo =>
                    enumerationType.IsAssignableFrom(fieldInfo.FieldType)
                   )
                .Select(fieldInfo => (TEnum)fieldInfo.GetValue(default)!);

            return fieldsForType.ToDictionary(x => x.Id);
        }

    }
}
