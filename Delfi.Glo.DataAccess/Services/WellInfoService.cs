using Delfi.Glo.Common.Constants;
using Delfi.Glo.Common.Services;
using Delfi.Glo.Entities.Dto;
using Delfi.Glo.Repository;


namespace Delfi.Glo.DataAccess.Services
{
    public class WellInfoService : IWellInfoService<WellInfoDto>
    {
        #region Public method
        /// <summary>
        /// GetWellInfoFromJsonFile get well info from json by well id
        /// </summary>
        /// <param name="WellId">by well id</param>
        /// <returns>WellInfoDto object</returns>
        public async Task<WellInfoDto?> GetWellInfoFromJsonFile(string WellId)
        {
            var wellsInfoInJson = (await UtilityService.ReadAsync<List<WellInfoDto>>(JsonFiles.WELLSINFO))?.AsQueryable();
            if (wellsInfoInJson != null)
            {
                var well = wellsInfoInJson.Where(x => x.WellId == WellId).FirstOrDefault();
                return well;
            }
            else
                return null;
        }

        #endregion
    }
}
