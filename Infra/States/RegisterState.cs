using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Infra.States
{
    public class RegisterState
    {
        public RegisterViewModel Input { get; set; } = new RegisterViewModel();
        public string SuccessReturn { get; set; }
        public IList<string> ErrorReturn { get; set; } = new List<string>();
    }

    public class RegisterViewModel: RegisterModel.InputModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Second Name")]
        public string SecondName { get; set; }
    }
}
