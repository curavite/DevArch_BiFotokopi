﻿
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.DataAccess;
using Entities.Concrete;
using Entities.Dtos;

namespace DataAccess.Abstract
{
    public interface IProductRepository : IEntityRepository<Product>
    {
        Task<IEnumerable<ProductDto>> GetProductsDtos();
        Task<Product> Delete2(int productId);
        Task<Boolean> GetSize(int productId, string size);
    }
}