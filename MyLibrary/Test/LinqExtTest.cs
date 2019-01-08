using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CommonLib.Extensions;

namespace Test
{
    [TestClass]
    public class LinqExtTest
    {
        [TestMethod]
        public void TestOrderExpressions()
        {
            List<TTest> tts = new List<TTest>();
            tts.Add(new TTest("test5", "5", 5));
            tts.Add(new TTest("test1", "1", 1));
            tts.Add(new TTest("test2", "2", 2));
            tts.Add(new TTest("test3", "3", 3));

            var ex1 = LinqExtensions.GetOrderExpression<TTest, string>("Value").Compile();//p=>p.propertyName
            Assert.AreEqual("test1", tts.OrderBy(ex1).FirstOrDefault().Fields);    
        }

        [TestMethod]
        public void TestCreateEqualExpressions()
        {
            List<TTest> tts = new List<TTest>();
            tts.Add(new TTest("test5", "5", 5));
            tts.Add(new TTest("test1", "1", 1));
            tts.Add(new TTest("test2", "2", 2));
            tts.Add(new TTest("test3", "3", 3));

            var ex1 = LinqExtensions.CreateEqual<TTest>("Value", "1", typeof(String)).Compile();//p=>p.propertyName == propertyValue
            var ex2 = LinqExtensions.CreateNotEqual<TTest>("Value", "5", typeof(String)).Compile();//p=>p.propertyName != propertyValue
            // p=>p.Value > "4"
            var ex3 = LinqExtensions.CreateGreaterThan<TTest>("Value", "4", typeof(String)).Compile();
            var ex3_ = LinqExtensions.CreateGreaterThan<TTest>("Number", 4, typeof(Int32)).Compile();
            // p=>p.Value < "2"
            var ex4 = LinqExtensions.CreateLessThan<TTest>("Value", "2", typeof(String)).Compile();

            var ex5 = LinqExtensions.CreateGreaterThanOrEqual<TTest>("Number", 5, typeof(Int32)).Compile();

            var ex6 = LinqExtensions.CreateLessThanOrEqual<TTest>("Value", "1", typeof(String)).Compile();
            Assert.AreEqual("test1", tts.Where(ex1).FirstOrDefault().Fields);
            Assert.AreEqual("test1", tts.Where(ex2).FirstOrDefault().Fields);
            
            Assert.AreEqual("test5", tts.Where(ex3).FirstOrDefault().Fields);
            Assert.AreEqual("test5", tts.Where(ex3_).FirstOrDefault().Fields);

            Assert.AreEqual("test1", tts.Where(ex4).FirstOrDefault().Fields);

            Assert.AreEqual(1, tts.Where(ex5).Count());

            Assert.AreEqual("test1", tts.Where(ex6).FirstOrDefault().Fields);
        }

        [TestMethod]
        public void TestContainsExpressions()
        {
            List<TTest> tts = new List<TTest>();
            tts.Add(new TTest("test5", "abctede", 5));
            tts.Add(new TTest("test1", "absdacde", 1));
            tts.Add(new TTest("test2", "abcdcTESTde", 2));
            tts.Add(new TTest("test3", "abcde", 3));

            //p=>p.propertyName.Contains(propertyValue)
            var ex1 = LinqExtensions.GetContains<TTest>("Value", "TEST").Compile();

            Assert.AreEqual("test2", tts.Where(ex1).FirstOrDefault().Fields);

            var ex2 = LinqExtensions.GetNotContains<TTest>("Value", "abc").Compile();
            Assert.AreEqual("test1", tts.Where(ex2).FirstOrDefault().Fields);
        }
        
        [TestMethod]
        public void TestMregeExpressions()
        {
            List<TTest> tts = new List<TTest>();
            tts.Add(new TTest("test5", "5", 5));
            tts.Add(new TTest("test1", "1", 2));
            tts.Add(new TTest("test2", "2", 2));
            tts.Add(new TTest("test3", "3", 3));

            var ex1 = LinqExtensions.CreateEqual<TTest>("Value", "1");
            var ex2 = LinqExtensions.CreateEqual<TTest>("Number", 2);

            var orEx = LinqExtensions.Or<TTest>(ex1, ex2).Compile();
            Assert.AreEqual(2, tts.Where(orEx).Count());
            var andEx = LinqExtensions.And<TTest>(ex1, ex2).Compile();
            Assert.AreEqual("test1", tts.Where(orEx).FirstOrDefault().Fields);
        }
    }

    public class TTest
    {
        public TTest()
        { 
            
        }
        public TTest(string fields, string value, int number)
        {
            this.Fields = fields;
            this.Value = value;
            this.Number = number;
        }
        public string Fields { get; set; }
        public string Value { get; set; }

        public int Number { get; set; }
    }

}
