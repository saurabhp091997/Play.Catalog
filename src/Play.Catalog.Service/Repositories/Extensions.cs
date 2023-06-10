using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Settings;

namespace Play.Catalog.Service.Repositories
{
    public static class Extension
    {
        public static IServiceCollection AddMongoDB(this IServiceCollection services)
        {
            //Add MongoDB database and Run on docker container with proper string representation
            //docker run -d --rm --name mongo -p 27017:27017 -v mongodbdata:/data/db mongo

            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

            //Dependency Injection for Mongo Settings
            services.AddSingleton(serviceProvider =>
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                var serviceSettings = configuration?.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
                var mongoDBSettings = configuration?.GetSection(nameof(MongoDBSettings)).Get<MongoDBSettings>();
                var mongoClient = new MongoClient(mongoDBSettings?.connectionString);
                return mongoClient.GetDatabase(serviceSettings?.ServiceName);
            });

            return services;
        }

        public static IServiceCollection AddMongoRepository<T>(this IServiceCollection services, string collectionName) where T : IEntity
        {
            //Dependency Injection for item repo
            services.AddSingleton<IRepository<Item>>(serviceProvider =>
            {
                var database = serviceProvider.GetService<IMongoDatabase>();
                if (database != null)
                {
                    return new MongoRepository<Item>(database, "items");
                }
                else
                {
                    var configuration = serviceProvider.GetService<IConfiguration>();
                    var serviceSettings = configuration?.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
                    var mongoClient = new MongoClient(configuration?.GetSection(nameof(MongoDBSettings)).Get<MongoDBSettings>()?.connectionString);
                    return new MongoRepository<Item>(mongoClient.GetDatabase(serviceSettings?.ServiceName), "items");
                }
            });

            return services;
        }
    }
}