using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ExpenseTrackerSP
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ViewAlerts : ContentPage
	{
		public ViewAlerts ()
		{
			InitializeComponent ();
		}

        private async void NavigateButton_Settings(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage());
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            alertsListView.ItemsSource = await App.Database.GetNotificationAsync();
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;
            bool answer = await DisplayAlert("Delete Item", "Would You Like To Delete The Alert?", "Yes", "No");

            var temp = (Notification)((ListView)sender).SelectedItem;

            if (answer)
            {
                await App.Database.DeleteNotificationAsync(temp);
            }

            alertsListView.ItemsSource = await App.Database.GetNotificationAsync();

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
        private async void SortButton_Clicked(object sender, EventArgs e)
        {
            string answer = await DisplayActionSheet("Sort By:", "Cancel", null, "Category", "Period", "High-Low", "Low-High");

            var temp = await App.Database.GetNotificationAsync();

            if (answer == "Period")
            {
                alertsListView.ItemsSource = temp.OrderByDescending(d => d.Period);
            }
            else if (answer == "Category")
            {
                alertsListView.ItemsSource = temp.OrderByDescending(d => d.CategoryName);
            }
            else if (answer == "High-Low")
            {
                alertsListView.ItemsSource = temp.OrderByDescending(d => d.MaxAmount);
            }
            else if (answer == "Low-High")
            {
                alertsListView.ItemsSource = temp.OrderBy(d => d.MaxAmount);
            }
        }
    }
}