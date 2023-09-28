#pragma warning disable
using Delfi.Glo.Common.Services;
using Delfi.Glo.Entities.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Delfi.Glo.DataAccess.Specifications
{
    public sealed class UniversityByNameSpecification : Specification<UniversitiesDto>
    {
        private readonly string universityName;

        public UniversityByNameSpecification(string universityName)
        {
            this.universityName = universityName;
        }

        public override Expression<Func<UniversitiesDto, bool>> ToExpression()
        {
            return university => university.Name.Contains(universityName);
        }
    }
}
#pragma warning restore