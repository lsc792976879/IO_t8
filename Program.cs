using System;
using System.Diagnostics;
using System.Text;
using Google.Protobuf;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProtoBuf;
using System.Windows;
using System.Xml.Schema;

namespace ConsoleApp3;

public class Program
{
    static void Main(string[] args)
    {
        //创建对象的实例
        Person lsc = new Person()
        {
            Name = "lsc",
            Age = 22,
            Id = "123456",
            Phone = new Phone()
            {
                Name = "P30",
                Type = "HUAWEI",
            },
            Son = null,
        };
        Person s = lsc;
        for (int i = 0; i < 20; i++)
        {
            Person son = new Person()
            {
                Name = "第" + i + "个人的姓名",
                Age = i,
                Id = i.ToString(),
                Phone = new Phone()
                {
                    Name = "第" + i + "台手机的名字",
                    Type = "第" + i + "台手机的id",
                },
                Son = null,
            };
            s.Son = son;
            s = son;
        }
        
        Console.WriteLine("测试效率");
        
        Person testPerson = new Person();
        int times = 10000;
        Stopwatch stopwatch = new Stopwatch();
        
        //json方案测试
        stopwatch.Restart();
        for (int i = 0; i < times; i++)
        {
            string tmp = JsonConvert.SerializeObject(lsc);
            Person person = JsonConvert.DeserializeObject<Person>(tmp);
        }
        stopwatch.Stop();
        Console.WriteLine("Json序列化方案耗时:" + stopwatch.Elapsed);
        using (MemoryStream memoryStream = new MemoryStream())
        {
            StreamWriter sw = new StreamWriter(memoryStream);
            sw.Write(JsonConvert.SerializeObject(lsc));
            Console.WriteLine("Json生成的序列长度为：" + memoryStream.ToArray().Length);
        }
        

        //Proto测试
        stopwatch.Restart();
        for (int i = 0; i < times; i++)
        {
            var tmp = lsc.ToByteArray();
            Person person = Person.Parser.ParseFrom(tmp);
        }
        stopwatch.Stop();
        Console.WriteLine("Proto序列化方案耗时:" + stopwatch.Elapsed);
        Console.WriteLine("Proto生成的序列长度为" + lsc.ToByteArray().Length);
    }

    static string outputBytes(byte[] buffer)
    {
        StringBuilder sb = new StringBuilder();
        foreach (var tmp in buffer)
        {
            sb.Append(tmp);
            sb.Append(' ');
        }
        return sb.ToString();
    }
}