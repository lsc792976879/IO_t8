using System;
using System.Text;
using Google.Protobuf;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProtoBuf;

namespace ConsoleApp3;

[ProtoContract]
public class Person
{
    [ProtoMember(1)]
    public string name;
    
    [ProtoMember(2)]
    public string id;
    
    [ProtoMember(3)]
    public int age;
    public Person(){}
    
    
    public Person(string name, string id, int age)
    {
        this.name = name;
        this.id = id;
        this.age = age;
    }

    public static byte[] Serialize_BinaryWriter(Person target)
    {
        using (MemoryStream memoryStream = new MemoryStream())
        {
            BinaryWriter bw = new BinaryWriter(memoryStream);
            byte[] name = Encoding.UTF8.GetBytes(target.name);
            bw.Write(name.Length);
            bw.Write(name);

            byte[] id = Encoding.UTF8.GetBytes(target.id);
            bw.Write(id.Length);
            bw.Write(id);
            
            bw.Write(target.age);

            return memoryStream.ToArray();
        }
    }

    public static byte[] Serialize_Json(Person target)
    {
        using (MemoryStream memoryStream = new MemoryStream())
        {
            BinaryWriter bw = new BinaryWriter(memoryStream);
            string serialize = JsonConvert.SerializeObject(target);
            bw.Write(serialize);
            return memoryStream.ToArray();
        }
    }

    public static byte[] Serialize_Protobuf(Person target)
    {
        using (MemoryStream memoryStream = new MemoryStream())
        {
            ProtoBuf.Serializer.Serialize(memoryStream,target);
            return memoryStream.ToArray();
        }
    }
}

public class Program
{
    static void Main(string[] args)
    {
        Person lsc = new Person("lsc", "4307232000112600XX",23);
        Person_Proto lsc_Proto = new Person_Proto()
        {
            Name = "lsc",
            Id = "4307232000112600XX",
            //Age = 23,
        };
        outputBytes(Person.Serialize_BinaryWriter(lsc));
        outputBytes(Person.Serialize_Json(lsc));
        outputBytes(Person.Serialize_Protobuf(lsc));
        outputBytes(lsc_Proto.ToByteArray());


    }

    static void outputBytes(byte[] buffer)
    {
        StringBuilder sb = new StringBuilder();
        foreach (var tmp in buffer)
        {
            sb.Append(tmp);
            sb.Append(' ');
        }
        Console.WriteLine(sb.ToString());
    }
}