using System.ComponentModel.DataAnnotations;

namespace OpenLedger.Infrastructure.Options
{
    public class DbOptions
    {
        [Required(ErrorMessage = "Db:Connection string is required.")]
        public required string ConnectionString { get; set; }
    }
}
