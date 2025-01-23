using Microsoft.EntityFrameworkCore;
using PixelPulse.Application.Interfaces.Repositories;
using PixelPulse.Domain.Entities;
using PixelPulse.Infrastructure.Content.Data;

namespace PixelPulse.Infrastructure.Services.Repositories;

public class ReviewRepository : IReviewRepository
{
    private AppIdentityContext _context;

    public ReviewRepository(AppIdentityContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(Review entity)
    {
        await _context.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<Review> GetByIdAsync(Guid id)
    {
        return await _context.Reviews
            .FirstOrDefaultAsync(k => k.ReviewId == id);
    }

    public async Task<List<Review>> SelectAsync()
    {
        return await _context.Reviews.ToListAsync();
    }

    public async Task UpdateAsync(Review entity)
    {
        entity.UpdatedAt = DateTime.Now;
        _context.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Review entity)
    {
        _context.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Review>> GetReviewByUserIdAsync(string userId)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId);
        
        if (user == null)
            return null;

        return await _context.Reviews
            .Where(k => k.AppUserId == user.Id)
            .ToListAsync();
    }
}