using Gatam.Application.CQRS.DTOS.ModulesDTO;
using Gatam.Application.CQRS.DTOS.RolesDTO;
using Gatam.Domain;
using System.ComponentModel.DataAnnotations;

namespace Gatam.Application.CQRS
{
    public class UserDTO
    {
        public  string? Id { get; set; }

        [Required(ErrorMessage = "Voornaam mag niet leeg zijn")]
        [MaxLength(15, ErrorMessage = "Voornaam is te lang")]
        public  string Name { get; set; }

        [Required(ErrorMessage = "Gebruikersnaam mag niet leeg zijn")]
        public  string Username { get; set; } 

        [Required(ErrorMessage = "Achternaam mag niet leeg zijn")]
        [MaxLength(15, ErrorMessage = "Achternaam is te lang")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Email moet ingevuld zijn")]
        [EmailAddress(ErrorMessage = "Email moet een geldig emailadres zijn")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Wachtwoord moet ingevuld zijn")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,}$",
                      ErrorMessage = "Wachtwoord moet minimaal 8 tekens bevatten, waaronder een hoofdletter, een kleine letter, een cijfer en een speciaal teken.")]
        public string PasswordHash { get; set; }


        [Required(ErrorMessage = "Telefoonnummer moet ingevuld zijn")]
        [RegularExpression(@"^\+?[1-9]\d{1,14}$",
                      ErrorMessage = "Telefoonnummer moet beginnen met een '+' gevolgd door 2 tot 15 cijfers")]
        public string? PhoneNumber { get; set; }
        public  List<string>? RolesIds { get; set; }
        
        public string? Picture { get; set; }

        public string? BegeleiderId { get; set; }
   
        public bool IsActive { get; set; }
      
        public List<UserModuleDTO>? UserModules { get; set; }
    }
}
