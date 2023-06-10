namespace Play.Catalog.Service.Settings
{
    public class MongoDBSettings
    {
        public string Host { get; init; } = null!;

        public int Port { get; init; }

        public string connectionString => $"mongodb://{Host}:{Port}";

    }
}
