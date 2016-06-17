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
using Windows.UI.Xaml.Media.Imaging;
using SQLite.Net;
using SQLite.Net.Attributes;
using System.Diagnostics;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace D301_LunchToGo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StepFive : Page
    {
        // Connection for SQL server
        SQLiteConnection conn;
        private string path;

        // Global var that holds the current meal the user has selected
        Meal currentMeal;

        public StepFive()
        {
            this.InitializeComponent();
            SetupPage();

            // Setup database
            path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "db.sqlite");
            conn = new SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);
            conn.CreateTable<OrderDB>();
            conn.CreateTable<MealDB>();
        }

        /// <summary>
        /// Setups defaults for the page
        /// </summary>
        private void SetupPage()
        {
            // turn step two controls off
            SwitchStepTwo(false);
        }

        // Place Order on button click
        private async void btnPlaceOrder_Click(object sender, RoutedEventArgs e)
        {
            // If there is an internet connection and at least 1 meal in the cart then attempt to send order
            string result = CanOrder();
            if (result == "Success")
            {
                btnPlaceOrder.IsEnabled = false;
                btnPlaceOrder.Content = "Processing";
                InsertOrderToDatabase();

                bool b = await OrderPoster.SendOrder();

                if (b)
                {
                    this.Frame.Navigate(typeof(StepSix));
                    btnPlaceOrder.IsEnabled = true;
                }
                   
                else
                {
                    btnPlaceOrder.Content = "Place Order";
                    var messageDialog = new Windows.UI.Popups.MessageDialog("Order failed to send. Check internet connection", "Error");
                    messageDialog.Commands.Add(new Windows.UI.Popups.UICommand { Label = "Ok", Id = 0 });
                    await messageDialog.ShowAsync();
                }
            }
            else
            {
                var messageDialog = new Windows.UI.Popups.MessageDialog(result, "Error");
                messageDialog.Commands.Add(new Windows.UI.Popups.UICommand { Label = "Ok", Id = 0 });
                await messageDialog.ShowAsync();
            }
        }

        // Goes back a frame on button click
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(StepFour));
        }

        /// <summary>
        /// Inserts current order into database
        /// </summary>
        private void InsertOrderToDatabase()
        {
            conn.DeleteAll<OrderDB>();
            conn.DeleteAll<MealDB>();
            conn.CreateTable<OrderDB>();
            conn.CreateTable<MealDB>();

            var s = conn.Insert(new OrderDB()
            {
                DeliveryDate = OrderManager.DeliveryDate,
                DeliveryTime = OrderManager.DeliveryTime,
                Region = OrderManager.Region,
                CustomerName = OrderManager.CustomerName,
                CustomerPhone = OrderManager.CustomerPhone,
                CustomerAddress = OrderManager.CustomerAddress,
                CustomerCity = OrderManager.CustomerCity,
                CreditCardName = OrderManager.CreditCardName,
                CreditCardNumber = OrderManager.CreditCardNumber,
                CreditCardCCV = OrderManager.CreditCardCCV,
                CreditCardMonth = OrderManager.CreditCardMonth,
                CreditCardYear = OrderManager.CreditCardYear
            });

            foreach (Meal m in OrderManager.Meals)
            {
                var x = conn.Insert(new MealDB()
                {
                    OrderID = s,
                    Dish = m.Dish,
                    Secondary = m.Secondary
                });
            }

            Debug.WriteLine("S = " +s);
        }

        // Adds meal to order
        private void btnAddToOrder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OrderManager.AddMeal(currentMeal);
                lbxOrders.Items.Add(currentMeal);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        // Clears all meals in order on button click
        private void btnClearOrder_Click(object sender, RoutedEventArgs e)
        {
            OrderManager.ClearMeals();
            lbxOrders.Items.Clear();
        }

        // When the user selects a meal in step 1
        private void selectMeal(object sender, TappedRoutedEventArgs e)
        {
            // Cast sender to image and setup meal
            Image meal = sender as Image;

            if (meal != null)
            {
                
                ChangeSelectedMeal(meal);
                ChangeOptions(meal.Name);
                currentMeal = new Meal(meal.Name, rboOptionOne.Content.ToString());
            }
        }

        /// <summary>
        /// Goes through image and changes selected image to the one the user clicked
        /// </summary>
        /// <param name="image">image that the user selected</param>
        private void ChangeSelectedMeal(Image image)
        {
            // Images set to default
            imgBeefNoodle.Source = new BitmapImage(new Uri("ms-appx:///Assets/MyAssets/imgBeefNoodle.png"));
            imgChickenSandwich.Source = new BitmapImage(new Uri("ms-appx:///Assets/MyAssets/imgChickenSandwich.png"));
            imgGreenSalad.Source = new BitmapImage(new Uri("ms-appx:///Assets/MyAssets/imgGreenSalad.png"));
            imgLambKorma.Source = new BitmapImage(new Uri("ms-appx:///Assets/MyAssets/imgLambKorma.png"));

            // Image user clicked is set to selected
            image.Source = new BitmapImage(new Uri("ms-appx:///Assets/MyAssets/" + image.Name + "Selected.png"));
        }

        /// <summary>
        ///  Sets up the radio button options under step 2
        /// </summary>
        /// <param name="mealName">Meal user selected in step 1</param>
        private void ChangeOptions(string mealName)
        {
            // Turns the radio buttons on and sets up the content within them based on meal the user selected
            SwitchStepTwo(true);
            switch (mealName)
            {
                case "imgBeefNoodle":
                    rboOptionOne.Content = "No Chilli Flakes";
                    rboOptionTwo.Content = "Extra Chilli Flakes";
                    rboOptionThree.Visibility = Visibility.Collapsed;
                    break;
                case "imgChickenSandwich":
                    rboOptionOne.Content = "White";
                    rboOptionTwo.Content = "Rye";
                    rboOptionThree.Content = "Wholemeal";
                    break;
                case "imgGreenSalad":
                    rboOptionOne.Content = "None";
                    rboOptionTwo.Content = "Ranch";
                    rboOptionThree.Content = "Vinaigrette";
                    break;
                case "imgLambKorma":
                    rboOptionOne.Content = "Mild";
                    rboOptionTwo.Content = "Medium";
                    rboOptionThree.Content = "Hot";
                    break;
                default:
                    SwitchStepTwo(false);
                    break;
            }

            // Default radio button is set
            rboOptionOne.IsChecked = true;
        }

        /// <summary>
        /// Turns step two controls on and off
        /// </summary>
        /// <param name="visible">Whether the controls should be on or off</param>
        private void SwitchStepTwo(bool visible)
        {
            if (visible)
            {
                txtChoose.Visibility = Visibility.Visible;
                rboOptionOne.Visibility = Visibility.Visible;
                rboOptionTwo.Visibility = Visibility.Visible;
                rboOptionThree.Visibility = Visibility.Visible;
                btnAddToOrder.Visibility = Visibility.Visible;
                rboOptionOne.IsChecked = true;
            }
            else
            {
                txtChoose.Visibility = Visibility.Collapsed;
                rboOptionOne.Visibility = Visibility.Collapsed;
                rboOptionTwo.Visibility = Visibility.Collapsed;
                rboOptionThree.Visibility = Visibility.Collapsed;
                btnAddToOrder.Visibility = Visibility.Collapsed;
            }
        }

        // When a radio button is clicked this event is fired to set up the selected item in the currentMeal
        private void changeSecondary(object sender, RoutedEventArgs e)
        {
            RadioButton r = sender as RadioButton;
            if (r != null)
            {
                if (currentMeal != null && currentMeal.Dish != null && currentMeal.Dish != "")
                    currentMeal.Secondary = r.Content.ToString();
            }
        }

        /// <summary>
        /// Checks whether or not the user can place the order
        /// </summary>
        /// <returns>If user can place order</returns>
        private string CanOrder()
        {
            // If the user has items in the cart then they can place the order else return false
            if (OrderManager.Meals == null)
                return "You must have at least one meal in the cart";
            else if (OrderManager.Meals.Count < 1)
                return "You must have at least one meal in the cart";

            // Whether or not the user has an internet connection
            if (NetworkConnectionTrigger.HasInternet())
                return "Success";
            else
                return "You must be connected to the internet";
        }
    }
}
