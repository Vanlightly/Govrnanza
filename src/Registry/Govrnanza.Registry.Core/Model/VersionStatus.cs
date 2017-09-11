using System;
using System.Collections.Generic;
using System.Text;

namespace Govrnanza.Registry.Core.Model
{
    public enum VersionStatus
    {
        Inception=0,
        Development=1,
        QA=2,
        Production=3,
        Deprecated=4,
        Retired=5,
        Cancelled=6
    }
}
