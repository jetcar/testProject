using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassLibrary1
{
    [Table("Users")]
    public class BackOfficeUser
    {
        [Key]
        public Guid UserGuid { get; set; }

        [MaxLength(64)]
        public string FirstName { get; set; }

        [MaxLength(64)]
        public string LastName { get; set; }

        [MaxLength(200)]
        public string FullName { get; set; }

        [MaxLength(254)]
        public string Username { get; set; }

        [MaxLength(254)]
        public string Email { get; set; }

        [MaxLength(50)]
        public string PersonalCode { get; set; }

        [MaxLength(254)]
        public string Password { get; set; }

        [MaxLength(2)]
        public string Country { get; set; }

        [MaxLength(5)]
        public string Language { get; set; }

        public Guid? LastCompany { get; set; }
        public string Settings { get; set; }
        public bool? BetaRole { get; set; }
        public DateTime? EmailLastValidated { get; set; }
        public DateTime? PersonalCodeLastValidated { get; set; }

        [MaxLength(254)]
        public string UserNotes { get; set; }

        [MaxLength(64)]
        public string ExternalId { get; set; }
    }
}