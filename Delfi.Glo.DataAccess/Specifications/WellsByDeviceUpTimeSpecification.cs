using Delfi.Glo.Common.Services;
using Delfi.Glo.Entities.Dto;
using System.Linq.Expressions;

namespace Delfi.Glo.DataAccess.Specifications
{
    public class WellsByDeviceUpTimeSpecification : Specification<WellDto>
    {
        private readonly WellListFilterDto _wellListFilter;

        public WellsByDeviceUpTimeSpecification(WellListFilterDto wellListFilter)
        {
            this._wellListFilter = wellListFilter;
        }

        public override Expression<Func<WellDto, bool>> ToExpression()
        {
            return a => _wellListFilter.DeviceUpTime != null ? _wellListFilter.DeviceUpTime.Any(b => b == a.DeviceUpTime) : false;
        }
    }
}
