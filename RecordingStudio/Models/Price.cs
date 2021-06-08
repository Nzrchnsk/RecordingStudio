using System.Collections.Generic;

namespace RecordingStudio.Models
{
    public class Price
    {
        public int Id { get; set; }
        public double CostPerHours { get; set; }

        public int StudioId { get; set; }
        public Studio Studio { get; set; }

        public List<Order> Orders { get; set; }
    }
}