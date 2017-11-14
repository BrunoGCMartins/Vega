using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vega.Core;
using vega.Core.Models;
using Vega.Core.models;
using Vega.Extensions;

namespace vega.Persistence
{
  public class VehicleRepository : IVehicleRepository
  {

    private readonly VegaDbContext context;

    public VehicleRepository(VegaDbContext context)
    {
        this.context = context;
    }

    public async Task<Vehicle> GetVehicle(int id, bool includeRelated = true) //to don't load a complete object, that will make more havy our application
    {
        if (!includeRelated)
          return await context.Vehicles.FindAsync(id);

        return await context.Vehicles
          .Include(v => v.Features)
            .ThenInclude(vf => vf.Feature)
          .Include(v => v.Model)
            .ThenInclude(m => m.Make)
          .SingleOrDefaultAsync(v => v.Id == id);
     }

    public void Add(Vehicle vehicle) 
    {
      context.Vehicles.Add(vehicle);
    }

    public void Remove(Vehicle vehicle)
    {
      context.Remove(vehicle);
    }

    public async Task<QueryResult<Vehicle>> GetVehicles(VehicleQuery queryObj)
    {
        var result = new QueryResult<Vehicle>();
        
        var query = context.Vehicles
                .Include(v => v.Model) //this bring the models
                  .ThenInclude(m => m.Make) //and this bring the complex objetct that is inside the model, in this case the makes
                .Include(v => v.Features)
                  .ThenInclude(vf => vf.Feature)
                .AsQueryable();

        if (queryObj.MakeId.HasValue)
        {
           query = query.Where(v => v.Model.MakeId == queryObj.MakeId.Value);
        }
        //Como q ele sabe q o meu v he o meu  context.vehicles?

        if (queryObj.ModelId.HasValue)
        {
           query = query.Where(v => v.ModelId == queryObj.ModelId);
        }

        //Sorting
        var columnsMap = new Dictionary<string, Expression<Func<Vehicle, object>>>()
        {
            ["make"] = v => v.Model.Make.Name,
            ["model"] = v => v.Model.Name,
            ["contactName"] = v => v.ContactName,
        };

        query = query.ApplyOrdering(queryObj, columnsMap);

        //Paging
        result.TotalItems = await query.CountAsync(); //contar quantos elementos temos

        query = query.ApplyPaging(queryObj);

        result.Items= await query.ToListAsync();

        return result;
    }
  }
}