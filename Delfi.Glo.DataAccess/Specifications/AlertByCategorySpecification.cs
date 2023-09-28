using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Delfi.Glo.Common.Services;
using Delfi.Glo.Entities.Dto;

namespace Delfi.Glo.DataAccess.Specifications
{
    public class AlertByCategorySpecification : Specification<AlertsDto>
    {
        public readonly AlertListFilterDto _alertListFilter;


        public AlertByCategorySpecification(AlertListFilterDto alertListFilter)
        {
            this._alertListFilter = alertListFilter;
        }
        public override Expression<Func<AlertsDto, bool>> ToExpression()
        {
            return a => _alertListFilter.Category != null && _alertListFilter.Category.Any(b => b == a.Category);
        }
    }
    
}
