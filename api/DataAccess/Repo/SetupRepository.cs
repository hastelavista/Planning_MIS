using Domain.Entities.Project;
using Domain.Entities.Setup;
using Domain.Interfaces;
using Domain.Utilities;
using Domain.ViewModel.GeneralSetup;
using Helper;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repo
{
    public class SetupRepository: ISetupRepository
    {
        private readonly DatabaseContext _db;
        private readonly ICurrentUserService _currentUser;


        public SetupRepository(DatabaseContext db, ICurrentUserService currentUser)
        {
            _db = db;
            _currentUser = currentUser;
        }


        // -----------------------------------------------------------
        // 🔹 GeneralSetup
        // -----------------------------------------------------------


        #region General Setup

        #region Fiscal Year
        public async Task<List<FiscalYear>> GetFiscalYear(int id = 0, string name = "", DateTime? currentDate = null)
        {

            var p = _db.SqlParameters.AddMore("@Id", id)
                                 .AddMore("@Name", string.IsNullOrEmpty(name) ? "" : (object)name)
                                 .AddMore("@CurrentDate", currentDate);
            var q = @"Select * from FiscalYear Where (@Id = 0 or Id = @Id) And (@Name = '' or Name = @Name) And 
                     (@CurrentDate is null or Convert(date, @CurrentDate) between DateFromEng and DateToEng) And
                      IsDeleted = 0";
            var data = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return data.TransformToList<FiscalYear>().ToList();
        }
        public async Task<FiscalYear> GetCurrentFiscalYear()
        {
            var databaseFY = await GetFiscalYear(currentDate: DateTime.Now);
            return databaseFY.FirstOrDefault();

            //if (databaseFY != null && databaseFY.Any())
            //    return databaseFY.First();

            //return FiscalYearHelper.GetCurrentFiscalYear();
        }

        public async Task<DbResponse> AddFiscalYear(FiscalYear model)
        {
            var p = model.PrepareSQLParameters();
            var q = QueryBuilder.GetCommandText["Setup.FiscalYear.Save"];
            var result = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return result;
        }
        public async Task<DbResponse> DeleteFiscalYear(int id)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                     .AddMore("@CurrentUser", _currentUser.Id);
            var q = @"Update FiscalYear Set IsDeleted = 1, ModifiedDate = GETDATE(), ModifiedBy = @CurrentUser Where Id = @Id";
            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }

        #endregion


        #region Office
        public async Task<List<OfficeInformation>> GetOfficeInfo()
        {
            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, "SELECT * FROM OfficeInformation");
            return dt.TransformToList<OfficeInformation>().ToList();
        }
        public async Task<DbResponse> SaveOfficeInfo(OfficeInformation model)
        {
            model.CurrentUser = _currentUser.Id;
            var p = model.PrepareSQLParameters();
            var q = QueryBuilder.GetCommandText["Setup.OfficeInformation.Save"];
            var result = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return result;

        }

        #endregion


        #region Department
        public async Task<List<Department>> GetDepartment(int id = 0, string name = "", string alias = "", bool getUserDepartmentonly = false, int userId = 0)
        {

            var p = _db.SqlParameters.AddMore("@Id", id)
                                    .AddMore("@Name", string.IsNullOrEmpty(name) ? "" : (object)name)
                                    .AddMore("@Alias", string.IsNullOrEmpty(alias) ? "" : (object)alias)
                                    .AddMore("@GetUserDeparmentOnly", getUserDepartmentonly)
                                    .AddMore("@UserId", userId);
            var q = QueryBuilder.GetCommandText["Setup.Department.Get"];
            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return dt.TransformToList<Department>().ToList();
        }

        public async Task<Department> GetDepartmentById(int id)
        {
            var dt = await GetDepartment(id);
            return dt.FirstOrDefault();
        }
        public async Task<DbResponse> AddDepartment(Department model)
        {
            var p = model.PrepareSQLParameters();
            var q = QueryBuilder.GetCommandText["Setup.Department.Save"];

            var dbResponse = await _db.ExecuteScalarAsync(CommandType.Text, q, p);
            
            if (dbResponse.HasError) 
                return dbResponse;

            return new DbResponse
            {
                Message = "Success",
                Response = dbResponse.Response
            };
        }

        public async Task<DbResponse> DeleteDepartment(int id)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                     .AddMore("@CurrentUser", _currentUser.Id);

            var q = @"Update Department Set IsDeleted = 1, ModifiedDate = GETDATE(), ModifiedBy = @CurrentUser Where Id = @Id";
            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }
        #endregion


        #region Designation
        public async Task<DataSet> GetDesignation(int id = 0, string name = "", int type = 0)
        {

            var p = _db.SqlParameters.AddMore("@Id", id)
                                    .AddMore("@Name", string.IsNullOrEmpty(name) ? "" : (object)name)
                                    .AddMore("@Type", type);

            var q = QueryBuilder.GetCommandText["Setup.Designation.Get"];
            var ds = await _db.ExecuteDataSetAsync(CommandType.Text, q, p);
            return ds;
        }

        public async Task<Designation> GetDesignationById(int id)
        {
            var list = await GetDesignation(id);
            var dt = list.Tables[1];
            return dt.TransformToList<Designation>().FirstOrDefault(d => d.Id == id);
        }

        public async Task<DbResponse> AddDesignation(Designation model)
        {
            model.CurrentUser = _currentUser.Id;

            var p = model.PrepareSQLParameters();
            var q = QueryBuilder.GetCommandText["Setup.Designation.Save"];
            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }

        public async Task<DbResponse> DeleteDesignation(int id)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                     .AddMore("@CurrentUser", _currentUser.Id);

            var q = @"Update Designation Set IsDeleted = 1, ModifiedDate = GETDATE(), ModifiedBy = @CurrentUser Where Id = @Id";
            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }
        #endregion


        #region PublicRepresentative

        public async Task<List<PublicRepresentative>> GetPublicRepresentative(int id = 0, string name = "", int designationId = 0, int wardId = 0)
        {

            var p = _db.SqlParameters.AddMore("@Id", id)
                                    .AddMore("@Name", string.IsNullOrEmpty(name) ? "" : (object)name)
                                    .AddMore("@DesignationId", designationId)
                                    .AddMore("@WardId", wardId);
            var q = QueryBuilder.GetCommandText["Setup.PublicRepresentative.Get"];
            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return dt.TransformToList<PublicRepresentative>().ToList();
        }

        public async Task<DbResponse> AddPublicRepresentative(PublicRepresentative model)
        {
            model.CurrentUser = _currentUser.Id;

            var p = model.PrepareSQLParameters();
            var q = QueryBuilder.GetCommandText["Setup.PublicRepresentative.Save"];
            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }

        public async Task<DbResponse> DeletePublicRepresentative(int id)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                     .AddMore("@CurrentUser", _currentUser.Id);

            var q = @"Update PublicRepresentative Set IsDeleted = 1, ModifiedDate = GETDATE(), ModifiedBy = @CurrentUser Where Id = @Id";
            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }

        #endregion


        #region Employee

        public async Task<IList<Employee>> GetEmployee(int id = 0, string name = "", string mobileNo = "", int departmentId = 0, int designationId = 0, bool forIncentive = false)
        {

            var p = _db.SqlParameters.AddMore("@Id", id)
                                    .AddMore("@Name", string.IsNullOrEmpty(name) ? "" : (object)name)
                                    .AddMore("@MobileNo", string.IsNullOrEmpty(mobileNo) ? "" : (object)mobileNo)
                                    .AddMore("@DepartmentId", departmentId)
                                    .AddMore("@DesignationId", designationId)
                                    .AddMore("@ForIncentive", forIncentive);

            var q = QueryBuilder.GetCommandText["Setup.Employee.Get"];
            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return dt.TransformToList<Employee>();
        }

        public async Task<List<Employee>> GetEmployeeWithUserName(int id = 0, string name = "", string mobileNo = "", int departmentId = 0, int designationId = 0, bool forIncentive = false)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                                .AddMore("@Name", string.IsNullOrEmpty(name) ? "" : (object)name)
                                                .AddMore("@MobileNo", string.IsNullOrEmpty(mobileNo) ? "" : (object)mobileNo)
                                                .AddMore("@DepartmentId", departmentId)
                                                .AddMore("@DesignationId", designationId)
                                                .AddMore("@ForIncentive", forIncentive);

            var q = QueryBuilder.GetCommandText["Setup.Employee.GetWithUserName"];
            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return dt.TransformToList<Employee>().ToList();
        }

        public async Task<Employee> GetEmployeeById(int id)
        {
            var dt = await GetEmployee(id);
            return dt.FirstOrDefault();
        }

        public async Task<DbResponse> AddEmployee(Employee model)
        {
            model.CurrentUser = _currentUser.Id;

            var p = model.PrepareSQLParameters();
            var q = QueryBuilder.GetCommandText["Setup.Employee.Save"];
            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }

        public async Task<DbResponse> DeleteEmployee(int id)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                     .AddMore("@CurrentUser", _currentUser.Id);

            var q = @"Update Employee Set IsDeleted = 1, ModifiedDate = GETDATE(), ModifiedBy = @CurrentUser Where Id = @Id";
            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }

        #endregion


        #region Ward Setup

        public async Task<List<Ward>> GetWard(int id = 0, string name = "")
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                    .AddMore("@Name", string.IsNullOrEmpty(name) ? "" : (object)name);
            var q = @"Select * from Ward Where(@Id = 0 or Id = @Id) and (@Name = 0 or Name = @Name) and IsDeleted = 0 Order BY displayOrder";
            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return dt.TransformToList<Ward>().ToList();
        }

        public async Task<DbResponse> AddWard(Ward model)
        {
            model.CurrentUser = _currentUser.Id;

            var p = model.PrepareSQLParameters();
            var q = QueryBuilder.GetCommandText["Setup.Ward.Save"];
            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }

        public async Task<DbResponse> DeleteWard(int id)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                     .AddMore("@CurrentUser", _currentUser.Id);

            var q = @"Update Ward Set IsDeleted = 1, ModifiedDate = GETDATE(), ModifiedBy = @CurrentUser Where Id = @Id";
            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }

        #endregion


        #region IncentiveRate Setup
        public async Task<List<VisitIncentiveRate>> GetVisitIncentiveRate(int id = 0, int designationId = 0)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                    .AddMore("@DesignationId", designationId);

            var q = QueryBuilder.GetCommandText["Setup.VisitIncentiveRate.Get"];
            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return dt.TransformToList<VisitIncentiveRate>().ToList();
        }

        public async Task<DbResponse> AddVisitIncentiveRate(VisitIncentiveRate model)
        {
            model.CurrentUser = _currentUser.Id;

            var p = model.PrepareSQLParameters();
            var q = QueryBuilder.GetCommandText["Setup.VisitIncentiveRate.Save"];
            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }

        public async Task<DbResponse> DeleteVisitIncentiveRate(int id)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                     .AddMore("@CurrentUser", _currentUser.Id);

            var q = @"Update dbo.VisitIncentiveRate Set IsDeleted = 1, ModifiedDate = GETDATE(), ModifiedBy = @CurrentUser Where Id = @Id";
            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }
        #endregion


        #region ProjectClosing Setup
        public async Task<List<ClosingProject>> GetProjectForClosing(int sourceFyId)
        {
            var p = _db.SqlParameters.AddMore("@SourceFyId", sourceFyId);

            var q = @"Select * from Project Where RunningFiscalYearId = @SourceFyId and IsCompleted = 0";
            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return dt.TransformToList<ClosingProject>().ToList();
        }


        public async Task<DbResponse> SaveProjectClosing(ProjectClosing model)
        {
            var p = model.PrepareSQLParameters();

            var q = QueryBuilder.GetCommandText["Setup.Closing.Save"];
            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }
        #endregion


        #region DropDownModel
        public async Task<List<DropDownModel>> GetDropDown(string tableName, bool isSystemData = false, string orderBy = "Name", string sortBy = "Asc")
        {
            var p = _db.SqlParameters.AddMore("@TableName", tableName)
                                    .AddMore("@IsSystemData", isSystemData)
                                    .AddMore("@OrderBy", orderBy)
                                    .AddMore("@SortBy", sortBy);
            var q = QueryBuilder.GetCommandText["Setup.DropdownModel.Get"];
            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return dt.TransformToList<DropDownModel>().ToList();

        }
        #endregion



        #endregion




        // -----------------------------------------------------------
        // 🔹 Comittee Setup
        // -----------------------------------------------------------


        #region Comittee Setup



        #region CommitteeType

        public async Task<List<CommitteeType>> GetCommitteeType(int? id, string name = "")
        {
            var p = _db.SqlParameters.AddMore("@Id", id ?? 0)
                                     .AddMore("@Name", string.IsNullOrEmpty(name) ? "" : (object)name);
            var q = @"Select * from CommitteeType 
                                    where (@Id = 0 or Id = @Id) And (@Name = '' or Name like '%' + @Name + '%') And IsDeleted = 0";

            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return dt.TransformToList<CommitteeType>().ToList();
        }

        public async Task<DbResponse> SaveCommitteeType(CommitteeType model)
        {
            var p = model.PrepareSQLParameters();
            var q = QueryBuilder.GetCommandText["Setup.CommitteeType.Save"];
            var response = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return response;
        }

        public async Task<DbResponse> DeleteCommitteeType(int id)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                     .AddMore("@CurrentUser", _currentUser.Id);
            var q = @"Update CommitteeType Set IsDeleted = 1, 
	                                       ModifiedDate = GETDATE(), ModifiedBy = @CurrentUser where Id = @Id";

            var result = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return result;
        }

        #endregion


        #region Committee

        public async Task<List<Committee>> GetCommittee(int id = 0, string name = "", string registrationNo = "", int fiscalYearId = 0, string userWardList = null)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                    .AddMore("@Name", string.IsNullOrEmpty(name) ? "" : (object)name)
                                    .AddMore("@RegistrationNo", string.IsNullOrEmpty(registrationNo) ? "" : (object)registrationNo)
                                    .AddMore("@FiscalYearId", fiscalYearId)
                                    .AddMore("@UserWardList", userWardList);

            var q = QueryBuilder.GetCommandText["Setup.Committee.Get"];
            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return dt.TransformToList<Committee>().ToList();

        }

        public async Task<Committee> GetCommitteeById(int id)
        {
            var dt = await GetCommittee(id: id);
            return dt.FirstOrDefault();
        }

        public async Task<DbResponse> SaveCommittee(Committee model)
        {
            var p = model.PrepareSQLParameters();
            var q = QueryBuilder.GetCommandText["Setup.Committee.Save"];
            var response = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);

            return response;
        }

        public async Task<DbResponse> DeleteCommittee(int id)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                     .AddMore("@CurrentUser", _currentUser.Id);
            var q = @"Update Committee Set IsDeleted = 1, 
	                                       ModifiedDate = GETDATE(), ModifiedBy = @CurrentUser where Id = @Id";

            var result = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return result;
        }

        #endregion


        #region CommitteeLetter
        public async Task<List<CommitteeLetter>> GetCommitteeLetter(int id = 0, int committeeId = 0, int letterTypeId = 0)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                    .AddMore("@CommitteeId", committeeId)
                                    .AddMore("@LetterTypeId", letterTypeId);
            var q = QueryBuilder.GetCommandText["Setup.CommitteeLetter.Get"];
            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return dt.TransformToList<CommitteeLetter>().ToList();

        }

        public async Task<DbResponse> SaveCommitteeLetter(CommitteeLetter model)
        {
            var p = model.PrepareSQLParameters();
            var q = QueryBuilder.GetCommandText["Setup.CommitteeLetter.Save"];
            var response = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return response;
        }

        public async Task<DbResponse> DeleteCommitteeLetter(int id)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                     .AddMore("@CurrentUser", _currentUser.Id);
            var q = @"Update CommitteeLetter Set IsDeleted = 1, 
	                                       ModifiedDate = GETDATE(), ModifiedBy = @CurrentUser where Id = @Id";

            var result = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return result;
        }

        #endregion


        #region Member

        public async Task<List<Member>> GetMember(int id = 0, int committeeId = 0, string name = "", int designationid = 0, string citizenshipNo = "", string mobileNo = "")
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                    .AddMore("@CommitteeId", committeeId)
                                    .AddMore("@Name", string.IsNullOrEmpty(name) ? "" : (object)name)
                                    .AddMore("@DesignationId", designationid)
                                    .AddMore("@CitizenshipNo", string.IsNullOrEmpty(citizenshipNo) ? "" : (object)citizenshipNo)
                                    .AddMore("@MobileNo", string.IsNullOrEmpty(mobileNo) ? "" : (object)mobileNo);

            var q = QueryBuilder.GetCommandText["Setup.Member.Get"];
            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return dt.TransformToList<Member>().ToList();
        }

        public async Task<Member> GetMemberById(int id = 0, int committeeId = 0)
        {
            var dt = await GetMember(id);
            return dt.FirstOrDefault();
        }

        public async Task<Member> GetMemberByCitizenshipNo(string citizenshipNo)
        {
            var p = _db.SqlParameters.AddMore("@CitizenshipNo", citizenshipNo);
            var q = @"Select * from Member Where CitizenshipNo = @CitizenshipNo and IsDeleted = 0";

            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return dt.TransformToObject<Member>();
        }

        public async Task<Member> CheckMemberByCitizenShipNumber(string citizenshipNo, int fyId, int committeeId)
        {
            var p = _db.SqlParameters.AddMore("@CitizenshipNo", citizenshipNo)
                                    .AddMore("@FyId", fyId)
                                    .AddMore("@CommitteeId", committeeId);

            var q = QueryBuilder.GetCommandText["Setup.MemberCheckCitizenshipNo"];
            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return dt.TransformToObject<Member>();
        }

        public async Task<Dictionary<string, decimal>> SaveMember(Member model)
        {
            var p = model.PrepareSQLParameters();
            var q = QueryBuilder.GetCommandText["Setup.Member.Save"];

            var dbResponse = await _db.ExecuteScalarAsync(CommandType.Text, q, p);

            if (dbResponse.HasError || dbResponse.Response == null)
                return new Dictionary<string, decimal>();

            var jsonResult = dbResponse.Response.ToString();
            var list = JsonConvert.DeserializeObject<List<Dictionary<string, decimal>>>(jsonResult);
            return list?.FirstOrDefault() ?? new Dictionary<string, decimal>();
        }

        public async Task<DbResponse> DeleteAllMember(int id)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                     .AddMore("@CurrentUser", _currentUser.Id);
            var q = @"Update Member Set IsDeleted = 1, 
	                                       ModifiedDate = GETDATE(), ModifiedBy = @CurrentUser where Id = @Id";

            var result = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return result;
        }

        public async Task<DbResponse> DeleteMember(int id)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                     .AddMore("@CurrentUser", _currentUser);
            var q = QueryBuilder.GetCommandText["Setup.Member.Delete"];

            var result = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return result;
        }

        #endregion


        #region CommitteeMember

        public async Task<List<Member>> GetMemberByCommitteeId(int committeeId = 0, string name = "", string citizenshipNo = "", string mobileNo = "")
        {
            var p = _db.SqlParameters
                                    .AddMore("@CommitteeId", committeeId)
                                    .AddMore("@Name", string.IsNullOrEmpty(name) ? "" : (object)name)
                                    .AddMore("@CitizenshipNo", string.IsNullOrEmpty(citizenshipNo) ? "" : (object)citizenshipNo)
                                    .AddMore("@MobileNo", string.IsNullOrEmpty(mobileNo) ? "" : (object)mobileNo);

            var q = QueryBuilder.GetCommandText["Setup.MemberGetByCommitteeId.Get"];
            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return dt.TransformToList<Member>().ToList();

        }

        #endregion


        #region Firm

        public async Task<List<Firm>> GetFirm(int id = 0, string name = "")
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                     .AddMore("@Name", string.IsNullOrEmpty(name) ? "" : (object)name);

            var q = @"Select f.*, ft.[Name] AS FirmTypeName from Firm as f 
                                            INNER JOIN dbo.FirmType as ft ON ft.Id = f.FirmTypeId
	                                        where (@Id = 0 or f.Id = @Id) and (@Name = '' or f.Name like '%'+ @Name + '%') and f.IsDeleted = 0";

            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return dt.TransformToList<Firm>().ToList();
        }

        public async Task<Firm> GetFirmById(int id = 0)
        {
            var dt = await GetFirm(id);
            return dt.FirstOrDefault();
        }

        public async Task<DbResponse> SaveFirm(Firm model)
        {
            var p = model.PrepareSQLParameters();
            var q = QueryBuilder.GetCommandText["Setup.Firm.Save"];
            var response = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return response;
        }

        public async Task<DbResponse> DeleteFirm(int id)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                     .AddMore("@CurrentUser", _currentUser.Id);
            var q = @"Update Firm Set IsDeleted = 1, 
	                                       ModifiedDate = GETDATE(), ModifiedBy = @CurrentUser where Id = @Id";

            var response = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return response;
        }

        #endregion


        #region Individual
        public async Task<List<Individual>> GetIndividual(int id = 0, string name = "")
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                     .AddMore("@Name", string.IsNullOrEmpty(name) ? "" : (object)name);

            var q = @"Select Id, Name, Address, MobileNo, IsEmployee, BankId, AccountNo FROM Individual
	                         WHERE(@Id = 0 or Id = @Id) and (@Name = '' or Name like '%' + @Name + '%') and IsDeleted = 0";

            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return dt.TransformToList<Individual>().ToList();
        }

        public async Task<Individual> GetIndividualById(int id)
        {
            var dt = await GetIndividual(id);
            return dt.FirstOrDefault();
        }

        public async Task<DbResponse> SaveIndividual(Individual model)
        {
            var p = model.PrepareSQLParameters();
            var q = QueryBuilder.GetCommandText["Setup.Individual.Save"];
            var response = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return response;
        }

        public async Task<DbResponse> DeleteIndividual(int id)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                     .AddMore("@CurrentUser", _currentUser.Id);
            var q = @"Update Individual Set IsDeleted = 1, 
	                                       ModifiedDate = GETDATE(), ModifiedBy = @CurrentUser where Id = @Id";

            var response = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return response;
        }

        #endregion



        #endregion




        // -----------------------------------------------------------
        // 🔹 Master Setup
        // -----------------------------------------------------------



        #region MasterSetup


        #region Activity

        public async Task<List<Activity>> GetActivity(int id = 0, int activityGroupId = 0, string name = "", string code = "", string referenceCode = "", int fiscalYearId = 0)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                    .AddMore("@ActivityGroupId", activityGroupId)
                                    .AddMore("@Name", string.IsNullOrEmpty(name) ? "" : (object)name)
                                    .AddMore("@Code", string.IsNullOrEmpty(code) ? "" : (object)code)
                                    .AddMore("@ReferenceCode", string.IsNullOrEmpty(referenceCode) ? "" : (object)referenceCode)
                                    .AddMore("@FiscalYearId", fiscalYearId);

            var q = QueryBuilder.GetCommandText["Setup.Activity.Get"];

            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return dt.TransformToList<Activity>().ToList();
        }

        public async Task<DbResponse> SaveActivity(Activity model)
        {
            var p = model.PrepareSQLParameters();
            var q = QueryBuilder.GetCommandText["Setup.Activity.Save"];
            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }

        public async Task<DbResponse> DeleteActivity(int id)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                     .AddMore("@currentUser", _currentUser.Id);

            var q = @"Update dbo.Activity Set IsDeleted = 1, ModifiedDate = GETDATE(), ModifiedBy = @CurrentUser Where Id = @Id";

            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }

        #endregion


        #region Activity Group
        public async Task<List<ActivityGroup>> GetActivityGroup(int id = 0, string name = "", string code = "")
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                    .AddMore("@Name", string.IsNullOrEmpty(name) ? "" : (object)name)
                                    .AddMore("@Code", string.IsNullOrEmpty(code) ? "" : (object)code);

            var q = QueryBuilder.GetCommandText["Setup.Activity.Get"];

            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return dt.TransformToList<ActivityGroup>().ToList();
        }

        public async Task<List<ActivityGroup>> GetActivityGroupDownList()
        {
            //var p = _db.SqlParameters.AddMore("@Id", id);                           
            var q = QueryBuilder.GetCommandText["Setup.DropDownList.ActivityGroup.Get"];
            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q);
            return dt.TransformToList<ActivityGroup>().ToList();
        }

        public async Task<DbResponse> SaveActivityGroup(ActivityGroup model)
        {
            var p = model.PrepareSQLParameters();
            var q = QueryBuilder.GetCommandText["Setup.ActivityGroup.Save"];
            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }

        public async Task<DbResponse> DeleteActivityGroup(int id)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                     .AddMore("@currentUser", _currentUser.Id);

            var q = @"Update dbo.ActivityGroup Set IsDeleted = 1, ModifiedDate = GETDATE(), ModifiedBy = @CurrentUser Where Id = @Id";

            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }

        public List<string> GetActivityGroupDisplayName(IList<ActivityGroup> list, int id)
        {
            var arr = new List<string>();
            var group = list.FirstOrDefault(x => x.Id == id);

            while (group != null)
            {
                arr.Insert(0, group.Name);
                group = group.ParentId.HasValue
                        ? list.FirstOrDefault(x => x.Id == group.ParentId.Value)
                        : null;
            }
            return arr;
        }

        #endregion

        #region Activity Rate
        public async Task<ActivityRate> GetActivityRate(int activityId = 0, int fiscalYearId = 0)
        {
            var p = _db.SqlParameters.AddMore("@ActivityId", activityId)
                                     .AddMore("@FiscalYearId", fiscalYearId);

            var q = @"Select * from ActivityRate where ActivityId = @ActivityId and FiscalYearId = @FiscalYearId and IsDeleted = 0";
            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return dt.TransformToObject<ActivityRate>();
        }
        #endregion


        #region AgreementFormat
        public async Task<List<AgreementFormat>> GetAgreementFormat(int id = 0, string name = "", int projectId = 0)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                    .AddMore("@Name", string.IsNullOrEmpty(name) ? "" : (object)name)
                                    .AddMore("@ProjectId", projectId);

            var q = QueryBuilder.GetCommandText["Setup.AgreementFormat.Get"];

            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return dt.TransformToList<AgreementFormat>().ToList();
        }
        public async Task<AgreementFormat> GetAgreementFormatById(int id)
        {
            var ds = await GetAgreementFormat(id);
            return ds.FirstOrDefault();
        }
        public async Task<DbResponse> SaveAgreementFormat(AgreementFormat model)
        {
            var p = model.PrepareSQLParameters();
            var q = QueryBuilder.GetCommandText["Setup.AgreementFormat.Save"];
            var response = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return response;
        }
        public async Task<DbResponse> DeleteAgreementFormat(int id)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                     .AddMore("@CurrentUser", _currentUser.Id);

            var q = @"Update dbo.AgreementFormat Set IsDeleted = 1, ModifiedDate = GETDATE(), ModifiedBy = @CurrentUser Where Id = @Id";
            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }
        public async Task<AgreementFormat> ProcessAgreementFormat(int projectId)
        {
            var agreementFormat = (await GetAgreementFormat(projectId: projectId)).FirstOrDefault();
            var metaDataTable = await GetMetaDataValue(projectId: projectId);

            if (metaDataTable != null && metaDataTable.Rows.Count > 0)
            {
                foreach (DataRow row in metaDataTable.Rows)
                {
                    var meta = row["MetaData"]?.ToString();
                    var value = row["Value"]?.ToString();

                    if (!string.IsNullOrEmpty(meta))
                        agreementFormat.Body = agreementFormat.Body.Replace(meta, value ?? string.Empty);
                }
            }
            return agreementFormat;
        }

        #endregion


        #region Bank

        public async Task<List<Bank>> GetBank(int id = 0, string name = "")
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                    .AddMore("@Name", string.IsNullOrEmpty(name) ? "" : (object)name);

            var q = @"SELECT * FROM Bank WHERE (@Id = 0 OR Id = @Id) AND IsDeleted = 0";

            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return dt.TransformToList<Bank>().ToList();
        }

        public async Task<DbResponse> SaveBank(Bank model)
        {

            var p = model.PrepareSQLParameters();
            var q = QueryBuilder.GetCommandText["Setup.Bank.Save"];
            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }

        public async Task<DbResponse> DeleteBank(int id)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                     .AddMore("@UserId", _currentUser.Id);

            var q = QueryBuilder.GetCommandText["Setup.Bank.Delete"];
            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }

        #endregion


        #region Beneficiary
        public async Task<List<Beneficiary>> GetBeneficiary(int id = 0, string name = "")
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                    .AddMore("@Name", string.IsNullOrEmpty(name) ? "" : (object)name);

            var q = @"SELECT * FROM Beneficiary 
                               WHERE (@Id = 0 OR Id = @Id) AND (@Name='' or  Name Like '%'+LTRIM(RTRIM(@Name))+'%') AND IsDeleted = 0";

            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return dt.TransformToList<Beneficiary>().ToList();
        }

        public async Task<Beneficiary> GetBeneficiaryById(int id)
        {
            var dt = await GetBeneficiary(id);
            return dt.FirstOrDefault();
        }

        public async Task<DbResponse> SaveBeneficiary(Beneficiary model)
        {

            var p = model.PrepareSQLParameters();
            var q = QueryBuilder.GetCommandText["Setup.Beneficiary.Save"];
            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }

        public async Task<DbResponse> DeleteBeneficiary(int id)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                     .AddMore("@CurrentUser", _currentUser.Id);

            var q = @"Update dbo.Activity Set IsDeleted = 1, ModifiedDate = GETDATE(), ModifiedBy = @CurrentUser Where Id = @Id";
            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }

        #endregion

        
        /// Budget ///
         
        #region Budget


        #region BudgetAllocation

        public async Task<List<BudgetAllocation>> GetBudgetAllocation(int id = 0, int fiscalYearId = 0, int currentUserId = 0, string name = "", int wardId = 0, bool parentRelation = false)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                     .AddMore("@Id", fiscalYearId)
                                     .AddMore("@CurrentUserId", currentUserId)
                                    .AddMore("@Name", string.IsNullOrEmpty(name) ? "" : (object)name)
                                    .AddMore("@WardId", wardId)
                                    .AddMore("@parentRelation", parentRelation);

            var q = QueryBuilder.GetCommandText["Setup.BudgetAllocation.Get"];
            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return dt.TransformToList<BudgetAllocation>().ToList();
        }

        public async Task<BudgetAllocation> GetBudgetAllocationById(int id, int currentUserId)
        {
            var dt = await GetBudgetAllocation(id, currentUserId: currentUserId);
            return dt.FirstOrDefault();
        }

        public async Task<List<BudgetAllocation>> GetBudgetAllocationDownList(int? id)
        {
            var p = _db.SqlParameters.AddMore("@Id", id);

            var q = QueryBuilder.GetCommandText["Setup.DropDownList.BudgetAllocation.Get"];
            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);

            return dt.TransformToList<BudgetAllocation>().ToList();
        }

        public async Task<DbResponse> SaveBudgetAllocation(BudgetAllocation model)
        {

            var p = model.PrepareSQLParameters();

            var errorParam = new SqlParameter("@ErrorMessage", SqlDbType.NVarChar, 400)
            {
                Direction = ParameterDirection.Output
            };

            p.Add(errorParam);

            var q = QueryBuilder.GetCommandText["Setup.BudgetAllocation.Save"];

            var dbResult = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);

            var message = dbResult.HasError
                                ? "An Error has occurred."
                                : errorParam.Value?.ToString() ?? string.Empty;
            return new DbResponse
            {
                HasError = dbResult.HasError || !string.IsNullOrWhiteSpace(message),
                Message = message,
                Response = dbResult.Response
            };
        }

        public async Task<DbResponse> DeleteBudgetAllocation(int id)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                     .AddMore("@CurrentUser", _currentUser.Id);

            var q = @"Update dbo.BudgetAllocation Set IsDeleted = 1, ModifiedDate = GETDATE(), ModifiedBy = @CurrentUser Where Id = @Id";
            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }

        public List<string> GetDisplayName(IList<BudgetAllocation> list, int id)
        {
            var arr = new List<string>();
            var group = list.FirstOrDefault(x => x.Id == id);

            while (group != null)
            {
                arr.Insert(0, group.Name);
                group = group.ParentId.HasValue
                    ? list.FirstOrDefault(x => x.Id == group.ParentId.Value)
                    : null;
            }
            return arr;
        }

        #endregion


        #region BudgetAllocationProject

        public async Task<List<BudgetAllocationProject>> GetBudgetAllocationProject(int allocationId)
        {
            var p = _db.SqlParameters.AddMore("@AllocationId", allocationId);
            var q = QueryBuilder.GetCommandText["Setup.BudgetAllocation.GetProjectDetail"];

            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return dt.TransformToList<BudgetAllocationProject>().ToList();
        }

        #endregion


        #region Budget Source

        public async Task<List<BudgetSource>> GetBudgetSource(int id = 0, string name = "", string codeno = "", int fiscalYearId = 0, bool withBudget = false)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                    .AddMore("@Name", string.IsNullOrEmpty(name) ? "" : (object)name);
            var q = QueryBuilder.GetCommandText["Setup.BudgetSource.Get"];

            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return dt.TransformToList<BudgetSource>().ToList();
        }

        public async Task<BudgetSource> GetBudgetSourceById(int id = 0)
        {
            var dt = await GetBudgetSource(id);
            return dt.FirstOrDefault();
        }

        public async Task<DbResponse> SaveBudgetSource(BudgetSource model)
        {

            var p = model.PrepareSQLParameters();
            var q = QueryBuilder.GetCommandText["Setup.BudgetSource.Save"];
            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }

        public async Task<DbResponse> DeleteBudgetSource(int id)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                     .AddMore("@CurrentUser", _currentUser.Id);

            var q = @"Update dbo.BudgetAllocation Set IsDeleted = 1, ModifiedDate = GETDATE(), ModifiedBy = @CurrentUser Where Id = @Id";
            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }


        #endregion


        #region BudgetSubTitle

        public async Task<List<BudgetSubTitle>> GetBudgetSubTitle(int id = 0, string name = "", string code = "", int expensesTypeId = 0)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                    .AddMore("@Name", string.IsNullOrEmpty(name) ? "" : (object)name)
                                    .AddMore("@Code", string.IsNullOrEmpty(code) ? "" : (object)code)
                                    .AddMore("@ExpensesTypeId", expensesTypeId);

            var q = QueryBuilder.GetCommandText["Setup.BudgetSubTitle.Get"];

            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return dt.TransformToList<BudgetSubTitle>().ToList();
        }

        public async Task<DbResponse> SaveBudgetSubTitle(BudgetSubTitle model)
        {

            var p = model.PrepareSQLParameters();
            var q = QueryBuilder.GetCommandText["Setup.BudgetSubTitle.Save"];
            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }

        public async Task<DbResponse> DeleteBudgetSubTitle(int id)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                     .AddMore("@CurrentUser", _currentUser.Id);

            var q = @"Update dbo.BudgetSubTitle Set IsDeleted = 1, ModifiedDate = GETDATE(), ModifiedBy = @CurrentUser Where Id = @Id";
            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }

        #endregion


        #endregion
        
        /// Budget ///

        #region Checklist
        public async Task<List<Checklist>> GetChecklist(int id = 0, string name = "")
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                    .AddMore("@Name", string.IsNullOrEmpty(name) ? "" : (object)name);

            var q = @"SELECT * FROM Checklist 
                               WHERE (@Id = 0 OR Id = @Id) AND (@Name='' or  Name Like '%'+LTRIM(RTRIM(@Name))+'%') AND IsDeleted = 0";

            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return dt.TransformToList<Checklist>().ToList();
        }

        public async Task<Checklist> GetChecklistById(int id)
        {
            var dt = await GetChecklist(id);
            return dt.FirstOrDefault();
        }

        public async Task<DbResponse> SaveChecklist(Checklist model)
        {

            var p = model.PrepareSQLParameters();
            var q = QueryBuilder.GetCommandText["Setup.Checklist.Save"];
            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }

        public async Task<DbResponse> DeleteChecklist(int id)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                     .AddMore("@CurrentUser", _currentUser.Id);

            var q = @"Update dbo.Checklist Set IsDeleted = 1, ModifiedDate = GETDATE(), ModifiedBy = @CurrentUser Where Id = @Id";
            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }

        public async Task<Checklist> GetChecklistName(int id)
        {
            var p = _db.SqlParameters.AddMore("@Id", id);
            var q = @"SELECT * FROM Checklist  WHERE (@Id = 0 OR Id = @Id) AND IsDeleted = 0";

            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return dt.TransformToObject<Checklist>();
        }

        #endregion


        #region Configuration
        public async Task<List<Configuration>> GetConfiguration(int id = 0, string name = "")
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                     .AddMore("@Name", string.IsNullOrEmpty(name) ? "" : (object)name);
            var q = @"Select * from Configuration 
                                    where (@Id = 0 or Id = @Id) And (@Name = '' or Name like '%' + @Name + '%') And IsDeleted = 0";

            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return dt.TransformToList<Configuration>().ToList();
        }

        public async Task<DbResponse> SaveConfiguration(Configuration model)
        {
            var p = model.PrepareSQLParameters();
            var q = QueryBuilder.GetCommandText["Setup.Configuration.Save"];
            var response = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return response;
        }

        public async Task<DbResponse> DeleteConfiguration(int id)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                     .AddMore("@UserId", _currentUser.Id);
            var q = @"Update Configuration Set IsDeleted = 1, 
	                                       ModifiedDate = GETDATE(), ModifiedBy = @UserId where Id = @Id";

            var result = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return result;
        }

        #endregion


        #region DonorType
        public async Task<List<DonorType>> GetDonorType(int id = 0, string name = "")
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                    .AddMore("@Name", string.IsNullOrEmpty(name) ? "" : (object)name);

            var q = @"SELECT * FROM DonorType 
                               WHERE (@Id = 0 OR Id = @Id) AND (@Name='' or  Name Like '%'+LTRIM(RTRIM(@Name))+'%') AND IsDeleted = 0";

            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return dt.TransformToList<DonorType>().ToList();
        }

        public async Task<DonorType> GetDonorTypeById(int id)
        {
            var dt = await GetDonorType(id);
            return dt.FirstOrDefault();
        }

        public async Task<DbResponse> SaveDonorType(DonorType model)
        {

            var p = model.PrepareSQLParameters();
            p.AddMore("@CurrentUser", _currentUser.Id);
            var q = QueryBuilder.GetCommandText["Setup.DonorType.Save"];
            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }

        public async Task<DbResponse> DeleteDonorType(int id)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                     .AddMore("@CurrentUser", _currentUser.Id);

            var q = @"Update dbo.DonorType Set IsDeleted = 1, ModifiedDate = GETDATE(), ModifiedBy = @CurrentUser Where Id = @Id";
            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }
        #endregion


        #region ExpensesHead
        public async Task<List<ExpensesHead>> GetExpensesHead(int id = 0, string name = "")
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                    .AddMore("@Name", string.IsNullOrEmpty(name) ? "" : (object)name);

            var q = QueryBuilder.GetCommandText["Setup.ExpensesHead.Get"];

            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return dt.TransformToList<ExpensesHead>().ToList();
        }
        public async Task<ExpensesHead> GetExpensesHeadById(int id)
        {
            var dt = await GetExpensesHead(id);
            return dt.FirstOrDefault();
        }
        public async Task<DbResponse> SaveExpensesHead(ExpensesHead model)
        {
            var p = model.PrepareSQLParameters();
            var q = QueryBuilder.GetCommandText["Setup.ExpensesHead.Save"];
            var response = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return response;
        }
        public async Task<DbResponse> DeleteExpensesHead(int id)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                     .AddMore("@CurrentUser", _currentUser.Id);

            var q = @"Update dbo.ExpensesHead Set IsDeleted = 1, ModifiedDate = GETDATE(), ModifiedBy = @CurrentUser Where Id = @Id";
            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }

        #endregion


        #region ItemType
        public async Task<List<ItemType>> GetItemType(int id = 0, string name = "")
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                    .AddMore("@Name", string.IsNullOrEmpty(name) ? "" : (object)name);

            var q = QueryBuilder.GetCommandText["Setup.ItemType.Get"];
            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return dt.TransformToList<ItemType>().ToList();
        }

        public async Task<DbResponse> SaveItemType(ItemType model)
        {

            var p = model.PrepareSQLParameters();
            var q = QueryBuilder.GetCommandText["Setup.ItemType.Save"];
            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }

        public async Task<DbResponse> DeleteItemType(int id)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                     .AddMore("@CurrentUser", _currentUser.Id);

            var q = @"Update dbo.ItemType Set IsDeleted = 1, ModifiedDate = GETDATE(), ModifiedBy = @CurrentUser Where Id = @Id";
            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }

        #endregion


        #region Item

        public async Task<List<Item>> GetItem(int id = 0, int itemTypeId = 0, string name = "", int fiscalYearId = 0)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                    .AddMore("@ItemTypeId", itemTypeId)
                                    .AddMore("@Name", string.IsNullOrEmpty(name) ? "" : (object)name)
                                    .AddMore("@FiscalYearId", fiscalYearId);

            var q = QueryBuilder.GetCommandText["Setup.Item.Get"];
            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return dt.TransformToList<Item>().ToList();
        }

        public async Task<DbResponse> SaveItem(Item model)
        {

            var p = model.PrepareSQLParameters();
            var q = QueryBuilder.GetCommandText["Setup.Item.Save"];
            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }

        public async Task<DbResponse> DeleteItem(int id)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                     .AddMore("@CurrentUser", _currentUser.Id);

            var q = @"Update dbo.Item Set IsDeleted = 1, ModifiedDate = GETDATE(), ModifiedBy = @CurrentUser Where Id = @Id";
            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }

        #endregion


        #region ItemRate
        public async Task<List<ItemRate>> GetItemRate(int fiscalYearId = 0, int id = 0, string name = "")
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                    .AddMore("@Name", string.IsNullOrEmpty(name) ? "" : (object)name)
                                    .AddMore("@FiscalYearId", fiscalYearId);

            var q = QueryBuilder.GetCommandText["Setup.ItemRate.Get"];
            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return dt.TransformToList<ItemRate>().ToList();
        }
        public async Task<DbResponse> SaveItemRate(List<ItemRate> list)
        {
            var sb = new StringBuilder();
            sb.Append("<root>");
            foreach (var item in list)
            {
                sb.Append($"<itemrate id='{item.Id}' itemId='{item.ItemId}' fiscalYearId='{item.FiscalYearId}' rate='{item.Rate}' />");
            }
            sb.Append("</root>");

            var p = _db.SqlParameters
                                    .AddMore("@CurrentUser", list.First().CurrentUser)
                                    .AddMore("@xml", sb.ToString());

            var q = QueryBuilder.GetCommandText["Setup.ItemRate.Save"];
            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }
        public async Task<DbResponse> DeleteItemRate(int id)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                     .AddMore("@CurrentUser", _currentUser.Id);

            var q = @"Update dbo.ItemRate Set IsDeleted = 1, ModifiedDate = GETDATE(), ModifiedBy = @CurrentUser Where Id = @Id";
            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }

        #endregion


        #region ImplementationModel
        public async Task<List<ImplementationModel>> GetImplementationModel(int id = 0, int projectModelId = 0, string name = "")
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                    .AddMore("@Name", string.IsNullOrEmpty(name) ? "" : (object)name)
                                    .AddMore("@ProjectModelId", projectModelId);

            var q = QueryBuilder.GetCommandText["Setup.ImplementationModel.Get"];

            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return dt.TransformToList<ImplementationModel>().ToList();
        }
        public async Task<DbResponse> SaveImplementationModel(ImplementationModel model)
        {
            var p = model.PrepareSQLParameters();
            var q = QueryBuilder.GetCommandText["Setup.ImplementationModel.Save"];
            var response = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return response;
        }
        public async Task<DbResponse> DeleteImplementationModel(int id)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                     .AddMore("@CurrentUser", _currentUser.Id);

            var q = @"Update dbo.ImplementationModel Set IsDeleted = 1, ModifiedDate = GETDATE(), ModifiedBy = @CurrentUser Where Id = @Id";
            var response = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return response;
        }

        #endregion


        #region ImplementationLevel
        public async Task<List<ImplementationLevel>> GetImplementationLevel(int id = 0, string name = "", string code = "", decimal threshold = 0)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                    .AddMore("@Name", string.IsNullOrEmpty(name) ? "" : (object)name)
                                    .AddMore("@Code", string.IsNullOrEmpty(code) ? "" : (object)code)
                                    .AddMore("@Threshold", threshold);

            var q = QueryBuilder.GetCommandText["Setup.ImplementationLevel.Get"];

            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return dt.TransformToList<ImplementationLevel>().ToList();
        }
        public async Task<DbResponse> SaveImplementationLevel(ImplementationLevel model)
        {
            var p = model.PrepareSQLParameters();
            var q = QueryBuilder.GetCommandText["Setup.ImplementationLevel.Save"];
            var response = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return response;
        }
        public async Task<DbResponse> DeleteImplementationLevel(int id)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                     .AddMore("@CurrentUser", _currentUser.Id);

            var q = @"Update dbo.ImplementationLevel Set IsDeleted = 1, ModifiedDate = GETDATE(), ModifiedBy = @CurrentUser Where Id = @Id";
            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }
        #endregion


        #region LetterFormat

        public async Task<List<LetterFormat>> GetLetterFormat(int id = 0, string name = "", int letterType = 0, int projectId = 0)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                    .AddMore("@Name", string.IsNullOrEmpty(name) ? "" : (object)name)
                                    .AddMore("@LetterType", letterType)
                                    .AddMore("@ProjectId", projectId);

            var q = QueryBuilder.GetCommandText["Setup.LetterFormat.Get"];

            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return dt.TransformToList<LetterFormat>().ToList();
        }
        public async Task<LetterFormat> GetLetterFormatById(int id)
        {
            var dt = await GetLetterFormat(id);
            return dt.FirstOrDefault();
        }

        public async Task<DbResponse> SaveLetterFormat(LetterFormat model)
        {
            var p = model.PrepareSQLParameters();
            p.AddMore(@"CurrentUser", _currentUser.Id);

            var q = QueryBuilder.GetCommandText["Setup.LetterFormat.Save"];
            var response = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return response;
        }
        public async Task<DbResponse> DeleteLetterFormat(int id)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                     .AddMore("@CurrentUser", _currentUser.Id);

            var q = @"Update dbo.LetterFormat Set IsDeleted = 1, ModifiedDate = GETDATE(), ModifiedBy = @CurrentUser Where Id = @Id";
            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }

        public async Task<DataTable> GetMetaDataValue(int projectId = 0, int committeeId = 0, int evaluationId = 0, int advanceId = 0, int timeExtendedId = 0, int currentUserId = 0)
        {
            var p = _db.SqlParameters.AddMore("@ProjectId", projectId)
                                    .AddMore("@CommitteeId", committeeId)
                                    .AddMore("@EvaluationId", evaluationId)
                                    .AddMore("@AdvanceId", advanceId)
                                    .AddMore("@TimeExtendedId", timeExtendedId)
                                    .AddMore("@CurrentUserId", currentUserId);

            var q = QueryBuilder.GetCommandText["Setup.LetterFormat.Process"];

            var ds = await _db.ExecuteDataSetAsync(CommandType.Text, q, p);
            return ds.Tables[0];
        }

        public async Task<LetterFormat> ProcessLetter(int projectId = 0, int letterType = 0, int committeeId = 0, int evaluationId = 0, int advanceId = 0, int timeExtendedId = 0, int currentUserId = 0)
        {
            var letterFormat = (await GetLetterFormat(letterType: letterType, projectId: projectId)).FirstOrDefault();
            var dt = await GetMetaDataValue(projectId, committeeId, evaluationId, advanceId, timeExtendedId, currentUserId);

            if (letterFormat != null && dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    var meta = row["MetaData"]?.ToString();
                    var value = row["Value"]?.ToString();

                    if (!string.IsNullOrEmpty(meta))
                        letterFormat.LetterBody = letterFormat.LetterBody.Replace(meta, value ?? string.Empty);
                }
                letterFormat.LetterBody = letterFormat.LetterBody.Replace("@Miti", DateMiti.GetDateMiti.GetMiti(DateTime.Now));

            }
            return letterFormat;
        }

        #endregion


        #region Norms
        public async Task<List<Norms>> GetNorms(int activityId, string name = "", int fiscalYearId = 0)
        {
            var p = _db.SqlParameters.AddMore("@ActivityId", activityId)
                                    .AddMore("@Name", string.IsNullOrEmpty(name) ? "" : (object)name)
                                    .AddMore("@FiscalYearId", fiscalYearId);

            var q = QueryBuilder.GetCommandText["Setup.Norms.Get"];
            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return dt.TransformToList<Norms>().ToList();
        }
        public async Task<DbResponse> SaveNorms(List<Norms> normsList)
        {
            if (normsList == null || !normsList.Any())
                return new DbResponse { HasError = true, Message = "No norms to save." };

            var sb = new StringBuilder();
            sb.Append("<root>");
            foreach (var item in normsList)
            {
                sb.Append($"<item itemId='{item.ItemId}' qty='{item.Qty}' />");
            }
            sb.Append("</root>");

            var first = normsList.First();
            var p = _db.SqlParameters.AddMore("@Id", first.Id)
                .AddMore("@XmlNormsDetail", sb.ToString())
                .AddMore("@ActivityId", first.ActivityId)
                .AddMore("@ActivityRateId", first.ActivityRateId)
                .AddMore("@UserId", first.CurrentUser)
                .AddMore("@FiscalYearId", first.FiscalYearId);

            var q = QueryBuilder.GetCommandText["Setup.Norms.Save"];
            var ds = await _db.ExecuteDataSetAsync(CommandType.Text, q, p);

            if (ds.Tables.Count > 0)
            {
                return new DbResponse
                {
                    HasError = false,
                    Message = "Success",
                    Response = ds.Tables[0]
                };
            }
            return new DbResponse { HasError = true, Message = "Failed to save norms." };

        }
        public async Task<DbResponse> DeleteNorms(int id)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                     .AddMore("@CurrentUser", _currentUser.Id);

            var q = @"Update dbo.Norms Set IsDeleted = 1, ModifiedDate = GETDATE(), ModifiedBy = @CurrentUser Where Id = @Id";
            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }

        #endregion


        #region NormsType
        public async Task<List<NormsType>> GetNormsType(int id = 0, string name = "")
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                    .AddMore("@Name", string.IsNullOrEmpty(name) ? "" : (object)name);

            var q = @"SELECT * FROM NormsType WHERE (@Id = 0 OR Id = @Id) AND (@Name = '' or Name like '%' + @Name + '%') AND IsDeleted = 0";
            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return dt.TransformToList<NormsType>().ToList();
        }
        public async Task<DbResponse> SaveNormsType(NormsType model)
        {
            var p = model.PrepareSQLParameters();
            var q = QueryBuilder.GetCommandText["Setup.NormsType.Save"];
            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }
        public async Task<DbResponse> DeleteNormsType(int id)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                     .AddMore("@CurrentUser", _currentUser.Id);

            var q = @"Update dbo.NormsType Set IsDeleted = 1, ModifiedDate = GETDATE(), ModifiedBy = @CurrentUser Where Id = @Id";
            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }
        #endregion


        #region ProgressIndicator
        public async Task<List<ProgressIndicator>> GetProgressIndicator(int id = 0, string name = "", string code = "")
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                    .AddMore("@Name", string.IsNullOrEmpty(name) ? "" : (object)name)
                                    .AddMore("@Code", string.IsNullOrEmpty(code) ? "" : (object)code);

            var q = QueryBuilder.GetCommandText["Setup.ProgressIndicator.Get"];
            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return dt.TransformToList<ProgressIndicator>().ToList();
        }
        public async Task<List<ProgressIndicator>> GetRemAccToProject(int projectCompletedGoalId, int projectId)
        {
            var p = _db.SqlParameters.AddMore("@projectCompletedGoalId", projectCompletedGoalId)
                                    .AddMore("@projectId", projectId);

            var q = QueryBuilder.GetCommandText["Setup.RemProgressIndicatorAccToProject.Get"];
            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return dt.TransformToList<ProgressIndicator>().ToList();
        }
        public async Task<DbResponse> SaveProgressIndicator(ProgressIndicator model)
        {
            var p = model.PrepareSQLParameters();
            var q = QueryBuilder.GetCommandText["Setup.ProgressIndicator.Save"];
            var result = await _db.ExecuteScalarAsync(CommandType.Text, q, p);
            return result;

            //if (result.HasError)
            //    throw new Exception(result.Message);
            //if (result.Response != null)
            //    return Convert.ToInt32(result.Response);
            //throw new Exception("No Id returned from the database.");        
        }
        public async Task<DbResponse> DeleteProgressIndicator(int id)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                     .AddMore("@CurrentUser", _currentUser.Id);

            var q = @"Update dbo.ProgressIndicator Set IsDeleted = 1, ModifiedDate = GETDATE(), ModifiedBy = @CurrentUser Where Id = @Id";
            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }
        #endregion


        #region ProjectGroup

        public async Task<List<ProjectGroup>> GetProjectGroup(int id = 0, string name = "", int sectorId = 0)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                    .AddMore("@SectorId", sectorId)
                                    .AddMore("@Name", string.IsNullOrEmpty(name) ? "" : (object)name);

            var q = QueryBuilder.GetCommandText["Setup.ProjectGroup.Get"];

            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return dt.TransformToList<ProjectGroup>().ToList();
        }
        public async Task<ProjectGroup> GetProjectGroupById(int id)
        {
            var dt = await GetProjectGroup(id);
            return dt.FirstOrDefault();
        }

        public async Task<List<ProjectGroup>> GetProjectGroupDownList()
        {
            //var p = _db.SqlParameters.AddMore("@Id", id);                           
            var q = QueryBuilder.GetCommandText["Setup.DropDownList.ProjectGroup.Get"];
            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q);
            return dt.TransformToList<ProjectGroup>().ToList();
        }

        public async Task<DbResponse> SaveProjectGroup(ProjectGroup model)
        {
            var p = model.PrepareSQLParameters();
            var q = QueryBuilder.GetCommandText["Setup.ProjectGroup.Save"];
            var response = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return response;
        }

        public async Task<DbResponse> DeleteProjectGroup(int id)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                     .AddMore("@currentUser", _currentUser.Id);

            var q = @"Update dbo.ProjectGroup Set IsDeleted = 1, ModifiedDate = GETDATE(), ModifiedBy = @CurrentUser Where Id = @Id";

            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }
        public async Task<List<ProjectGroup>> GetProjectGroupGetParentName(int Id, int ParentId)
        {
            var p = _db.SqlParameters.AddMore("@Id", Id)
                                    .AddMore("@ParentId", ParentId);
            var q = QueryBuilder.GetCommandText["Setup.ProjectGroupParentName.Get"];

            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return dt.TransformToList<ProjectGroup>().ToList();
        }
        public List<string> GetProjectGroupDisplayName(IList<ProjectGroup> list, int id)
        {
            var arr = new List<string>();
            var group = list.FirstOrDefault(x => x.Id == id);

            while (group != null)
            {
                arr.Insert(0, group.Name);
                group = group.ParentId.HasValue
                        ? list.FirstOrDefault(x => x.Id == group.ParentId.Value)
                        : null;
            }
            return arr;
        }

        #endregion


        #region Sector
        public async Task<List<Sector>> GetSector(int id = 0, string name = "", string code = "")
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                    .AddMore("@Name", string.IsNullOrEmpty(name) ? "" : (object)name)
                                    .AddMore("@Code", string.IsNullOrEmpty(code) ? "" : (object)code);

            var q = @"Select * from Sector
	                  Where (@Id = 0 or Id = @Id) And (@Name = '' or Name like '%' + @Name + '%') and (@Code = '' or Code like '%' + @Code + '%') and IsDeleted = 0";

            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return dt.TransformToList<Sector>().ToList();
        }
        public async Task<DbResponse> SaveSector(Sector model)
        {
            var p = model.PrepareSQLParameters();
            var q = QueryBuilder.GetCommandText["Setup.Sector.Save"];
            var response = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return response;
        }
        public async Task<DbResponse> DeleteSector(int id)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                     .AddMore("@CurrentUser", _currentUser.Id);

            var q = @"Update dbo.Sector Set IsDeleted = 1, ModifiedDate = GETDATE(), ModifiedBy = @CurrentUser Where Id = @Id";
            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }
        #endregion


        #region SubSector
        public async Task<List<SubSector>> GetSubSector(int id = 0, int sectorId = 0, string name = "", string code = "")
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                     .AddMore("@SectorId", sectorId)
                                    .AddMore("@Name", string.IsNullOrEmpty(name) ? "" : (object)name)
                                    .AddMore("@Code", string.IsNullOrEmpty(code) ? "" : (object)code);

            var q = QueryBuilder.GetCommandText["Setup.SubSector.Get"];

            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return dt.TransformToList<SubSector>().ToList();
        }
        public async Task<DbResponse> SaveSubSector(SubSector model)
        {
            var p = model.PrepareSQLParameters();
            var q = QueryBuilder.GetCommandText["Setup.SubSector.Save"];
            var response = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return response;
        }
        public async Task<DbResponse> DeleteSubSector(int id)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                     .AddMore("@CurrentUser", _currentUser.Id);

            var q = @"Update dbo.SubSector Set IsDeleted = 1, ModifiedDate = GETDATE(), ModifiedBy = @CurrentUser Where Id = @Id";
            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }
        #endregion


        #region Target
        public async Task<List<Target>> GetTarget(int id = 0, string name = "", string code = "")
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                    .AddMore("@Name", string.IsNullOrEmpty(name) ? "" : (object)name)
                                    .AddMore("@Code", string.IsNullOrEmpty(code) ? "" : (object)code);

            var q = @"Select * from Target 
                      Where	(@Id = 0 or Id = @Id) And (@Name = '' or Name = @Name) And (@Code = '' or Code = @Code) And IsDeleted = 0";

            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return dt.TransformToList<Target>().ToList();
        }
        public async Task<DbResponse> SaveTarget(Target model)
        {
            var p = model.PrepareSQLParameters();
            var q = QueryBuilder.GetCommandText["Setup.Target.Save"];
            var response = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return response;
        }
        public async Task<DbResponse> DeleteTarget(int id)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                     .AddMore("@CurrentUser", _currentUser.Id);

            var q = @"Update dbo.Target Set IsDeleted = 1, ModifiedDate = GETDATE(), ModifiedBy = @CurrentUser Where Id = @Id";
            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }
        #endregion


        #region Unit
        public async Task<List<Unit>> GetUnit(int id = 0, string name = "", string description = "")
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                    .AddMore("@Name", string.IsNullOrEmpty(name) ? "" : (object)name)
                                    .AddMore("@Description", string.IsNullOrEmpty(description) ? "" : (object)description);

            var q = QueryBuilder.GetCommandText["Setup.Unit.Get"];

            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return dt.TransformToList<Unit>().ToList();
        }
        public async Task<DbResponse> SaveUnit(Unit model)
        {
            var p = model.PrepareSQLParameters();
            var q = QueryBuilder.GetCommandText["Setup.Unit.Save"];
            var response = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return response;
        }
        public async Task<DbResponse> DeleteUnit(int id)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                     .AddMore("@CurrentUser", _currentUser.Id);

            var q = @"Update dbo.Unit Set IsDeleted = 1, ModifiedDate = GETDATE(), ModifiedBy = @CurrentUser Where Id = @Id";
            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }
        #endregion


        #region UnitType
        public async Task<List<UnitType>> GetUnitType(int id = 0, string name = "", string name_np = "")
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                    .AddMore("@Name", string.IsNullOrEmpty(name) ? "" : (object)name)
                                    .AddMore("@Name_Np", string.IsNullOrEmpty(name_np) ? "" : (object)name_np);

            var q = @"SELECT * FROM UnitType 
                      WHERE (@Id = 0 OR Id = @Id) AND (@Name = '' or Name like '%' + @Name + '%') AND (@Name_Np = '' or Name_Np Like '%' + @Name_Np + '%')  AND IsDeleted = 0";

            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return dt.TransformToList<UnitType>().ToList();
        }
        public async Task<DbResponse> SaveUnitType(UnitType model)
        {
            var p = model.PrepareSQLParameters();
            var q = QueryBuilder.GetCommandText["Setup.UnitType.Save"];
            var response = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return response;
        }
        public async Task<DbResponse> DeleteUnitType(int id)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                     .AddMore("@CurrentUser", _currentUser.Id);

            var q = @"Update dbo.UnitType Set IsDeleted = 1, ModifiedDate = GETDATE(), ModifiedBy = @CurrentUser Where Id = @Id";
            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }
        #endregion


        #endregion

    }
}
