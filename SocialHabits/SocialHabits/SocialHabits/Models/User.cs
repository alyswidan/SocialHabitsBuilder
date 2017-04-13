
using System.Collections.Generic;


namespace SocialHabits.Models
{
    public class User

    {
        public string Name { get; set; }
        public string Bio { get; set; }
        public List<User> Connections { get; set; }
        public List<Habit> Habits { get; set; }
        public string ImageURL { get; set; }
 }
}
