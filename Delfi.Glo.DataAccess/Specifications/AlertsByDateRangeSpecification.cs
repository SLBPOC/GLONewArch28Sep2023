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
    public sealed class AlertsByDateRangeSpecification : Specification<AlertsDto>
    {
        public readonly string _fromDate;
        public readonly string _toDate;
        public readonly bool result = true;
        public AlertsByDateRangeSpecification(string FromDate, string ToDate)
        {
            _fromDate = FromDate;
            _toDate = ToDate;
        }
        public override Expression<Func<AlertsDto, bool>> ToExpression()
        {
            return a => (_fromDate != null && _toDate != null && _fromDate != "" && _toDate != "") ? (Convert.ToDateTime(a.Date) >= Convert.ToDateTime(_fromDate)
                                               && Convert.ToDateTime(a.Date) <= Convert.ToDateTime(_toDate)) : result;
        }
    }
}
