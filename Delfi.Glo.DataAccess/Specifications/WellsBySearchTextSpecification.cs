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
                    || (a.CommStatus ?? "").ToLower().Contains(search)
                    || (a.WellStatus ?? "").ToLower().Contains(search)
                    || (a.ControllerStatus ?? "").ToLower().Contains(search)
                    || (a.CommStatus ?? "").ToLower().Contains(search)
                    || ((a.SPM ?? new WellChartDetails()).ToString() ?? "").Contains(search) || ((a.PumpFillage ?? new WellChartDetails()).ToString() ?? "").Contains(search)
                    || ((a.InferredProduction ?? new WellChartDetails()).ToString() ?? "").Contains(search) || a.NoOfAlerts.ToString().Contains(search)
                    || ((a.EffectiveRunTime ?? new WellChartDetails()).ToString() ?? "").Contains(search) || ((a.CyclesToday ?? new WellChartDetails()).ToString() ?? "").Contains(search)
                    || ((a.StructuralLoad ?? new WellChartDetails()).ToString() ?? "").Contains(search) || ((a.GearboxLoad ?? new WellChartDetails()).ToString() ?? "").Contains(search)
                    || ((a.MinMaxLoad ?? new WellChartDetailsMinMax()).ToString() ?? "").Contains(search) || ((a.RodStress ?? new WellChartDetails()).ToString() ?? "").Contains(search)
                    || (a.DateAndTime ?? "").Contains(search);
        }


    }
}
