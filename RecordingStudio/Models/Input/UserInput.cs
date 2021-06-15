using RecordingStudio.Dto;

namespace RecordingStudio.Models.Input
{
    public class UserInput : LoginDto
    {
        public string Email { get; set; }
    }
}