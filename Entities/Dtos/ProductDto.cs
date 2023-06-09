﻿using Core.Entities;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class ProductDto: Concrete.BaseEntity, IDto
    {
        public string UserName { get; set; }

        public int  UserId { get; set; }

        public string ProductName { get; set; }

        public string ProductColor { get; set; }

        public string Size { get; set; }
    }
}
