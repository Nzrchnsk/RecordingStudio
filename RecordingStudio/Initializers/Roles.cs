using System.Collections.Generic;

namespace RecordingStudio.Initializers
{
    public static class Rolse
    {
        public const string Admin = "Admin";
        public const string User = "User";
        public static readonly List<string> AllRoles = new List<string>
        {
            Admin,
            User, 
        };
    }
}