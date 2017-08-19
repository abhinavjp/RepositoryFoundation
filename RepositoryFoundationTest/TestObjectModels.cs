using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryFoundationTest
{
    public class TestObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public override bool Equals(object obj)
        {
            if(!(obj is TestObject))
            {
                return false;
            }
            var testObj = obj as TestObject;
            return testObj.Id == Id && testObj.Name == Name && testObj.DateOfBirth == DateOfBirth;
        }
        public override int GetHashCode()
        {
            return Id;
        }
    }

    public class TestNestedObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public TestObject TestingObject { get; set; }
        public override bool Equals(object obj)
        {
            if (!(obj is TestNestedObject))
            {
                return false;
            }
            var testObj = obj as TestNestedObject;
            return testObj.Id == Id && testObj.Name == Name && testObj.DateOfBirth == DateOfBirth && testObj.TestingObject.Equals(TestingObject);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }

    public class TestNestedListObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public List<TestObject> TestingObject { get; set; }
        public override bool Equals(object obj)
        {
            if (!(obj is TestNestedListObject))
            {
                return false;
            }
            var testObj = obj as TestNestedListObject;
            return testObj.Id == Id && testObj.Name == Name && testObj.DateOfBirth == DateOfBirth && testObj.TestingObject.All(a=>TestingObject.Any(ta => ta.Equals(a)));
        }
        public override int GetHashCode()
        {
            return Id;
        }
    }
}
