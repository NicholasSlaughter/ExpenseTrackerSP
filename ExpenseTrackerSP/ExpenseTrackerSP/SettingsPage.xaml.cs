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
	public partial class SettingsPage : ContentPage
	{
		public SettingsPage ()
		{
			InitializeComponent ();
		}
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            periodPicker.ItemsSource = await App.Database.GetPeriodAsync();
            categoryPicker.ItemsSource = await App.Database.GetCategoryAsync();
        }
        async void SubmitButton_Clicked(object sender, EventArgs e)
        {
            if (categoryPicker.SelectedIndex != -1)
            {
                if (periodPicker.SelectedIndex != -1)
                {
                    var categoryTemp = (Category)categoryPicker.SelectedItem;
                    var periodTemp = (Period)periodPicker.SelectedItem;

                    var categoryName = categoryTemp.Name;
                    var periodName = periodTemp.Name;

                    double tempOut;

                    //error handling for amount field
                    if (!string.IsNullOrWhiteSpace(amountEntry.Text) && double.TryParse(amountEntry.Text, out tempOut))
                    {
                        await App.Database.SaveNotificationAsync(new Notification
                        {
                            MaxAmount = double.Parse(amountEntry.Text),
                            CurrentAmount = 0,
                            Period = periodName,
                            CategoryName = categoryName
                        });
                        var a = new Period();
                        amountEntry.Text = string.Empty;
                        categoryPicker.SelectedItem = null;
                        periodPicker.SelectedItem = null;
                    }
                    else
                    {
                        await DisplayAlert("Alert", "You can only enter a number in the amount field!", "OK");
                        amountEntry.Text = string.Empty;
                        categoryPicker.SelectedItem = null;
                        periodPicker.SelectedItem = null;
                    }
                }
                else
                {
                    await DisplayAlert("Alert", "You have to select a period!", "OK");
                    amountEntry.Text = string.Empty;
                    categoryPicker.SelectedItem = null;
                    periodPicker.SelectedItem = null;
                }
            }
            else
            {
                await DisplayAlert("Alert", "You have to select a category!", "OK");
                amountEntry.Text = string.Empty;
                categoryPicker.SelectedItem = null;
                periodPicker.SelectedItem = null;
            }
        }
    }
}