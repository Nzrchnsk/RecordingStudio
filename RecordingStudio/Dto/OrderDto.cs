using System;
using RecordingStudio.Models;

namespace RecordingStudio.Dto
{
    public class OrderDto
    {
        public int StudioId { get; set; }
        public DateTime Date { get; set; }
        public int FromTime  { get; set; }
        public int ToTime  { get; set; }
    }
}