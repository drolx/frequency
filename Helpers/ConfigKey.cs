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
using System;
using System.Collections;
using Microsoft.Extensions.Configuration;

namespace uhf_rfid_catch.Helpers
{
    public class ConfigKey
    {
        private static readonly ConfigContext _settingsContext = new ConfigContext();

        /****
        **    Base configurations for the daemon.
        **    
        **/
        
        // Port for web view
        public readonly int WEB_PORT = _settingsContext.GetSection("WebPort").Get<int>();
        // Option to persist capture data for the daemon.
        public readonly bool PERSIST_DATA = _settingsContext.GetSection("PersistData").Get<bool>();
        // Data persist duration.
        public readonly int PERSIST_DURATION_DAYS = _settingsContext.GetSection("PersistDurationDays").Get<int>();
        // Option to allow all, or a list of host.
        public readonly string ALLOWED_HOSTS = _settingsContext.GetSection("AllowedHosts").Get<string>();
        
        
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
        
        // The Main store e.g MySql or PostgresSQl
        public static readonly string DATABASE_STORE = _settingsContext.GetSection("Database:Store").Get<string>();
        
        /// <summary>
        /// Redis distributed caching configurations.
        /// </summary>
        // Redis hostname or IP address information.
        public static readonly string REDIS_HOST = _settingsContext.GetSection("Database:Redis:Host").Get<string>();
        // Redis host port
        public static readonly int REDIS_PORT = _settingsContext.GetSection("Database:Redis:Port").Get<int>();
        // Redis instance type, either master or slave.
        public static readonly string REDIS_INSTANCE = _settingsContext.GetSection("Database:Redis:Instance").Get<string>();
        
        // In Memory SQLite caching options mostly for IOT mode.
        public static readonly string DATABASE_INMEMORY = _settingsContext.GetSection("Database:InMemory").Get<string>();
        
        /****
        **    Server mode configurations for the daemon.
        **    
        **/
        
        // Option to enable or disable server mode.
        public readonly bool SERVER_ENABLE = _settingsContext.GetSection("ServerMode:Enable").Get<bool>();
        // Get Configured protocols list.
        public readonly IEnumerable SERVER_PROTOCOLS = _settingsContext.GetList("ServerMode:Protocols");
        
        
        
        
        
        // Usage of array type temp.
//        ConfigKey nq1 = new ConfigKey();
//        IEnumerable test1 = nq1.SERVER_PROTOCOLS;
//            foreach (IConfigurationSection VARIABLE in test1)
//            {
//                Console.WriteLine(VARIABLE.GetValue<string>("ProtocolName"));
//            }

    }
}
