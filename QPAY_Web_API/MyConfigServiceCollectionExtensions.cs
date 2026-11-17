using Microsoft.Extensions.DependencyInjection;
using QPay.API.Extensions;
using QPay.API.Models;
using QPay.BAL.IRepository;
using QPay.BAL.IRepository.Admin;
using QPay.BAL.IRepository.AMSCommon;
using QPay.BAL.IRepository.BankInvoice;
using QPay.BAL.IRepository.BankNonInvoice;
using QPay.BAL.IRepository.Billing;
using QPay.BAL.IRepository.Common;
using QPay.BAL.IRepository.Customer;
using QPay.BAL.IRepository.GlobalMaster;
using QPay.BAL.IRepository.InputAggregator;
using QPay.BAL.IRepository.Invoice;
using QPay.BAL.IRepository.IR;
using QPay.BAL.IRepository.Master;
using QPay.BAL.IRepository.PayrollInput;
using QPay.BAL.IRepository.Process;
using QPay.BAL.IRepository.PurchaseOrder;
using QPay.BAL.IRepository.Reports;
using QPay.BAL.IRepository.SalaryAdvance;
using QPay.BAL.IRepository.SalaryReleaseInvoice;
using QPay.BAL.IRepository.Tools;
using QPay.BAL.Repository;
using QPay.BAL.Repository.Admin;
using QPay.BAL.Repository.AMSCommon;
using QPay.BAL.Repository.BankInvoice;
using QPay.BAL.Repository.BankNonInvoice;
using QPay.BAL.Repository.Billing;
using QPay.BAL.Repository.Common;
using QPay.BAL.Repository.Customer;
using QPay.BAL.Repository.GlobalMaster;
using QPay.BAL.Repository.InputAggregator;
using QPay.BAL.Repository.Invoice;
using QPay.BAL.Repository.IR;
using QPay.BAL.Repository.Master;
using QPay.BAL.Repository.Process;
using QPay.BAL.Repository.Reports;
using QPay.BAL.Repository.SalaryAdvance;
using QPay.BAL.Repository.SalaryReleaseInvoice;
using QPay.BAL.Repository.Tools;
using QPay.DAL.Repository.PayrollInput;
using QPay.IRepository.iRepository.Common;
using QPay.IRepository.Repository.Common;
using QPay.IRepository.Repository.Master;
using QPay.IRepository.Repository.PayrollInput;
using QPay.IRepository.Repository.PurchaseOrder;
using QPay.UI.Master;
using Qzone.IRepository.SplitCulture;

