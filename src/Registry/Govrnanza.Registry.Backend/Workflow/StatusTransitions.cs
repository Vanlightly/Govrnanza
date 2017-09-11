using Govrnanza.Registry.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Govrnanza.Registry.Backend.Workflow
{
    public class StatusTransitions
    {
        public static IEnumerable<VersionStatus> GetNextValidStatuses(VersionStatus status)
        {
            switch (status)
            {
                case VersionStatus.Inception:
                    yield return VersionStatus.Development;
                    yield return VersionStatus.Cancelled;
                    break;
                case VersionStatus.Development:
                    yield return VersionStatus.QA;
                    yield return VersionStatus.Cancelled;
                    break;
                case VersionStatus.QA:
                    yield return VersionStatus.Development;
                    yield return VersionStatus.Production;
                    yield return VersionStatus.Cancelled;
                    break;
                case VersionStatus.Production:
                    yield return VersionStatus.Deprecated;
                    yield return VersionStatus.Retired;
                    break;
                case VersionStatus.Deprecated:
                    yield return VersionStatus.Retired;
                    break;
                case VersionStatus.Retired:
                    break;
                case VersionStatus.Cancelled:
                    yield return VersionStatus.Inception;
                    break;
            }
        }
    }
}
