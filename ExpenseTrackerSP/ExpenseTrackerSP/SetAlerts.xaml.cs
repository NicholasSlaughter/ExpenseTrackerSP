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
	public partial class SetAlerts : ContentPage
	{
		public SetAlerts ()
		{
			InitializeComponent ();
		}
        //Let the pickers know which table in the database to pull from
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            periodPicker.ItemsSource = await App.Database.GetPeriodAsync();
            categoryPicker.ItemsSource = await App.Database.GetCategoryAsync();
        }
        //When a user submits an alert the program checks to see if the data is valid and if it is then the program enters it into the database.If not then an error message is displayed to the screen.
        async void SubmitButton_Clicked(object sender, EventArgs e)
        {
            //Check to see if a category was selected
            if (categoryPicker.SelectedIndex != -1)
            {
                //Check to see if a period was selected
                if (periodPicker.SelectedIndex != -1)
                {
                    //Check to see if a valid amount was entered
                    if (double.Parse(amountEntry.Text) >= 0 && double.Parse(amountEntry.Text) <= 1000000)
                    {
                        var categoryTemp = (Category)categoryPicker.SelectedItem;
                        var periodTemp = (Period)periodPicker.SelectedItem;

                        var categoryName = categoryTemp.Name;
                        var periodName = periodTemp.Name;
                        double tempOut;
                        string formatedAmount = string.Format("{0:0.00}", double.Parse(amountEntry.Text)); //Formats a double to the hundreth place

                        //error handling for amount field
                        if (!string.IsNullOrWhiteSpace(amountEntry.Text) && double.TryParse(amountEntry.Text, out tempOut))
                        {
                            //Enters amount into the database
                            await App.Database.SaveNotificationAsync(new Notification
                            {
                                MaxAmount = Convert.ToDouble(formatedAmount),
                                CurrentAmount = 0,
                                Period = periodName,
                                CategoryName = categoryName
                            });
                            var a = new Period();
                            amountEntry.Text = string.Empty;
                            categoryPicker.SelectedItem = null;
                            periodPicker.SelectedItem = null;

                            await DisplayAlert("Confirmed", "You Entered an Alert", "OK");
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
                        await DisplayAlert("Alert", "You can only enter a number between 0 and 1 million dollars", "OK");
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