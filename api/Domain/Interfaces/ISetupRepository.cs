using Domain.Entities.Project;
using Domain.Entities.Setup;
using Domain.ViewModel.GeneralSetup;
using Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ISetupRepository
    {


        // -----------------------------------------------------------
        // 🔹 GeneralSetup
        // -----------------------------------------------------------

        #region GeneralSetup



        #region FiscalYear
        Task<List<FiscalYear>> GetFiscalYear(int id = 0, string name = "", DateTime? currentDate = null);
        Task<FiscalYear> GetCurrentFiscalYear();
        Task<DbResponse> AddFiscalYear(FiscalYear model);
        Task<DbResponse> DeleteFiscalYear(int id);
        #endregion


        #region Office
        Task<List<OfficeInformation>> GetOfficeInfo();
        Task<DbResponse> SaveOfficeInfo(OfficeInformation model);

        #endregion


        #region Department
        Task<List<Department>> GetDepartment(int id = 0, string name = "", string alias = "", bool getUserDepartmentonly = false, int userId = 0);
        Task<Department> GetDepartmentById(int id);
        Task<DbResponse> AddDepartment(Department model);
        Task<DbResponse> DeleteDepartment(int id);

        #endregion


        #region Designation
        Task<DataSet> GetDesignation(int id = 0, string name = "", int type = 0);
        Task<Designation> GetDesignationById(int id);
        Task<DbResponse> AddDesignation(Designation model);
        Task<DbResponse> DeleteDesignation(int id);

        #endregion


        #region PublicRepresentative
        Task<List<PublicRepresentative>> GetPublicRepresentative(int id = 0, string name = "", int designationId = 0, int wardId = 0);
        Task<DbResponse> AddPublicRepresentative(PublicRepresentative model);
        Task<DbResponse> DeletePublicRepresentative(int id);

        #endregion


        #region Employee
        Task<Employee> GetEmployeeById(int id);
        Task<IList<Employee>> GetEmployee(int id = 0, string name = "", string mobileNo = "", int departmentId = 0, int designationId = 0, bool forIncentive = false);
        Task<List<Employee>> GetEmployeeWithUserName(int id = 0, string name = "", string mobileNo = "", int departmentId = 0, int designationId = 0, bool forIncentive = false);
        Task<DbResponse> AddEmployee(Employee model);
        Task<DbResponse> DeleteEmployee(int id);
        #endregion


        #region Ward
        Task<List<Ward>> GetWard(int id = 0, string name = "");
        Task<DbResponse> AddWard(Ward model);
        Task<DbResponse> DeleteWard(int id);

        #endregion


        #region IncentiveRate Setup
        Task<List<VisitIncentiveRate>> GetVisitIncentiveRate(int id = 0, int designationId = 0);
        Task<DbResponse> AddVisitIncentiveRate(VisitIncentiveRate model);
        Task<DbResponse> DeleteVisitIncentiveRate(int id);
        #endregion


        #region ProjectClosing Setup
        Task<List<ClosingProject>> GetProjectForClosing(int sourceFyId);
        Task<DbResponse> SaveProjectClosing(ProjectClosing model);
        #endregion


        #region DropDownModel
        Task<List<DropDownModel>> GetDropDown(string tableName, bool isSystemData = false, string orderBy = "Name", string sortBy = "Asc");
        #endregion


        #endregion




        // -----------------------------------------------------------
        // 🔹 Committee Setup
        // -----------------------------------------------------------

        #region Comittee Setup 



        #region CommitteeType

        Task<List<CommitteeType>> GetCommitteeType(int? id, string name = "");
        Task<DbResponse> SaveCommitteeType(CommitteeType model);
        Task<DbResponse> DeleteCommitteeType(int id);

        #endregion


        #region Committee

        Task<List<Committee>> GetCommittee(int id = 0, string name = "", string registrationNo = "", int fiscalYearId = 0, string userWardList = null);
        Task<Committee> GetCommitteeById(int id);
        Task<DbResponse> SaveCommittee(Committee model);
        Task<DbResponse> DeleteCommittee(int id);

        #endregion


        #region CommitteeLetter
        Task<List<CommitteeLetter>> GetCommitteeLetter(int id = 0, int committeeId = 0, int letterTypeId = 0);
        Task<DbResponse> SaveCommitteeLetter(CommitteeLetter model);
        Task<DbResponse> DeleteCommitteeLetter(int id);
        #endregion


        #region Letters

        #endregion


        #region Member
        Task<List<Member>> GetMember(int id = 0, int committeeId = 0, string name = "", int designationid = 0, string citizenshipNo = "", string mobileNo = "");
        Task<Member> GetMemberById(int id = 0, int committeeId = 0);
        Task<Member> GetMemberByCitizenshipNo(string citizenshipNo);
        Task<Member> CheckMemberByCitizenShipNumber(string citizenshipNo, int fyId, int committeeId);
        Task<Dictionary<string, decimal>> SaveMember(Member model);
        Task<DbResponse> DeleteAllMember(int id);
        Task<DbResponse> DeleteMember(int id);
        #endregion


        #region CommitteeMember
        Task<List<Member>> GetMemberByCommitteeId(int committeeId = 0, string name = "", string citizenshipNo = "", string mobileNo = "");
        #endregion


        #region Firm
        Task<List<Firm>> GetFirm(int id = 0, string name = "");
        Task<Firm> GetFirmById(int id = 0);
        Task<DbResponse> SaveFirm(Firm model);
        Task<DbResponse> DeleteFirm(int id);
        #endregion


        #region Individual
        Task<List<Individual>> GetIndividual(int id = 0, string name = "");
        Task<Individual> GetIndividualById(int id);
        Task<DbResponse> SaveIndividual(Individual model);
        Task<DbResponse> DeleteIndividual(int id);
        #endregion





        #endregion




        // -----------------------------------------------------------
        // 🔹 Master Setup
        // -----------------------------------------------------------

        #region Master Setup
        

        #region Activity
        Task<List<Activity>> GetActivity(int id = 0, int activityGroupId = 0, string name = "", string code = "", string referenceCode = "", int fiscalYearId = 0);
        Task<DbResponse> SaveActivity(Activity model);
        Task<DbResponse> DeleteActivity(int id);

        #endregion


        #region Activity Group
        Task<List<ActivityGroup>> GetActivityGroup(int id = 0, string name = "", string code = "");
        Task<List<ActivityGroup>> GetActivityGroupDownList();
        Task<DbResponse> SaveActivityGroup(ActivityGroup model);
        Task<DbResponse> DeleteActivityGroup(int id);
        List<string> GetActivityGroupDisplayName(IList<ActivityGroup> list, int id);

        #endregion

        #region  ActivityRate
        Task<ActivityRate> GetActivityRate(int activityId = 0, int fiscalYearId = 0);

        #endregion

                           
        #region Bank
        Task<List<Bank>> GetBank(int id = 0, string name = "");
        Task<DbResponse> SaveBank(Bank model);      
        Task<DbResponse> DeleteBank(int id);

        #endregion


        #region Beneficiary
        Task<List<Beneficiary>> GetBeneficiary(int id = 0, string name = "");
        Task<Beneficiary> GetBeneficiaryById(int id);
        Task<DbResponse> SaveBeneficiary(Beneficiary model);
        Task<DbResponse> DeleteBeneficiary(int id);

        #endregion



        #region Budget


        #region BudgetAllocation

        Task<List<BudgetAllocation>> GetBudgetAllocation(int id = 0, int fiscalYearId = 0, int currentUserId = 0, string name = "", int wardId = 0, bool parentRelation = false);
        Task<BudgetAllocation> GetBudgetAllocationById(int id, int currentUserId);
        Task<List<BudgetAllocation>> GetBudgetAllocationDownList(int? id);
        Task<DbResponse> SaveBudgetAllocation(BudgetAllocation model);
        Task<DbResponse> DeleteBudgetAllocation(int id);
        List<string> GetDisplayName(IList<BudgetAllocation> list, int id);

        #endregion


        #region BudgetAllocationProject
        Task<List<BudgetAllocationProject>> GetBudgetAllocationProject(int allocationId);

        #endregion


        #region Budget Source

        Task<List<BudgetSource>> GetBudgetSource(int id = 0, string name = "", string codeno = "", int fiscalYearId = 0, bool withBudget = false);
        Task<BudgetSource> GetBudgetSourceById(int id = 0);
        Task<DbResponse> SaveBudgetSource(BudgetSource model);
        Task<DbResponse> DeleteBudgetSource(int id);

        #endregion


        #region BudgetSubTitle

        Task<List<BudgetSubTitle>> GetBudgetSubTitle(int id = 0, string name = "", string code = "", int expensesTypeId = 0);
        Task<DbResponse> SaveBudgetSubTitle(BudgetSubTitle model);
        Task<DbResponse> DeleteBudgetSubTitle(int id);

        #endregion


        #endregion



        #region Checklist
        Task<List<Checklist>> GetChecklist(int id = 0, string name = "");
        Task<Checklist> GetChecklistById(int id);
        Task<DbResponse> SaveChecklist(Checklist model);
        Task<DbResponse> DeleteChecklist(int id);
        Task<Checklist> GetChecklistName(int id);

        #endregion


        #region Configuration

        Task<List<Configuration>> GetConfiguration(int id = 0, string name = "");
        Task<DbResponse> SaveConfiguration(Configuration model);
        Task<DbResponse> DeleteConfiguration(int id);

        #endregion


        #region DonorType
        Task<List<DonorType>> GetDonorType(int id = 0, string name = "");
        Task<DonorType> GetDonorTypeById(int id);
        Task<DbResponse> SaveDonorType(DonorType model);
        Task<DbResponse> DeleteDonorType(int id);

        #endregion


        #region ExpensesHead
        Task<List<ExpensesHead>> GetExpensesHead(int id = 0, string name = "");
        Task<ExpensesHead> GetExpensesHeadById(int id);
        Task<DbResponse> SaveExpensesHead(ExpensesHead model);
        Task<DbResponse> DeleteExpensesHead(int id);
        #endregion


        #region ItemType
        Task<List<ItemType>> GetItemType(int id = 0, string name = "");
        Task<DbResponse> SaveItemType(ItemType model);
        Task<DbResponse> DeleteItemType(int id);

        #endregion


        #region Item
        Task<List<Item>> GetItem(int id = 0, int itemTypeId = 0, string name = "", int fiscalYearId = 0);
        Task<DbResponse> SaveItem(Item model);
        Task<DbResponse> DeleteItem(int id);

        #endregion


        #region ItemRate
        Task<List<ItemRate>> GetItemRate(int fiscalYearId = 0, int id = 0, string name = "");
        Task<DbResponse> SaveItemRate(List<ItemRate> list);
        Task<DbResponse> DeleteItemRate(int id);

        #endregion


        #region Implementation Model
        Task<List<ImplementationModel>> GetImplementationModel(int id = 0, int projectModelId = 0, string name = "");
        Task<DbResponse> SaveImplementationModel(ImplementationModel model);
        Task<DbResponse> DeleteImplementationModel(int id);

        #endregion


        #region Implementation Level
        Task<List<ImplementationLevel>> GetImplementationLevel(int id = 0, string name = "", string code = "", decimal threshold = 0);
        Task<DbResponse> SaveImplementationLevel(ImplementationLevel model);
        Task<DbResponse> DeleteImplementationLevel(int id);

        #endregion


        #region LetterFormat
        Task<List<LetterFormat>> GetLetterFormat(int id = 0, string name = "", int letterType = 0, int projectId = 0);
        Task<LetterFormat> GetLetterFormatById(int id);
        Task<DbResponse> SaveLetterFormat(LetterFormat model);
        Task<DbResponse> DeleteLetterFormat(int id);
        Task<DataTable> GetMetaDataValue(int projectId = 0, int committeeId = 0, int evaluationId = 0, int advanceId = 0, int timeExtendedId = 0, int currentUserId = 0);
        Task<LetterFormat> ProcessLetter(int projectId = 0, int letterType = 0, int committeeId = 0, int evaluationId = 0, int advanceId = 0, int timeExtendedId = 0, int currentUserId = 0);


        #endregion


        #region Norms
        Task<List<Norms>> GetNorms(int activityId, string name = "", int fiscalYearId = 0);
        Task<DbResponse> SaveNorms(List<Norms> normsList);
        Task<DbResponse> DeleteNorms(int id);

        #endregion


        #region NormsType
        Task<List<NormsType>> GetNormsType(int id = 0, string name = "");
        Task<DbResponse> SaveNormsType(NormsType model);
        Task<DbResponse> DeleteNormsType(int id);
            
        #endregion


        #region ProgressIndicator
        Task<List<ProgressIndicator>> GetProgressIndicator(int id = 0, string name = "", string code = "");
        Task<List<ProgressIndicator>> GetRemAccToProject(int projectCompletedGoalId, int projectId);
        Task<DbResponse> SaveProgressIndicator(ProgressIndicator model);
        Task<DbResponse> DeleteProgressIndicator(int id);

        #endregion


        #region Project Group
        Task<List<ProjectGroup>> GetProjectGroup(int id = 0, string name = "", int sectorId = 0);
        Task<ProjectGroup> GetProjectGroupById(int id);
        Task<List<ProjectGroup>> GetProjectGroupDownList();
        Task<DbResponse> SaveProjectGroup(ProjectGroup model);
        Task<DbResponse> DeleteProjectGroup(int id);
        Task<List<ProjectGroup>> GetProjectGroupGetParentName(int Id, int ParentId);
        List<string> GetProjectGroupDisplayName(IList<ProjectGroup> list, int id);

        #endregion


        #region Sector
        Task<List<Sector>> GetSector(int id = 0, string name = "", string code = "");
        Task<DbResponse> SaveSector(Sector model);
        Task<DbResponse> DeleteSector(int id);

        #endregion


        #region SubSector
        Task<List<SubSector>> GetSubSector(int id = 0, int sectorId = 0, string name = "", string code = "");
        Task<DbResponse> SaveSubSector(SubSector model);
        Task<DbResponse> DeleteSubSector(int id);

        #endregion


        #region Target
        Task<List<Target>> GetTarget(int id = 0, string name = "", string code = "");
        Task<DbResponse> SaveTarget(Target model);
        Task<DbResponse> DeleteTarget(int id);

        #endregion


        #region Unit
        Task<List<Unit>> GetUnit(int id = 0, string name = "", string description = "");
        Task<DbResponse> SaveUnit(Unit model);
        Task<DbResponse> DeleteUnit(int id);

        #endregion


        #region UnitType
        Task<List<UnitType>> GetUnitType(int id = 0, string name = "", string name_np = "");
        Task<DbResponse> SaveUnitType(UnitType model);
        Task<DbResponse> DeleteUnitType(int id);

        #endregion



        #endregion
    }
}
