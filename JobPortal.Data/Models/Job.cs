namespace JobPortal.Data.Models
{
    public class Job : BaseEntities
    {
        public int CompanyId { get; set; }
        public string Position { get; set; }
        public string Description { get; set; }
        public DateTime ExpirationDate { get; set; } = DateTime.Now.AddDays(15); // Yayında kalma süresi
        public int QualityScore { get; set; }

        // İlişki: Bir iş bir şirkete ait olur
        public Company Company { get; set; }

        // Kullanıcı ile ilişki
        public int UserId { get; set; }  // Kullanıcı ID'si
        public User User { get; set; }   // Kullanıcı referansı

        // İlan kalite skoru hesaplaması için opsiyonel alanlar
        public string? Benefits { get; set; } // Opsiyonel yan haklar
        public string? EmploymentType { get; set; } // Çalışma türü (Tam zamanlı, part-time)
        public decimal? Salary { get; set; } // Opsiyonel ücret bilgisi
    }
}
