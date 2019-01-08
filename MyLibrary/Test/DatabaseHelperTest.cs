using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DatabaseInvoke;

namespace Test
{
    [TestClass]
    public class DatabaseHelperTest
    {
        //[TestMethod]
        //public void TestDatabaseHelper()
        //{
        //    DatabaseHelper dbHelper = new DatabaseHelper();
        //    string sql = "select count(1) from sysobjects";
        //    dbHelper.ExecuteSql(sql);
        //}

        [TestMethod]
        public void TestExecuteSql()
        {
            DatabaseHelper dbHelper = new DatabaseHelper("dbtest1");
            string sql = "select count(1) from sysobjects";
            //dbHelper.GetDataSet(sql);
            //Assert.IsTrue(true);    
        }
    }
}
