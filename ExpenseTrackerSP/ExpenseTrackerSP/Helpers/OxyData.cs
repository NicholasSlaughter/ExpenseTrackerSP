using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Text;

//Information to use OxyPlot comes from the following website:
//https://www.c-sharpcorner.com/article/using-oxyplot-with-xamarin-forms/
namespace ExpenseTrackerSP
{
    public class OxyData
    {
        public PlotModel LineModel { get; set; }

        public OxyData(List<Expense> e)
        {
            LineModel = CreateLineChart(e); //Sets the model to be a line graph
        }
        //Inputs data into the line graph
        public PlotModel CreateLineChart(List<Expense> expense)
        {
            var plotModel = new PlotModel { Title = "Expenses Over Time" };
            var lineSeries = new LineSeries() //sets the line graph elements
            {
                Color = OxyColors.SkyBlue,
                MarkerType = MarkerType.Circle,
                MarkerSize = 6,
                MarkerStroke = OxyColors.White,
                MarkerFill = OxyColors.SkyBlue,
                MarkerStrokeThickness = 1.5
            };
            var xAxis = new DateTimeAxis { Position = AxisPosition.Bottom, StringFormat = "MM/dd/yyyy", Title= "Date" };
            var linearAxis = new LinearAxis { Position = ValueAxisPosition(), Title = "Amount" };

            var counter = expense.Count;

            for (int i = 0; i < counter; i++)
            {
                lineSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(expense[i].Date), expense[i].Amount)); //Adds points to the graph
            }

            plotModel.Series.Add(lineSeries);
            plotModel.Axes.Add(xAxis);
            plotModel.Axes.Add(linearAxis);
            return plotModel;
        }

        private AxisPosition CategoryAxisPosition()
        {
            if (typeof(BarSeries) == typeof(ColumnSeries))
            {
                return AxisPosition.Left;
            }

            return AxisPosition.Bottom;
        }

        private AxisPosition ValueAxisPosition()
        {
            if (typeof(BarSeries) == typeof(ColumnSeries))
            {
                return AxisPosition.Bottom;
            }

            return AxisPosition.Left;
        }
    }
}
