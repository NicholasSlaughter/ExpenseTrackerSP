using OxyPlot;
using OxyPlot.Series;
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
	public partial class GraphPage : ContentPage
	{
		public GraphPage ()
		{
			InitializeComponent ();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var tempList = await App.Database.GetExpenseAsync();
            var data = new OxyData(tempList);
            BindingContext = data;
        }
        private async void SortButton_Clicked(object sender, EventArgs e)
        {
            var temp = await App.Database.GetExpenseAsync();

            string answer = await DisplayActionSheet("Sort By:", "Cancel", null, "Eating Out", "Clothes", "Entertainment", "Fuel", "Gift", "Travel");

            var data = new OxyData(temp.FindAll(d => d.CategoryName.Equals(answer)));

            BindingContext = data;
        }
    }
}