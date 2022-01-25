//
// ConfigKey.cs
//
// Author:
//       Godwin peter .O <me@godwin.dev>
//
// Copyright (c) 2020 MIT
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System.Collections;
using Microsoft.Extensions.Configuration;

namespace Proton.Frequency.Terminal.Helpers
{
    public class ConfigKey
    {
        private static readonly ConfigContext _settingsContext = new ConfigContext();

        /****
        **    Base configurations for the daemon.
        **    
        **/

        // Port for web view
        public readonly bool BASE_WEB_ENABLE = _settingsContext.GetSection("WebEnable").Get<bool>();

        // Port for web view
        public readonly int BASE_WEB_PORT = (int)_settingsContext.GetSection("WebPort").Get<decimal>();

        // Option to persist capture data for the daemon.
        public readonly bool BASE_PERSIST_DATA = _settingsContext.GetSection("PersistData").Get<bool>();

        // Data persist duration.
        public readonly int BASE_PERSIST_DURATION_DAYS = (int)_settingsContext.GetSection("PersistDurationDays").Get<decimal>();

        // Option to allow all, or a list of host.
        public readonly string BASE_ALLOWED_HOSTS = _settingsContext.GetSection("AllowedHosts").Get<string>();

        /****
        **    Logging configurations for the daemon.
        **    
        **/

        // Enable or disables logging.
        public readonly bool LOGGING_ENABLE = _settingsContext.GetSection("Logging:Enable").Get<bool>();

        // the Default log level for the daemon.
        public readonly string LOGGING_LEVEL = _settingsContext.GetSection("Logging:LogLevel:Default").Get<string>();

        /****
        **    Database or cache configurations for the daemon.
        **    
        **/

        // The Selected type of database to connect.
        public readonly string DATA_STORE_TYPE = _settingsContext.GetSection("Database:Type").Get<string>();

        // The Main store e.g MySql or PostgresSQl
        public readonly string DATA_STORE = _settingsContext.GetSection("Database:Store").Get<string>();

        // Redis instance type, either master or slave.
        public readonly string DATA_REDIS_INSTANCE = _settingsContext.GetSection("Database:Redis:Instance").Get<string>();

        // In Memory SQLite caching options mostly for IOT mode.
        public readonly string DATA_DATABASE_IN_MEMORY = _settingsContext.GetSection("Database:InMemory").Get<string>();

        /****
        **    Server mode configurations for the daemon.
        **    
        **/

        // Option to enable or disable server mode.
        public readonly bool SERVER_ENABLE = _settingsContext.GetSection("ServerMode:Enable").Get<bool>();

        // Option to allow option to save captured data to the store in server mode.
        public readonly bool SERVER_STORE = _settingsContext.GetSection("ServerMode:Store").Get<bool>();

        // Get Configured protocols list.
        public readonly IEnumerable SERVER_PROTOCOLS = _settingsContext.GetList("ServerMode:Protocols");

        // Option to allow forwarding captured data through HTTP request.
        public readonly bool SERVER_FORWARD = _settingsContext.GetSection("ServerMode:Forward").Get<bool>();

        /****
        **    IOT mode configurations for the daemon.
        **    
        **/

        // Option to enable or disable IOT remote functionality.
        public readonly bool IOT_MODE_ENABLE = _settingsContext.GetSection("IOTMode:Enable").Get<bool>();

        // The IOT mode unique identity for the daemon instance.
        public readonly string IOT_UNIQUE_ID = _settingsContext.GetSection("IOTMode:UniqueId").Get<string>();

        // IOT mode required protocol to decode.
        public readonly string IOT_PROTOCOL = _settingsContext.GetSection("IOTMode:Protocol").Get<string>();

        /****
        **    IOT Remote configurations for the daemon.
        **    
        **/

        // Option to enable or disable Remote Forwarding.
        public readonly bool IOT_REMOTE_HOST_ENABLE = _settingsContext.GetSection("IOTMode:Remote:Enable").Get<bool>();
        // Remote Host URI.
        public readonly string IOT_REMOTE_HOST_URL = _settingsContext.GetSection("IOTMode:Remote:HostUrl").Get<string>();

        // Remote Host HTTP call method.
        public readonly string IOT_REMOTE_HOST_METHOD = _settingsContext.GetSection("IOTMode:Remote:Method").Get<string>();

        // Option to enable or disable Remote Host HTTP authentication.
        public readonly bool IOT_REMOTE_HOST_AUTH = _settingsContext.GetSection("IOTMode:Remote:Auth").Get<bool>();

        // Remote Host HTTP authentication username.
        public readonly string IOT_REMOTE_HOST_USERNAME = _settingsContext.GetSection("IOTMode:Remote:Username").Get<string>();

        // Remote Host HTTP authentication password.
        public readonly string IOT_REMOTE_HOST_PASSWORD = _settingsContext.GetSection("IOTMode:Remote:Password").Get<string>();

