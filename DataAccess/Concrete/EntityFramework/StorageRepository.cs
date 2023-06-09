﻿
using System;
using System.Linq;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Concrete.EntityFramework.Contexts;
using DataAccess.Abstract;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Entities.Dtos;
using Core.Entities.Concrete;
using static MongoDB.Driver.WriteConcern;

namespace DataAccess.Concrete.EntityFramework
{
    public class StorageRepository : EfEntityRepositoryBase<Storage, ProjectDbContext>, IStorageRepository
    {
        public StorageRepository(ProjectDbContext context) : base(context)
        {
        }

        public async Task<bool> AmountControll(int productId, int amount)
        {
            var isOkey = await Context.Storages.AnyAsync(p=>p.ProductId == productId && p.UnitsInStock < amount);
            return isOkey;
        }



        //public async Task<bool> ExistsProduct(int productId, string size, int amount)
        //{
        //    var isOkeyWareHouse = await Context.Storages.AnyAsync(u => u.ProductId == productId && u.UnitsInStock >= amount && u.IsReady == true && u.isDeleted == false);
        //    return isOkeyWareHouse;
        //}

        public async Task<IEnumerable<StorageDto>> GetStorageDtos()
        {
            var list =  await (from store in Context.Storages
                              join user in Context.Users on store.CreatedUserId equals user.UserId
                              join product in Context.Products on store.ProductId equals product.Id
                              where store.isDeleted == false
                              select new StorageDto
                              {
                                  Id = store.Id,
                                  ProductId = store.ProductId,
                                  ProductName = product.ProductName,
                                  IsReady = store.IsReady,
                                  UnitsInStock = store.UnitsInStock,
                                  UserName = user.FullName,
                                  CreatedUserId = store.CreatedUserId,
                                  CreatedDate  = store.CreatedDate,
                                  isDeleted = store.isDeleted,
                                  LastUpdatedDate = store.LastUpdatedDate,
                                  LastUpdatedUserId = store.LastUpdatedUserId,
                                  Status = store.Status,
                                  Size = store.Size
                              }).ToListAsync();

            return list;
        }

        public async Task<bool> StorageReadyControll(int productId, bool status)
        {
            var isOkeyStorage = await Context.Storages.AnyAsync(u => u.ProductId == productId && u.Status==true && u.IsReady == true && u.isDeleted == false);
            return isOkeyStorage;
        }
    }
}
