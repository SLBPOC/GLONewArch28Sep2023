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
    public sealed class AlertsByLevelSpecification : Specification<AlertsDto>
    {
        public readonly string _alertLevel;
        public AlertsByLevelSpecification(string alertLevel)
        {

            this._alertLevel = alertLevel;

        }
        public override Expression<Func<AlertsDto, bool>> ToExpression()
        {
            return a => (a.AlertLevel ?? "").Contains(_alertLevel);
        }
    }
}
