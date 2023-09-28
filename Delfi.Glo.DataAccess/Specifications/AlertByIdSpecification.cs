using Delfi.Glo.Common.Services;
using Delfi.Glo.Entities.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Delfi.Glo.DataAccess.Specifications
{
    public sealed class AlertByIdSpecification : Specification<AlertsDto>
    {
        public readonly int? _alertId;
        public AlertByIdSpecification(int? AlertId)
        {
            this._alertId = AlertId;
        }
        public override Expression<Func<AlertsDto, bool>> ToExpression()
        {
            return a => a.AlertId == _alertId;
        }
    }
}
