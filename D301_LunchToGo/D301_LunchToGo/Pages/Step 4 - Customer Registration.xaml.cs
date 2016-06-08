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

        private void SetupPage()
        {
            // DB QUERY HERE - REMEBER DETAILS = Y
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
        }

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
            if (tbxCCV.Text.Length != 3)
                return false;
            if (tbxExpiryMonth.Text.Length != 2)
                return false;
            if (tbxExpiryYear.Text.Length != 2)
                return false;
            if (!ValidateCreditCard())
                return false;

            return true;
        }

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

        private bool ValidateCreditCard()
        {
            // Remove non numbers from credit card field and check length
            string cc = "";
            tbxCreditCardNumber.Text.Trim();

            foreach (char c in tbxCreditCardNumber.Text)
            {
                if (char.IsNumber(c))
                    cc += c;
            }
            
            //If length falls within credit card number length
            if (cc.Length > 12 && cc.Length < 20)
            {
                tbxCreditCardNumber.Text = cc;
                return true;
            }

            return false;
        }

        private void RememberDetails()
        {
            // Store in database
            /*
             * rememberDetails
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
                var messageDialog = new Windows.UI.Popups.MessageDialog("Please ensure all fields are filled out", "Error");
                messageDialog.Commands.Add(new Windows.UI.Popups.UICommand { Label = "Ok", Id = 0 });
                await messageDialog.ShowAsync();
            }
        }

        private void rboRememberDetails_Checked(object sender, RoutedEventArgs e)
        {
          
        }
    }
}
