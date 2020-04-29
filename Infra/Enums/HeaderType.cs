using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Infra.Enums
{
    public enum HeaderType
    {
        [Description("long")]
        Long,
        [Description("integer")]
        Integer,
        [Description("float")]
        Float,
        [Description("text")]
        Text
    }
}
