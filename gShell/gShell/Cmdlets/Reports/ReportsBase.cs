﻿using System;
using System.Management.Automation;
using System.Collections.Generic;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using reports_v1 = Google.Apis.Admin.Reports.reports_v1;
using Data = Google.Apis.Admin.Reports.reports_v1.Data;

using gShell.dotNet.Utilities;
using gShell.dotNet.Utilities.OAuth2;
using gReports = gShell.dotNet.Reports;

namespace gShell.Cmdlets.Reports
{
    /// <summary>
    /// /// The base class for all Google Reports API calls within the PowerShell Cmdlets.
    /// </summary>
    public abstract class ReportsBase : OAuth2CmdletBase
    {
        #region Properties
        protected static gReports greports { get; set; }
        protected Activities activities { get; set; }
        protected Channels channels { get; set; }
        protected CustomerUsageReports customerUsageReports { get; set; }
        protected UserUsageReports userUsageReports { get; set; }

        [Parameter(
            Mandatory = false)]
        [ValidateNotNullOrEmpty]
        public string Domain { get; set; }

        protected override string apiNameAndVersion { get { return greports.apiNameAndVersion; } }

        #endregion

        #region Constructors

        public ReportsBase()
        {
            greports = new gReports();
            activities = new Activities();
            channels = new Channels();
            customerUsageReports = new CustomerUsageReports();
            userUsageReports = new UserUsageReports();
        }

        #endregion

        #region PowerShell Methods
        protected override void BeginProcessing()
        {
            var secrets = CheckForClientSecrets();
            if (secrets != null)
            {
                IEnumerable<string> scopes = EnsureScopesExist(Domain);
                Domain = greports.BuildService(Authenticate(scopes, secrets)).domain;

                GWriteProgress = new gWriteProgress(WriteProgress);
            }
            else
            {
                WriteError(new ErrorRecord(null, (new Exception(
                    "Client Secrets must be set before running cmdlets. Run 'Get-Help " 
                    + "Set-gShellClientSecrets -online' for more information."))));
            }
        }
        #endregion

        #region Authentication & Processing
        /// <summary>
        /// A method specific to each inherited object, called during authentication. Must be implemented.
        /// </summary>
        protected override AuthenticatedUserInfo Authenticate(IEnumerable<string> Scopes, ClientSecrets Secrets)
        {
            return greports.Authenticate(apiNameAndVersion, Scopes, Secrets);
        }
        #endregion

        #region Wrapped Methods
        //the following methods assume that the service has been authenticated first.

        #region Activity
        public class Activities
        {
            public List<Data.Activity> List(gReports.Activities.ApplicationName applicationName, string userKey,
                gReports.Activities.ActivitiesListProperties properties = null)
            {
                properties = (properties != null) ? properties : new gReports.Activities.ActivitiesListProperties();
                properties.startProgressBar = StartProgressBar;
                properties.updateProgressBar = UpdateProgressBar;

                return greports.activities.List(applicationName, userKey, properties);
            }

            public Data.Channel Watch(
                Data.Channel body, string userKey, gReports.Activities.ApplicationName applicationName)
            {
                return greports.activities.Watch(body, userKey, applicationName);
            }
        }
        #endregion

        #region Channels
        public class Channels
        {
            public string Stop(Data.Channel body)
            {
                return greports.channels.Stop(body);
            }
        }
        #endregion

        #region CustomerUsageReports
        public class CustomerUsageReports
        {
            public gUsageReport Get(string date,
                gReports.CustomerUsageReports.CustomerUsageReportsGetProperties properties = null)
            {
                properties = (properties != null) ? properties : new
                    gReports.CustomerUsageReports.CustomerUsageReportsGetProperties();
                properties.startProgressBar = StartProgressBar;
                properties.updateProgressBar = UpdateProgressBar;

                return new gUsageReport(greports.customerUsageReports.Get(date, properties));
            }
        }

        #endregion

        #region UserUsageReports
        public class UserUsageReports
        {
            public gUsageReport Get(string userKey, string date,
                gReports.UserUsageReports.UserUsageReportsGetProperties properties = null)
            {
                properties = (properties != null) ? properties : new
                    gReports.UserUsageReports.UserUsageReportsGetProperties();
                properties.startProgressBar = StartProgressBar;
                properties.updateProgressBar = UpdateProgressBar;

                return new gUsageReport(greports.userUsageReports.Get(userKey, date, properties));
            }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// A powershell friendly representation of the UsageReport class.
    /// </summary>
    public class gUsageReport
    {
        public string Kind;
        public Dictionary<string, List<Data.UsageReport.ParametersData>> properties =
            new Dictionary<string, List<Data.UsageReport.ParametersData>>();
        public List<Data.UsageReports.WarningsData> Warnings = new List<Data.UsageReports.WarningsData>();

        public gUsageReport(Data.UsageReports reports)
        {
            Kind = reports.Kind;

            if (reports.Warnings != null && reports.Warnings.Count != 0)
            {
                Warnings.AddRange(reports.Warnings);
            }

            foreach (Data.UsageReport.ParametersData result in reports.UsageReportsValue[0].Parameters)
            {
                //sites:num_sites, for example
                string[] names = result.Name.Split(':');
                if (!properties.ContainsKey(names[0])) {
                    properties[names[0]] = new List<Data.UsageReport.ParametersData>();
                }

                properties[names[0]].Add(result);
            }
        }

        public List<Data.UsageReport.ParametersData> this[string key]
        {
            get
            {
                return properties[key];
            }
            set
            {
                properties[key] = value;
            }
        }
    }
}
