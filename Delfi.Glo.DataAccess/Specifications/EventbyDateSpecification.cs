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
    public sealed class EventbyDateSpecification: Specification<EventsDto>
    {
        public readonly string _fromDate;
        public readonly string _toDate;
        public EventbyDateSpecification(string FromDate, string ToDate)
        {
            _fromDate = FromDate;
            _toDate = ToDate;
        }
        public override Expression<Func<EventsDto, bool>> ToExpression()
        {
            return a => a.CreationDateTime.HasValue &&
                      DateTime.Compare(a.CreationDateTime.Value, Convert.ToDateTime(_fromDate)) >= 0 &&
                      DateTime.Compare(a.CreationDateTime.Value, Convert.ToDateTime(_toDate).AddDays(1)) < 0;
        }
    }
}
