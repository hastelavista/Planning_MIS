

namespace Domain.Entities.Setup
{
    public class ImplementationLevel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal Threshold { get; set; }


        public int CurrentUser { get; set; }
    }
}
