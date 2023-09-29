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
    public sealed class WellsModeOfOperationSpecifincation: Specification<WellDto>
    {
        private readonly string status;
        public WellsModeOfOperationSpecifincation(string status)
        {
            this.status = status;
        }

        public override Expression<Func<WellDto, bool>> ToExpression()
        {
            return a => (a.ApprovalStatus ?? "").Contains(status);
        }
    }
    }
    

