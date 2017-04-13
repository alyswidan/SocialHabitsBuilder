using System;
using System.Diagnostics;
using System.Threading.Tasks;
using SocialHabits;
using SocialHabits.Abstractions;
using SocialHabits.views;
using Xamarin.Forms;

namespace SocialHabits.viewModels
{
    public class LoginPageViewModel:BaseViewModel
    {
        public LoginPageViewModel()
        {
            Title = "Social Habits Builder";
        }
        Command loginCmd;
        public Command LoginCommand => loginCmd ?? (loginCmd = new Command(async () => await ExecuteLoginCommand().ConfigureAwait(false)));
        async Task ExecuteLoginCommand()
        {
            if(IsBusy)return;
            IsBusy = true;

            try
            {
                await App.CloudService.LoginAsync();
                Debug.WriteLine("------------------herre");
                var h = new HabitsListPage();
                //Application.Current.MainPage = new NavigationPage(new LoginPage());
                //await Application.Current.MainPage.DisplayAlert("an alert ", "login was successfull", "ok");
                 //Application.Current.MainPage.Navigation.RemovePage(Application.Current.MainPage);
                await Task.Delay(100);
                Application.Current.MainPage = new NavigationPage(h);

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Login] Error = {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }




    }
}