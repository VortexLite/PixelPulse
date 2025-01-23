using Microsoft.AspNetCore.Mvc;
using PixelPulse.Application.Interfaces.Repositories;
using PixelPulse.Domain.Entities;

namespace PixelPulse.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoriesController(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> CreateCategoryAsync([FromBody]Category category)
    {
        if (category == null)
        {
            return BadRequest("Category cannot be null.");
        }
        await _categoryRepository.CreateAsync(category);
        return Ok(category);
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetByIdCategoryAsync(Guid categoryId)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId);
        if (category == null)
        {
            return NotFound();
        }
        
        return Ok(category);
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetAllCategoriesAsync()
    {
        var categories = await _categoryRepository.SelectAsync();
        if (categories == null)
        {
            return NotFound();
        }
        
        return Ok(categories);
    }

    [HttpPut("[action]")]
    public async Task<IActionResult> UpdateCategoryAsync(Category category)
    {
        await _categoryRepository.UpdateAsync(category);
        return Ok(category);
    }

    [HttpDelete("[action]")]
    public async Task<IActionResult> DeleteCategoryAsync(Guid categoryId)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId);
        if (category == null)
        {
            return NotFound();
        }

        await _categoryRepository.DeleteAsync(category);
        return NoContent();
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetCategoryByNameAsync(string name)
    {
        var category = await _categoryRepository.GetCategoryByNameAsync(name);
        if (category == null)
        {
            return NotFound();
        }
        
        return Ok(category);
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetCategoriesWithSubCategoriesAsync()
    {
        var categories = await _categoryRepository.GetCategoriesWithSubCategoriesAsync();
        if (categories == null)
        {
            return NotFound();
        }
        return Ok(categories);
        
    }
}