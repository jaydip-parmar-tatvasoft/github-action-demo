﻿using Data.Exceptions;
using Domain.Entities.Roles;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly ApplicationDbContext mDataContext;

    public RoleRepository(ApplicationDbContext dbContext)
    {
        mDataContext = dbContext;
    }

    public async Task<Role?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        Role? RawData = await mDataContext.Roles.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        return RawData;
    }

    public async Task CreateAsync(Role role, CancellationToken cancellationToken = default)
    {
        Role? ExistingItem = await GetByIdAsync(role.Id, cancellationToken);
        if (ExistingItem is not null)
            throw new DuplicateDataException("Role Id", role.Id.ToString());

        Role NewItem = role;

        await mDataContext.Roles.AddAsync(NewItem);
    }

    public async Task<List<Role>> GetAll(CancellationToken cancellationToken = default)
    {
        List<Role> RawData = await mDataContext.Roles.ToListAsync(cancellationToken);
        return RawData;
    }
    public async Task<bool> IsNameUniqueAsync(string name, CancellationToken cancellationToken)
    {
        Role? RawData = await mDataContext.Roles.FirstOrDefaultAsync(x => x.Name == name);
        return (RawData is null);
    }

    public async Task<Role?> GetByName(string name, CancellationToken cancellationToken)
    {
        Role? RawData = await mDataContext.Roles.FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
        return RawData;
    }
}
