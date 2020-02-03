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
using System;
using System.Diagnostics;
using System.IO;
using NLog;

namespace uhf_rfid_catch.Handlers
{
    public class ConsoleLogger
    {
        private static Logger _logger;

        public ConsoleLogger()
        {
            string LogFilePath;
            if (File.Exists("NLog.config"))
            {
                LogFilePath = "NLog.config";
            }
            else if (File.Exists("uhf_rfid_catch.exe.nlog"))
            {
                LogFilePath = "uhf_rfid_catch.exe.nlog";
            }
            else
            {
                LogFilePath = "";
                Console.WriteLine("No Log configuration found..");
            }

            _logger = NLog.Web.NLogBuilder.ConfigureNLog(LogFilePath).GetLogger("ConsoleOnly");
        }

        public void Trigger(string Type, string LogString)
        {
            switch(Type)
            {
                case "Info":
                    _logger.Info(LogString);
                break;
                case "Error":
                    _logger.Error(LogString);
                break;
                case "Warn":
                    _logger.Warn(LogString);
                    break;
                default:
                    _logger.Debug(LogString);
                    break;
            }
            
        }
    }
}
