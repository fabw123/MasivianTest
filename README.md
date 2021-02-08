# MasivianTest
Environment variables:
- SERVER_HOST: host direction of the Redis database
- LOG_MIN_LEVEL: minimun configuration of the Log
- CW_ACCESS_KEY: AWS Access key used for saving the logs in Cloud Watch
- CW_SECRET_KEY: AWS Secret key used for saving the logs in Cloud Watch

External libraries added:
- AWS.Logger.NLog
- Newtonsoft.Json
- NLog
- NLog.Web.AspNetCore
- StackExchange.Redis
