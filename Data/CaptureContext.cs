//
// CaptureContext.cs
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
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using uhf_rfid_catch.Handlers;
using uhf_rfid_catch.Helpers;
using uhf_rfid_catch.Models;

namespace uhf_rfid_catch.Data
{
    public class CaptureContext : DbContext
    {
        private readonly ConsoleLogger _consolelog;
        private readonly ConfigKey _config;
        private readonly NetworkCheck _network;
        
        // Override cached IOT Sqlite data push to cloud.
        public bool PushStore { get; set; } = false;
        
        // Transition between active store.
        public string HotSwap { get; set; } = "None";
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Reader> Readers { get; set; }
        public DbSet<Scan> Scans { get; set; }
        public CaptureContext()
        {
            _consolelog = new ConsoleLogger();
            _config = new ConfigKey();
            _network = new NetworkCheck();
        }

        public CaptureContext(DbContextOptions<CaptureContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                if (_config.IOT_MODE_ENABLE)
                {
                    if (PushStore && _network.Status())
                    {
                        if (HotSwap == "Store")
                        {
                            HotSwap = "Stream";
                        }
                        _consolelog.Trigger("Info", "IOT-MODE MEMORY BYPASS STORE");
                        optionsBuilder.UseSqlite(_config.DATA_STORE);
                    }
                    else if (!_network.Status())
                    {
//                        _consolelog.Trigger("Info", "IOT-MODE MAIN STORE");
                        optionsBuilder.UseSqlite(_config.DATA_STORE);
                    }
                    else if (_network.Status())
                    {
//                        _consolelog.Trigger("Info", "IOT-MODE MEMORY");
                        var connection = new SqliteConnection(_config.DATA_DATABASE_INMEMORY);
                        connection.Open();
                        optionsBuilder.UseSqlite(connection);
                    }
                    
                    
                }
                else
                {
                    switch (_config.DATA_STORE_TYPE)
                    {
                        case "Sqlite":
                            optionsBuilder.UseSqlite(_config.DATA_STORE); 
                            break;
                        case "Mysql":
                            break;
                    }
                }
                
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reader>().HasIndex(e => new {e.UniqueId}).IsUnique();
            modelBuilder.Entity<Tag>().HasIndex(e => new {e.UniqueId}).IsUnique();
            
            base.OnModelCreating(modelBuilder);
        }
    }

}
