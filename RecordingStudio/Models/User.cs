using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace RecordingStudio.Models
{
    public class User : IdentityUser<int>
    {
        public List<Order> Orders { get; set; }
    }
}