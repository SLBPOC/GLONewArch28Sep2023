using Delfi.Glo.Common.Services;
using Delfi.Glo.Entities.Dto;
using System.Linq.Expressions;
namespace Delfi.Glo.DataAccess.Specifications
{
    public sealed class AlertsBySearchTextSpecification : Specification<AlertsDto>
    {
        public readonly string _search;
        public AlertsBySearchTextSpecification(string searchText)
        {
            this._search = searchText;
        }
        public override Expression<Func<AlertsDto, bool>> ToExpression()
        {
            return a =>(a.WellName ?? "").ToLower().Contains(_search) || (a.AlertLevel ?? "").ToLower().Contains(_search)
                                                               || (a.Date ?? "").ToLower().Contains(_search)
                                                               || (a.Desc ?? "").ToLower().Contains(_search)
                                                               || (a.Status ?? "").ToLower().Contains(_search)
                                                               ||(a.Category??"").ToLower().Contains(_search);
        }
    }
}
