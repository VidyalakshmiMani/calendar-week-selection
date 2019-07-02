using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Syncfusion.SfCalendar.XForms;

namespace CalendarWeekSelection
{
    public class CalendarViewModel : INotifyPropertyChanged
    {
        private SelectionRange selectedDays;
        
        /// <summary>
        /// Selected Days
        /// </summary>
        public SelectionRange SelectedDays
        {
            get
            {
                return selectedDays;
            }
            set
            {
                selectedDays = value;
                RaisePropertyChanged("SelectedDays");
            }
        }

        /// <summary>
        /// Property changed event handler
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raising Property changed event
        /// </summary>
        /// <param name="propertyName"></param>
        public void RaisePropertyChanged(string propertyName)
        {
            this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
