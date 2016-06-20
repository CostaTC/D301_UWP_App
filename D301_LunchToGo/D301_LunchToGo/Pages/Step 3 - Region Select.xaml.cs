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
    public sealed partial class StepThree : Page
    {
        // Setup page
        public StepThree()
        {
            this.InitializeComponent();
            SetupPage();
        }

        /// <summary>
        /// Setup defaults for page
        /// </summary>
        private void SetupPage()
        {
            // If there is a delivery time then set it up else setup default
            if (OrderManager.Region != null)
            {
                List<RadioButton> rButtons = new List<RadioButton>();
                rButtons.Add(rboWhanganui);
                rButtons.Add(rboWairarapa);
                rButtons.Add(rboManawatu);

                foreach (RadioButton r in rButtons)
                {
                    if (r.Content.ToString() == OrderManager.Region)
                        r.IsChecked = true;
                }
            }
            else
            {
                rboWhanganui.IsChecked = true;
            }
        }

        // Go back a page on button click
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(StepTwo));
        }

        // Go forward a page on button click
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(StepFour));
        }

        // Set region to radio button that was pressed
        private void radioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton)
            {
                RadioButton r = (RadioButton)sender;
                OrderManager.Region = r.Content.ToString();
            }
        }
    }
}
