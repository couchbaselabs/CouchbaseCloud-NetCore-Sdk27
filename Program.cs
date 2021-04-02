using System;
using Couchbase.Extensions.DependencyInjection;
using Couchbase.Extensions.DnsDiscovery;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CouchbaseCloud_NetCore
{
    class Program
    {
        static void Main(string[] args)
        {
            var section = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Couchbase");
            IServiceCollection services = new ServiceCollection();
            services.AddLogging(configure => configure.AddFile("Logs/myapp-{Date}.txt", LogLevel.Debug));
            services.AddCouchbase(section);
            services.AddCouchbaseBucket<INamedBucketProvider>("beer-sample");
            services.AddCouchbaseDnsDiscovery();

            var provider = services.BuildServiceProvider();
            var bucketProvider = provider.GetService<INamedBucketProvider>();
            var bucket = bucketProvider.GetBucket();

            var result = bucket.Upsert("id", new {The = "JsonDoc"});
            Console.WriteLine($"Added a doc: {result.Success}");
            Console.Read();
        }
    }
}
