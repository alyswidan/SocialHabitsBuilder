using SocialHabits.Services;


namespace SocialHabits.Models
{
    public class Habit:TableData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int DaysLeft { get; set; }
        public int Duration { get; set; }

        public double PercentComplete => DaysLeft * 1.0 / Duration;
    }
}
