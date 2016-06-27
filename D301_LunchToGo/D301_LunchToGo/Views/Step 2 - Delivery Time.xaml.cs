using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using D301_LunchToGo.Models;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace D301_LunchToGo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StepTwo : Page
    {
        // Construct page
        public StepTwo()
        {
            this.InitializeComponent();
            CheckDate();
            SetupPage();  
        }

        /// <summary>
        /// Setup page defaults
        /// </summary>
        private void SetupPage()
        {
            // If there is an order date then set else set as default
            if (OrderManager.DeliveryDate.Year != 0001)
            {
                cdpDatePicker.Date = OrderManager.DeliveryDate;
            }
                
            else
            {
                cdpDatePicker.Date = cdpDatePicker.MinDate;
                DateTimeOffset dt = (DateTimeOffset)cdpDatePicker.Date;
                OrderManager.DeliveryDate = dt.DateTime;
            }

            // If there is a delivery time then set it up else setup default
            if (OrderManager.DeliveryTime != null)
            {
                List<RadioButton> rButtons = new List<RadioButton>();
                rButtons.Add(rbo1145);
                rButtons.Add(rbo115);
                rButtons.Add(rbo1215);
                rButtons.Add(rbo1245);

                foreach (RadioButton r in rButtons)
                {
                    if (r.Content.ToString() == OrderManager.DeliveryTime)
                        r.IsChecked = true;
                }
            }
            else
            {
                rbo1145.IsChecked = true;
            }
        }

        /// <summary>
        /// Checks time restriction on 10:30 orders
        /// </summary>
        private void CheckDate()
        {
            // If before 10:30am then can order now else min date is set to next day
            if ((DateTime.Now.Hour == 10 && DateTime.Now.Minute <= 30) || DateTime.Now.Hour < 10)
            {
                cdpDatePicker.MinDate = DateTime.Now;
                cdpDatePicker.Date = DateTime.Now;
                return;
            }

            cdpDatePicker.MinDate = DateTime.Now.AddDays(1);
            cdpDatePicker.Date = DateTime.Now.AddDays(1);

        }

        // Go to next page on button click
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(StepThree));
        }

        // Go back a page on button click
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(StepOne));
        }

        // Set time to what the user selects
        private void ChangeDate(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton)
            {
                RadioButton r = (RadioButton)sender;
                OrderManager.DeliveryTime = r.Content.ToString();
            }
        }

        // Set date to what the user selects
        private void cdpDatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            try
            {
                DateTimeOffset dt = (DateTimeOffset)cdpDatePicker.Date;
                OrderManager.DeliveryDate = dt.DateTime;
            }
            catch
            {

            }
            
        }
    }
}
