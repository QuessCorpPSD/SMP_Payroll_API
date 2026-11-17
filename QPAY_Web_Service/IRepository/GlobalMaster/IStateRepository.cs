using QPay.UI.Models.GlobalMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.GlobalMaster
{
    public interface IStateRepository
    {
            Task<List<State>> GetAllState(string? stateName, int? regionId, int? stateId, string schema);
            Task<string> Create(string xml, string mode, int createdBy, string schema);
            Task<List<Region>> GetAllRegion(string schema);      

    }
}
