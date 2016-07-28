namespace Domain
{
    public class UserDomainModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        public double HourlyWage { get; set; }
        public string JobTitle { get; set; }
        public string CompanyName { get; set; }
    }
}