namespace QPay.API
{
    public static class MyConfigServiceCollectionExtensions
    {
        public static IServiceCollection AddConfig(this IServiceCollection services, IConfiguration config)
        {
            return services;
        }
    }
    public static class Bootstrapper
    {
        public static void AddServices(this IServiceCollection services)
        {

            services.AddHttpClient();
            services.AddSingleton<IJwtTokenService, JwtTokenService>();
            services.AddSingleton<IEmailService, EmailService>();

            #region Dependencies  PSD DI
            services.AddScoped<ILoginRepository, LoginRepository>();
            services.AddTransient<IPayRegisterRepository, PayRegisterRepository>();
            services.AddSingleton<IAssignmentRepository, AssignmentRepository>();
            services.AddSingleton<IDashboardRepository, DashboardRepository>();
            services.AddSingleton<ICheckInCheckOutRepository, CheckInCheckOutRepository>();
            services.AddSingleton<IAdminDashboardRepository, AdminDashboardRepository>();
            services.AddSingleton<IFinancialYearRepository, FinancialYearRepository>();
            services.AddSingleton<IBillableDaysRepository, BillableDaysRepository>();
            services.AddSingleton<IInvoiceInitiationRepository, InvoiceInitiationRepository>();
            #endregion

            #region Dependencies   SOP DI
            services.AddSingleton<IQARepository, QARepository>();
            #endregion

            // Added BY Masi
            #region Dependencies Common Master DI (Depdency Injection) 
            services.AddScoped<IAccesstypeRepository, AccesstypeRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ICommonRepository, CommonRepository>();
            services.AddScoped<IProcessCategoryRepository, ProcessCategoryRepository>();

            #endregion

            #region Dependencies Common Transaction DI (Depdency Injection) 

            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IDesignationRepository, DesignationRepository>();
            services.AddScoped<IClientAddressRespository, ClientAddressRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IServiceChargeRepository, ServiceChargeRepository>();
            services.AddScoped<ICostCenterMappingRepository, CostCenterMappingRepository>();
            services.AddScoped<ICompanyPaycodeMappingRepository, CompanyPaycodeMappingRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            services.AddScoped<ICPFRepository, CPFRepository>();
            services.AddScoped<ISDLRepository, SDLRepository>();
            services.AddScoped<IPaycodeRepository, PaycodeRepository>();
            services.AddScoped<ISHGRepository, SHGRepository>();

            services.AddScoped<IGSTInvoiceRepository, GSTInvoiceRepository>();
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            services.AddScoped<IInvoiceCultureRepository, InvoiceCultureRepository>();
            services.AddScoped<IInvoiceInitiationRepository, InvoiceInitiationRepository>();

            services.AddScoped<IHolidayMasterRepository, HolidayMasterRepository>();
            services.AddScoped<IInvoiceRuleRepository, InvoiceRuleRepository>();
            services.AddScoped<ILeaveMasterRepository, LeaveMasterRepository>();
            services.AddScoped<ILeaveRuleRepository, LeaveRuleRepository>();
            services.AddScoped<ILeaveOpeningBalanceRepository, LeaveOpeningBalanceRepository>();
            services.AddScoped<IVendorEmployeeRepository, VendorEmployeeRepository>();

            services.AddScoped<IOnboardingRepository, OnboardingRepository>();
            services.AddScoped<ITimesheetRepository, TimesheetRepository>();
            services.AddScoped<IActivationLwdRepository, ActivationLwdRepository>();
            services.AddScoped<IIncrementRepository, IncrementRepository>();

            services.AddScoped<IAllowReprocessRepository, AllowReprocessRepository>();
            services.AddScoped<IArrearAttendanceProcessRepository, ArrearAttendanceProcessRepository>();
            services.AddScoped<IAttendanceBatchIdUpdateRepository, AttendanceBatchIdUpdateRepository>();
            services.AddScoped<IAttendanceProcessRepository, AttendanceProcessRepository>();
            services.AddScoped<IFNFRevokeRepository, FNFRevokeRepository>();
            services.AddScoped<IITAdjustmentRepository, ITAdjustmentRepository>();
            services.AddScoped<ILockPayperiodRepository, LockPayperiodRepository>();
            services.AddScoped<ILOPAdjustmentProcessRepository, LOPAdjustmentProcessRepository>();
            services.AddScoped<IOnetimeReplacementRepository, OnetimeReplacementRepository>();
            services.AddScoped<IOtherIncomeRepository, OtherIncomeRepository>();
            services.AddScoped<IPayProcessRepository, PayProcessRepository>();
            services.AddScoped<IPayRegisterUploadRepository, PayRegisterUploadRepository>();
            services.AddScoped<IPayTransactionRepository, PayTransactionRepository>();
            services.AddScoped<IReimbursementCalendarRepository, ReimbursementCalendarRepository>();

            services.AddScoped<IEmployeePOApproveRepository, EmployeePOApproveRepository>();
            services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();


            services.AddScoped<IBillingReportRepository, BillingReportRepository>();
            services.AddScoped<IInvoiceLeaveBalanceReportRepository, InvoiceLeaveBalanceReportRepository>();
            services.AddScoped<IInvoiceSummaryRepository, InvoiceSummaryRepository>();
            services.AddScoped<ILeaveBalanceReportRepository, LeaveBalanceReportRepository>();
            services.AddScoped<INetpaySummaryRepository, NetpaySummaryRepository>();
            services.AddScoped<IOtherIncomeEntitywiseRepository, OtherIncomeEntitywiseRepository>();
            services.AddScoped<IOtherIncomeProcessEmployeeRepository, OtherIncomeProcessEmployeeRepository>();
            services.AddScoped<IOtherIncomeReportRepository, OtherIncomeReportRepository>();
            services.AddScoped<IPayregisterEntitywiseRepository, PayregisterEntitywiseRepository>();
            services.AddScoped<IPayregisterUnprocessedRepository, PayregisterUnprocessedRepository>();
            services.AddScoped<IPayslipReportRepository, PayslipReportRepository>();
            services.AddScoped<IPoReportRepository, PoReportRepository>();
            services.AddScoped<IProcessEmployeeRepository, ProcessEmployeeRepository>();
            services.AddScoped<ITimesheetSummaryReportRepository, TimesheetSummaryReportRepository>();


            services.AddScoped<IRequestRepository, RequestRepository>();

            services.AddScoped<IApproveRepository, ApproveRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();


            services.AddScoped<ISalaryRequestInvoiceRepository, SalaryRequestInvoiceRepository>();

            services.AddScoped<IDynamicUploadRepository, DynamicUploadRepository>();
            services.AddScoped<IGstRepository, GstRepository>();
            services.AddScoped<IProvisionalInvoiceRepository, ProvisionalInvoiceRepository>();
            services.AddScoped<IPOInvoiceInitiateRepository, POInvoiceInitiateRepository>();

            services.AddScoped<IBillingPayFrequencyRepository, BillingPayFrequencyRepository>();

            services.AddScoped<IPayFrequencyRepository, PayFrequencyRepository>();

            services.AddScoped<IEntityRepository, EntityRepository>();
            services.AddScoped<IInvoiceLegalEntityRepository, InvoiceLegalEntityRepository>();
            services.AddScoped<ICorporateBankRepository, CorporateBankRepository>();

            services.AddScoped<IITCalenderRepository, ITCalenderRepository>();

            services.AddScoped<IBankRepository, BankRepository>();
            services.AddScoped<IFormulaRepository, FormulaRepository>();
            services.AddScoped<ISiteMasterRepository, SiteMasterRepository>();
            services.AddScoped<IVendorMasterRepository, VendorMasterRepository>();
            services.AddScoped<IBandRepository, BandRepository>();
            services.AddScoped<ICreditNoteRepository, CreditNoteRepository>();
            services.AddScoped<ICreditNoteApproveRepository, CreditNoteApproveRepository>();
            services.AddScoped<ICreditNoteUpdateRepository, CreditNoteUpdateRepository>();


            services.AddScoped<IClientBillableReportRepository, ClientBillableReportRepository>();
            services.AddScoped<IGenericUploadRepository, GenericUploadRepository>();
            services.AddScoped<IBillingUBRRepository, BillingUBRRepository>();
            services.AddScoped<ICompanyPermissionRepository, CompanyPermissionRepository>();
            services.AddScoped<ICompanyInvoiceFormatRepository, CompanyInvoiceFormatRepository>();


            services.AddScoped<IPOBalanceReportRepository, POBalanceReportRepository>();
            services.AddScoped<ICreditNoteBalanceReportRepository, CreditNoteBalanceReportRepository>();
            services.AddScoped<IIncrementReportRepository, IncrementReportRepository>();
            services.AddScoped<ISapBookClosureRepository, SapBookClosureRepository>();
            services.AddScoped<IMaterialCodeRepository, MaterialCodeRepository>();
            services.AddScoped<ICancelDocumentRepository, CancelDocumentRepository>();

            services.AddScoped<IFormCreationRepository, FormCreationRepository>();

            #endregion

            services.AddScoped<IESIRepository, ESIRepository>();
            services.AddScoped<ILWFRepository, LWFRepository>();
            services.AddScoped<IPTRepository, PTRepository>();
            services.AddScoped<IPFRepository, PFRepository>();
            services.AddScoped<IAMSCommonRepository, AMSCommonRepository>();
            services.AddScoped<ITDSSlabMasterRepository, TDSSlabMasterRepository>();
            services.AddScoped<IBranchMasterRepository, BranchMasterRepository>();
            services.AddScoped<IComputationRuleRepository, ComputationRuleRepository>();
            services.AddScoped<IPayPeriodUnlockRepository, PayPeriodUnlockRepository>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<IStateRepository, StateRepository>();

            services.AddScoped<ICompanyProvidedBenefitsRepository, CompanyProvidedBenefitsRepository>();
            services.AddScoped<IChildrenEducationAllowanceRepository, ChildrenEducationAllowanceRepository>();
            services.AddScoped<IGratuityRepository, GratuityRepository>();
            services.AddScoped<IHRARepository, HRARepository>();
            services.AddScoped<IIncomeLossHousingPropertyRepository, IncomeLossHousingPropertyRepository>();
            services.AddScoped<ILTACalculationRepository, LTACalculationRepository>();
            services.AddScoped<IPreviousEmploymentRepository, PreviousEmploymentRepository>();
            services.AddScoped<ITaxDeclarationAndActualRepository, TaxDeclarationAndActualRepository>();

            services.AddScoped<IPromotionIncrementRepository, PromotionIncrementRepository>();
            services.AddScoped<ISalaryReleasePendingApprovalRepository, SalaryReleasePendingApprovalRepository>();
            services.AddScoped<IBatchGenerationRepository, BatchGenerationRepository>();
            services.AddScoped<IBankNeftCultureInvoiceRepository, BankNeftCultureInvoiceRepository>();
            services.AddScoped<ISplitCultureRepository, SplitCultureRepository>();
            services.AddScoped<IIRRepository, IRRepository>();
            services.AddSingleton<IGenericUpload, GenericUpload>();
            services.AddSingleton<ICompanyPermissionRepository, CompanyPermissionRepository>();
            services.AddSingleton<IUserManagement, UserManagementRepository>();
            services.AddSingleton<ISGPayRegisterRepository, SGPayRegisterRepository>();
            services.AddScoped<IInputAggregatorAttendanceRepository, InputAggregatorAttendanceRepository>();
            services.AddSingleton<IInputAggregatorRepository, InputAggregatorRepository>();
            services.AddSingleton<ILeaveCategoryRepository, LeaveCategoryRepository>();
            services.AddScoped<INIBatchGenerationRepository, NIBatchGenerationRepository>();


            services.AddSingleton<IEmployeeSalaryReleaseRepository, EmployeeSalaryReleaseRepository>();
            services.AddSingleton<IHoldEmpSalaryRepository, HoldEmpSalaryRepository>();
            services.AddSingleton<IReleaseholdemployeesalaryRepository, ReleaseholdemployeesalaryRepository>();
            services.AddSingleton<IUtrDetailsRepository, UtrDetailsRepository>();
            services.AddSingleton<IBankNeftCultureInvoiceRepository, BankNeftCultureInvoiceRepository>();
            services.AddSingleton<Ibankadvisesplitculturerepository, Bankadvisesplitculturerepository>();
            services.AddSingleton<IPartialHoldReleaseRepository, PartialHoldReleaseRepository>();
            services.AddSingleton<IInvoiceInitiationAgainstProfomaRepository, InvoiceInitiationAgainstProfomaRepository>();
            services.AddSingleton<IPermHireRepository, PermHireRepository>();
        }
    }

}
