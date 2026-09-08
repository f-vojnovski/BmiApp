using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmiApp.Service.Dto.Auth
{
    public class UserDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(32, ErrorMessage = "Your password is limited to {2} to {1} characters", MinimumLength= 8)]
        public string Password { get; set; }
    }
}
