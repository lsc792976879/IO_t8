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

    public override string ToString()
    {
        return "姓名为：" + this.name + "，年龄为：" + this.age + ",身份证号为：" + this.id;
    }

    public static void Serialize_Encoding(Person target,FileStream fs)
    {
        BinaryWriter bw = new BinaryWriter(fs);
        byte[] name = Encoding.UTF8.GetBytes(target.name);
        bw.Write(name.Length);
        bw.Write(name);

        byte[] id = Encoding.UTF8.GetBytes(target.id);
        bw.Write(id.Length); 
        bw.Write(id);
        bw.Write(target.age);
        
    }
    
    public static Person Deserialize_Encoding(FileStream fs)
    {
        Person tmp = new Person();
        BinaryReader br = new BinaryReader(fs);
        int len;
        
        len = br.ReadInt32();
        tmp.name = Encoding.UTF8.GetString(br.ReadBytes(len));
        
        len = br.ReadInt32();
        tmp.id = Encoding.UTF8.GetString(br.ReadBytes(len));
        
        tmp.age = br.ReadInt32();
        return tmp;
    }

    public static void Serialize_Json(Person target, string filePath)
    {
        File.WriteAllText(filePath,JsonConvert.SerializeObject(target));
    }

    public static Person Deserialize_Json(string filePath)
    {
        return JsonConvert.DeserializeObject<Person>(File.ReadAllText(filePath));
    }
    
    public static byte[] Serialize_Protobuf(Person target,MemoryStream memoryStream)
    {
        ProtoBuf.Serializer.Serialize(memoryStream,target);
        return memoryStream.ToArray();
    }
    
    
}

public class Program
{
    static void Main(string[] args)
    {
        //创建对象的实例
        Person lsc = new Person("lsc", "4307232000112600XX",23);
        Person_Proto lsc_Proto = new Person_Proto()
        {
            Name = "lsc",
            Id = "4307232000112600XX",
            Age = 23,
        };
        
        //使用binaryreader和binarywriter来序列化数据
        Console.WriteLine("使用binaryreader和binarywriter来序列化数据:");
        string filePath = @"C:\Users\lsc\Desktop\C#学习\IO\IOlearning3\ConsoleApp3\data\data_binaryreader.txt";
        using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
        {
            Person.Serialize_Encoding(lsc,fs);
            fs.Seek(0, SeekOrigin.Begin);
            Person result_binary = Person.Deserialize_Encoding(fs);
            Console.WriteLine(result_binary.ToString());
        }
        outputBytes(File.ReadAllBytes(filePath));
        

        //使用json进行序列化数据
        Console.WriteLine("\n使用Json来序列化数据:");
        string jsonFilePath = @"C:\Users\lsc\Desktop\C#学习\IO\IOlearning3\ConsoleApp3\data\data_json.json";
        if(!File.Exists(jsonFilePath)) File.Create(jsonFilePath).Close(); 
        Person.Serialize_Json(lsc,jsonFilePath);
        Person result_json = Person.Deserialize_Json(jsonFilePath);
        
        Console.WriteLine(result_json.ToString());
        outputBytes(File.ReadAllBytes(jsonFilePath));
        
        
        //使用protobuf进行序列化数据
        Console.WriteLine("\n使用protobuf来序列化数据:");
        string protoFilePath = @"C:\Users\lsc\Desktop\C#学习\IO\IOlearning3\ConsoleApp3\data\data_proto.txt";
        using (FileStream fs = new FileStream(protoFilePath, FileMode.OpenOrCreate))
        {
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(lsc_Proto.ToByteArray());

            fs.Seek(0, SeekOrigin.Begin);
            BinaryReader br = new BinaryReader(fs);
            var buffer = br.ReadBytes((int)fs.Length);
            Person_Proto result_Proto = Person_Proto.Parser.ParseFrom(buffer);
            Console.WriteLine(result_Proto.ToString());
        }
        outputBytes(File.ReadAllBytes(protoFilePath));
        
    }

    static void outputBytes(byte[] buffer)
    {
        StringBuilder sb = new StringBuilder();
        foreach (var tmp in buffer)
        {
            sb.Append(tmp);
            sb.Append(' ');
        }
        Console.WriteLine("字节数组为：" + sb.ToString());
    }
}