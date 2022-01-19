//
// Scan.cs
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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Proton.Frequency.Service.Models
{
    public class Scan
    {
        public Scan()
        {
        }

        [Key, Required]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ReaderId { get; set; }
        public Guid TagId { get; set; }
        public Guid AntennaId { get; set; }
        public DateTime CaptureTime { get; set; } = DateTime.Now;
        [MaxLength(1)]
        public int? synced { get; set; } = 0;
        public Reader Reader { get; set; }
        public Tag Tag { get; set; }
        public Antenna Antenna { get; set; }
    }
}
