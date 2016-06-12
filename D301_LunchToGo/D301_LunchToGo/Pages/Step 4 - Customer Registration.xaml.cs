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
using SQLite.Net;
using SQLite.Net.Attributes;
using System.Diagnostics;
using Windows.Storage;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace D301_LunchToGo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StepFour : Page
    {

        // Connection for SQL server
        SQLiteConnection conn;
        private string path;

        public StepFour()
        {
            this.InitializeComponent();
            SetupPage();

            // Setup database
            
            
            //    conn.CreateTable<CustomerDetailsDB>();
        }

        /// <summary>
        /// Sets up page defaults
        /// </summary>
        private async void SetupPage()
        {
            path = Path.Combine(ApplicationData.Current.LocalFolder.Path, "db.sqlite");
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile notesFile = await storageFolder.CreateFileAsync("db.sqlite", CreationCollisionOption.OpenIfExists);            
            conn = new SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);

            // If the table CustomerDetailsDB is not empty then there is previous saved information
            // To load onto page
            try
            {
                if (conn.Table<CustomerDetailsDB>().Count() > 0)
                {
                    List<CustomerDetailsDB> cList = conn.Query<CustomerDetailsDB>("Select * from CustomerDetailsDB ORDER BY CustName ASC LIMIT 1");

                    foreach (CustomerDetailsDB c in cList)
                    {
                        tbxCustName.Text = c.CustName ?? "";
                        tbxCustAddr.Text = c.CustAddr ?? "";
                        tbxCustPhone.Text = c.CustPhone ?? "";
                        tbxCustCity.Text = c.CustCity ?? "";
                    }

                    rboRememberDetails.IsChecked = true;
                }
                else
                {
                    tbxCustName.Text = OrderManager.CustomerName ?? "";
                    tbxCustAddr.Text = OrderManager.CustomerAddress ?? "";
                    tbxCustPhone.Text = OrderManager.CustomerPhone ?? "";
                    tbxCustCity.Text = OrderManager.CustomerCity ?? "";
                    rboRememberDetails.IsChecked = false;
                }
            }
            catch
            {
                tbxCustName.Text = OrderManager.CustomerName ?? "";
                tbxCustAddr.Text = OrderManager.CustomerAddress ?? "";
                tbxCustPhone.Text = OrderManager.CustomerPhone ?? "";
                tbxCustCity.Text = OrderManager.CustomerCity ?? "";
                rboRememberDetails.IsChecked = false;
            }
            

            // Load in credit card details if any
            tbxCreditCardName.Text = OrderManager.CreditCardName ?? "";
            tbxCreditCardNumber.Text = OrderManager.CreditCardNumber ?? "";
            tbxCCV.Text = OrderManager.CreditCardCCV ?? "";
            tbxExpiryMonth.Text = OrderManager.CreditCardMonth ?? "";
            tbxExpiryYear.Text = OrderManager.CreditCardYear ?? "";

        }

        /// <summary>
        /// Checks fields to ensure none are empty or exceed max and min lengths
        /// </summary>
        /// <returns>True if all fields are correctly entered into</returns>
        private string CheckFields()
        {
            // Check all fields to ensure their validity
            if (String.IsNullOrEmpty(tbxCustName.Text) || String.IsNullOrWhiteSpace(tbxCustName.Text))
                return "Input Customer Name";
            if (String.IsNullOrEmpty(tbxCustAddr.Text) || String.IsNullOrWhiteSpace(tbxCustAddr.Text))
                return "Input Customer Address";
            if (String.IsNullOrEmpty(tbxCustPhone.Text) || String.IsNullOrWhiteSpace(tbxCustPhone.Text))
                return "Input Customer Phone";
            if (String.IsNullOrEmpty(tbxCreditCardName.Text) || String.IsNullOrWhiteSpace(tbxCreditCardName.Text))
                return "Input Credit Card Name";
            if (String.IsNullOrEmpty(tbxCreditCardNumber.Text) || String.IsNullOrWhiteSpace(tbxCreditCardNumber.Text))
                return "Input Credit Card Number";
            if (String.IsNullOrEmpty(tbxCustCity.Text) || String.IsNullOrWhiteSpace(tbxCustCity.Text))
                return "Input Customer City";
            if (String.IsNullOrEmpty(tbxCCV.Text) || String.IsNullOrWhiteSpace(tbxCCV.Text))
                return "Input CCV";
            return ValidateCreditCard();
        }

        /// <summary>
        /// Cleans up credit card details input and confirms whether or not credit card number, ccv and expiry dates are valid
        /// </summary>
        /// <returns>True if credit card is valid</returns>
        private string ValidateCreditCard()
        {
            // Remove non numbers from credit card field and check length
            string cc = CleanString(tbxCreditCardNumber.Text);
            if (cc.Length <= 12 && cc.Length > 19)
                return "Credit Card Number is incorrect length";
            else
                tbxCreditCardNumber.Text = cc;

            // Remove any non numbers from CCV and check length
            cc = CleanString(tbxCCV.Text);
            if (cc.Length != 3)
                return "CCV is incorrect length";
            else
                tbxCCV.Text = cc;

            // Remove any non numbers from Expiry date, check length, and check to ensure credit card has not expired
            cc = CleanString(tbxExpiryMonth.Text);
            if (cc.Length != 2)
                return "Expiry Month is incorrect length";
            else
                tbxExpiryMonth.Text = cc;

            cc = CleanString(tbxExpiryYear.Text);
            if (cc.Length != 2)
                return "Expiry Year is incorrect length";
            else
                tbxExpiryYear.Text = cc;

            // Expiry Check
            if (int.Parse(tbxExpiryYear.Text) < int.Parse(DateTime.Now.ToString("yy")))
                return "Unable to use Credit Card - it has expired";
            if (int.Parse(tbxExpiryMonth.Text) < int.Parse(DateTime.Now.ToString("MM")) && int.Parse(tbxExpiryYear.Text) == int.Parse(DateTime.Now.ToString("yy")))
                return "Unable to use Credit Card - it has expired";

            return "Success";
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
        /// Saves fields to the Ordermanager class for later use
        /// </summary>
        private void SaveFields()
        {
            // Save fields to order manager class
            OrderManager.CustomerName = tbxCustName.Text;
            OrderManager.CustomerAddress = tbxCustAddr.Text;
            OrderManager.CustomerPhone = tbxCustPhone.Text;
            OrderManager.CustomerCity = tbxCustCity.Text;
            OrderManager.CreditCardName = tbxCreditCardName.Text;
            OrderManager.CreditCardNumber = tbxCreditCardNumber.Text;
            OrderManager.CreditCardCCV = tbxCCV.Text;
            OrderManager.CreditCardMonth = tbxExpiryMonth.Text;
            OrderManager.CreditCardYear = tbxExpiryYear.Text;
        }

        /// <summary>
        /// Places customer details into database
        /// </summary>
        private void RememberDetails()
        {

            conn.DeleteAll<CustomerDetailsDB>();

            conn.CreateTable<CustomerDetailsDB>();
            var s = conn.Insert(new CustomerDetailsDB()
            {
                CustName = tbxCustName.Text,
                CustPhone = tbxCustPhone.Text,
                CustAddr = tbxCustAddr.Text,
                CustCity = tbxCustCity.Text
            });

            Debug.WriteLine("SQL Data entered");
        }

        /// <summary>
        /// Deletes any record in the CustomerDetailsDB Database
        /// </summary>
        private void CancelDetails()
        {
            conn.DeleteAll<CustomerDetailsDB>();
            Debug.WriteLine("SQL Data forgotten");
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(StepThree));
        }

        private async void btnNext_Click(object sender, RoutedEventArgs e)
        {
            string validRegistration = CheckFields();
           // If fields are properly entered into then save details and move to next step, else tell user
            if (validRegistration == "Success")
            {
                OrderManager.CreditCardValid = true;
                SaveFields();
                if ((bool)rboRememberDetails.IsChecked)
                    RememberDetails();
                else
                    CancelDetails();
                this.Frame.Navigate(typeof(StepFive));
            }
            else
            {
                OrderManager.CreditCardValid = false;
                var messageDialog = new Windows.UI.Popups.MessageDialog(validRegistration, "Error");
                messageDialog.Commands.Add(new Windows.UI.Popups.UICommand { Label = "Ok", Id = 0 });
                await messageDialog.ShowAsync();
            }
        }

        private void rboRememberDetails_Checked(object sender, RoutedEventArgs e)
        {
            if ((bool)rboRememberDetails.IsChecked)
                OrderManager.RememberDetails = true;
            else
                OrderManager.RememberDetails = false;
        }

    }
}
