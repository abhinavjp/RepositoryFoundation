using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositoryFoundation.Helper.XmlHandler;

namespace RepositoryFoundationTest
{
    [TestClass]
    public class XmlBuilderTester
    {
        [TestMethod]
        public void BasicTest()
        {
            var _serializingPath = @"D:\XML\Test\Test.xml";
            var _deserializingPath = @"D:\XML\Test\Test.xml";

            var serializingObject = new TestObject
            {
                Id = 1,
                Name = "Messi",
                DateOfBirth = new DateTime(1987, 6, 25)
            };
            XmlBuilder.Serialize(serializingObject, _serializingPath);
            var deserializedObj = XmlBuilder.Deserialize<TestObject>(_deserializingPath);
            Assert.AreEqual(true, serializingObject.Equals(deserializedObj));
        }

        [TestMethod]
        public void NestedTest()
        {
            var _serializingPath = @"D:\XML\Test\NestedTest.xml";
            var _deserializingPath = @"D:\XML\Test\NestedTest.xml";

            var serializingObject = new TestNestedObject
            {
                Id = 1,
                Name = "Messi",
                DateOfBirth = new DateTime(1987, 6, 25),
                TestingObject = new TestObject
                {
                    Id = 1,
                    Name = "Leonel",
                    DateOfBirth = new DateTime(1960, 1, 30)
                }
            };
            XmlBuilder.Serialize(serializingObject, _serializingPath);
            var deserializedObj = XmlBuilder.Deserialize<TestNestedObject>(_deserializingPath);
            Assert.AreEqual(true, serializingObject.Equals(deserializedObj));
        }

        [TestMethod]
        public void ListTest()
        {
            var _serializingPath = @"D:\XML\Test\ListTest.xml";
            var _deserializingPath = @"D:\XML\Test\ListTest.xml";

            var serializingObject = new TestNestedListObject
            {
                Id = 1,
                Name = "Messi",
                DateOfBirth = new DateTime(1987, 6, 25),
                TestingObject = new System.Collections.Generic.List<TestObject>
                {
                    new TestObject
                    {
                        Id = 1,
                        Name = "Leonel",
                        DateOfBirth = new DateTime(1960, 1, 30)
                    },

                    new TestObject
                    {
                        Id = 2,
                        Name = "Andres",
                        DateOfBirth = new DateTime(1978, 5, 21)
                    }
                }
            };
            XmlBuilder.Serialize(serializingObject, _serializingPath);
            var deserializedObj = XmlBuilder.Deserialize<TestNestedListObject>(_deserializingPath);
            Assert.AreEqual(true, serializingObject.Equals(deserializedObj));
        }
    }
}
