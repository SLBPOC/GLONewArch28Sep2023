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
    public sealed class EventbySearchTextSpecification: Specification<EventsDto>
    {
        public readonly string _search;
        public EventbySearchTextSpecification(string searchText)
        {
            this._search = searchText;
        }
        public override Expression<Func<EventsDto, bool>> ToExpression()
        {
            return a => (a.WellName ?? "").ToLower().Contains(_search) || (a.EventType ?? "").ToLower().Contains(_search)
                                                               || (a.CreationDateTime.ToString() ?? "").ToLower().Contains(_search)
                                                               || (a.EventDescription ?? "").ToLower().Contains(_search)
                                                               || (a.UpdatedBy ?? "").ToLower().Contains(_search);
        }
    }
}
