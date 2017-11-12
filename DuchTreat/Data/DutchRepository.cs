﻿using DutchTreat.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuchTreat.Data
{
    public class DutchRepository : IDutchRepository
    {
        private readonly DuchContext _ctx;
        private readonly ILogger<DutchRepository> _logger;

        public DutchRepository(DuchContext ctx, ILogger<DutchRepository> logger)
        {
            _ctx = ctx;
            _logger = logger;
        }

        public void AddEntity(object model)
        {
            _ctx.Add(model);
        }

        public void AddOrder(Order newOrder)
        {
            foreach (var item in newOrder.Items)
            {
                item.Product = _ctx.Products.Find(item.Product.Id);
            }
            AddEntity(newOrder);
        }

        public IEnumerable<Order> GetAllOrders(bool includeItems)
        {
            if (includeItems) { 
            return _ctx
                .Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .ToList();
            }
            else
            {
                return _ctx
                      .Orders
                      .ToList();
            }
        }

        public IEnumerable<Order> GetAllOrdersByUser(string user, bool includeItems)
        {
            if (includeItems)
            {
                return _ctx
                    .Orders
                    .Where(o => o.User.UserName == user)
                    .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                    .ToList();
            }
            else
            {
                return _ctx
                      .Orders
                      .Where(o => o.User.UserName == user)
                      .ToList();
            }
        }

        public IEnumerable<Product> GetAllProducts()
        {
            try
            {

            _logger.LogInformation("GetAllProducts was called");
            return _ctx.Products
                       .OrderBy(p => p.Title)
                       .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get all products:{0}", ex);
                return null;
            }


        }

        public Order GetOrderById(string username, int id)
        {
            return _ctx
                .Orders
                .Where(o => o.Id == id && o
                .User.UserName == username)
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefault();
        }

        public IEnumerable<Product> GetProductByCategory(string category)
        {
            return _ctx.Products
                    .Where(p => p.Category == category)
                    .ToList();
        }

        public bool SaveAll()
        {
           return _ctx.SaveChanges() > 0;
        }
    }
}