using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositoryFoundation.Helper.ExpressionBuilder;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

namespace RepositoryFoundationTest
{
    [TestClass]
    public class QueryBuilderTests
    {
        private List<TestObject> _testList;
        [TestMethod]
        public void ConnectingExpressionsTests()
        {
            InitVariables();
            Expression<Func<TestObject, bool>> exp = (e) => e.Name == "Messi";
            var filteredList = _testList.Where(exp.AndAlso(e => e.DateOfBirth == new DateTime(1985, 4, 4)).OrElse(e=>e.Id==1).Compile()).ToList();
            Assert.AreEqual(4, filteredList.Count);
        }

        private void InitVariables()
        {
            _testList = new List<TestObject>
            {
                new TestObject
                {
                    Id = 1,
                    Name = "Messi",
                    DateOfBirth = new DateTime(1985, 4, 4)
                },
                new TestObject
                {
                    Id = 1,
                    Name = "Suarez",
                    DateOfBirth = new DateTime(1989, 1, 2)
                },
                new TestObject
                {
                    Id = 1,
                    Name = "Neymar",
                    DateOfBirth = new DateTime(1993, 5, 6)
                },
                new TestObject
                {
                    Id = 1,
                    Name = "Messi",
                    DateOfBirth = new DateTime(1999, 7, 7)
                }
            };
        }
    }
}
