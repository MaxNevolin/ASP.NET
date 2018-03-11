using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.ComponentModel.DataAnnotations;

namespace Diplom_project.Models
{
        public class WebstoreUser : MembershipUser
        {
            [Required(ErrorMessage = "Поле {0} має бути заповнено")]
            [Display(Name = "Логін (електронна адреса)")]
            public string UserName { get; set; }

            [Required(ErrorMessage = "Поле {0} має бути заповнено")]
            [DataType(DataType.Password)]
            [Display(Name = "Пароль")]
            public string Password { get; set; }

         /*   [Required]
            [DataType(DataType.EmailAddress)]
            public string Role { get; set; }*/

            [Display(Name = "Запамятати")]
            public bool RememberMe { get; set; }
        }
}