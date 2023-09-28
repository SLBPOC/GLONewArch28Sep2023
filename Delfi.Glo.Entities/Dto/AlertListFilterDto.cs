using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delfi.Glo.Entities.Dto
{
    public class AlertListFilterDto
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string? SearchText { get; set; }
        public string? SortColumn { get; set; }
        public string? SortDirection { get; set; }
        public string? SearchStatus { get; set; }
        public DateRange? DateRange { get; set; }
        public string[]? WellNames { get; set; }
        public string[]? Category { get; set; }

        public int[]? Ids { get; set; }
    }
    public class DateRange
    {
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
    }

    public class DefaultAlertFilterValues
    {
        public List<WellIdName>? WellNames { get; set; }
        public List<Category>? Category { get; set; }
     
    }

    public class Category
    {
        public string? Value { get; set; }
    }
}
