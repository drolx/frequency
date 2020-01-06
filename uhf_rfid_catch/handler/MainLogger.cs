//
// MainLogger.cs
//
// Author:
//       Godwin peter .O <me@godwin.dev>
//
// Copyright (c) 2020 MIT 2019
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
using System.IO;
using System.Linq;
using System.Reflection;
using NLog;

namespace uhf_rfid_catch.handler
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

        public void Error(String returnText)
        {
            try
            {
                _logger.Error(returnText);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Something bad happened");
            }
        }

        public void Info(String returnText)
        {
            try
            {
                _logger.Info(returnText);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Something bad happened");
            }
        }

        public void Warn(String returnText)
        {
            try
            {
                _logger.Warn(returnText);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Something bad happened");
            }
        }

        public void Debug(String returnText)
        {
            try
            {
                _logger.Debug(returnText);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Something bad happened");
            }
        }
    }
}
