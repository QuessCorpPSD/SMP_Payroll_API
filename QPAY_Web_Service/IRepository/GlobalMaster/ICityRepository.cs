using QPay.UI.Models.GlobalMaster;
using QPay.UI.Models.Invoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.GlobalMaster
{
    public interface ICityRepository
    {
        Task<List<City>> GetAllCity(string? cityName, int? stateId, int? cityId, string schema);
        Task<string> Create(string xml, string mode, int createdBy, string schema);

        Task<List<Circle>> GetAllCircle(int stateId, string schema);
    }
}
