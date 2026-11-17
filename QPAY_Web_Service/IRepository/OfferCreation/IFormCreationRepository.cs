using QPay.UI.Customer;
using QPay.UI.GlobalMaster;
using QPay.UI.Models;
using QPay.UI.Models.OfferCreation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.GlobalMaster
{
    public interface IFormCreationRepository
    {
        Task<FormCreation.FormResponse> Search(int? FormId, int? ClientId, int? EntityId, int? SiteId);
        // Task<DataSet> Search(int? FormId, int? ClientId, int? EntityId, int? SiteId);

    }
}
