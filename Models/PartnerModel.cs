
namespace Aafeben.Models
{
    public class PartnerModel
    {
        public int Id { get; set; }
        public required string UrlLink { get; set; }
        public required string Name { get; set; }
        public string? Image { get; set; }

        // relations
    }
}