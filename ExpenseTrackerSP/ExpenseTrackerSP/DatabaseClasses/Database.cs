﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using SQLiteNetExtensionsAsync.Extensions;

namespace ExpenseTrackerSP
{
    public class Database
    {
        readonly SQLiteAsyncConnection _database;

        public Database(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Category>().Wait();
            _database.CreateTableAsync<Expense>().Wait();
            _database.CreateTableAsync<Notification>().Wait();
            _database.CreateTableAsync<Period>().Wait();

            //Initializes the Caterogies if it has no elements
            Task<int> catCount = _database.Table<Category>().CountAsync();
            if (catCount.Result == 0)
            {
                var newCat = new Category();
                newCat.Name = "Eating Out";
                _database.InsertAsync(newCat);
                newCat = new Category();
                newCat.Name = "Entertainment";
                _database.InsertAsync(newCat);
                newCat = new Category();
                newCat.Name = "Groceries";
                _database.InsertAsync(newCat);
                newCat = new Category();
                newCat.Name = "Other";
                _database.InsertAsync(newCat);
            }

            //Initializes the Periods if it has no elements
            Task<int> perCount = _database.Table<Period>().CountAsync();
            if (perCount.Result == 0)
            {
                var newPer = new Period();
                newPer.Name = "Week";
                _database.InsertAsync(newPer);
                newPer = new Period();
                newPer.Name = "Month";
                _database.InsertAsync(newPer);
                newPer = new Period();
                newPer.Name = "Year";
                _database.InsertAsync(newPer);
            }
        }

        /////////////////////////////////////// Category Get Set //////////////////////////
        //Gets a category from the database
        public Task<List<Category>> GetCategoryAsync()
        {
            return _database.Table<Category>().ToListAsync();
        }
        //Need to get rid of
        public Task<int> SaveCategoryAsync(Category category)
        {
            return _database.InsertAsync(category);
        }

        /////////////////////////////////////// Period Get Set //////////////////////////
        //Gets a category from the database
        public Task<List<Period>> GetPeriodAsync()
        {
            return _database.Table<Period>().ToListAsync();
        }
        //Need to get rid of
        public Task<int> SavePeriodAsync(Period period)
        {
            return _database.InsertAsync(period);
        }

        /////////////////////////////////////// Expense Get Set //////////////////////////
        //Gets a Expense from the database
        public Task<List<Expense>> GetExpenseAsync()
        {
            return _database.Table<Expense>().ToListAsync();
        }
        //Save an expense into the database
        public Task<int> SaveExpenseAsync(Expense expense)
        {
            return _database.InsertAsync(expense);
        }

        public Task<int> DeleteExpenseAsync(Expense expense)
        {
            return _database.DeleteAsync(expense);
        }

        /////////////////////////////////////// Notification Get Set //////////////////////////
        //Gets a Expense from the database
        public Task<List<Notification>> GetNotificationAsync()
        {
            return _database.Table<Notification>().ToListAsync();
        }
        //Save an expense into the database
        public Task<int> SaveNotificationAsync(Notification notification)
        {
            return _database.InsertAsync(notification);
        }
        //Delete a Notification into the database
        public Task<int> DeleteNotificationAsync(Notification notification)
        {
            return _database.DeleteAsync(notification);
        }
        //Updates a Notification in the database
        public Task<int> UpdateNotificationAsync(Notification notification)
        {
            return _database.UpdateAsync(notification);
        }
    }
}
