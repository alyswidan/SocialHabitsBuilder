using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SocialHabits.Abstractions
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region private variables
        private string _title="";
        private bool _isBusy;
        #endregion

        public string Title
        {
            get { return _title; }
            set {SetProperty(ref _title,value); }
        }

      
        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }


        protected void SetProperty<T>(ref T store, T value, [CallerMemberName] string propName=null)
        {

            if (EqualityComparer<T>.Default.Equals(store, value))return;
            store = value;
            
            OnPropertyChanged(propName);
        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}