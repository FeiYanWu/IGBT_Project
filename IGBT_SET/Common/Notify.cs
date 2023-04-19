using System.ComponentModel;

namespace IGBT_SET.Common
{
    public class Notify : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected virtual void SetAndNotifyIfChanged<T>(string propertyName, ref T oldValue, T newValue)
        {
            if (oldValue == null && newValue == null)
            {
                return;
            }

            if (oldValue != null && oldValue.Equals(newValue))
            {
                return;
            }

            if (newValue != null && newValue.Equals(oldValue))
            {
                return;
            }

            oldValue = newValue;
            RaisePropertyChanged(propertyName);
        }
    }
}
