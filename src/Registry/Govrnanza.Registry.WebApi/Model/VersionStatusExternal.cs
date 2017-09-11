using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Govrnanza.Registry.WebApi.Model
{
    /// <summary>
    /// The status of the API version in the workflow
    /// </summary>
    public enum VersionStatusExternal
    {
        /// <summary>
        /// Inception pahse
        /// </summary>
        Inception = 0,
        /// <summary>
        /// Development phase
        /// </summary>
        Development = 1,
        /// <summary>
        /// QA Phase
        /// </summary>
        QA = 2,
        /// <summary>
        /// In Production
        /// </summary>
        Production = 3,
        /// <summary>
        /// In Production but deprecated
        /// </summary>
        Deprecated = 4,
        /// <summary>
        /// No longer in production
        /// </summary>
        Retired = 5,
        /// <summary>
        /// The version was cancelled before reaching production
        /// </summary>
        Cancelled = 6
    }
}
