using Domain.Common;
using Domain.Entities.Setup;
using Domain.Interfaces;
using Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Planning_MIS.API.DocumentAPI;
using System.Data;

namespace Planning_MIS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterController : ControllerBase
    {
        private readonly IUnitOfWork _repo;
        private readonly DocumentAPIHelper _docAPIHelper;
        private readonly ICurrentUserService _currentUser;

        public MasterController(IUnitOfWork repo, DocumentAPIHelper docAPIHelper,
            ICurrentUserService currentUser)
        {
            _repo = repo;
            _docAPIHelper = docAPIHelper;
            _currentUser = currentUser;
        }

        #region Activity

        [HttpGet("activity")]
        [Authorize(Policy = nameof(ePermission.ActivityDetail))]
        public async Task<IActionResult> ActivityDetail(int id = 0, int activityGroupId = 0, string name = "", string code = "", string referenceCode = "")
        {
            var model = await _repo.Setup.GetActivity(id, activityGroupId, name, code, referenceCode);
            return Ok(model);
        }

        [HttpGet("activity/{id}")]
        [Authorize(Policy = nameof(ePermission.ActivityDetail))]
        public async Task<IActionResult> GetActivityById(int id)
        {
            var model = (await _repo.Setup.GetActivity(id)).FirstOrDefault();
            if (model == null) 
                return NotFound(new { message = "Activity not found" });

            return Ok(model);
        }

        [HttpPost("activity")]
        [Authorize(Policy = nameof(ePermission.ActivityEntry))]
        public async Task<IActionResult> CreateActivity([FromBody] Activity model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            model.CurrentUser = _currentUser.Id;
            var response = await _repo.Setup.SaveActivity(model);

            if (response.HasError)
                return BadRequest(response.Message);

            return Ok(new { message = "Activity Created" });
        }

        [HttpPut("activity/{id}")]
        [Authorize(Policy = nameof(ePermission.ActivityEntry))]
        public async Task<IActionResult> UpdateActivity(int id, [FromBody] Activity model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = (await _repo.Setup.GetActivity(id)).FirstOrDefault();
            if (existing == null) 
                return NotFound(new { message = "Activity not found" });

            model.Id = id;
            model.CurrentUser = _currentUser.Id;
            var response = await _repo.Setup.SaveActivity(model);

            if (response.HasError)
                return BadRequest(response.Message);

            return Ok(new { message = "Activity Updated" });
        }

        [HttpDelete("activity/{id}")]
        [Authorize(Policy = nameof(ePermission.ActivityEntry))]
        public async Task<IActionResult> DeleteActivity(int id)
        {
            var result = await _repo.Setup.DeleteActivity(id);

            if (result.HasError)
                return StatusCode(500, new { error = result.Message });
 
            if (result.Response != null && int.TryParse(result.Response.ToString(), out int rows) && rows > 0)
                return Ok(new { message = "Activity deleted" });

            return NotFound(new { message = "Activity not found" });  
        }


        #endregion


        #region Activity Group

        [HttpGet("activity-group")]
        [Authorize(Policy = nameof(ePermission.ActivityDetail))]
        public async Task<IActionResult> ActivityGroupDetails(int id = 0, string name = "", string code = "")
        {
            var model = await _repo.Setup.GetActivityGroup(id, name, code);
            return Ok(model);
        }

        [HttpGet("activity-group-dropdownlist")]
        [Authorize(Policy = nameof(ePermission.ActivityDetail))]
        public async Task<IActionResult> ActivityGroupDropdown()
        {
            var model = await _repo.Setup.GetActivityGroupDownList();
            return Ok(model);
        }

        [HttpPost("activity-group")]
        [Authorize(Policy = nameof(ePermission.ActivityEntry))]
        public async Task<IActionResult> CreateActivityGroup([FromBody] ActivityGroup model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            model.CurrentUser = _currentUser.Id;
            var response = await _repo.Setup.SaveActivityGroup(model);

            if (response.HasError)
                return BadRequest(response.Message);

            return Ok(new { message = "Activity Group Created" });
        }

        [HttpPut("activity-group/{id}")]
        [Authorize(Policy = nameof(ePermission.ActivityEntry))]
        public async Task<IActionResult> UpdateActivityGroup(int id, [FromBody] ActivityGroup model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = (await _repo.Setup.GetActivityGroup(id)).FirstOrDefault();
            if (existing == null)
                return NotFound(new { message = "Activity group not found" });

            model.Id = id;
            model.CurrentUser = _currentUser.Id;
            var response = await _repo.Setup.SaveActivityGroup(model);

            if (response.HasError)
                return BadRequest(response.Message);

            return Ok(new { message = "Activity Group Updated" });
        }

        [HttpDelete("activity-group/{id}")]
        [Authorize(Policy = nameof(ePermission.ActivityEntry))]
        public async Task<IActionResult> DeleteActivityGroup(int id)
        {
            var result = await _repo.Setup.DeleteActivity(id);

            if (result.HasError)
                return StatusCode(500, new { error = result.Message });

            if (result.Response != null && int.TryParse(result.Response.ToString(), out int rows) && rows > 0)
                return Ok(new { message = "Activity Group deleted" });

            return NotFound(new { message = "Activity Group not found" });
        }

        #endregion


        #region Bank

        [HttpGet("bank")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataDetail))]
        public async Task<IActionResult> BankDetail(int id = 0, string name = "")
        {
            var model = await _repo.Setup.GetBank(id, name);
            return Ok(model);
        }

        [HttpGet("bank/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataDetail))]
        public async Task<IActionResult> GetBankById(int id)
        {
            var model = (await _repo.Setup.GetBank(id)).FirstOrDefault();
            if (model == null)
                return NotFound(new { message = "Bank not found" });

            return Ok(model);
        }

        [HttpPost("bank")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> CreateBank([FromBody] Bank model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            model.CurrentUser = _currentUser.Id;
            var response = await _repo.Setup.SaveBank(model);

            if (response.HasError)
                return BadRequest(response.Message);

            return Ok(new { message = "Bank Created" });
        }

        [HttpPut("bank/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> UpdateBank(int id, [FromBody] Bank model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = (await _repo.Setup.GetBank(id)).FirstOrDefault();
            if (existing == null)
                return NotFound(new { message = "Bank not found" });

            model.Id = id;
            model.CurrentUser = _currentUser.Id;
            var response = await _repo.Setup.SaveBank(model);

            if (response.HasError)
                return BadRequest(response.Message);

            return Ok(new { message = "Bank Updated" });
        }

        [HttpDelete("bank/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> DeleteBank(int id)
        {
            var result = await _repo.Setup.DeleteBank(id);

            if (result.HasError)
                return StatusCode(500, new { error = result.Message });

            if (result.Response != null && int.TryParse(result.Response.ToString(), out int rows) && rows > 0)
                return Ok(new { message = "Bank deleted" });

            return NotFound(new { message = "Bank not found" });
        }

        #endregion


        #region Beneficiary
        [HttpGet("beneficiary")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataDetail))]
        public async Task<IActionResult> BeneficiaryDetail(int id = 0, string name = "")
        {
            var model = await _repo.Setup.GetBeneficiary(id, name);
            return Ok(model);
        }

        [HttpGet("beneficiary/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataDetail))]
        public async Task<IActionResult> GetBeneficiaryById(int id)
        {
            var model = (await _repo.Setup.GetBeneficiary(id)).FirstOrDefault();
            if (model == null)
                return NotFound(new { message = "Beneficiary not found" });

            return Ok(model);
        }

        [HttpPost("beneficiary")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> CreateBeneficiary([FromBody] Beneficiary model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            model.CurrentUser = _currentUser.Id;
            var response = await _repo.Setup.SaveBeneficiary(model);

            if (response.HasError)
                return BadRequest(response.Message);

            return Ok(new { message = "Beneficiary Created" });
        }

        [HttpPut("beneficiary/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> UpdateBeneficiary(int id, [FromBody] Beneficiary model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = (await _repo.Setup.GetBeneficiary(id)).FirstOrDefault();
            if (existing == null)
                return NotFound(new { message = "Beneficiary not found" });

            model.Id = id;
            model.CurrentUser = _currentUser.Id;
            var response = await _repo.Setup.SaveBeneficiary(model);

            if (response.HasError)
                return BadRequest(response.Message);

            return Ok(new { message = "Beneficiary Updated" });
        }

        [HttpDelete("beneficiary/{id}")]

        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> DeleteBeneficiary(int id)
        {
            var result = await _repo.Setup.DeleteBeneficiary(id);

            if (result.HasError)
                return StatusCode(500, new { error = result.Message });

            if (result.Response != null && int.TryParse(result.Response.ToString(), out int rows) && rows > 0)
                return Ok(new { message = "Beneficiary deleted" });

            return NotFound(new { message = "Beneficiary not found" });
        }

        #endregion


        #region Budget


        #region BudgetAllocation



        #endregion


        #region BudgetAllocationProject


        #endregion


        #region Budget Source



        #endregion


        #region BudgetSubTitle



        #endregion


        #endregion


        #region Checklist

        [HttpGet("checklist")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataDetail))]
        public async Task<IActionResult> ChecklistDetails(int id = 0, string name = "")
        {
            var model = await _repo.Setup.GetChecklist(id, name);
            return Ok(model);
        }

        [HttpGet("checklist/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataDetail))]
        public async Task<IActionResult> GetChecklistById(int id)
        {
            var model = (await _repo.Setup.GetChecklist(id)).FirstOrDefault();
            if (model == null)
                return NotFound(new { message = "Checklist not found" });

            return Ok(model);
        }

        [HttpPost("checklist")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> CreateChecklist([FromBody] Checklist model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            model.CurrentUser = _currentUser.Id;
            var response = await _repo.Setup.SaveChecklist(model);

            if (response.HasError)
                return BadRequest(response.Message);

            return Ok(new { message = "checklist Created" });
        }

        [HttpPut("checklist/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> UpdateChecklist(int id, [FromBody] Checklist model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = (await _repo.Setup.GetBank(id)).FirstOrDefault();
            if (existing == null)
                return NotFound(new { message = "Checklist not found" });

            model.Id = id;
            model.CurrentUser = _currentUser.Id;
            var response = await _repo.Setup.SaveChecklist(model);

            if (response.HasError)
                return BadRequest(response.Message);

            return Ok(new { message = "Checklist Updated" });
        }

        [HttpDelete("checklist/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> DeleteChecklist(int id)
        {
            var result = await _repo.Setup.DeleteChecklist(id);

            if (result.HasError)
                return StatusCode(500, new { error = result.Message });

            if (result.Response != null && int.TryParse(result.Response.ToString(), out int rows) && rows > 0)
                return Ok(new { message = "Checklist deleted" });

            return NotFound(new { message = "Checklist not found" });
        }

        #endregion


        #region Configuration

        [HttpGet("configuration")]
        [Authorize(Policy = nameof(ePermission.ConfigurationEnry))]
        public async Task<IActionResult> ConfigurationDetail(int id = 0, string name = "")
        {
            var model = await _repo.Setup.GetConfiguration(id, name);
            return Ok(model);
        }

        [HttpGet("configuration/{id}")]
        [Authorize(Policy = nameof(ePermission.ConfigurationEnry))]
        public async Task<IActionResult> GetConfigurationById(int id)
        {
            var model = (await _repo.Setup.GetConfiguration(id)).FirstOrDefault();
            if (model == null)
                return NotFound(new { message = "Configuration not found" });

            return Ok(model);
        }

        [HttpPost("configuration")]
        [Authorize(Policy = nameof(ePermission.ConfigurationEnry))]
        public async Task<IActionResult> CreateConfiguration([FromBody] Configuration model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            model.CurrentUser = _currentUser.Id;
            var response = await _repo.Setup.SaveConfiguration(model);

            if (response.HasError)
                return BadRequest(response.Message);

            return Ok(new { message = "Configuration Created" });
        }

        [HttpPut("configuration/{id}")]
        [Authorize(Policy = nameof(ePermission.ConfigurationEnry))]
        public async Task<IActionResult> UpdateConfiguration(int id, [FromBody] Configuration model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = (await _repo.Setup.GetBank(id)).FirstOrDefault();
            if (existing == null)
                return NotFound(new { message = "Configuration not found" });

            model.Id = id;
            model.CurrentUser = _currentUser.Id;
            var response = await _repo.Setup.SaveConfiguration(model);

            if (response.HasError)
                return BadRequest(response.Message);

            return Ok(new { message = "Configuration Updated" });
        }

        [HttpDelete("configuration/{id}")]
        [Authorize(Policy = nameof(ePermission.ConfigurationEnry))]
        public async Task<IActionResult> DeleteConfiguration(int id)
        {
            var result = await _repo.Setup.DeleteBank(id);

            if (result.HasError)
                return StatusCode(500, new { error = result.Message });

            if (result.Response != null && int.TryParse(result.Response.ToString(), out int rows) && rows > 0)
                return Ok(new { message = "Configuration deleted" });

            return NotFound(new { message = "Configuration not found" });
        }

        #endregion


        #region DonorType

        [HttpGet("donortype")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataDetail))]
        public async Task<IActionResult> DonorTypeDetail(int id = 0, string name = "")
        {
            var model = await _repo.Setup.GetDonorType(id, name);
            return Ok(model);
        }

        [HttpGet("donortype/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataDetail))]
        public async Task<IActionResult> GetDonorTypeById(int id)
        {
            var model = (await _repo.Setup.GetDonorType(id)).FirstOrDefault();
            if (model == null)
                return NotFound(new { message = "DonorType not found" });

            return Ok(model);
        }

        [HttpPost("donortype")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> CreateDonorType([FromBody] DonorType model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            //model.CurrentUser = _currentUser.Id; //added directly to repo

            var response = await _repo.Setup.SaveDonorType(model);

            if (response.HasError)
                return BadRequest(response.Message);

            return Ok(new { message = "DonorType Created" });
        }

        [HttpPut("donortype/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> UpdateDonorType(int id, [FromBody] DonorType model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = (await _repo.Setup.GetDonorType(id)).FirstOrDefault();
            if (existing == null)
                return NotFound(new { message = "DonorType not found" });

            model.Id = id;
            //model.CurrentUser = _currentUser.Id;
            var response = await _repo.Setup.SaveDonorType(model);

            if (response.HasError)
                return BadRequest(response.Message);

            return Ok(new { message = "DonorType Updated" });
        }

        [HttpDelete("donortype/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> DeleteDonorType(int id)
        {
            var result = await _repo.Setup.DeleteDonorType(id);

            if (result.HasError)
                return StatusCode(500, new { error = result.Message });

            if (result.Response != null && int.TryParse(result.Response.ToString(), out int rows) && rows > 0)
                return Ok(new { message = "DonorType deleted" });

            return NotFound(new { message = "DonorType not found" });
        }

        #endregion


        #region ExpensesHead

        [HttpGet("expenseshead")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataDetail))]
        public async Task<IActionResult> ExpensesHeadDetail(int id = 0, string name = "")
        {
            var model = await _repo.Setup.GetExpensesHead(id, name);
            return Ok(model);
        }

        [HttpGet("expenseshead/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataDetail))]
        public async Task<IActionResult> GetExpensesHeadById(int id)
        {
            var model = await _repo.Setup.GetExpensesHeadById(id);
            if (model == null)
                return NotFound(new { message = "ExpensesHead not found" });

            return Ok(model);
        }

        [HttpPost("expenseshead")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> CreateExpensesHead([FromBody] ExpensesHead model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            model.CurrentUser = _currentUser.Id;
            var response = await _repo.Setup.SaveExpensesHead(model);

            if (response.HasError)
                return BadRequest(response.Message);

            return Ok(new { message = "ExpensesHead Created" });
        }

        [HttpPut("expenseshead/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> UpdateExpensesHead(int id, [FromBody] ExpensesHead model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = await _repo.Setup.GetExpensesHeadById(id);
            if (existing == null)
                return NotFound(new { message = "ExpensesHead not found" });

            model.Id = id;
            model.CurrentUser = _currentUser.Id;
            var response = await _repo.Setup.SaveExpensesHead(model);

            if (response.HasError)
                return BadRequest(response.Message);

            return Ok(new { message = "ExpensesHead Updated" });
        }

        [HttpDelete("expenseshead/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> DeleteExpensesHead(int id)
        {
            var result = await _repo.Setup.DeleteExpensesHead(id);

            if (result.HasError)
                return StatusCode(500, new { error = result.Message });

            if (result.Response != null && int.TryParse(result.Response.ToString(), out int rows) && rows > 0)
                return Ok(new { message = "ExpensesHead deleted" });

            return NotFound(new { message = "ExpensesHead not found" });
        }

        #endregion


        #region ItemType

        [HttpGet("itemtype")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataDetail))]
        public async Task<IActionResult> ItemTypeDetail(int id = 0, string name = "")
        {
            var model = await _repo.Setup.GetItemType(id, name);
            return Ok(model);
        }

        [HttpGet("itemtype/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataDetail))]
        public async Task<IActionResult> GetItemTypeById(int id)
        {
            var model = (await _repo.Setup.GetBank(id)).FirstOrDefault();
            if (model == null)
                return NotFound(new { message = "ItemType not found" });

            return Ok(model);
        }

        [HttpPost("itemtype")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> CreateItemType([FromBody] ItemType model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            model.CurrentUser = _currentUser.Id;
            var response = await _repo.Setup.SaveItemType(model);

            if (response.HasError)
                return BadRequest(response.Message);

            return Ok(new { message = "ItemType Created" });
        }

        [HttpPut("itemtype/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> UpdateItemType(int id, [FromBody] ItemType model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = (await _repo.Setup.GetItemType(id)).FirstOrDefault();
            if (existing == null)
                return NotFound(new { message = "ItemType not found" });

            model.Id = id;
            model.CurrentUser = _currentUser.Id;
            var response = await _repo.Setup.SaveItemType(model);

            if (response.HasError)
                return BadRequest(response.Message);

            return Ok(new { message = "ItemType Updated" });
        }

        [HttpDelete("itemtype/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> DeleteItemType(int id)
        {
            var result = await _repo.Setup.DeleteItemType(id);

            if (result.HasError)
                return StatusCode(500, new { error = result.Message });

            if (result.Response != null && int.TryParse(result.Response.ToString(), out int rows) && rows > 0)
                return Ok(new { message = "ItemType deleted" });

            return NotFound(new { message = "ItemType not found" });
        }

        #endregion


        #region Item


        [HttpGet("item")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataDetail))]
        public async Task<IActionResult> ItemDetail(int id = 0, string name = "")
        {
            var currFyId = _currentUser.FiscalYearId;
            var model = await _repo.Setup.GetItem(id, 0, name: name, fiscalYearId: currFyId);
            return Ok(model);
        }

        [HttpGet("item/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataDetail))]
        public async Task<IActionResult> GetItemById(int id)
        {
           var currFyId = _currentUser.FiscalYearId;
            var model = (await _repo.Setup.GetItem(id, fiscalYearId: currFyId)).FirstOrDefault();
            if (model == null)
                return NotFound(new { message = "Item not found" });

            return Ok(model);
        }

        [HttpPost("item")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> CreateItem([FromBody] Item model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            model.CurrentUser = _currentUser.Id;
            var response = await _repo.Setup.SaveItem(model);

            if (response.HasError)
                return BadRequest(response.Message);

            return Ok(new { message = "Item Created" });
        }

        [HttpPut("item/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> UpdateItem(int id, [FromBody] Item model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = (await _repo.Setup.GetItem(id)).FirstOrDefault();
            if (existing == null)
                return NotFound(new { message = "Item not found" });

            model.Id = id;
            model.CurrentUser = _currentUser.Id;
            var response = await _repo.Setup.SaveItem(model);

            if (response.HasError)
                return BadRequest(response.Message);

            return Ok(new { message = "Item Updated" });
        }

        [HttpDelete("item/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var result = await _repo.Setup.DeleteItem(id);

            if (result.HasError)
                return StatusCode(500, new { error = result.Message });

            if (result.Response != null && int.TryParse(result.Response.ToString(), out int rows) && rows > 0)
                return Ok(new { message = "Item deleted" });

            return NotFound(new { message = "Item not found" });
        }


        #endregion


        #region ItemRate

        [HttpGet("item-rate")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataDetail))]
        public async Task<IActionResult> ItemRateDetail(int id = 0, string name = "")
        {
            var currFyId =_currentUser.FiscalYearId;
            var model = await _repo.Setup.GetItemRate(currFyId, id, name);
            return Ok(model);
        }

        [HttpGet("item-rate/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataDetail))]
        public async Task<IActionResult> GetItemRateById(int id)
        {
            var model = (await _repo.Setup.GetItemRate(id)).FirstOrDefault();
            if (model == null)
                return NotFound(new { message = "ItemRate not found" });

            return Ok(model);
        }

        [HttpPost("item-rate")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> CreateItemRate([FromBody] List<ItemRate> model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var currentUserId = _currentUser.Id;
            var fiscalYearId = _currentUser.FiscalYearId;

            foreach (var item in model)
            {          
                item.FiscalYearId = fiscalYearId;
                item.CurrentUser = currentUserId;
            }
            var response = await _repo.Setup.SaveItemRate(model);

            if (response.HasError)
                return BadRequest(response.Message);

            return Ok(new { message = "ItemRate Created" });
        }

        [HttpPut("item-rate/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> UpdateItemRate(int id, [FromBody] List<ItemRate> model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = (await _repo.Setup.GetItemRate(id)).FirstOrDefault();
            if (existing == null)
                return NotFound(new { message = "ItemRate not found" });

            foreach (var item in model)
            {
                item.Id = id;
                //item.FiscalYearId = existing.FiscalYearId;
                item.CurrentUser = _currentUser.Id;
            }
            var response = await _repo.Setup.SaveItemRate(model);

            if (response.HasError)
                return BadRequest(response.Message);

            return Ok(new { message = "ItemRate Updated" });
        }

        [HttpDelete("item-rate/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> DeleteItemRate(int id)
        {
            var result = await _repo.Setup.DeleteItemRate(id);

            if (result.HasError)
                return StatusCode(500, new { error = result.Message });

            if (result.Response != null && int.TryParse(result.Response.ToString(), out int rows) && rows > 0)
                return Ok(new { message = "ItemRate deleted" });

            return NotFound(new { message = "ItemRate not found" });
        }

        #endregion


        #region ImplementationModel
        [HttpGet("implementationmodel")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataDetail))]
        public async Task<IActionResult> ImplementationModelDetail(int id = 0, int projectModelId = 0, string name = "")
        {
            var model = await _repo.Setup.GetImplementationModel(id, projectModelId, name);
            return Ok(model);
        }

        [HttpGet("implementationmodel/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataDetail))]
        public async Task<IActionResult> GetImplementationModelById(int id)
        {
            var model = (await _repo.Setup.GetImplementationModel(id: id)).FirstOrDefault();
            if (model == null)
                return NotFound(new { message = "ImplementationModel not found" });

            return Ok(model);
        }

        [HttpPost("implementationmodel")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> CreateImplementationModel([FromBody] ImplementationModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            model.CurrentUser = _currentUser.Id;
            var response = await _repo.Setup.SaveImplementationModel(model);

            if (response.HasError)
                return BadRequest(response.Message);

            return Ok(new { message = "ImplementationModel Created" });
        }

        [HttpPut("implementationmodel/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> UpdateImplementationModel(int id, [FromBody] ImplementationModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = (await _repo.Setup.GetImplementationModel(id)).FirstOrDefault();
            if (existing == null)
                return NotFound(new { message = "ImplementationModel not found" });

            model.Id = id;
            model.CurrentUser = _currentUser.Id;
            var response = await _repo.Setup.SaveImplementationModel(model);

            if (response.HasError)
                return BadRequest(response.Message);

            return Ok(new { message = "ImplementationModel Updated" });
        }

        [HttpDelete("implementationmodel/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> DeleteImplementationModel(int id)
        {
            var result = await _repo.Setup.DeleteImplementationModel(id);

            if (result.HasError)
                return StatusCode(500, new { error = result.Message });

            if (result.Response != null && int.TryParse(result.Response.ToString(), out int rows) && rows > 0)
                return Ok(new { message = "ImplementationModel deleted" });

            return NotFound(new { message = "ImplementationModel not found" });
        }


        #endregion


        #region ImplementationLevel

        [HttpGet("implementationlevel")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataDetail))]
        public async Task<IActionResult> ImplementationLevelDetail(int id = 0, string name = "")
        {
            var model = await _repo.Setup.GetImplementationLevel(id, name);
            return Ok(model);
        }

        [HttpGet("implementationlevel/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataDetail))]
        public async Task<IActionResult> GetImplementationLevelById(int id)
        {
            var model = (await _repo.Setup.GetImplementationLevel(id)).FirstOrDefault();
            if (model == null)
                return NotFound(new { message = "ImplementationLevel not found" });

            return Ok(model);
        }

        [HttpPost("implementationlevel")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> CreateImplementationLevel([FromBody] ImplementationLevel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            model.CurrentUser = _currentUser.Id;
            var response = await _repo.Setup.SaveImplementationLevel(model);

            if (response.HasError)
                return BadRequest(response.Message);

            return Ok(new { message = "ImplementationLevel Created" });
        }

        [HttpPut("implementationlevel/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> UpdateImplementationLevel(int id, [FromBody] ImplementationLevel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = (await _repo.Setup.GetImplementationLevel(id)).FirstOrDefault();
            if (existing == null)
                return NotFound(new { message = "ImplementationLevel not found" });

            model.Id = id;
            model.CurrentUser = _currentUser.Id;
            var response = await _repo.Setup.SaveImplementationLevel(model);

            if (response.HasError)
                return BadRequest(response.Message);

            return Ok(new { message = "ImplementationLevel Updated" });
        }

        [HttpDelete("implementationlevel/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> DeleteImplementationLevel(int id)
        {
            var result = await _repo.Setup.DeleteBank(id);

            if (result.HasError)
                return StatusCode(500, new { error = result.Message });

            if (result.Response != null && int.TryParse(result.Response.ToString(), out int rows) && rows > 0)
                return Ok(new { message = "ImplementationLevel deleted" });

            return NotFound(new { message = "ImplementationLevel not found" });
        }
        #endregion


        #region LetterFormat


        [HttpGet("letterformat")]
        [Authorize(Policy = nameof(ePermission.LetterFormatDetail))]
        public async Task<IActionResult> LetterFormatDetail(int id = 0)
        {
            var model = await _repo.Setup.GetLetterFormat(id);
            return Ok(model);
        }

        [HttpGet("letterformat/{id}")]
        [Authorize(Policy = nameof(ePermission.LetterFormatDetail))]
        public async Task<IActionResult> GetLetterFormatById(int id)
        {
            var model = (await _repo.Setup.GetLetterFormat(id)).FirstOrDefault();
            if (model == null)
                return NotFound(new { message = "LetterFormat not found" });

            return Ok(model);
        }

        [HttpPost("letterformat")]
        [Authorize(Policy = nameof(ePermission.LetterFormatEntry))]
        public async Task<IActionResult> CreateLetterFormat([FromBody] LetterFormat model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            //model.CurrentUser = _currentUser.Id;
            var response = await _repo.Setup.SaveLetterFormat(model);

            if (response.HasError)
                return BadRequest(response.Message);

            return Ok(new { message = "LetterFormat Created" });
        }

        [HttpPut("letterformat/{id}")]
        [Authorize(Policy = nameof(ePermission.LetterFormatEntry))]
        public async Task<IActionResult> UpdateLetterFormat(int id, [FromBody] LetterFormat model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = (await _repo.Setup.GetLetterFormat(id)).FirstOrDefault();
            if (existing == null)
                return NotFound(new { message = "LetterFormat not found" });

            model.Id = id;
            //model.CurrentUser = _currentUser.Id;
            var response = await _repo.Setup.SaveLetterFormat(model);

            if (response.HasError)
                return BadRequest(response.Message);

            return Ok(new { message = "LetterFormat Updated" });
        }

        [HttpDelete("letterformat/{id}")]
        [Authorize(Policy = nameof(ePermission.LetterFormatEntry))]
        public async Task<IActionResult> DeleteLetterFormat(int id)
        {
            var result = await _repo.Setup.DeleteLetterFormat(id);

            if (result.HasError)
                return StatusCode(500, new { error = result.Message });

            if (result.Response != null && int.TryParse(result.Response.ToString(), out int rows) && rows > 0)
                return Ok(new { message = "LetterFormat deleted" });

            return NotFound(new { message = "LetterFormat not found" });
        }

        #endregion


        #region Norms

        [HttpGet("norms/{activityId}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataDetail))]
        public async Task<IActionResult> GetNormsDetail(int activityId)
        {
            int fiscalYearId = _currentUser.FiscalYearId;

            var activity = (await _repo.Setup.GetActivity(activityId)).FirstOrDefault();
            var activityRate = await _repo.Setup.GetActivityRate(activityId, fiscalYearId);

            if (activity == null)
                return NotFound(new { message = "Activity not found" });

            var norms = await _repo.Setup.GetNorms(activityId, fiscalYearId: fiscalYearId);

            var response = new
            {
                activityId = activityId,
                activityName = activity.Name,
                activityGroup = activity.ActivityGroup,
                activityUnit = activity.Unit,
                qtyFor = activity.QtyFor,
                items = await _repo.Setup.GetItem(fiscalYearId: fiscalYearId),
                activityRateId = activityRate?.Id ?? 0,
                totalAmount = activityRate?.TotalAmount ?? 0,
                ratePerUnit = activityRate?.RatePerUnit ?? 0,

                norms = norms
            };

            return Ok(response);
        }

        [HttpPost("norms")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> CreateNorms([FromBody] List<Norms> norms)
        {
            if (norms == null || !norms.Any())
                return BadRequest(new { message = "No norms data provided" });

            if (!ModelState.IsValid) return BadRequest(ModelState);

            foreach (var item in norms) {

                item.CurrentUser = _currentUser.Id;
                item.FiscalYearId = _currentUser.FiscalYearId;
                //item.ActivityRateId = norms.First().ActivityRateId;
                //item.ActivityId = norms.First().ActivityId;
            }

            var result = await _repo.Setup.SaveNorms(norms);

            if (result.HasError)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = "Norms Created" });
             
        }
        

        #endregion


        #region NormsType

        [HttpGet("norms-type")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataDetail))]
        public async Task<IActionResult> NormsTypeDetail(int id = 0, string name = "")
        {
            var model = await _repo.Setup.GetBank(id, name);
            return Ok(model);
        }

        [HttpGet("norms-type/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataDetail))]
        public async Task<IActionResult> GetNormsTypeById(int id)
        {
            var model = (await _repo.Setup.GetNormsType(id)).FirstOrDefault();
            if (model == null)
                return NotFound(new { message = "NormsType not found" });

            return Ok(model);
        }

        [HttpPost("norms-type")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> CreateNormsType([FromBody] NormsType model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            model.CurrentUser = _currentUser.Id;
            var response = await _repo.Setup.SaveNormsType(model);

            if (response.HasError)
                return BadRequest(response.Message);

            return Ok(new { message = "NormsType Created" });
        }

        [HttpPut("norms-type/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> UpdateNormsType(int id, [FromBody] NormsType model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = (await _repo.Setup.GetNormsType(id)).FirstOrDefault();
            if (existing == null)
                return NotFound(new { message = "NormsType not found" });

            model.Id = id;
            model.CurrentUser = _currentUser.Id;
            var response = await _repo.Setup.SaveNormsType(model);

            if (response.HasError)
                return BadRequest(response.Message);

            return Ok(new { message = "NormsType Updated" });
        }

        [HttpDelete("norms-type/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> DeleteNormsType(int id)
        {
            var result = await _repo.Setup.DeleteNormsType(id);

            if (result.HasError)
                return StatusCode(500, new { error = result.Message });

            if (result.Response != null && int.TryParse(result.Response.ToString(), out int rows) && rows > 0)
                return Ok(new { message = "NormsType deleted" });

            return NotFound(new { message = "NormsType not found" });
        }
        #endregion


        #region ProgressIndicator

        [HttpGet("progressindicator")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataDetail))]
        public async Task<IActionResult> ProgressIndicatorDetail(int id = 0, string name = "", string code = "")
        {
            var model = await _repo.Setup.GetProgressIndicator(id, name, code);
            return Ok(model);
        }

        [HttpGet("progressindicator/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataDetail))]
        public async Task<IActionResult> GetProgressIndicatorById(int id)
        {
            var model = (await _repo.Setup.GetProgressIndicator(id)).FirstOrDefault();
            if (model == null)
                return NotFound(new { message = "ProgressIndicator not found" });

            return Ok(model);
        }

        [HttpPost("progressindicator")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> CreateProgressIndicator([FromBody] ProgressIndicator model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            model.CurrentUser = _currentUser.Id;
            var dbResponse = await _repo.Setup.SaveProgressIndicator(model);

            if (dbResponse.HasError)
                return BadRequest(dbResponse.Message);

            if (dbResponse.Response != null)
            {
                var id = Convert.ToInt32(dbResponse.Response);
                return Ok(new { message = "ProgressIndicator Created", id = id });
            }

            return BadRequest(new { message = "Failed to create. No ID returned." });
        }

        [HttpPut("progressindicator/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> UpdateProgressIndicator(int id, [FromBody] ProgressIndicator model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = (await _repo.Setup.GetProgressIndicator(id)).FirstOrDefault();
            if (existing == null)
                return NotFound(new { message = "ProgressIndicator not found" });

            model.Id = id;
            model.CurrentUser = _currentUser.Id;
            var response = await _repo.Setup.SaveProgressIndicator(model);

            if (response.HasError)
                return BadRequest(response.Message);

            return Ok(new { message = "ProgressIndicator Updated"});
        }

        [HttpDelete("progressindicator/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> DeleteProgressIndicator(int id)
        {
            var result = await _repo.Setup.DeleteProgressIndicator(id);

            if (result.HasError)
                return StatusCode(500, new { error = result.Message });

            if (result.Response != null && int.TryParse(result.Response.ToString(), out int rows) && rows > 0)
                return Ok(new { message = "ProgressIndicator deleted" });

            return NotFound(new { message = "ProgressIndicator not found" });
        }

        #endregion


        #region ProjectGroup

        [HttpGet("projectgroup")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataDetail))]
        public async Task<IActionResult> ProjectGroupDetail(int id = 0, string name = "")
        {
            var model = await _repo.Setup.GetProjectGroup(id, name);
            return Ok(model);
        }

        [HttpGet("projectgroup/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataDetail))]
        public async Task<IActionResult> GetProjectGroupById(int id)
        {
            var model = await _repo.Setup.GetProjectGroupById(id);
            if (model == null)
                return NotFound(new { message = "ProjectGroup not found" });

            return Ok(model);
        }

        [HttpGet("projectgroup/dropdown")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataDetail))]
        public async Task<IActionResult> GetProjectGroupDropdown()
        {
            var list = await _repo.Setup.GetDropDown("ProjectGroup");
            return Ok(list);
        }

        [HttpPost("projectgroup")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> CreateProjectGroup([FromBody] ProjectGroup model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (model.Id == model.ParentId)
            {
                return BadRequest(new
                {
                    error = "योजना ग्रुप र मुख्य ग्रुप एउटै हुन सक्दैन । कृपया अर्को ग्रुप छान्नुहोस् ।"
                });
            }

            model.CurrentUser = _currentUser.Id;
            var response = await _repo.Setup.SaveProjectGroup(model);

            if (response.HasError)
                return BadRequest(response.Message);

            return Ok(new { message = "ProjectGroup Created" });
        }

        [HttpPut("projectgroup/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> UpdateProjectGroup(int id, [FromBody] ProjectGroup model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = await _repo.Setup.GetProjectGroupById(id);
            if (existing == null)
                return NotFound(new { message = "ProjectGroup not found" });

            if (model.Id == model.ParentId)
            {
                return BadRequest(new
                {
                    error = "योजना ग्रुप र मुख्य ग्रुप एउटै हुन सक्दैन । कृपया अर्को ग्रुप छान्नुहोस् ।"
                });
            }

            model.Id = id;
            model.CurrentUser = _currentUser.Id;
            var response = await _repo.Setup.SaveProjectGroup(model);

            if (response.HasError)
                return BadRequest(response.Message);

            return Ok(new { message = "ProjectGroup Updated" });
        }

        [HttpDelete("projectgroup/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> DeleteProjectGroup(int id)
        {
            var result = await _repo.Setup.DeleteProjectGroup(id);

            if (result.HasError)
                return StatusCode(500, new { error = result.Message });

            if (result.Response != null && int.TryParse(result.Response.ToString(), out int rows) && rows > 0)
                return Ok(new { message = "ProjectGroup deleted" });

            return NotFound(new { message = "ProjectGroup not found" });
        }

        [HttpGet("projectgroup/sector/{sectorId}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataDetail))]
        public async Task<IActionResult> GetProjectGroupBySector(int sectorId = 0)
        {
            var projectgroup = await _repo.Setup.GetProjectGroupDownList();

            var parentList = projectgroup
                .Where(x => x.ParentId.HasValue).Select(x => x.ParentId.Value).ToList();

            List<ProjectGroup> list = new();

            foreach (var item in projectgroup.Where(x => !parentList.Contains(x.Id)))
            {
                item.DisplayName =
                    string.Join(" > ", _repo.Setup.GetProjectGroupDisplayName(projectgroup, item.Id));
                list.Add(item);
            }
            var result = list.Where(x => x.SectorId == sectorId);

            return Ok(result);
        }

        #endregion


        #region Sector

        [HttpGet("sector")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataDetail))]
        public async Task<IActionResult> SectorDetail(int id = 0, string name = "", string code = "")
        {
            var model = await _repo.Setup.GetSector(id, name, code);
            return Ok(model);
        }

        [HttpGet("sector/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataDetail))]
        public async Task<IActionResult> GetSectorById(int id)
        {
            var model = (await _repo.Setup.GetSector(id)).FirstOrDefault();
            if (model == null)
                return NotFound(new { message = "Sector not found" });

            return Ok(model);
        }

        [HttpPost("sector")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> CreateSector([FromBody] Sector model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            model.CurrentUser = _currentUser.Id;
            var response = await _repo.Setup.SaveSector(model);

            if (response.HasError)
                return BadRequest(response.Message);

            return Ok(new { message = "Sector Created" });
        }

        [HttpPut("sector/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> UpdateSector(int id, [FromBody] Sector model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = (await _repo.Setup.GetSector(id)).FirstOrDefault();
            if (existing == null)
                return NotFound(new { message = "Sector not found" });

            model.Id = id;
            model.CurrentUser = _currentUser.Id;
            var response = await _repo.Setup.SaveSector(model);

            if (response.HasError)
                return BadRequest(response.Message);

            return Ok(new { message = "Sector Updated" });
        }

        [HttpDelete("sector/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> DeleteSector(int id)
        {
            var result = await _repo.Setup.DeleteSector(id);

            if (result.HasError)
                return StatusCode(500, new { error = result.Message });

            if (result.Response != null && int.TryParse(result.Response.ToString(), out int rows) && rows > 0)
                return Ok(new { message = "Sector deleted" });

            return NotFound(new { message = "Sector not found" });
        }

        #endregion


        #region SubSector

        [HttpGet("subsector")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataDetail))]
        public async Task<IActionResult> SubSectorDetail(int id = 0, int sectorId = 0, string name = "", string code = "")
        {
            var model = await _repo.Setup.GetSubSector(id, sectorId, name, code);
            return Ok(model);
        }

        [HttpGet("subsector/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataDetail))]
        public async Task<IActionResult> GetSubSectorById(int id)
        {
            var model = (await _repo.Setup.GetSubSector(id)).FirstOrDefault();
            if (model == null)
                return NotFound(new { message = "SubSector not found" });

            return Ok(model);
        }

        [HttpPost("subsector")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> CreateSubSector([FromBody] SubSector model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            model.CurrentUser = _currentUser.Id;
            var response = await _repo.Setup.SaveSubSector(model);

            if (response.HasError)
                return BadRequest(response.Message);

            return Ok(new { message = "SubSector Created" });
        }

        [HttpPut("subsector/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> UpdateSubSector(int id, [FromBody] SubSector model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = (await _repo.Setup.GetSubSector(id)).FirstOrDefault();
            if (existing == null)
                return NotFound(new { message = "SubSector not found" });

            model.Id = id;
            model.CurrentUser = _currentUser.Id;
            var response = await _repo.Setup.SaveSubSector(model);

            if (response.HasError)
                return BadRequest(response.Message);

            return Ok(new { message = "SubSector Updated" });
        }

        [HttpDelete("subsector/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> DeleteSubSector(int id)
        {
            var result = await _repo.Setup.DeleteSubSector(id);

            if (result.HasError)
                return StatusCode(500, new { error = result.Message });

            if (result.Response != null && int.TryParse(result.Response.ToString(), out int rows) && rows > 0)
                return Ok(new { message = "SubSector deleted" });

            return NotFound(new { message = "SubSector not found" });
        }

        [HttpGet("subsector-by-sector/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataDetail))]
        public async Task<IActionResult> GetSubSectorBySectorId(int id)
        {
            var model = (await _repo.Setup.GetSubSector(sectorId: id))
                                   .Where(x => x.IsUsable == true);
            if (model == null)
                return NotFound(new { message = "SubSector not found" });

            return Ok(model);
        }

        #endregion


        #region Target


        [HttpGet("target")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataDetail))]
        public async Task<IActionResult> TargetDetail(int id = 0, string name = "")
        {
            var model = await _repo.Setup.GetTarget(id, name);
            return Ok(model);
        }

        [HttpGet("target/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataDetail))]
        public async Task<IActionResult> GetTargetById(int id)
        {
            var model = (await _repo.Setup.GetBank(id)).FirstOrDefault();
            if (model == null)
                return NotFound(new { message = "Target not found" });

            return Ok(model);
        }

        [HttpPost("target")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> CreateTarget([FromBody] Target model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            model.CurrentUser = _currentUser.Id;
            var response = await _repo.Setup.SaveTarget(model);

            if (response.HasError)
                return BadRequest(response.Message);

            return Ok(new { message = "Target Created" });
        }

        [HttpPut("target/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> UpdateTarget(int id, [FromBody] Target model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = (await _repo.Setup.GetTarget(id)).FirstOrDefault();
            if (existing == null)
                return NotFound(new { message = "Target not found" });

            model.Id = id;
            model.CurrentUser = _currentUser.Id;
            var response = await _repo.Setup.SaveTarget(model);

            if (response.HasError)
                return BadRequest(response.Message);

            return Ok(new { message = "Target Updated" });
        }

        [HttpDelete("target/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> DeleteTarget(int id)
        {
            var result = await _repo.Setup.DeleteTarget(id);

            if (result.HasError)
                return StatusCode(500, new { error = result.Message });

            if (result.Response != null && int.TryParse(result.Response.ToString(), out int rows) && rows > 0)
                return Ok(new { message = "Target deleted" });

            return NotFound(new { message = "Target not found" });
        }
        #endregion


        #region Unit

        [HttpGet("unit")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataDetail))]
        public async Task<IActionResult> UnitDetail(int id = 0, string name = "", string description = "")
        {
            var model = await _repo.Setup.GetUnit(id, name, description);
            return Ok(model);
        }

        [HttpGet("unit/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataDetail))]
        public async Task<IActionResult> GetUnitById(int id)
        {
            var model = (await _repo.Setup.GetUnit(id)).Where(x => x.IsUsable);
            if (model == null)
                return NotFound(new { message = "Unit not found" });

            return Ok(model);
        }

        [HttpPost("unit")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> CreateUnit([FromBody] Unit model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            model.CurrentUser = _currentUser.Id;
            var response = await _repo.Setup.SaveUnit(model);

            if (response.HasError)
                return BadRequest(response.Message);

            return Ok(new { message = "Unit Created" });
        }

        [HttpPut("unit/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> UpdateUnit(int id, [FromBody] Unit model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = (await _repo.Setup.GetUnit(id)).Where(x => x.IsUsable);
            if (existing == null)
                return NotFound(new { message = "Unit not found" });

            model.Id = id;
            model.CurrentUser = _currentUser.Id;
            var response = await _repo.Setup.SaveUnit(model);

            if (response.HasError)
                return BadRequest(response.Message);

            return Ok(new { message = "Unit Updated" });
        }

        [HttpDelete("unit/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> DeleteUnit(int id)
        {
            var result = await _repo.Setup.DeleteUnit(id);

            if (result.HasError)
                return StatusCode(500, new { error = result.Message });

            if (result.Response != null && int.TryParse(result.Response.ToString(), out int rows) && rows > 0)
                return Ok(new { message = "Unit deleted" });

            return NotFound(new { message = "Unit not found" });
        }
        #endregion


        #region UnitType

        [HttpGet("unittype")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataDetail))]
        public async Task<IActionResult> UnitTypeDetail(int id = 0, string name = "", string name_np = "")
        {
            var model = await _repo.Setup.GetUnitType(id, name, name_np);
            return Ok(model);
        }

        [HttpGet("unittype/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataDetail))]
        public async Task<IActionResult> GetUnitTypeById(int id)
        {
            var model = (await _repo.Setup.GetBank(id)).FirstOrDefault();
            if (model == null)
                return NotFound(new { message = "UnitType not found" });

            return Ok(model);
        }

        [HttpPost("unittype")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> CreateUnitType([FromBody] UnitType model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            model.CurrentUser = _currentUser.Id;
            var response = await _repo.Setup.SaveUnitType(model);

            if (response.HasError)
                return BadRequest(response.Message);

            return Ok(new { message = "UnitType Created" });
        }

        [HttpPut("unittype/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> UpdateUnitType(int id, [FromBody] UnitType model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = (await _repo.Setup.GetUnitType(id)).FirstOrDefault();
            if (existing == null)
                return NotFound(new { message = "UnitType not found" });

            model.Id = id;
            model.CurrentUser = _currentUser.Id;
            var response = await _repo.Setup.SaveUnitType(model);

            if (response.HasError)
                return BadRequest(response.Message);

            return Ok(new { message = "UnitType Updated" });
        }

        [HttpDelete("unittype/{id}")]
        [Authorize(Policy = nameof(ePermission.ProjectMasterDataEntry))]
        public async Task<IActionResult> DeleteUnitType(int id)
        {
            var result = await _repo.Setup.DeleteUnitType(id);

            if (result.HasError)
                return StatusCode(500, new { error = result.Message });

            if (result.Response != null && int.TryParse(result.Response.ToString(), out int rows) && rows > 0)
                return Ok(new { message = "UnitType deleted" });

            return NotFound(new { message = "UnitType not found" });
        }

        #endregion

    }
}