        // Option for minimum delay in seconds that allowed for an HTTP call.
        public readonly int IOT_MIN_REMOTE_HOST_DELAY = (int)(1000 * _settingsContext.GetSection("IOTMode:Remote:MinHostDelay").Get<decimal>());

        // Option for minimum sync frequency in seconds that allowed for push to server.
        public readonly int IOT_MIN_REMOTE_FREQ = (int)(1000 * _settingsContext.GetSection("IOTMode:Remote:Frequency").Get<decimal>());

        // Option for maximum remote push count to server at once.
        public readonly int IOT_MIN_REMOTE_PUSH_COUNT = (int)_settingsContext.GetSection("IOTMode:Remote:PushCount").Get<decimal>();

        // Others

        // Option for how long a card is disallowed from generation a HTTP push after an entry.
        public readonly int IOT_MIN_REPEAT_FREQ = (int)_settingsContext.GetSection("IOTMode:MinRepeatFrequency").Get<decimal>();

        // Option for how long a card is disallowed from generation a HTTP push after an entry.
        public readonly int IOT_MAX_ASSETS_PERSIST_DAYS = (int)_settingsContext.GetSection("IOTMode:PersistDurationDays").Get<decimal>();

        // Options to enable or disable auto read mode.
        public readonly bool IOT_AUTO_READ = _settingsContext.GetSection("IOTMode:AutoRead").Get<bool>();

        // Options to enable or disable auto read mode.
        public readonly bool IOT_NETWORK_CHECK = _settingsContext.GetSection("IOTMode:NetworkCheck").Get<bool>();

        // Options to enable or disable auto read mode.
        public readonly string IOT_NETWORK_CHECK_ADDRESS = _settingsContext.GetSection("IOTMode:NetworkCheckAddress").Get<string>();

        // Options to configure maximum network connection timeout in seconds.
        public readonly int IOT_NETWORK_CHECK_TIMEOUT = (int)(1000 * _settingsContext.GetSection("IOTMode:NetworkCheckTimeout").Get<decimal>());

        /****
        **    IOT-Serial connection configurations for the daemon.
        **    
        **/

        // Options to enable or disable serial connection for IOT mode.
        public readonly bool IOT_SERIAL_ENABLE = _settingsContext.GetSection("IOTMode:Connections:Serial:Enable").Get<bool>();

        // Options for getting serial connection port name.
        public readonly string IOT_SERIAL_PORTNAME = _settingsContext.GetSection("IOTMode:Connections:Serial:PortName").Get<string>();

        // Options to get serial connection BaudRate.
        public readonly int IOT_SERIAL_BAUDRATE = (int)_settingsContext.GetSection("IOTMode:Connections:Serial:BaudRate").Get<decimal>();

        // Options to get serial connection DataBits.
        public readonly int IOT_SERIAL_DATABITS = (int)_settingsContext.GetSection("IOTMode:Connections:Serial:DataBits").Get<decimal>();

        // Options for number of times serial connection will be retried after a failed connection.
        public readonly int IOT_SERIAL_CONN_RETRY = (int)_settingsContext.GetSection("IOTMode:Connections:Serial:ConnectionRetries").Get<decimal>();

        // Options to configure minimum serial connection timeout in seconds.
        public readonly int IOT_SERIAL_CONN_TIMEOUT = (int)(1000 * _settingsContext.GetSection("IOTMode:Connections:Serial:ConnectionTimeout").Get<decimal>());

        /****
        **    IOT-network TCP/UDP connection configurations for the daemon.
        **    
        **/

        // Options to enable or disable network connection for IOT mode.
        public readonly bool IOT_NETWORK_ENABLE = _settingsContext.GetSection("IOTMode:Connections:Network:Enable").Get<bool>();

        // Options for getting network connection protocol type UDP/TCP.
        public readonly string IOT_NETWORK_TYPE = _settingsContext.GetSection("IOTMode:Connections:Network:Type").Get<string>();

        // Options for getting network connection address.
        public readonly string IOT_NETWORK_ADDRESS = _settingsContext.GetSection("IOTMode:Connections:Network:Address").Get<string>();

        // Options to get network connection port.
        public readonly int IOT_NETWORK_PORT = (int)_settingsContext.GetSection("IOTMode:Connections:Network:Port").Get<decimal>();

        // Options for number of times network connection will be retried after a failed connection.
        public readonly int IOT_NETWORK_CONN_RETRY = (int)_settingsContext.GetSection("IOTMode:Connections:Network:ConnectionRetries").Get<decimal>();

        // Options to configure minimum network connection timeout in seconds.
        public readonly int IOT_NETWORK_CONN_TIMEOUT = (int)(1000 * _settingsContext.GetSection("IOTMode:Connections:Network:ConnectionTimeout").Get<decimal>());

        public ConfigKey() { }
    }
}
