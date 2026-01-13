using Azure.Core;
using DateMiti;
using Domain.Common;
using Domain.Entities.Setup;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Planning_MIS.API.DocumentAPI;
using Planning_MIS.API.ViewModels.Committee;
using Planning_MIS.DocumentAPI.File;
using System.Reflection;
using System.Text.Json;

namespace Planning_MIS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommitteeController : ControllerBase
    {
        private readonly IUnitOfWork _repo;
        private readonly ICurrentUserService _currentUser;
        private readonly DocumentAPIHelper _docAPIHelper;


        public CommitteeController(IUnitOfWork repo, DocumentAPIHelper docAPIHelper,
            ICurrentUserService currentUser)
        {
            _repo = repo;
            _docAPIHelper = docAPIHelper;
            _currentUser = currentUser;
        }


        [HttpGet("committee-heading/{id}")]
        public async Task<IActionResult> CommitteeHeading(int id)
        {
            var committees = await _repo.Setup.GetCommittee(id);
            return committees.Any() ? Ok(committees.First()) : NotFound();
        }


        #region CommitteeType

        [HttpGet("committee-type")]
        public async Task<IActionResult> CommitteeTypeDetail(int? id, string name = "")
        {

            var model = await _repo.Setup.GetCommitteeType(id, name);
            if (model == null || !model.Any())
            {
                return NotFound("No CommitteeTypes Found.");
            }
            return Ok(model);
        }

        [HttpPost("committee-type")]
        [Authorize(Policy = nameof(ePermission.CommitteeEntry))]
        public async Task<IActionResult> CreateCommitteeType([FromBody] CommitteeType model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            model.CurrentUser = _currentUser.Id;

            var result = await _repo.Setup.SaveCommitteeType(model);
            return Ok(result);
        }

        [HttpPut("committee-type/{id}")]
        [Authorize(Policy = nameof(ePermission.CommitteeEntry))] 
        public async Task<IActionResult> UpdateCommitteeType(int id, [FromBody] CommitteeType model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
           
            var existing = (await _repo.Setup.GetCommitteeType(id)).FirstOrDefault();
            if (existing == null) return NotFound($"CommitteeType with ID {id} not found.");

            model.Id = id;
            model.CurrentUser = _currentUser.Id;

            var result = await _repo.Setup.SaveCommitteeType(model);

            if (result.HasError)
                return BadRequest(result.Message);

            return Ok(new { Success = "Committee type updated." });
        }


        [HttpDelete("committee-type/{id}")]
        [Authorize(Policy = nameof(ePermission.CommitteeEntry))]
        public async Task<IActionResult> DeleteCommitteeType(int id)
        {
            var result = await _repo.Setup.DeleteCommitteeType(id);
            if (!result.HasError)
            {
                if (result.Response != null && int.TryParse(result.Response.ToString(), out int rows) && rows > 0)
                    return Ok("Deleted");
                
                return NotFound();
            }
            return StatusCode(500, result.Message);
        }

        #endregion


        #region Committee

        [HttpGet("committee")]
        public async Task<IActionResult> CommitteeDetails(int id = 0, string name = "", string registrationNo = "")
        {
            int? currentFyId = _currentUser.FiscalYearId;
            var userWardList = JsonSerializer.Serialize(_currentUser.WardIds);

            var model = await _repo.Setup.GetCommittee(id, name, registrationNo, currentFyId ?? 0, userWardList);
            if (model == null || !model.Any())
            {
                return NotFound("No Committees Found.");
            }
            return Ok(model);
        }

        [HttpGet("committee/{id?}")]
        [Authorize(Policy = nameof(ePermission.CommitteeEntry))]
        public async Task<IActionResult> GetCommittee(int id = 0)
        {
            var committee = id > 0
                 ? await _repo.Setup.GetCommitteeById(id)
                 : new Committee();

            var documents = await _docAPIHelper.GetFileTypes("Planning", "Committee");

            var files = id > 0 
                ? await _docAPIHelper.GetFileRecords($"Plan_Committee_{id}")
                : new List<FileRecord>();

            return Ok(new
            {
                Committee = committee,
                Documents = documents,
                FileRecords = files
            });
        }


        [HttpPost("committee")]
        [Authorize(Policy = nameof(ePermission.CommitteeEntry))]
        public async Task<IActionResult> CreateCommittee([FromBody] Committee model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            model.CurrentUser = _currentUser.Id;
                model.RegisteredDate = DateMiti.GetDateMiti.GetDate(model.RegisteredMiti);

            var result = await _repo.Setup.SaveCommittee(model);
            return Ok(result);
        }


        [HttpPut("committee/{id}")]
        [Authorize(Policy = nameof(ePermission.CommitteeEntry))]
        public async Task<IActionResult> UpdateCommittee(int id, [FromBody] Committee model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existing = (await _repo.Setup.GetCommittee(id)).FirstOrDefault();
            if (existing == null) return NotFound($"Committee with ID {id} not found.");

            model.Id = id;
            model.CurrentUser = _currentUser.Id;

            var result = await _repo.Setup.SaveCommittee(model);

            if (result.HasError)
                return BadRequest(result.Message);

            return Ok(new { Success = "Committee updated." });
        }


        [HttpDelete("committee/{id}")]
        [Authorize(Policy = nameof(ePermission.CommitteeEntry))]
        public async Task<IActionResult> DeleteCommittee(int id)
        {
            var result = await _repo.Setup.DeleteCommittee(id);
            if (!result.HasError)
            {
                if (result.Response != null && int.TryParse(result.Response.ToString(), out int rows) && rows > 0)
                    return Ok("Deleted");

                return NotFound();
            }
            return StatusCode(500, result.Message);
        }

        #endregion


        #region Committee Letter

        [HttpGet("committee-letter")]
        [Authorize(Policy = nameof(ePermission.CommitteeEntry))]
        public async Task<IActionResult> CommitteeLetterDetail()
        {
            var model = await _repo.Setup.GetCommitteeLetter();
            if (model == null || !model.Any())
                return NotFound();

            return Ok(model);
        }

        [HttpGet("committee-letter-by-id/{id}")]
        [Authorize(Policy = nameof(ePermission.CommitteeEntry))]
        public async Task<IActionResult> GetCommitteeLetterById(int id)
        {
            var letters = await _repo.Setup.GetCommitteeLetter(committeeId: id);
            var model = letters.FirstOrDefault();

            if (model == null)  return NotFound();
            return Ok(model);
        }

        [HttpDelete("committee-letter/{id}")]
        [Authorize(Policy = nameof(ePermission.CommitteeEntry))]
        public async Task<IActionResult> DeleteCommitteeLetter(int id)
        {
            if (id <= 0) return BadRequest("Invalid ID.");

            var result = await _repo.Setup.DeleteCommitteeLetter(id);
            if (!result.HasError)
            {
                if (result.Response != null && int.TryParse(result.Response.ToString(), out int rows) && rows > 0)
                    return Ok("Deleted");

                return NotFound();
            }
            return StatusCode(500, result.Message);
        }

        [HttpGet("committee-letter-print")]
        [Authorize(Policy = nameof(ePermission.CommitteeEntry))]
        public async Task<IActionResult> CommitteeLetterPrint(int id, int committeeId)
        {
            var committeeMember = await _repo.Setup.GetMemberByCommitteeId(committeeId);
            var committeeLetter = (await _repo.Setup.GetCommitteeLetter(id)).FirstOrDefault();
            if (committeeLetter == null ) return NotFound();

            return Ok(new
            { 
                CommitteeMember = committeeMember,
                LetterHeadOffice = committeeLetter.LetterHeadOffice
            });
        }

        #endregion


        #region Letters

        [HttpPost("committee-letter/generate")]
        [Authorize(Policy = nameof(ePermission.CommitteeEntry))]
        public async Task<IActionResult> GenerateCommitteeLetter([FromBody] CommitteeLetterRequest letterReq)
        {
            if (letterReq.CommitteeId <= 0)
                return BadRequest("CommitteeId is required.");

            var committee = await _repo.Setup.GetCommitteeById(letterReq.CommitteeId);
            if (committee == null)
                return NotFound("Committee not found.");

            var letterFormat = await _repo.Setup.ProcessLetter(committeeId: letterReq.CommitteeId, projectId: letterReq.ProjectId, letterType: letterReq.LetterType);
            
            if (letterFormat == null)
                return BadRequest("चिठी नमुना भेटेन । कृपया पहिले चिठिको नमुना राख्नुहोस् ।");

            var letter = new CommitteeLetter
            {
                CommitteeId = committee.Id,
                CommitteeName = committee.Name,
                LetterFormatId = letterFormat.Id,
                LetterTypeId = letterFormat.LetterTypeId,
                Subject = letterFormat.Subject,
                LetterBody = letterFormat.LetterBody,
                Miti = GetDateMiti.GetMiti(DateTime.Now)
            };

            return Ok(letter);

        }

        #endregion


        #region Committee Member
        [HttpGet("committee-member")]
        [Authorize(Policy = nameof(ePermission.CommitteeDetail))]
        public async Task<IActionResult> CommitteeMemberDetails(int committeeId = 0, string name = "", string citizenshipNo = "", string mobileNo = "")
        {
            var model = await _repo.Setup.GetMemberByCommitteeId(committeeId: committeeId, name: name, citizenshipNo: citizenshipNo, mobileNo: mobileNo);
            if (model == null || !model.Any())
                return NotFound();

            return Ok(model);
        }


        [HttpGet("committee-members-by-committee-id/{id}")]
        [Authorize(Policy = nameof(ePermission.CommitteeDetail))]
        public async Task<IActionResult> CommitteeMembersByCommitteeId(int id)
        {
            var model = await _repo.Setup.GetMemberByCommitteeId(id);
            if (model == null || !model.Any())
                return NotFound();
            return Ok(model);
        }

        #endregion


        #region Member

        [HttpGet("member")]
        public async Task<IActionResult> MemberDetails(string name = "", string citizenshipNo = "", string mobileNo = "")
        {

            var model = await _repo.Setup.GetMember(name: name, mobileNo: mobileNo, citizenshipNo: citizenshipNo);
            if (model == null || !model.Any())
            {
                return NotFound("No Members Found.");
            }
            return Ok(model);
        }

        [HttpGet("member-check-by-citizenship-no")]
        public async Task<IActionResult> CheckCitizenshipNo(string citizenshipNo, int committeeId = 0)
        {

            if (string.IsNullOrWhiteSpace(citizenshipNo))
                return BadRequest("Citizenship number is required.");

            var fyId = _currentUser.FiscalYearId;

            var model = await _repo.Setup.CheckMemberByCitizenShipNumber(citizenshipNo, fyId, committeeId);
            if (model != null)
                return Ok($"यो सदस्य पहिले नै {model.Committee} मा छ ।");

            return Ok(true);
        }

        [HttpGet("member-get-by-citizenship-no")]
        public async Task<IActionResult> GetMemberByCitizenshipNo(string citizenshipNo)
        {

            if (string.IsNullOrWhiteSpace(citizenshipNo))
                return BadRequest("Citizenship number is required.");

            var model = await _repo.Setup.GetMemberByCitizenshipNo(citizenshipNo);
            if (model == null) return NotFound();
            
            return Ok(model);
        }

        [HttpPost("member")]
        [Authorize(Policy = nameof(ePermission.CommitteeEntry))]
        public async Task<IActionResult> CreateMember([FromBody] Member model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            model.CurrentUser = _currentUser.Id;

            var result = await _repo.Setup.SaveMember(model);
            return Ok(result);
        }

        [HttpPut("member/{id}")]
        [Authorize(Policy = nameof(ePermission.CommitteeEntry))]
        public async Task<IActionResult> UpdateMember(int id, [FromBody] Member model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existing = (await _repo.Setup.GetCommitteeType(id)).FirstOrDefault();
            if (existing == null) return NotFound($"CommitteeType with ID {id} not found.");

            model.Id = id;
            model.CurrentUser = _currentUser.Id;

            var result = await _repo.Setup.SaveMember(model);
            return Ok(new { Success = "Committee type updated." });
        }

        [HttpDelete("member/{id}")]
        [Authorize(Policy = nameof(ePermission.CommitteeEntry))]
        public async Task<IActionResult> DeleteMember(int id)
        {
            var result = await _repo.Setup.DeleteMember(id);
            if (!result.HasError)
            {
                if (result.Response != null && int.TryParse(result.Response.ToString(), out int rows) && rows > 0)
                    return Ok("Deleted");

                return NotFound();
            }
            return StatusCode(500, result.Message);
        }

        #endregion


        #region Firm

        [HttpGet("firm")]
        [Authorize(Policy = nameof(ePermission.FirmDetail))]
        public async Task<IActionResult> GetFirms(int id = 0, string name = "")
        {
            var model = await _repo.Setup.GetFirm(id, name);
            if (model == null || !model.Any())
                return NotFound();
            return Ok(model);
        }

        [HttpGet("firm/{id}")]
        [Authorize(Policy = nameof(ePermission.FirmDetail))]
        public async Task<IActionResult> GetFirmById(int id)
        {
            var model = await _repo.Setup.GetFirmById(id);
            if (model == null) return NotFound();

            return Ok(model);
        }

        [HttpPost("firm")]
        [Authorize(Policy = nameof(ePermission.FirmEntry))]
        public async Task<IActionResult> CreateFirm([FromBody] Firm model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            model.CurrentUser = _currentUser.Id;
            var dbResponse = await _repo.Setup.SaveFirm(model);

            if (dbResponse.HasError)
                return BadRequest(dbResponse.Message);

            return Ok(new { Success = "Firm Created" });
        }

        [HttpPut("firm/{id}")]
        [Authorize(Policy = nameof(ePermission.FirmEntry))]
        public async Task<IActionResult> UpdateFirm(int id, [FromBody] Firm model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = (await _repo.Setup.GetFirmById(id));
            if (existing == null) return NotFound();

            model.Id = id;
            model.CurrentUser = _currentUser.Id;
            var dbResponse = await _repo.Setup.SaveFirm(model);

            if (dbResponse.HasError)
                return BadRequest(dbResponse.Message);

            return Ok(new { Message = "Firm Updated!" });
        }

        [HttpDelete("firm/{id}")]
        [Authorize(Policy = nameof(ePermission.FirmEntry))]
        public async Task<IActionResult> DeleteFirm(int id)
        {
            var result = await _repo.Setup.DeleteFirm(id);
            if (!result.HasError)
            {
                if (result.Response != null && int.TryParse(result.Response.ToString(), out int rows) && rows > 0)
                    return Ok("Deleted");

                return NotFound();
            }
            return StatusCode(500, result.Message);

        }

        #endregion


        #region Individual

        [HttpGet("individual")]
        [Authorize(Policy = nameof(ePermission.IndividualEntry))]
        public async Task<IActionResult> IndividualDetails(int id = 0, string name = "")
        {
            var model = await _repo.Setup.GetIndividual(id, name);
            if (model == null || !model.Any())
                return NotFound();
            return Ok(model);
        }

        [HttpGet("individual/{id}")]
        [Authorize(Policy = nameof(ePermission.IndividualEntry))]
        public async Task<IActionResult> GetIndividualById(int id)
        {
            var model = await _repo.Setup.GetIndividualById(id);
            if (model == null) return NotFound();

            return Ok(model);
        }

        [HttpPost("individual")]
        [Authorize(Policy = nameof(ePermission.IndividualEntry))]
        public async Task<IActionResult> CreateIndividual([FromBody] Individual model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            model.CurrentUser = _currentUser.Id;
            var dbResponse = await _repo.Setup.SaveIndividual(model);

            if (dbResponse.HasError)
                return BadRequest(dbResponse.Message);

            return Ok(new { Success = "Individual Created" });
        }

        [HttpPut("individual/{id}")]
        [Authorize(Policy = nameof(ePermission.IndividualEntry))]
        public async Task<IActionResult> UpdateIndividual(int id, [FromBody] Individual model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = (await _repo.Setup.GetIndividualById(id));
            if (existing == null) return NotFound();

            model.Id = id;
            model.CurrentUser = _currentUser.Id;
            var dbResponse = await _repo.Setup.SaveIndividual(model);

            if (dbResponse.HasError)
                return BadRequest(dbResponse.Message);

            return Ok(new { Message = "Individual Updated" });
        }

        [HttpDelete("individual/{id}")]
        [Authorize(Policy = nameof(ePermission.IndividualEntry))]
        public async Task<IActionResult> DeleteIndividual(int id)
        {
            var result = await _repo.Setup.DeleteIndividual(id);
            if (!result.HasError)
            {
                if (result.Response != null && int.TryParse(result.Response.ToString(), out int rows) && rows > 0)
                    return Ok("Deleted");

                return NotFound();
            }
            return StatusCode(500, result.Message);

        }

        #endregion

    }
}
