using System;

namespace RecordingStudio.Models
{
    public class Order
    {
        public int Id { get; set; }
        
        public DateTime DateTime { get; set; }
        public bool IsActive { get; set; }
        
        /// <summary>
        /// true - оплачено
        /// false - не оплачено
        /// </summary>
        public bool PaymentStatus { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int PriceId { get; set; }
        public Price Price { get; set; }
    }
}