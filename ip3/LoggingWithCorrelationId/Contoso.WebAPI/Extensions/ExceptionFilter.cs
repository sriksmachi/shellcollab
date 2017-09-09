namespace Contoso.WebAPI.Extensions
{
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using System.Linq;
    using Contoso.WebAPI.Models;


    /// <summary>
    /// ExceptionFilter. The Exception Filter will be executed on the event of Exception 
    /// </summary>
    public class ExceptionFilter : IExceptionFilter
    {
        /// <summary>
        ///  Reference to the loggers
        /// </summary>
        private readonly ILogger<ExceptionFilter> loggerException;

        /// <summary>
        /// Reference to Application Insights telemetry Client
        /// </summary>
        private readonly TelemetryClient telemetryClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionFilter" /> class.
        /// </summary>
        /// <param name="logger">An instance of ILogger</param>
        /// <param name="filterOptions">An instance of the ExceptionFilterOptions class</param>
        /// <param name="applicationInsightsOptions">An instance of Application Insights option</param>
        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            this.loggerException = logger;
            this.telemetryClient = new TelemetryClient();
        }

        /// <summary>
        /// Method invoked on Exception
        /// </summary>
        /// <param name="context">Exception Context</param>
        public void OnException(ExceptionContext context)
        {
            var telemetry = new RequestTelemetry();
            TelemetryConfiguration.Active.TelemetryInitializers.OfType<OperationIdTelemetryInitializer>().Single().Initialize(telemetry);
            var correlationId = telemetry.Context.Properties[Constants.CorrelationId];

            this.loggerException.LogCritical(5001, context.Exception, $"Unhandled exception captured in global filter: Correlation Id - {correlationId}");

            ExceptionResponse currentException = new ExceptionResponse();
            currentException.Exception = context.Exception;
            currentException.Message = "Error occurred while processing the request";
            currentException.CorrelationId = correlationId;

            // log exception to application insights
            var exceptionTelemetry = new ExceptionTelemetry(context.Exception);
            exceptionTelemetry.ProblemId = correlationId;
            this.telemetryClient.TrackException(exceptionTelemetry);

            context.Result = new ContentResult()
            {
                ContentType = "application/json",
                Content = JsonConvert.SerializeObject(currentException),
                StatusCode = Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError
            };
        }
    }
}
