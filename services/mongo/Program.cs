using Confluent.Kafka;
using Confluent.Kafka.Admin;
using mongo;
using mongo.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json.Serialization;

var config = new ConsumerConfig
{
    BootstrapServers = "broker:29092",
    GroupId = "group.mongo",
    AutoOffsetReset = AutoOffsetReset.Earliest
};

var topics = new List<string>() { "postgres.public.institutes", "postgres.public.departments", "postgres.public.specialities", "postgres.public.courses" };

using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
consumer.Subscribe(topics);

while(true)
{
    try
    {
        Console.WriteLine("CONSUMING");
        var result = consumer.Consume();

        JObject jObject = JObject.Parse(result.Value);
        var beforeJson = jObject["before"]?.ToString();
        var afterJson = jObject["after"]?.ToString();
        var op = jObject["op"]?.ToString();
        var table = jObject["source"]?["table"]?.ToString()!;

        Console.WriteLine($"MESSAGE: {table} - {op} : \n {afterJson}");
        if (op == "u") Console.WriteLine("BEFORE JSON ===>:\n" + result.Value);

        var updater = CollectionProvider.GetUpdater(table);
        updater?.Execute(beforeJson!, afterJson!, op!);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"ERROR: {ex.Message} \n {ex.StackTrace}");
        Thread.Sleep(5000);
    }
}