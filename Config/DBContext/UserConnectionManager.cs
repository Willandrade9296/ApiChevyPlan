using System.Collections.Concurrent;

namespace ApiDatalife.Config.DBContext
{


    public interface IUserConnectionManager
    {
        void SetUserConnection(string userId, string connectionString);
        string GetUserConnection(string userId);
        void RemoveUserConnection(string userId);
    }
    public class UserConnectionManager : IUserConnectionManager
    {
        private readonly ConcurrentDictionary<string, string> _userConnections = new();

        public void SetUserConnection(string userId, string connectionString)
        {
            _userConnections.AddOrUpdate(userId, connectionString, (key, oldValue) => connectionString);
        }

        public string GetUserConnection(string userId)
        {
            return _userConnections.TryGetValue(userId, out var connection) ? connection : null;
        }

        public void RemoveUserConnection(string userId)
        {
            _userConnections.TryRemove(userId, out _);
        }
    }
}
