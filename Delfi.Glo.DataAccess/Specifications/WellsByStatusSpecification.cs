using Delfi.Glo.Common.Services;
using Delfi.Glo.Entities.Dto;
using System.Linq.Expressions;

namespace Delfi.Glo.DataAccess.Specifications
{

    public sealed class WellsByStatusSpecification : Specification<WellDto>
    {
        private readonly string status;

        public WellsByStatusSpecification(string status)
        {
            this.status = status;
        }

        public override Expression<Func<WellDto, bool>> ToExpression()
        {
            return a => (a.WellStatus ?? "").Contains(status);
        }


    }
}
