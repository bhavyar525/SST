using System.ComponentModel.DataAnnotations;

namespace ScrumStandUpTrackerProject.Models
{
    public class Developer
    {
        [Key]
        public int Id { get; set; }  // Primary Key

        [Required, MaxLength(50)]
        public string UserName { get; set; }

        [Required, MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }  // Store hashed password here

    }
}