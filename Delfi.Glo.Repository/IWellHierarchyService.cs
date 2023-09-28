using Delfi.Glo.Entities.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delfi.Glo.Repository;

public interface IWellHierarchyService
{
    Task<IEnumerable<Node>> GetWellHierarchy();

    Task<WellHierarchResponse> GetWellsWithHierarchy();

    Task<WellHierarchResponse> SearchInWellHierarchy(WellHierarchyRequest request);

}