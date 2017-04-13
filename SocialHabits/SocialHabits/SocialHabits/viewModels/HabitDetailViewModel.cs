using System;
using System.Diagnostics;
using System.Threading.Tasks;
using SocialHabits.Abstractions;
using SocialHabits.Models;
using Xamarin.Forms;

namespace SocialHabits.viewModels
{
    public class HabitDetailViewModel:BaseViewModel
    {
 

            ICloudTable<Habit> table = App.CloudService.GetTable<Habit>();

            public HabitDetailViewModel(Habit habit = null)
            {
                if (habit != null)
                {
                    Habit = habit;
                    Title = habit.Name;
                }
                else
                {
                    Habit = new Habit() { Name = "Start a new Habit", Description = "",Duration = 0,DaysLeft = 0};
                    Title = "New Habit";
                }
            }

            public Habit Habit { get; set; }

            Command cmdSave;
            public Command SaveCommand => cmdSave ?? (cmdSave = new Command(async () => await ExecuteSaveCommand()));

            async Task ExecuteSaveCommand()
            {
                if (IsBusy)
                    return;
                IsBusy = true;

                try
                {
                    if (Habit.Id == null)
                    {
                        await table.CreateItemAsync(Habit);
                    }
                    else
                    {
                        await table.UpdateItemAsync(Habit);
                    }
                    MessagingCenter.Send<HabitDetailViewModel>(this, "HabitsChanged");
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[Habit Detail] Save error: {ex.Message}");
                }
                finally
                {
                    IsBusy = false;
                }
            }

            Command cmdDelete;
            public Command DeleteCommand => cmdDelete ?? (cmdDelete = new Command(async () => await ExecuteDeleteCommand()));

            async Task ExecuteDeleteCommand()
            {
                if (IsBusy)
                    return;
                IsBusy = true;

                try
                {
                    if (Habit.Id != null)
                    {
                        await table.DeleteItemAsync(Habit);
                    }
                    MessagingCenter.Send<HabitDetailViewModel>(this, "HabitsChanged");
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[Habit Detail] Save error: {ex.Message}");
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }
    }
