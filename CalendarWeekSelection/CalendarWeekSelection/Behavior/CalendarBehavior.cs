using System; 
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using Syncfusion.SfCalendar.XForms;
using Xamarin.Forms;

namespace CalendarWeekSelection
{
    /// <summary>
    /// Calendar Behavior class
    /// </summary>
    public class CalendarBehavior : Behavior<ContentPage>
    {
        private CalendarViewModel viewModel;
        private readonly IList<int> weekNumbers = new List<int>();

        /// <summary>
        /// Begins when the behavior attached to the view. 
        /// </summary>
        /// <param name="bindable">bindable value</param>
        protected override void OnAttachedTo(BindableObject bindable)
        {
            base.OnAttachedTo(bindable);
            var calendar = (bindable as MainPage).FindByName<SfCalendar>("calendar");
            if (calendar == null)
            {
                return;
            }

            calendar.SelectionChanged += Calendar_SelectionChanged;
        }

        /// <summary>
        /// Selection Changed event
        /// </summary>
        /// <param name="sender">return the object</param>
        /// <param name="e">Selection Changed Event Args</param>
        private void Calendar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (viewModel == null)
            {
                viewModel = (CalendarViewModel)(sender as SfCalendar).BindingContext;
            }

            if (e.DateAdded.Count == 0)
            {
                viewModel.SelectedDays = GetTotalWeekDays(e.DateAdded[0]);
            }
            else
            {
                if (GetWeekOfYear(e.DateAdded[0]) != GetWeekOfYear(e.DateAdded[e.DateAdded.Count - 1]))
                {
                    viewModel.SelectedDays = GetTotalWeekDays(e.DateAdded[0], e.DateAdded[e.DateAdded.Count - 1]);
                }
                else
                {
                    viewModel.SelectedDays = GetTotalWeekDays(e.DateAdded[0]);
                }
            }
        }

        /// <summary>
        /// Method for Week Days
        /// </summary>
        /// <param name="startDateRange">dateTime value</param>
        /// <param name="endDateRange">endDateTime value</param>
        /// <returns></returns>
        public SelectionRange GetTotalWeekDays(DateTime startDateRange, DateTime? endDateRange = null)
        {
            if (endDateRange == null)
            {
                var days = DayOfWeek.Sunday - startDateRange.DayOfWeek;
                var startDate = startDateRange.AddDays(days);
                ObservableCollection<DateTime> dates = new ObservableCollection<DateTime>();
                for (var i = 0; i < 7; i++)
                {
                    dates.Add(startDate.Date);
                    startDate = startDate.AddDays(1);
                }

                return new SelectionRange(dates[0], dates[dates.Count - 1]);
            }
            else
            {
                ObservableCollection<DateTime> dates = new ObservableCollection<DateTime>();
                var startDayOfWeek = DayOfWeek.Sunday - startDateRange.DayOfWeek;
                var startDate = startDateRange.AddDays(startDayOfWeek);

                var endDayOfWeek = DayOfWeek.Saturday - endDateRange?.DayOfWeek;
                var endDate = endDateRange?.AddDays((int)endDayOfWeek);

                var difference = (endDate - startDate);

                for (var i = 0; i < ((TimeSpan)difference).Days + 1; i++)
                {
                    dates.Add(startDate.Date);
                    startDate = startDate.AddDays(1);
                }

                return new SelectionRange(dates[0], dates[dates.Count - 1]);
            }
        }

        /// <summary>
        /// Method for Get Week
        /// </summary>
        /// <param name="time">time value</param>
        /// <returns></returns>
        public static int GetWeekOfYear(DateTime time)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Sunday);
        }
    }
}
