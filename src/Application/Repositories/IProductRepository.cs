using System;
using Domain.Models.Products;

namespace Application.Repositories;

public interface IProductRepository
{
    Task AddRangeAsync(IEnumerable<Product> products);
}
