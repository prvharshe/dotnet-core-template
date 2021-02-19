﻿
using System.Linq;
using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace SampleWebAPIApplication.Middlewares
{
    public class CorrelationLogEventEnricher : ILogEventEnricher
    {
        public const string CorrelationPropertyName = "CorrelationId";

        public CorrelationLogEventEnricher(IHttpContextAccessor httpContextAccessor, string correlationIdHeaderKey)
        {
            HttpContextAccessor = httpContextAccessor;
            CorrelationIdHeaderKey = correlationIdHeaderKey;
        }

        public IHttpContextAccessor HttpContextAccessor { get; }
        public string CorrelationIdHeaderKey { get; }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (HttpContextAccessor.HttpContext == null) return;

            var requestHeaders = HttpContextAccessor.HttpContext.Request.Headers;
            var correlationId = requestHeaders[CorrelationIdHeaderKey].FirstOrDefault();
            if (correlationId != null)
            {
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(CorrelationPropertyName, correlationId));
            }
        }
    }
}
