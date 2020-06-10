using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ExpenseTrackerSP
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        //If the period for the alert getting triggered has gone over its period then reset the counter to 0
        private void CurrentAmountResetter(List<Notification> overspendingChecker)
        {
            if (DateTime.Today.DayOfWeek.ToString() == "Sunday")
            {
                for (int i = 0; i <= overspendingChecker.Count; i++)
                {
                    if (overspendingChecker[i].CategoryName == "Week")
                    {
                        overspendingChecker[i].CurrentAmount = 0;
                    }
                }
            }

            if (DateTime.Today.Day == 1)
            {
                for (int i = 0; i <= overspendingChecker.Count; i++)
                {
                    if (overspendingChecker[i].CategoryName == "Month")
                    {
                        overspendingChecker[i].CurrentAmount = 0;
                    }
                }
            }

            if (DateTime.Today.DayOfYear == 1)
            {
                for (int i = 0; i <= overspendingChecker.Count; i++)
                {
                    if (overspendingChecker[i].CategoryName == "Year")
                    {
                        overspendingChecker[i].CurrentAmount = 0;
                    }
                }
            }
        }
        //Checks to see if a user is over spending
        private async void AlertChecker(List<Notification> overspendingChecker, string categoryName, double amount)
        {
            for (int i = 0; i < overspendingChecker.Count; i++)
            {
                if (overspendingChecker[i].CategoryName == categoryName)
                {
                    overspendingChecker[i].CurrentAmount += amount;
                    await App.Database.UpdateNotificationAsync(overspendingChecker[i]);

                    if (overspendingChecker[i].CurrentAmount > overspendingChecker[i].MaxAmount)
                    {
                        if (overspendingChecker[i].Period == "Week")
                        {
                            await DisplayAlert("Alert", "You have surpassed your weekly limit for " + overspendingChecker[i].CategoryName, "OK");
                        }
                        else if (overspendingChecker[i].Period == "Month")
                        {
                            await DisplayAlert("Alert", "You have surpassed your monthly limit for " + overspendingChecker[i].CategoryName, "OK");
                        }
                        else if (overspendingChecker[i].Period == "Year")
                        {
                            await DisplayAlert("Alert", "You have surpassed your yearly limit for " + overspendingChecker[i].CategoryName, "OK");
                        }
                    }
                }
            }
        }
        //Goes to history page
        private async void NavigateButton_History(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HistoryPage());
        }
        //Goes to view alerts page
        private async void ViewAlerts_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ViewAlerts());
        }
        //Let the category picker pull the categories in the database
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            categoryPicker.ItemsSource = await App.Database.GetCategoryAsync();
        }
        //When a user submits an expense the program checks to see if the data is valid and if it is then the program enters it into the database. If not then an error message is displayed to the screen.
        async void SubmitButton_Clicked(object sender, EventArgs e)
        {
            double tempOut;
            //Check to see if a category was picked
            if (categoryPicker.SelectedIndex != -1)
            {
                //Check to see if a valid expense was entered
                if (double.TryParse(amountEntry.Text, out tempOut) && double.Parse(amountEntry.Text) >= 0 && double.Parse(amountEntry.Text) <= 1000000)
                {
                    var tempCategory = (Category)categoryPicker.SelectedItem;
                    var categoryName = tempCategory.Name;
                    string formatedAmount = string.Format("{0:0.00}", double.Parse(amountEntry.Text)); //Formats a double to the hundreth place

                    if (!string.IsNullOrWhiteSpace(amountEntry.Text) && double.TryParse(amountEntry.Text, out tempOut))
                    {
                        //Enter data into the database
                        await App.Database.SaveExpenseAsync(new Expense
                        {
                            Amount = Convert.ToDouble(formatedAmount),
                            Date = DateTime.Now,
                            CategoryName = categoryName
                        });

                        var overspendingChecker = await App.Database.GetNotificationAsync();

                        CurrentAmountResetter(overspendingChecker);

                        AlertChecker(overspendingChecker, categoryName, double.Parse(amountEntry.Text));

                        amountEntry.Text = string.Empty;
                        categoryPicker.SelectedItem = null;

                        await DisplayAlert("Confirmed", "You Entered an Expense", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Alert", "You can only enter a number in the amount field!", "OK");
                        amountEntry.Text = string.Empty;
                        categoryPicker.SelectedItem = null;
                    }
                }
                else
                {
                    await DisplayAlert("Alert", "You can only enter a number between 0 and 1 million dollars", "OK");
                    amountEntry.Text = string.Empty;
                    categoryPicker.SelectedItem = null;
                }
            }
            else
            {
                await DisplayAlert("Alert", "You have to select a category!", "OK");
                amountEntry.Text = string.Empty;
                categoryPicker.SelectedItem = null;
            }
        }
    }
}
