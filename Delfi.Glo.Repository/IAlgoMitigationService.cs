using Delfi.Glo.Entities.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delfi.Glo.Repository
{
    public interface IAlgoMitigationService<T> where T : class
    {
        Task<IEnumerable<T>> GetChartData();
    }
}
