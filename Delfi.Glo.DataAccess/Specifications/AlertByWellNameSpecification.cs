using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Delfi.Glo.Common.Constants;
using Delfi.Glo.Common.Services;
using Delfi.Glo.Entities.Dto;
namespace Delfi.Glo.DataAccess.Specifications
{
    public class AlertByWellNameSpecification : Specification<AlertsDto>
    {
        public readonly AlertListFilterDto _alertListFilter;
        private readonly bool result=false;

        public AlertByWellNameSpecification(AlertListFilterDto alertListFilter)
        {
            this._alertListFilter = alertListFilter;
        }
        public override Expression<Func<AlertsDto, bool>> ToExpression()
        {
            return a => _alertListFilter.WellNames != null ? _alertListFilter.WellNames.Any(b => b == a.WellName) : result;
        }
    }
    

}
