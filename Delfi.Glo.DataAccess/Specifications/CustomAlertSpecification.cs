using Delfi.Glo.Entities.Dto;
using Delfi.Glo.Common.Services;
using System.Linq.Expressions;

namespace Delfi.Glo.DataAccess.Specifications
{
    public sealed class CustomAlertSpecification : Specification<CustomAlertDto>
    {
        private readonly int? id;

        public CustomAlertSpecification(int? _id)
        {
            this.id = _id;
        }
        public override Expression<Func<CustomAlertDto, bool>> ToExpression()
        {
            return x => x.Id == id;
        }
    }
}