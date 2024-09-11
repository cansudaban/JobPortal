namespace JobPortal.Common.Dtos.CompanyDtos
{
    public class CompanyCreateDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public int JobPostingLimit { get; set; }
    }
}
