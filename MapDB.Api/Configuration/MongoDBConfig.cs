namespace MapDB.Api.Configuration{
    public class MongoDBConfig{

        public string Host { get; set; }

        public int Port { get; set; }

        public string User { get; set; }

        public string Password { get; set; }

        public MongoDBConfig(string Host, int Port, string User, string Password){
            this.Host = Host;
            this.Port = Port;
            this.User = User;
            this.Password = Password;
        }

        public string ConnectionString { 
            get{
            return $"mongodb://{User}:{Password}@{Host}:{Port}"; // string interpolation
            } 
        }
    }
}