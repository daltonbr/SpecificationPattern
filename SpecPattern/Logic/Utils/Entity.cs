namespace Logic.Utils;

public abstract class Entity
{
    public Guid Guid { get; }

    protected Entity()
    {
        Guid = Guid.NewGuid();
    }

    public override bool Equals(object? obj)
    {
        var other = obj as Entity;

        if (ReferenceEquals(other, null))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (GetType() != other.GetType())
        {
            return false;
        }

        if (this.Guid == Guid.Empty || other.Guid == Guid.Empty)
        {
            return false;
        }

        return Guid == other.Guid;
    }

    public static bool operator ==(Entity a, Entity b)
    {
        if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
        {
            return true;
        }

        if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
        {
            return false;
        }

        return a.Equals(b);
    }

    public static bool operator !=(Entity a, Entity b)
    {
        return !(a == b);
    }

    public override int GetHashCode()
    {
        return (this.GetType().ToString() + Guid).GetHashCode();
    }
}
