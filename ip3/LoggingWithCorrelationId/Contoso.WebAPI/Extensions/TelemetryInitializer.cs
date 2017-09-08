using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Contoso.WebAPI.Models;

namespace Contoso.WebAPI.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.ApplicationInsights.Extensibility.ITelemetryInitializer" />
    public class OperationIdTelemetryInitializer : ITelemetryInitializer
    {
        /// <summary>
        /// Initializes properties of the specified <see cref="T:Microsoft.ApplicationInsights.Channel.ITelemetry" /> object.
        /// </summary>
        /// <param name="telemetry"></param>
        public void Initialize(ITelemetry telemetry)
        {
            var requestTelemetry = telemetry as RequestTelemetry;
            if (requestTelemetry == null) return;
            if (!requestTelemetry.Context.Properties.ContainsKey(Constants.CorrelationId))
                requestTelemetry.Context.Properties[Constants.CorrelationId] = Guid.NewGuid().ToString();
        }
    }
}
