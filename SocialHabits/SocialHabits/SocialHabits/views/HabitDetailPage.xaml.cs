using SocialHabits.Models;
using SocialHabits.viewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SocialHabits.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HabitDetailPage : ContentPage
    {
        public HabitDetailPage(Habit habit = null)
        {     
            InitializeComponent();
            BindingContext = new HabitDetailViewModel(habit);
        }
    }
}
