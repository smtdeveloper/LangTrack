using LangTrack.Domain.Entities;

namespace LangTrack.Application.Interfaces;

public interface IPermissionRepository
{
    Task<IEnumerable<Permission>> GetAllAsync();
    Task<Permission?> GetByIdAsync(Guid id);
    Task<Permission> CreateAsync(Permission permission);
    Task<bool> UpdateAsync(Permission permission);
    Task<bool> DeleteAsync(Guid id);
}
