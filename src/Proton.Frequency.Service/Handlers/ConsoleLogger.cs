//
// ConsoleLogger.cs
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
using Proton.Frequency.Service.Helpers;

namespace Proton.Frequency.Service.Handlers
{
    public class ConsoleLogger
    {
        private readonly ConfigKey _config;
        private static Logger _logger;

        public ConsoleLogger()
        {
            _config = new ConfigKey();

            _logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetLogger("ConsoleOnly");
        }

        public void Trigger(string Type, string returnText)
        {
            if (_config.LOGGING_ENABLE)
            {
                try
                {
                    switch (Type)
                    {
                        case "Error":
                            _logger.Error(returnText);
                            break;
                        case "Info":
                            _logger.Info(returnText);
                            break;
                        case "Warn":
                            _logger.Warn(returnText);
                            break;
                        case "Debug":
                            _logger.Debug(returnText);
                            break;
                        case "Fatal":
                            _logger.Fatal(returnText);
                            break;
                        default:
                            _logger.Error("Something really bad happened");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Something bad happened");
                }
            }

        }
    }
}
