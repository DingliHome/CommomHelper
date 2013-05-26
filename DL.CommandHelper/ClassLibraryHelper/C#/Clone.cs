using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace ClassLibraryHelper.C_
{
    [Serializable]
    public class EmployeeClone : ICloneable
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public Department Department { get; set; }

        //浅拷贝
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        //深拷贝
        public EmployeeClone DeepEmployeeClone()
        {
            using (Stream stream=new MemoryStream())//创建内存流
            {
                IFormatter formatter=new BinaryFormatter();
                formatter.Serialize(stream,this);//将对象序列化到流
                stream.Seek(0, SeekOrigin.Begin);//将流指针指向第一个字符
                return formatter.Deserialize(stream) as EmployeeClone;//反序列化对象到另一个对象
            }
        }
        //泛型 深拷贝
        public T Clone<T>(T t) where　T:class
        {
            using (Stream stream=new MemoryStream())
            {
                IFormatter formatter=new BinaryFormatter();
                formatter.Serialize(stream,t);
                stream.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(stream) as T;
            }
        }
    }
    [Serializable]
    public class Department
    {
        public string DepartmentName { get; set; }
    }

    
}
