using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infra.States
{
    public class LoginState
    {
        public LoginModel.InputModel Input { get; private set; } = new LoginModel.InputModel();
    }
}
