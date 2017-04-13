using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SocialHabits.Abstractions;
using SocialHabits.Models;
using SocialHabits.views;

using Xamarin.Forms;

namespace SocialHabits.viewModels
{
    public class HabitsListViewModel:BaseViewModel
    {
        public HabitsListViewModel()
        {
            Habits = new ObservableCollection<Habit>();
            Title = "Unauthorized user";
       
            RefreshList();
        }

        private ObservableCollection<Habit> _habits;

        public ObservableCollection<Habit> Habits
        {
            get { return _habits; }
            set { SetProperty(ref _habits,value); }
        }

        private Habit _selectedHabit;

        public Habit SelectedHabit
        {
            get { return _selectedHabit; }
            set
            {
                SetProperty(ref _selectedHabit,value);
                if (_selectedHabit != null)
                {
                    Application.Current.MainPage.Navigation.PushAsync(new HabitDetailPage(_selectedHabit));
                    SelectedHabit = null;
                }
            }
        }

        Command _refreshCmd;
        public Command RefreshCommand => _refreshCmd ?? (_refreshCmd = new Command(async () => await ExecuteRefreshCommand()));

        async Task ExecuteRefreshCommand()
        {
            if(IsBusy)return;
            IsBusy = true;

            try
            {
                
                var table = App.CloudService.GetTable<Habit>();
                var identity = await App.CloudService.GetIdentityAsync();
                var list = await table.ReadAllItemsAsync();
                if (identity == null)
                {
                    Debug.WriteLine("--------------------------------->nooooooo");
                }
                Debug.WriteLine(identity?.UserId);
                var name = identity?.UserClaims.FirstOrDefault(i => Regex.IsMatch(i.Type,"/name$")).Value;
                Title = $"{name}'s habits";
                Habits.Clear();
                foreach (var item in list)
                {
                    Debug.WriteLine("--------------------------->"+item.Name);
                    Habits.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(Habits.Count);
                Debug.WriteLine($"[HabitsList] Error loading items: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }


        Command _addNewCmd;
        public Command AddNewHabitCommand => _addNewCmd ?? (_addNewCmd = new Command(async () => await ExecuteAddNewHabitCommand()));

        async Task ExecuteAddNewHabitCommand()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                await Application.Current.MainPage.Navigation.PushAsync(new HabitDetailPage());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[TaskList] Error in AddNewItem: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        Command logoutcmd;
        public Command LogoutCommand => logoutcmd ?? (logoutcmd = new Command(async () => await ExecuteLogoutCommand().ConfigureAwait(false)));

        async Task ExecuteLogoutCommand()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                await App.CloudService.LoginAsync();
                var h = new LoginPage();
                await Task.Delay(100);
                Application.Current.MainPage = new NavigationPage(h);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Logout] Error = {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task RefreshList()
        {
            await ExecuteRefreshCommand();

            /*
             when the habit changes in the habit detail page inform the habits list 
             to refresh
             */
            MessagingCenter.Subscribe<HabitDetailPage>(this, "HabitsChanged", async (sender) =>
            {
                Debug.WriteLine("sending updates backk");
                await ExecuteRefreshCommand();
            });
        }
    }
}