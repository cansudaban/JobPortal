using StackExchange.Redis;

namespace JobPortal.Common.Services
{
    public class RedisConnectionService
    {
        private readonly ConnectionMultiplexer _redis;

        public RedisConnectionService(string connectionString)
        {
            _redis = ConnectionMultiplexer.Connect(connectionString);
        }

        public IDatabase GetDatabase() => _redis.GetDatabase();
    }
}
