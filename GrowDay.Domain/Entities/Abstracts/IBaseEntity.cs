namespace GrowDay.Domain.Entities.Abstracts
{
    public interface IBaseEntity
    {
        public string Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }

    }
}
