﻿namespace JobPortal.Common.Dtos.CompanyDtos
{
    public class CompanyDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public int JobPostingLimit { get; set; }
    }
}
