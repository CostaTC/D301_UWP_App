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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace D301_LunchToGo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StepFour : Page
    {
        public StepFour()
        {
            this.InitializeComponent();
            SetupPage();
        }

        /// <summary>
        /// Sets up page defaults
        /// </summary>
        private void SetupPage()
        {
            // DB QUERY HERE - REMEBER DETAILS = Y
            /* then rbo is checked
             * load custname,phone,addr into boxes
             */

            // else load customers into textboxes

            // Load customers into textboxes
            tbxCustName.Text = OrderManager.CustomerName ?? "";
            tbxCustAddr.Text = OrderManager.CustomerAddress ?? "";
            tbxCustPhone.Text = OrderManager.CustomerPhone ?? "";
            tbxCreditCardName.Text = OrderManager.CreditCardName ?? "";
            tbxCreditCardNumber.Text = OrderManager.CreditCardNumber ?? "";
            tbxCCV.Text = OrderManager.CreditCardCCV ?? "";
            tbxExpiryMonth.Text = OrderManager.CreditCardMonth ?? "";
            tbxExpiryYear.Text = OrderManager.CreditCardYear ?? "";
            rboRememberDetails.IsChecked = false;
        }

        /// <summary>
        /// Checks fields to ensure none are empty or exceed max and min lengths
        /// </summary>
        /// <returns>True if all fields are correctly entered into</returns>
        private bool CheckFields()
        {
            // Check all fields to ensure their validity
            if (String.IsNullOrEmpty(tbxCustName.Text) || String.IsNullOrWhiteSpace(tbxCustName.Text))
                return false;
            if (String.IsNullOrEmpty(tbxCustAddr.Text) || String.IsNullOrWhiteSpace(tbxCustAddr.Text))
                return false;
            if (String.IsNullOrEmpty(tbxCustPhone.Text) || String.IsNullOrWhiteSpace(tbxCustPhone.Text))
                return false;
            if (String.IsNullOrEmpty(tbxCreditCardName.Text) || String.IsNullOrWhiteSpace(tbxCreditCardName.Text))
                return false;
            if (String.IsNullOrEmpty(tbxCreditCardNumber.Text) || String.IsNullOrWhiteSpace(tbxCreditCardNumber.Text))
                return false;
            if (String.IsNullOrEmpty(tbxCCV.Text) || String.IsNullOrWhiteSpace(tbxCCV.Text))
                return false;
            if (!ValidateCreditCard())
                return false;

            return true;
        }

        /// <summary>
        /// Saves fields to the Ordermanager class for later use
        /// </summary>
        private void SaveFields()
        {
            // Save fields to order manager class
            OrderManager.CustomerName = tbxCustName.Text;
            OrderManager.CustomerAddress = tbxCustAddr.Text;
            OrderManager.CustomerPhone = tbxCustPhone.Text;
            OrderManager.CreditCardName = tbxCreditCardName.Text;
            OrderManager.CreditCardNumber = tbxCreditCardNumber.Text;
            OrderManager.CreditCardCCV = tbxCCV.Text;
            OrderManager.CreditCardMonth = tbxExpiryMonth.Text;
            OrderManager.CreditCardYear = tbxExpiryYear.Text;
        }

        /// <summary>
        /// Cleans up credit card details input and confirms whether or not credit card number, ccv and expiry dates are valid
        /// </summary>
        /// <returns>True if credit card is valid</returns>
        private bool ValidateCreditCard()
        {
            // Remove non numbers from credit card field and check length
            string cc = CleanString(tbxCreditCardNumber.Text);

            if (cc.Length <= 12 && cc.Length > 19)
                return false;
            else
                tbxCreditCardNumber.Text = cc;

            // Remove any non numbers from CCV and check length
            cc = CleanString(tbxCCV.Text);
            if (cc.Length != 3)
                return false;
            else
                tbxCCV.Text = cc;

            // Remove any non numbers from Expiry date, check length, and check to ensure credit card has not expired
            cc = CleanString(tbxExpiryMonth.Text);
            if (cc.Length != 2)
                return false;
            else
                tbxExpiryMonth.Text = cc;

            cc = CleanString(tbxExpiryYear.Text);
            if (cc.Length != 2)
                return false;
            else
                tbxExpiryYear.Text = cc;

            // Expiry Check
            if (int.Parse(tbxExpiryYear.Text) < int.Parse(DateTime.Now.ToString("yy")))
                return false;
            if (int.Parse(tbxExpiryMonth.Text) < int.Parse(DateTime.Now.ToString("MM")) && int.Parse(tbxExpiryYear.Text) == int.Parse(DateTime.Now.ToString("yy")))
                return false;

            return true;
        }

        /// <summary>
        /// Removes all non numbers from string
        /// </summary>
        /// <param name="toClean">string that is to be cleaned</param>
        /// <returns>a string with only numbers</returns>
        private string CleanString(string toClean)
        {
            toClean = toClean.Trim();
            string cleanedString = "";

            foreach (char c in toClean)
            {
                if (char.IsNumber(c))
                    cleanedString += c;
            }

            return cleanedString;
        }

        /// <summary>
        /// Places customer details into database
        /// </summary>
        private void RememberDetails()
        {
            // Store in database
            /*
             * rememberDetails -> Y or N
             * custname
             * custphone
             * custaddr
             */
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(StepThree));
        }

        private async void btnNext_Click(object sender, RoutedEventArgs e)
        {
           // If fields are properly entered into then save details and move to next step, else tell user
            if (CheckFields())
            {
                OrderManager.CreditCardValid = true;
                SaveFields();
                if ((bool)rboRememberDetails.IsChecked)
                    RememberDetails();
                this.Frame.Navigate(typeof(StepFive));
            }
            else
            {
                OrderManager.CreditCardValid = false;
                var messageDialog = new Windows.UI.Popups.MessageDialog("Please ensure all fields are filled out correctly", "Error");
                messageDialog.Commands.Add(new Windows.UI.Popups.UICommand { Label = "Ok", Id = 0 });
                await messageDialog.ShowAsync();
            }
        }

        private void rboRememberDetails_Checked(object sender, RoutedEventArgs e)
        {
          
        }
    }
}
