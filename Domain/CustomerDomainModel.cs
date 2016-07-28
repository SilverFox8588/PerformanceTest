namespace Domain
{
    public class CustomerDomainModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RegionId { get; set; }
        public int? PriorityId { get; set; }
        public byte Type { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }

        //extended
        public string PriorityName { get; set; }
        public string RegionName { get; set; }
        public double Hours { get; set; }
        public double Cost { get; set; }

        public UserDomainModel FirstUser { get; set; }
    }
}
