using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
