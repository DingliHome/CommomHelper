using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClassLibraryHelper.C_;

namespace TestCommonHelper
{
    [TestClass()]
    public class SerializerTest
    {
        [TestMethod]
        public void PersonTest()
        {
            Person person=new Person("ding","li");
            BinarySerializer.SerializerToFile(person, "D://", "person.txt");
            var file = BinarySerializer.DeserializerFromFile<Person>("D://person.txt");
            Console.WriteLine(file.FirstName);
            Console.WriteLine(file.SecondName);
        }
    }

    [Serializable]
    public class Person:ISerializable
    {
        public Person(string first,string second)
        {
            FirstName = first;
            SecondName = second;
        }
        public Person(SerializationInfo first, StreamingContext second)
        {
            FirstName = first.GetString("firstName");
            SecondName = first.GetString("secondName");
        }
        public string FirstName { get; set; }
        public string SecondName { get; set; }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
           info.AddValue("firstName",FirstName);
           info.AddValue("secondName",SecondName);
        }
    }
}
