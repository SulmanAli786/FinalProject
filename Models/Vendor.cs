﻿using System;
using System.Collections.Generic;

namespace Shopping_Cart.Models
{
    public partial class Vendor
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public string? PhoneNo { get; set; }
        public string? Email { get; set; }
    }
}
