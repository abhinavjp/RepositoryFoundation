using System;
using RepositoryFoundation.Helper.Mapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace RepositoryFoundationTest
{
    [TestClass]
    public class MapperHelperTest
    {
        [TestMethod]
        public void DeepCopyTest()
        {
            var testObj = new TestObject
            {
                Id = 1,
                Name = "Messi",
                DateOfBirth = new DateTime(1985, 4, 4)
            };
            var testObjCopy = testObj.CreateDeepCopy();
            var testObjNoCopy = testObj;
            testObj.Name = "Suarez";
            Assert.AreEqual(testObj.Name, testObjNoCopy.Name);
            Assert.AreNotEqual(testObj.Name, testObjCopy.Name);
            testObjCopy.Name = "Neymar";
            Assert.AreNotEqual(testObjCopy.Name, testObjNoCopy.Name);
            Assert.AreNotEqual(testObjCopy.Name, testObj.Name);
        }
    }
    
}
