namespace JobPortal.Data
{
    public abstract class BaseEntities
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int CreatedUserId { get; set; }
        public int? UpdatedUserId { get; set; }
        public int? DeletedUserId { get; set; }
        public bool IsDeleted { get; set; }

    }
}