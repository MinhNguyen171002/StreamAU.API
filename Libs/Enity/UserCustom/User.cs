using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libs.Enity.UserCustom
{
    public class User
    {
        [Key]
        public string UserId { get; set; }
        public string UserName { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [ForeignKey(nameof(UserId))]
        public IdentityUser user { get; set; }
        [NotMapped]
        [System.ComponentModel.DataAnnotations.Compare("Password")]
        public string PasswordConfirmed { get; set; }
    }
}
