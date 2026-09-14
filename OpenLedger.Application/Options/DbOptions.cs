using System.ComponentModel.DataAnnotations;

namespace OpenLedger.Application.Options
{
    public class DbOptions
    {
        public const string SectionName = "DbOptions";

        [Required(ErrorMessage = "DbConnectionString is required.")]
        public string? DbConnectionString { get; set; }
    }
}
