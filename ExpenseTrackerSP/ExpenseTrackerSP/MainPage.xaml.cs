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

        private async void NavigateButton_History(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HistoryPage());
        }

        private async void ViewAlerts_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ViewAlerts());
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            categoryPicker.ItemsSource = await App.Database.GetCategoryAsync();
        }

        async void SubmitButton_Clicked(object sender, EventArgs e)
        {
            if (categoryPicker.SelectedIndex != -1)
            {
                var tempCategory = (Category)categoryPicker.SelectedItem;
                var categoryName = tempCategory.Name;
                double tempOut;

                if (!string.IsNullOrWhiteSpace(amountEntry.Text) && double.TryParse(amountEntry.Text, out tempOut))
                {
                    
                   await App.Database.SaveExpenseAsync(new Expense
                   {
                       Amount = double.Parse(amountEntry.Text),
                       Date = DateTime.Now,
                       CategoryName = categoryName
                   });

                   var overspendingChecker = await App.Database.GetNotificationAsync();

                   CurrentAmountResetter(overspendingChecker);

                   AlertChecker(overspendingChecker, categoryName, double.Parse(amountEntry.Text));

                   amountEntry.Text = string.Empty;
                   categoryPicker.SelectedItem = null;
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
                await DisplayAlert("Alert", "You have to select a category!", "OK");
                amountEntry.Text = string.Empty;
                categoryPicker.SelectedItem = null;
            }
        }
    }
}
