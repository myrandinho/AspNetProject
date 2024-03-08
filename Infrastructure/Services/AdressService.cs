

using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class AdressService(AppDbContext context)
{
    private readonly AppDbContext _context = context;


    public async Task<AdressEntity> GetAdressAsync(string UserId)
    {
        var adressEntity = await _context.Adresses.FirstOrDefaultAsync(a => a.UserId == UserId);
        return adressEntity!;
    }

    public async Task<bool> CreateAdressAsync(AdressEntity entity)
    {
        _context.Adresses.Add(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateAdressAsync(AdressEntity entity)
    {
        var existing = await _context.Adresses.FirstOrDefaultAsync(x => x.UserId == entity.UserId);
        if (existing != null)
        {
            _context.Entry(existing).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        return false;
    }
}
