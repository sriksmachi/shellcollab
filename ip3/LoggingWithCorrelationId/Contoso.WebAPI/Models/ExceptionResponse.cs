using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contoso.WebAPI.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ExceptionResponse
    {
        /// <summary>
        /// Gets or sets CorrelationId
        /// </summary>
        public string CorrelationId { get; set; }

        /// <summary>
        /// Gets or sets Message for Exception
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets Exception Details
        /// </summary>
        public Exception Exception { get; set; }
    }

    public static class Constants
    {
        /// <summary>
        /// The correlation identifier
        /// </summary>
        public static string CorrelationId = "correlationId";
    }
}
