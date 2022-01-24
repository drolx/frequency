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
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Proton.Frequency.Terminal.Handlers;
using Proton.Frequency.Terminal.Helpers;
using Proton.Frequency.Terminal.Models;

namespace Proton.Frequency.Terminal.Data
{
    public class CaptureContext : DbContext
    {
        private readonly ConsoleLogger _consolelog;
        private readonly ConfigKey _config;
        private readonly NetworkCheck _network;

        // Override cached IOT Sqlite data push to cloud.
        public bool PushStore { get; set; } = false;
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Reader> Readers { get; set; }
        public DbSet<Scan> Scans { get; set; }
        public DbSet<Antenna> Antennae { get; set; }
        public DbSet<Proton.Frequency.Terminal.Models.Terminal> Terminals { get; set; }
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
            var netBool = _network.Status();
            if (!optionsBuilder.IsConfigured)
            {
                if (_config.IOT_MODE_ENABLE
                    && !_config.SERVER_STORE
                    && !_config.SERVER_ENABLE)
                {
                    if (PushStore && netBool)
                    {
                        // Bypass RAM to use embedded DB
                        optionsBuilder.UseSqlite(_config.DATA_STORE);
                    }
                    else if (!_network.Status())
                    {
                        // Use embedded DB
                        optionsBuilder.UseSqlite(_config.DATA_STORE);
                    }
                    else if (netBool)
                    {
                        // Use RAM for TEMP caching.
                        var connection = new SqliteConnection(_config.DATA_DATABASE_IN_MEMORY);
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
                            optionsBuilder.UseSqlite(_config.DATA_STORE);
                            break;
                        case "Postgres":
                            optionsBuilder.UseSqlite(_config.DATA_STORE);
                            break;
                        case "Mssql":
                            optionsBuilder.UseSqlite(_config.DATA_STORE);
                            break;
                        default:
                            optionsBuilder.UseSqlite(_config.DATA_STORE);
                            break;
                    }
                }

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reader>().HasIndex(e => new { e.UniqueId }).IsUnique();
            modelBuilder.Entity<Reader>().HasMany(k => k.Antennae)
                .WithOne(e => e.Reader).IsRequired().IsRequired(false);

            modelBuilder.Entity<Tag>().HasIndex(e => new { e.UniqueId }).IsUnique();

            modelBuilder.Entity<Antenna>().HasIndex(e => new { e.UniqueId }).IsUnique();
            modelBuilder.Entity<Antenna>().HasIndex(e => new { e.ReaderId }).IsUnique();
            modelBuilder.Entity<Antenna>().HasOne(k => k.Reader)
                .WithMany(e => e.Antennae).IsRequired(false);

            base.OnModelCreating(modelBuilder);
        }
    }

}
