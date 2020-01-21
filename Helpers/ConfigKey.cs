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
        public static readonly int WEB_PORT = Convert.ToInt32(_settingsContext.Resolve("WebPort"));
        // Option to persist capture data for the daemon.
        public static readonly bool PERSIST_DATA = Convert.ToBoolean(_settingsContext.Resolve("PersistData"));
        // Data persist duration.
        public static readonly int PERSIST_DURATION_DAYS = Convert.ToInt32(_settingsContext.Resolve("PersistDurationDays"));
        // Option to allow all, or a list of host.
        public static readonly string ALLOWED_HOSTS = _settingsContext.Resolve("AllowedHosts");
        
        
        /****
        **    Logging configurations for the daemon.
        **    
        **/
        // Enable or disables logging.
        public static readonly bool ENABLE_LOGGING = Convert.ToBoolean(_settingsContext.Resolve("Logging:Enable"));
        // the Default log level for the daemon.
        public static readonly string LOGGING_LEVEL = _settingsContext.Resolve("Logging:LogLevel:Default");
        
    }
}
