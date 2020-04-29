using System;
using System.Collections.Generic;
using System.Text;

namespace Infra.Enums
{
    public enum OrderState
    {
        Waiting,
        WaitingToImport,
        Imported,
        Error,
        ToConfigurate,
        WaitingToConfiguredNotMapp
    }
}
