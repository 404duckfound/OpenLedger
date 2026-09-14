using System.ComponentModel.DataAnnotations;

namespace OpenLedger.Application.Options
{
    public class EmailOptions
    {
        public const string SectionName = "EmailOptions";

        [Required(ErrorMessage = "SmtpServer is required.")]
        public string? SmtpServer { get; set; }
        [Required(ErrorMessage = "SmtpPort is required.")]
        public int SmtpPort { get; set; }
        [Required(ErrorMessage = "FromName is required.")]
        public string? FromName { get; set; }
        [Required(ErrorMessage = "FromEmail is required.")]
        public string? FromEmail { get; set; }
        [Required(ErrorMessage = "EnableSsl is required.")]
        public bool EnableSsl { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = "SmtpUsername is required.")]
        public string? SmtpUsername { get; set; }
        [Required(AllowEmptyStrings = true, ErrorMessage = "SmtpPassword is required.")]
        public string? SmtpPassword { get; set; }
    }
}
