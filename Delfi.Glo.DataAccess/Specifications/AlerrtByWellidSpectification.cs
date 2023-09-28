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
    public class AlerrtByWellidSpectification:Specification<AlertsDto>
    {
        public readonly AlertListFilterDto _alertListFilter;
        private readonly bool result = false;

        public AlerrtByWellidSpectification(AlertListFilterDto alertListFilter)
        {
            this._alertListFilter = alertListFilter;
        }
        public override Expression<Func<AlertsDto, bool>> ToExpression()
        {
            return a => _alertListFilter.Ids != null ? _alertListFilter.Ids.Contains( Convert.ToInt32(a.WellId)) : result;
        }
    }
}
