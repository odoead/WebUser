using System;
using WebUser.Domain.entities;

namespace WebUser.shared
{
    public class ObjectID<T> : IComparable<ObjectID<T>>, IEquatable<ObjectID<T>> where T : class
    {
        public int Value { get; }
        public ObjectID(int Id)
        {
            Value = Id;
        }
        public int CompareTo(ObjectID<T>? other)
        {
            return Value.CompareTo(other.Value); ;
        }

        public bool Equals(ObjectID<T>? other)
        {
            return Value.Equals(other.Value) ;
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is ObjectID<T> other && Equals(other);
        }
        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => Value.ToString();
        public static bool operator ==(ObjectID<T> a, ObjectID<T> b) => a.CompareTo(b) == 0;
        public static bool operator !=(ObjectID<T> a, ObjectID<T> b) => !(a == b);
    }
}
