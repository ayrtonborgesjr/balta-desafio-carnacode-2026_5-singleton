using ConfigurationManager.Console.Services;
using ConfigManager = ConfigurationManager.Console.Core.ConfigurationManager;

var db = new DatabaseService();
var api = new ApiService();
var cache = new CacheService();
var log = new LoggingService();

db.Connect();
api.MakeRequest();
cache.Connect();
log.Log("Sistema iniciado");

// Teste de consistência
ConfigManager.Instance.UpdateSetting("LogLevel", "Debug");

log.Log("Agora em Debug");