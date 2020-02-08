using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ExpenseTrackerSP
{
    public class Notification
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Period { get; set; }
        public double MaxAmount { get; set; }
        public double CurrentAmount { get; set; }
        public string CategoryName { get; set; }
    }
}
