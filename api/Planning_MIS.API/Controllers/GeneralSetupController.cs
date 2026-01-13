using Domain.Common;
using Domain.Entities.Project;
using Domain.Entities.Setup;
using Domain.Interfaces;
using Domain.ViewModel.GeneralSetup;
using Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Planning_MIS.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralSetupController : ControllerBase
    {
        private readonly IUnitOfWork _repo;
        private readonly ICurrentUserService _currentUser;

        public GeneralSetupController(IUnitOfWork repo, ICurrentUserService currentUser)
        {
            _repo = repo;
            _currentUser = currentUser;
        }


        #region Office Setup

        [HttpGet("office")]
        [Authorize(Policy = nameof(ePermission.OfficeSetup))]
        public async Task<IActionResult> GetOfficeInfo()
        {
            var office = await _repo.Setup.GetOfficeInfo();
            return office.Any() ? Ok(office.First()) : NotFound();
        }

        [HttpPost("office")]
        [Authorize(Policy = nameof(ePermission.OfficeSetup))]
        public async Task<IActionResult> CreateOffice([FromBody] OfficeInformation model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            model.CurrentUser = _currentUser.Id;
            var result = await _repo.Setup.SaveOfficeInfo(model);
            return Ok(result);
        }

        //[HttpPut("office/{id}")]
        //[Authorize(Policy = nameof(ePermission.OfficeSetup))]
        //public async Task<IActionResult> UpdateOffice(int id, [FromBody] OfficeInformation model)
        //{
        //    if (!ModelState.IsValid) return BadRequest(ModelState);

        //    var existingOffice = await _repo.Setup.GetOfficeInfo(id);
        //    if (existingOffice == null) return NotFound(); 

        //    model.CurrentUser = _currentUser.Id;
        //    var result = await _repo.Setup.SaveOfficeInfo(model);
        //    return Ok(result);
        //}

        #endregion


        #region FiscalYear Setup

        [HttpGet("fiscalyear")]
        [Authorize(Policy = nameof(ePermission.FiscalYearDetail))]
        public async Task<IActionResult> GetFiscalYears(int id = 0, string name = "", DateTime? currentDate = null)
        {
            var fiscalyear = await _repo.Setup.GetFiscalYear(id, name, currentDate);
            return fiscalyear.Any() ? Ok(fiscalyear) : NotFound();
        }

        [HttpGet("currentfiscalyear")]
        [Authorize(Policy = nameof(ePermission.FiscalYearDetail))]
        public async Task<IActionResult> GetCurrentFiscalYear()
        {
            var fiscalyear = await _repo.Setup.GetCurrentFiscalYear();
            return fiscalyear != null ? Ok(fiscalyear) : NotFound();
        }

        [HttpPost("fiscalyear")]
        [Authorize(Policy = nameof(ePermission.FiscalYearEntry))]
        public async Task<IActionResult> CreateFiscalYear([FromBody] FiscalYear model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            model.CurrentUser = _currentUser.Id;
            var result = await _repo.Setup.AddFiscalYear(model);
            return Ok(result);
        }

        [HttpPut("fiscalyear/{id}")]
        [Authorize(Policy = nameof(ePermission.FiscalYearEntry))]
        public async Task<IActionResult> UpdateFiscalYear(int id, [FromBody] FiscalYear model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingFiscalYear = await _repo.Setup.GetFiscalYear(id);
            if (existingFiscalYear == null) return NotFound();

            model.Id = id;
            model.CurrentUser = _currentUser.Id;

            var result = await _repo.Setup.AddFiscalYear(model);
            return Ok(result);
        }

        [HttpDelete("fiscalyear/{id}")]
        [Authorize(Policy = nameof(ePermission.FiscalYearEntry))]
        public async Task<IActionResult> DeleteFiscalYear(int id)
        {
            var result = await _repo.Setup.DeleteFiscalYear(id);
            return Ok(result);
        }

        #endregion


        #region Department Setup

        [HttpGet("department")]
        [Authorize(Policy = nameof(ePermission.DepartmentDetail))]
        public async Task<IActionResult> GetDepartments(int id = 0, string name = "", string alias = "" , bool getUserDepartmentonly = false, int userId = 0)
        {
            var departments = await _repo.Setup.GetDepartment(id, name, alias, getUserDepartmentonly, userId);
            return Ok(departments);
        }

        [HttpGet("department/{id}")]
        [Authorize(Policy = nameof(ePermission.DepartmentDetail))]
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            var department = await _repo.Setup.GetDepartmentById(id);
            return Ok(department);
        }

        [HttpPost("department")]
        [Authorize(Policy = nameof(ePermission.DepartmentEntry))]
        public async Task<IActionResult> CreateDepartment([FromBody] Department model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dbResponse = await _repo.Setup.AddDepartment(model);
            if (dbResponse.HasError)
                return BadRequest(dbResponse.Message);

            var departmentId = dbResponse.Response;
            return CreatedAtAction(nameof(GetDepartmentById), new {id = departmentId });
        }

        [HttpPut("department/{id}")]
        [Authorize(Policy = nameof(ePermission.DepartmentEntry))]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] Department model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing= (await _repo.Setup.GetDepartment(id)).FirstOrDefault();
            if (existing == null) return NotFound();

            model.CurrentUser = _currentUser.Id;

            var dbResponse = await _repo.Setup.AddDepartment(model);
            if (dbResponse.HasError)
                return BadRequest(dbResponse.Message);

            var departmentId = dbResponse.Response;
            return Ok(new { Message = "Department Updated!", DepartmentId = departmentId });
        }

        [HttpDelete("department/{id}")]
        [Authorize(Policy = nameof(ePermission.DepartmentEntry))]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var result = await _repo.Setup.DeleteDepartment(id);
            return Ok(result);
        }
        
        #endregion


        #region Designation Setup

        [HttpGet("designation")]
        [Authorize(Policy = nameof(ePermission.DesignationDetail))]
        public async Task<IActionResult> GetDesignation(int id = 0, string name = "", int type = 0)
        {
            var designationTable = await _repo.Setup.GetDesignation(id, name, type);
            var result = designationTable.Tables[1].TransformToList<Designation>();
            return Ok(result);
        }

        [HttpPost("designation")]
        [Authorize(Policy = nameof(ePermission.DesignationEntry))]
        public async Task<IActionResult> CreateDesignation([FromBody] Designation model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dbResponse = await _repo.Setup.AddDesignation(model);
            return Ok(dbResponse);
        }

        [HttpPut("designation/{id}")]
        [Authorize(Policy = nameof(ePermission.DesignationEntry))]
        public async Task<IActionResult> UpdateDesignation(int id, [FromBody] Designation model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = (await _repo.Setup.GetDesignationById(id));
            if (existing == null) return NotFound();

            model.Id = id;
            model.CurrentUser = _currentUser.Id;

            var dbResponse = await _repo.Setup.AddDesignation(model);
            return Ok(dbResponse);
        }

        [HttpDelete("designation/{id}")]
        [Authorize(Policy = nameof(ePermission.DesignationEntry))]
        public async Task<IActionResult> DeleteDesignation(int id)
        {
            var result = await _repo.Setup.DeleteDesignation(id);
            return Ok(result);
        }

        #endregion


        #region Public Representative

        [HttpGet("public-representative")]
        [Authorize(Policy = nameof(ePermission.PublicRepresentativeDetail))]
        public async Task<IActionResult> GetPublicRepresentatives(int id = 0, string name = "", int designationId = 0, int wardId = 0)
        {
            var result = await _repo.Setup.GetPublicRepresentative(id, name, designationId, wardId);
            return Ok(result);
        }

        [HttpGet("public-representative/{id}")]
        [Authorize(Policy = nameof(ePermission.PublicRepresentativeDetail))]
        public async Task<IActionResult> GetPublicRepresentativeById(int id)
        {
            var result = (await _repo.Setup.GetPublicRepresentative(id)).FirstOrDefault();
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost("public-representative")]
        public async Task<IActionResult> CreatePublicRepresentative([FromBody] PublicRepresentative model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            model.CurrentUser = _currentUser.Id;
            var result = await _repo.Setup.AddPublicRepresentative(model);
            return Ok(result);
        }

        [HttpPut("public-representative/{id}")]
        public async Task<IActionResult> UpdatePublicRepresentative(int id, [FromBody] PublicRepresentative model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = await _repo.Setup.GetPublicRepresentative(id);
            if (existing == null)
                return NotFound();

            model.Id = id;
            model.CurrentUser = _currentUser.Id;

            var result = await _repo.Setup.AddPublicRepresentative(model);
            return Ok(result);
        }

        [HttpDelete("public-representative/{id}")]
        [Authorize(Policy = nameof(ePermission.PublicRepresentativeEntry))]
        public async Task<IActionResult> DeletePublicRepresentative(int id)
        {
            var result = await _repo.Setup.DeletePublicRepresentative(id);
            return Ok(result);
        }
        
        #endregion


        #region Employee Setup


        [HttpGet("employee")]
        [Authorize(Policy = nameof(ePermission.EmployeeDetail))]
        public async Task<IActionResult> GetEmployees(int id = 0, string name = "", string mobileNo = "", int departmentId = 0, int designationId = 0)
        {
            var result = await _repo.Setup.GetEmployee(id, name, mobileNo, departmentId, designationId);
            return Ok(result);
        }


        [HttpGet("employee/{id}")]
        [Authorize(Policy = nameof(ePermission.EmployeeDetail))]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var model = (await _repo.Setup.GetEmployee(id)).FirstOrDefault();
            return model == null ? NotFound() : Ok(model);
        }


        [HttpPost("employee")]
        [Authorize(Policy = nameof(ePermission.EmployeeEntry))]
        public async Task<IActionResult> CreateEmployee([FromBody] Employee model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            model.CurrentUser = _currentUser.Id;

            var result = await _repo.Setup.AddEmployee(model);
            return Ok(result);
        }


        [HttpPut("employee/{id}")]
        [Authorize(Policy = nameof(ePermission.EmployeeEntry))]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] Employee model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = await _repo.Setup.GetEmployeeById(id);
            if (existing == null)
                return NotFound();

            model.Id = id;
            model.CurrentUser = _currentUser.Id;

            var result = await _repo.Setup.AddEmployee(model);
            return Ok(result);
        }


        [HttpDelete("employee/{id}")]
        [Authorize(Policy = nameof(ePermission.EmployeeEntry))]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            await _repo.Setup.DeleteEmployee(id);
            return Ok(new { message = "Deleted." });
        }
        #endregion


        #region Ward Setup

        [HttpGet("ward")]
        [Authorize(Policy = nameof(ePermission.WardDetail))]
        public async Task<IActionResult> GetWards(int id = 0, string name = "")
        {
            var result = await _repo.Setup.GetWard(id, name);
            return Ok(result);
        }


        [HttpGet("ward/{id}")]
        [Authorize(Policy = nameof(ePermission.WardDetail))]
        public async Task<IActionResult> GetWardById(int id)
        {
            var model = (await _repo.Setup.GetWard(id)).FirstOrDefault();
            return model == null ? NotFound() : Ok(model);
        }


        [HttpPost("ward")]
        [Authorize(Policy = nameof(ePermission.WardEntry))]
        public async Task<IActionResult> CreateWard([FromBody] Ward model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            model.CurrentUser = _currentUser.Id;

            var result = await _repo.Setup.AddWard(model);
            return Ok(result);
        }


        [HttpPut("ward/{id}")]
        [Authorize(Policy = nameof(ePermission.WardEntry))]
        public async Task<IActionResult> UpdateWard(int id, [FromBody] Ward model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = await _repo.Setup.GetWard(id);
            if (existing == null)
                return NotFound();

            model.Id = id;
            model.CurrentUser = _currentUser.Id;

            var result = await _repo.Setup.AddWard(model);
            return Ok(result);
        }

        [HttpDelete("ward/{id}")]
        [Authorize(Policy = nameof(ePermission.WardEntry))]
        public async Task<IActionResult> DeleteWard(int id)
        {
            await _repo.Setup.DeleteWard(id);
            return Ok(new { message = "Deleted." });
        }

        #endregion


        #region IncentiveRate Setup

        [HttpGet("visit-incentive-rate")]
        [Authorize(Policy = nameof(ePermission.IncentiveRateDetail))]
        public async Task<IActionResult> GetVisitIncentiveRate(int id = 0, int designationId = 0)
        {
            var result = await _repo.Setup.GetVisitIncentiveRate(id, designationId);
            return Ok(result);
        }

        [HttpPost("visit-incentive-rate")]
        [Authorize(Policy = nameof(ePermission.IncentiveRateEntry))]
        public async Task<IActionResult> CreateVisitIncentiveRate([FromBody] VisitIncentiveRate model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            model.CurrentUser = _currentUser.Id;

            var result = await _repo.Setup.AddVisitIncentiveRate(model);
            return Ok(result);
        }

        [HttpPost("visit-incentive-rate/{id}")]
        [Authorize(Policy = nameof(ePermission.IncentiveRateEntry))]
        public async Task<IActionResult> UpdateVisitIncentiveRate(int id, [FromBody] VisitIncentiveRate model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = await _repo.Setup.GetVisitIncentiveRate(id);
            if (existing == null)
                return NotFound();

            model.Id = id;
            model.CurrentUser = _currentUser.Id;

            var result = await _repo.Setup.AddVisitIncentiveRate(model);
            return Ok(result);
        }


        [HttpDelete("visit-incentive-rate/{id}")]
        [Authorize(Policy = nameof(ePermission.IncentiveRateEntry))]
        public async Task<IActionResult> DeleteVisitIncentiveRate(int id)
        {
            await _repo.Setup.DeleteVisitIncentiveRate(id);
            return Ok(new { message = "Deleted." });
        }

        #endregion


        #region ProjectClosing Setup

        [HttpGet("project-closing")]
        public async Task<IActionResult> GetProjectClosing()
        {
            var fiscalYears = await _repo.Setup.GetFiscalYear();
            var currentFY = await _repo.Setup.GetCurrentFiscalYear(); 
            
            var closingProjects = await _repo.Setup.GetProjectForClosing(currentFY.Id);

            return Ok(new
            {
                fiscalYearList = fiscalYears,
                projects = closingProjects
            });
        }

        [HttpPost("project-closing")]
        public async Task<IActionResult> SaveProjectClosing([FromBody] ProjectClosing model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _repo.Setup.SaveProjectClosing(model);
            return Ok(result);
        }

        #endregion

    }
}
