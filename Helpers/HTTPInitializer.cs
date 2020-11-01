//
// HTTPInitializer.cs
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
using System.Net.Http;
using Tiny.RestClient;

namespace RFIDIOT.Helpers
{
    public class HTTPInitializer
    {
        private ConfigKey _config;
        private TinyRestClient _httpclient;

        public HTTPInitializer()
        {
            _config = new ConfigKey();
            // Initialize HTTP calls
            _httpclient = new TinyRestClient(new HttpClient(), _config.IOT_REMOTE_HOST_URL);
            _httpclient.Settings.DefaultTimeout = TimeSpan.FromSeconds(_config.IOT_MIN_REMOTE_HOST_DELAY);
            _httpclient.Settings.DefaultHeaders.Add("X-IOT", "by gpproton...");
            // HTTP Auth
            if (!String.IsNullOrEmpty(_config.IOT_REMOTE_HOST_USERNAME)
                && !String.IsNullOrEmpty(_config.IOT_REMOTE_HOST_PASSWORD)
                && _config.IOT_REMOTE_HOST_AUTH)
            {
                _httpclient.Settings.DefaultHeaders.AddBasicAuthentication(_config.IOT_REMOTE_HOST_USERNAME, _config.IOT_REMOTE_HOST_PASSWORD);
            }
        }

        public TinyRestClient Resolve()
        {
            return _httpclient;
        }
        
        


    }
}
