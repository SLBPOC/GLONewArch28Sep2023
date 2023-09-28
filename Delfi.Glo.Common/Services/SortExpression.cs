//disabling warning for now will fix when actual api will be consumed.
#pragma warning disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delfi.Glo.Common.Services
{
    public class SortExpression
    {
        public string Dir { get; set; }
        public string Field { get; set; }

    }
}   
#pragma warning restore