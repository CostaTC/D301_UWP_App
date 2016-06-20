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
    public sealed partial class StepSix : Page
    {
        // Setup page with delivery time information
        public StepSix()
        {
            this.InitializeComponent();
            System.Diagnostics.Debug.WriteLine(OrderManager.OrderDetails());
            txtDeliveryTime.Text += " " + OrderManager.DeliveryTime;
        }

        // Close application on buttonclick
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }
    }
}
