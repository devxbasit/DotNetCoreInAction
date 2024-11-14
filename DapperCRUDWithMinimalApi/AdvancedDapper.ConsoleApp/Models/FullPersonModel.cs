namespace AdvancedDapper.ConsoleApp.Models
{
    public class FullPersonModel : PersonModel
    {
        public int Id { get; set; }
        public PhoneModel CellPhone { get; set; }
    }
}
