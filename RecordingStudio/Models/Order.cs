using System;

namespace RecordingStudio.Models
{
    public class Order
    {
        public int Id { get; set; }
        
        public DateTime FromDateTime { get; set; }
        public DateTime ToDateTime { get; set; }
        
        /// <summary>
        /// true - оплачено
        /// false - не оплачено
        /// </summary>
        public bool PaymentStatus { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
        
        public int StudioId { get; set; }
        public Studio Studio { get; set; }

    }
}