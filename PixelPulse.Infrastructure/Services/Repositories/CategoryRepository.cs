using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using PixelPulse.Application.Interfaces.Repositories;
using PixelPulse.Domain.Entities;
using PixelPulse.Infrastructure.Content.Data;

namespace PixelPulse.Infrastructure.Services.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppIdentityContext _context;
    private readonly IMemoryCache _cache;
    private readonly ILogger<CategoryRepository> _logger;
    private readonly string _cacheKeyAll = "categories_all";
    private readonly string _cacheKeyPrefix = "category_";

    public CategoryRepository(AppIdentityContext context, IMemoryCache cache, ILogger<CategoryRepository> logger)
    {
        _context = context;
        _cache = cache;
        _logger = logger;
    }
    
    public async Task CreateAsync(Category entity, CancellationToken cancellationToken = default)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync(cancellationToken))
        {
            try
            {
                await _context.Categories.AddAsync(entity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                _cache.Remove(_cacheKeyAll);
                _cache.Set(_cacheKeyPrefix + entity.Id, entity);
                _logger.LogInformation($"Category created: {entity.Id}");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.LogError($"Category creation failed, name {entity.Name}. {ex.Message}");
                throw;
            }
        }
    }

    public async Task<Category> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        if (_cache.TryGetValue(_cacheKeyPrefix + id, out Category cachedCategory))
        {
            _logger.LogInformation($"Category found: {_cacheKeyPrefix + id}");
            return cachedCategory;
        }
        else
        {
            cachedCategory = await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
            _logger.LogInformation($"Category found: {cachedCategory.Id} {cachedCategory.Name}");

            if (cachedCategory != null)
            {
                _cache.Set(_cacheKeyPrefix + id, cachedCategory, TimeSpan.FromMinutes(30));
                _logger.LogInformation($"Category cache created: {_cacheKeyPrefix + id}");
            }
        
            return cachedCategory;
        }
    }

    public async Task<IReadOnlyList<Category>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        if (_cache.TryGetValue(_cacheKeyAll, out IReadOnlyList<Category> cachedCategories))
        {
            _logger.LogInformation($"Category found: {_cacheKeyAll}");
            return cachedCategories;
        }
        else
        {
            cachedCategories = await _context.Categories
                .AsNoTracking()
                .ToListAsync(cancellationToken);
            _logger.LogInformation($"Categories founds");

            if (cachedCategories != null)
            {
                _cache.Set(_cacheKeyAll, cachedCategories, TimeSpan.FromMinutes(30));
                _logger.LogInformation($"Categories cache created: {_cacheKeyAll}");
            }
            
            return cachedCategories;
        }
    }

    public async Task UpdateAsync(Category entity, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            _context.Categories.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            _cache.Remove(_cacheKeyAll);
            _cache.Remove(_cacheKeyPrefix + entity.Id);
            _cache.Set(_cacheKeyPrefix + entity.Id, entity);
            _logger.LogInformation($"Category updated: {entity.Id}");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            _logger.LogError(ex, $"Category update failed: {entity.Id}. {ex.Message}");
            throw;
        }
    }

    public async Task DeleteAsync(Category entity, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            _context.Categories.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            _cache.Remove(_cacheKeyAll);
            _cache.Remove(_cacheKeyPrefix + entity.Id);
            _logger.LogInformation($"Category deleted: {entity.Id}");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            _logger.LogError(ex, $"Category delete failed: {entity.Id}. {ex.Message}");
            throw;
        }
    }

    public async Task CreateRangeAsync(List<Category> categories, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await _context.Categories.AddRangeAsync(categories, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            _cache.Remove(_cacheKeyAll);
            _cache.Set(_cacheKeyAll, categories, TimeSpan.FromMinutes(30));
            _logger.LogInformation($"Category created: {categories.Count}");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            _logger.LogError(ex, $"Category range creation failed. {ex.Message}");
            throw;
        }
    }

    public async Task DeleteRangeAsync(List<Category> categories, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            _context.Categories.RemoveRange(categories);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            _cache.Remove(_cacheKeyAll);
            _logger.LogInformation($"Category deleted: {categories.Count}");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            _logger.LogError(ex, $"Category range deletion failed. {ex.Message}");
            throw;
        }
    }
}