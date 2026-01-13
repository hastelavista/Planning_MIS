namespace Planning_MIS.API.ViewModels.Committee
{
    public class CommitteeLetterDTO
    {
        public int CommitteeId { get; set; }
        public string CommitteeName { get; set; }
        public int LetterFormatId { get; set; }
        public int LetterTypeId { get; set; }
        public string Subject { get; set; }
        public string LetterBody { get; set; }
        public string Miti { get; set; }
    }

    public class CommitteeLetterRequest
    {
        public int CommitteeId { get; set; }
        public int ProjectId { get; set; }
        public int LetterType { get; set; }
    }
}
