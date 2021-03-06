﻿namespace SDammann.WebApi.Versioning {
    using System;
    using System.Globalization;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Dispatcher;
    using System.Web.Http.Routing;


    /// <summary>
    ///   Represents an <see cref="IHttpControllerSelector" /> implementation that supports versioning and selects an controller based on versioning by convention (namespace.Api.Version1.xxxController). The controller to invoke is determined by the "version" key in the routing dictionary.
    /// </summary>
    public sealed class RouteVersionedControllerSelector : VersionedControllerSelector {
        private const string VersionKey = "version";

        /// <summary>
        ///   Initializes a new instance of the <see cref="RouteVersionedControllerSelector" /> class.
        /// </summary>
        /// <param name="configuration"> The configuration. </param>
        public RouteVersionedControllerSelector (HttpConfiguration configuration) : base(configuration) {
        }

        protected override ControllerIdentification GetControllerIdentificationFromRequest (HttpRequestMessage request) {
            if (request == null) {
                throw new ArgumentNullException("request");
            }

            IHttpRouteData routeData = request.GetRouteData();
            if (routeData == null) {
                return default(ControllerIdentification);
            }

            // Look up controller in route data
            string controllerName = this.GetControllerNameFromRequest(request);

            // Also try the version if possible
            object apiVersionObj;
            string apiVersion = null;
            int version;
            if (routeData.Values.TryGetValue(VersionKey, out apiVersionObj) &&
                !String.IsNullOrWhiteSpace(apiVersionObj as string)) {
                apiVersion = apiVersionObj as string;
            }

            return new ControllerIdentification(controllerName, apiVersion);
        }
    }
}