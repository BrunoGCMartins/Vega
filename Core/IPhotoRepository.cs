using System.Collections.Generic;
using System.Threading.Tasks;
using Vega.Core.models;

namespace vega.Core
{
    public interface IPhotoRepository
    {
         Task<IEnumerable<Photo>> GetPhotos(int vehicleId);
    }
}