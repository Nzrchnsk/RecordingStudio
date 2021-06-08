using System.Collections;
using System.Collections.Generic;

namespace RecordingStudio.Models
{
    public class Studio
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }

        public List<Price> Prices { get; set; }
    }
}