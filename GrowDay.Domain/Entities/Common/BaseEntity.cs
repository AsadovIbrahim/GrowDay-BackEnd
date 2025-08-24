using GrowDay.Domain.Entities.Abstracts;


namespace GrowDay.Domain.Entities.Common
{
    public class BaseEntity : IBaseEntity
    {
        public string Id { get; set; }=Guid.NewGuid().ToString();
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastModifiedAt { get; set; }=DateTime.UtcNow;
    }
}
