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
using D301_LunchToGo.Models;

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
            // Sets up database connection
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
            // Trim fields
            tbxCustName.Text = tbxCustName.Text.Trim();
            tbxCustAddr.Text = tbxCustAddr.Text.Trim();
            tbxCustPhone.Text = tbxCustPhone.Text.Trim();
            tbxCustCity.Text = tbxCustCity.Text.Trim();

            // Check all fields to ensure their validity
            if (String.IsNullOrWhiteSpace(tbxCustName.Text))
                return "Input Customer Name";
            if (String.IsNullOrWhiteSpace(tbxCustAddr.Text))
                return "Input Customer Address";
            if (String.IsNullOrWhiteSpace(tbxCustPhone.Text))
                return "Input Customer Phone";
            if (String.IsNullOrWhiteSpace(tbxCreditCardName.Text))
                return "Input Credit Card Name";
            if (String.IsNullOrWhiteSpace(tbxCreditCardNumber.Text))
                return "Input Credit Card Number";
            if (String.IsNullOrWhiteSpace(tbxCustCity.Text))
                return "Input Customer City";
            if (String.IsNullOrWhiteSpace(tbxCCV.Text))
                return "Input CCV";
            if (tbxCustPhone.Text.Length < 7)
                return "Phone number does not exceed minimum length";
            if (tbxCustPhone.Text.Length > 15)
                return "Phone number exceeds maximum length";
            if (tbxCreditCardName.Text.Length > 30)
                return "Credit card name exceeds maximum length";
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
                return "Credit Card Number too short";
            else
                tbxCreditCardNumber.Text = cc;

            if (!HasCreditCardNumber(cc))
                return "Credit Card is incorrect";

            // Remove any non numbers from CCV and check length
            cc = CleanString(tbxCCV.Text);
            if (cc.Length != 3)
                return "CCV is incorrect";
            else
                tbxCCV.Text = cc;

            // Remove any non numbers from Expiry date, check length, and check to ensure credit card has not expired
            cc = CleanString(tbxExpiryMonth.Text);
            if (cc.Length != 2)
                return "Expiry Month is incorrect";
            else
                tbxExpiryMonth.Text = cc;

            cc = CleanString(tbxExpiryYear.Text);
            if (cc.Length != 2)
                return "Expiry Year is incorrect";
            else
                tbxExpiryYear.Text = cc;

            // Expiry Check
            if (int.Parse(tbxExpiryYear.Text) < int.Parse(DateTime.Now.ToString("yy")))
                return "Unable to use Credit Card - it has expired";
            if (int.Parse(tbxExpiryMonth.Text) < int.Parse(DateTime.Now.ToString("MM")) && int.Parse(tbxExpiryYear.Text) == int.Parse(DateTime.Now.ToString("yy")))
                return "Unable to use Credit Card - it has expired";

            return "Success";
        }

        // Compares input string to credit card regex and returns whether or not credit card was valid
        public bool HasCreditCardNumber(string input)
        {
            string[] Patterns = new string[] { "4[0-9]{12}(?:[0-9]{3})?", "5[1-5][0-9]{14}", "3[47][0-9]{13}", "3(?:0[0-5]|[68][0-9])[0-9]{11}", "6(?:011|5[0-9]{2})[0-9]{12}", "(?:2131|1800|35\\d{3})\\d{11}" };

            foreach (var pattern in Patterns)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(input, pattern, System.Text.RegularExpressions.RegexOptions.Multiline))
                {
                    return true;
                }
            }

            return false;
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
            conn.CreateTable<CustomerDetailsDB>();
            try
            {
                conn.DeleteAll<CustomerDetailsDB>();
            }
            catch(Exception) { }
            

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
            try
            {
                conn.DeleteAll<CustomerDetailsDB>();
            }
            catch
            {

            }
            
            Debug.WriteLine("SQL Data forgotten");
        }

        // Goes back a page on button click
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(StepThree));
        }

        // Goes forward a page on button click and checks all fields are valid
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
                messageDialog.Commands.Add(new Windows.UI.Popups.UICommand
                {
                    Label = "Ok", Id = 0
                });
                await messageDialog.ShowAsync();
            }
        }

        // Remembers customer details if user inputs to
        private void rboRememberDetails_Checked(object sender, RoutedEventArgs e)
        {
            if ((bool)rboRememberDetails.IsChecked)
                OrderManager.RememberDetails = true;
            else
                OrderManager.RememberDetails = false;
        }

    }
}
