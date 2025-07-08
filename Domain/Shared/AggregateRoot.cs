namespace Domain.Shared
{
    public abstract class AggregateRoot : Entity
    {
        public AggregateRoot(Guid id) : base(id) { }
    }
}
