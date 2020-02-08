using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ExpenseTrackerSP
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoryPage : ContentPage
    {
        public HistoryPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            listView.ItemsSource = await App.Database.GetExpenseAsync();
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;
            bool answer = await DisplayAlert("Delete Expense", "Would You Like To Delete The Expense?", "Yes", "No");

            var temp = (Expense)((ListView)sender).SelectedItem;

            if (answer)
            {
                await App.Database.DeleteExpenseAsync(temp);

                var alertList = await App.Database.GetNotificationAsync();
                double negatvieChecker = 0;

                for (int i = 0; i < alertList.Count; i++)
                {
                    if (alertList[i].CategoryName == temp.CategoryName)
                    {
                        negatvieChecker = alertList[i].CurrentAmount - temp.Amount;
                        if (negatvieChecker < 0)
                        {
                            alertList[i].CurrentAmount = 0;
                        }
                        else
                        {
                            alertList[i].CurrentAmount -= temp.Amount;
                        }
                        await App.Database.UpdateNotificationAsync(alertList[i]);
                    }
                }

                await DisplayAlert("Alert", "$" + temp.Amount + " of " + temp.CategoryName + " has been deleted from your expenses", "OK");
            }

            listView.ItemsSource = await App.Database.GetExpenseAsync();

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        private async void GraphPage_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GraphPage());
        }

        private async void SortButton_Clicked(object sender, EventArgs e)
        {
            string answer = await DisplayActionSheet("Sort By:", "Cancel", null, "Category", "New-Old", "Old-New", "High-Low", "Low-High");

            var temp = await App.Database.GetExpenseAsync();

            if (answer== "New-Old")
            {
                listView.ItemsSource = temp.OrderByDescending(d => d.Date);
            }
            if (answer == "Old-New")
            {
                listView.ItemsSource = temp.OrderBy(d => d.Date);
            }
            else if(answer == "Category")
            {
                listView.ItemsSource = temp.OrderByDescending(d => d.CategoryName);
            }
            else if (answer == "High-Low")
            {
                listView.ItemsSource = temp.OrderByDescending(d => d.Amount);
            }
            else if (answer == "Low-High")
            {
                listView.ItemsSource = temp.OrderBy(d => d.Amount);
            }
        }
    }
}
