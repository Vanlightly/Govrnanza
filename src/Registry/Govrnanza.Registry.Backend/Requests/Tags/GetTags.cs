using Govrnanza.Registry.Backend.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Govrnanza.Registry.Backend.Requests.Tags
{
    /// <summary>
    /// A request to get tags. Leave all properties unassigned to get all tags
    /// </summary>
    public class GetTags : IRequest<Response<IEnumerable<string>>>
    {
        /// <summary>
        /// Optional. Gets tags of a given API
        /// </summary>
        public string ApiName { get; set; }
    }
}
