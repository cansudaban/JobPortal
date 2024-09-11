namespace JobPortal.Data.Models
{
    public class Company : BaseEntities
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public int JobPostingLimit { get; set; }

        // Company ile ilişkili olan ilanlar
        public ICollection<Job> Jobs { get; set; }
    }

}
