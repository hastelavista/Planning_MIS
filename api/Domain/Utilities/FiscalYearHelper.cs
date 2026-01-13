using Domain.Entities.Setup;

namespace Domain.Utilities
{
    public class FiscalYearHelper
    {
        public static FiscalYear GetCurrentFiscalYear()
        {
            DateTime now = DateTime.Now;

            int fyStartYear;
            int fyEndYear;

            if (now.Month > 7 || (now.Month == 7 && now.Day >= 16))
            {
                fyStartYear = now.Year - 57; 
                fyEndYear = fyStartYear + 1;
            }
            else
            {
                fyEndYear = now.Year - 57;
                fyStartYear = fyEndYear - 1;
            }

            string fyName = $"{fyStartYear}/{fyEndYear.ToString()[2..]}";

            string nepaliDateFrom = $"{fyStartYear}-04-01";
            string nepaliDateTo = $"{fyEndYear}-03-31";

            DateTime dateFromEng = new DateTime(fyStartYear + 57, 7, 16); // Start of FY
            DateTime dateToEng = new DateTime(fyEndYear + 57, 7, 15);      // End of FY

            return new FiscalYear
            {
                Id = 0,                  
                Name = fyName,
                Name_En = fyName,       
                Code = 0,              
                StartYear = fyStartYear,
                EndYear = fyEndYear,
                DisplayPosition = 1,     
                DateFrom = nepaliDateFrom,
                DateTo = nepaliDateTo,
                DateFromEng = dateFromEng,
                DateToEng = dateToEng,
                IsDeleted = false
            };
        }
    }
}
