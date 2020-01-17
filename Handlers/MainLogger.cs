//
// MainLogger.cs
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
using NLog;

namespace uhf_rfid_catch.Handlers
{
    public interface IMainLogger
    {
        void Info(String returnText);
        void Error(String returnText);
        void Warn(String returnText);
        void Debug(String returnText);
    }

    public class MainLogger : IMainLogger
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public MainLogger()
        {
        }

        public void Trigger(String logType, String returnText)
        {
            try
            {
                switch (logType) {
                    case "Error" :
                    Error(returnText);
                    break;
                    case "Info":
                        Info(returnText);
                        break;
                    case "Warn":
                        Warn(returnText);
                        break;
                    case "Debug":
                        Debug(returnText);
                        break;
                    default:
                        Error("Something really bad happened");
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Something bad happened");
            }
        }

            public void Error(String returnText)
            {
                    _logger.Error(returnText);
            }

            public void Info(String returnText)
            {
                    _logger.Info(returnText);
            }

            public void Warn(String returnText)
            {
                _logger.Warn(returnText);
            }

            public void Debug(String returnText)
            {
                _logger.Debug(returnText);
            }
    }
}
