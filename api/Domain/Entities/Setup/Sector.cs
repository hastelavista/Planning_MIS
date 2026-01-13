

namespace Domain.Entities.Setup
{
    public class Sector
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsUsable { get; set; }


        public int CurrentUser { get; set; }
    }
}
