using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Agent
{
    public class Agent
    {
        public const int MaxValue = 100;
        public const string TimestampFormat = "yyyy-MM-dd'T'HH:mm:ssZ";

        public readonly string Name;
        private readonly Random random;
        private readonly DataContractJsonSerializer jsonSerializer;

        public Agent(string name)
        {
            Name = name;
            random = new Random();
            jsonSerializer = new DataContractJsonSerializer(typeof(Message), new DataContractJsonSerializerSettings {
                DateTimeFormat = new DateTimeFormat(TimestampFormat)
            });
        }

        private byte NextRand()
        {
            return (byte)(random.Next(MaxValue) + 1);
        }

        public void GenerateMessage(Stream stream)
        {
            var msg = new Message(Name, NextRand(), NextRand(), NextRand());
            jsonSerializer.WriteObject(stream, msg);
        }

        [DataContract]
        public struct Message
        {
            [DataMember(Name = "name")]
            public readonly string Name;

            [DataMember(Name = "created_at")]
            public readonly DateTime CreatedAt;

            [DataMember(Name = "a")]
            public readonly byte A;

            [DataMember(Name = "b")]
            public readonly byte B;

            [DataMember(Name = "c")]
            public readonly byte C;

            public Message(string name, byte a, byte b, byte c)
            {
                Name = name;
                CreatedAt = DateTime.Now;
                A = a;
                B = b;
                C = c;
            }
        }
    }
}
