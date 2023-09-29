using Delfi.Glo.Common.Services;
using Delfi.Glo.Entities.Dto;
using System.Linq.Expressions;

namespace Delfi.Glo.DataAccess.Specifications
{

    public sealed class WellsBySearchTextSpecification : Specification<WellDto>
    {
        private readonly string search;

        public WellsBySearchTextSpecification(string searchText)
        {
            this.search = searchText;
        }

        public override Expression<Func<WellDto, bool>> ToExpression()
        {
            return a => (a.WellName ?? "").ToLower().Contains(search)
                    || (a.CurrentCycleStatus ?? "").ToLower().Contains(search)
                    || (a.WellStatus ?? "").ToLower().Contains(search)
                    || (a.LastCycleStatus ?? "").ToLower().Contains(search)
                    || (a.ApprovalMode ?? "").ToLower().Contains(search)
                     || (a.ApprovalStatus ?? "").ToLower().Contains(search);
        }


    }
}
