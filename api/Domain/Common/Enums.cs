using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    public class Enums
    {
        public enum MasterTable
        {
            Ward,
            BudgetSubTitle,
            ExpensesHead,
            Member,
            Designation,
            BudgetSourceLevel,
            BudgetSource,
            UnitType,
            Unit,
            Committee,
            LetterType,
            ExpensesType,
            Sector,
            SubSector,
            ProjectType,
            Role,
            Target,
            ImplementationLevel,
            Department,
            ActivityGroup,
            Activity,
            FiscalYear,
            Permission,
            ProjectModel,
            Firm,
            FirmType,
            Employee,
            Bank,
            MainProject,
            DepositType,
            CommitteeType,
            Nature,
            ImplementationModel,
            Individual,
            ItemType,
            Item,
            NormsType
        }
    }

    public enum eLetterType
    {
        Agreement = 1,
        WorkOrder = 2,
        Payment = 3,
        RegistrationCertificate = 4,
        AccountOpening = 5,
        ProjectTransferForm = 6,
        ProjectClearance = 7,
        AdvanceLetter = 8,
        TimeExtendedLetter = 9,
        ProjectAcceptanceLetter = 10,
        LetterOpeningProposal = 11,
        IntentNoticeLetter = 12,
        Registered_RateSubmissionLetter = 13,
        AccountClosing = 14,
        AgreementCommittee = 15,
        PaymentCommittee = 16,
        PaymentRelease = 17
    }

    public enum ePermission
    {
        UserManagement = 1,
        OfficeSetup = 2,
        DepartmentEntry = 3,
        DepartmentDetail = 4,
        DesignationEntry = 5,
        DesignationDetail = 6,
        PublicRepresentativeEntry = 7,
        PublicRepresentativeDetail = 8,
        EmployeeEntry = 9,
        EmployeeDetail = 10,
        FiscalYearEntry = 11,
        FiscalYearDetail = 12,
        WardEntry = 13,
        WardDetail = 14,
        IncentiveRateEntry = 15,
        IncentiveRateDetail = 16,
        ConfigurationEnry = 17,
        ConfigurationDetail = 18,
        ProjectMasterDataEntry = 19,
        ProjectMasterDataDetail = 20,
        ActivityEntry = 21,
        ActivityDetail = 22,
        LetterFormatEntry = 23,
        LetterFormatDetail = 24,
        AgreementFormatEntry = 25,
        AgreementFormatDetail = 26,
        ProjectEntry = 27,
        ProjectDelete = 28,
        ProjectDetail = 29,
        ProjectCostEstimationEntry = 30,
        ProjectCostEstimationDetail = 31,
        ProjectCostEstimationDelete = 32,
        ProjectLetterEntry = 33,
        ProjectLetterPrint = 34,
        ProjectEvaluationEntry = 35,
        ProjectEvaluationDetail = 36,
        ProjectEvaluationDelete = 37,
        ProjectPaymentEntry = 38,
        ProjectPaymentDetail = 39,
        ProjectPaymentDelete = 40,
        ProjectAgreementEntry = 41,
        ProjectAgreementDetail = 42,
        ProjectAgreementDelete = 43,
        ContractorCostEntry = 44,
        ContractorCostDetail = 45,
        ContractorCostDelete = 46,
        DepositEntry = 47,
        DepositDetail = 48,
        DepositDelete = 49,
        CommitteeEntry = 50,
        CommitteeDetail = 51,
        FirmEntry = 52,
        FirmDetail = 53,
        BackDateEntry = 54,
        ProjectInchargeAssignment = 55,
        ProjectInchargeDetail = 56,
        ProjectProgressGoal = 57,
        ProjectProgressGoalEntry = 58,
        ProjectProgressGoalDelete = 59,
        ProjectCostEstimationCheck = 60,
        ProjectCostEstimationApprove = 61,
        BudgetEntry = 62,
        BudgetDelete = 63,
        BudgetTransferEntry = 64,
        BudgetTransferDelete = 65,
        IndividualEntry = 66,
        IndividualDetail = 67,
        ProjectCompletedEntry = 68
    }

    public enum eExcelImports
    {
        MainProject = 1,
        Project = 2,
        ProjectCostEstimation = 3
    }

    public enum eReportType
    {
        ExpiredReport = 1,
        NearlyExpiredReport = 2,
        PaidProjectReport = 3,
        KaryakramReport = 4
    }

}
