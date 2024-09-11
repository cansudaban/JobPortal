namespace JobPortal.Common.Dtos.JobDtos
{
    public class JobCreateDto
    {
        public string Position { get; set; }
        public string Description { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int CompanyId { get; set; }
        public string EmploymentType { get; set; }
        public string Benefits { get; set; }
        public decimal Salary { get; set; }
        public int QualityScore { get; set; }
    }
}
