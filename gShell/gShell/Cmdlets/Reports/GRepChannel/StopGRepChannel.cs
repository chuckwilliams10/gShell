﻿using System;
using System.Collections.Generic;
using System.Management.Automation;
using Data = Google.Apis.Admin.Reports.reports_v1.Data;

namespace gShell.Cmdlets.Reports.GRepChannel
{
    [Cmdlet(VerbsLifecycle.Stop, "GRepChannel",
          SupportsShouldProcess = true,
          HelpUri = @"https://github.com/squid808/gShell/wiki/Stop-GRepChannel")]
    public class StopGRepChannel : ReportsBase
    {
        #region Properties

        [Parameter(
            Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Id { get; set; }

        [Parameter(
            Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string ResourceId { get; set; }

        [Parameter()]
        public SwitchParameter Force { get; set; }
        

        #endregion

        protected override void ProcessRecord()
        {
            if (ShouldProcess("Report Channel", "Stop-GRepChannel"))
            {
                if (Force || ShouldContinue((String.Format("Resource with Id {0} will be stopped on channel with Id {1}\nContinue?",
                    ResourceId, Id)), "Confirm Channel Stop"))
                {
                    try
                    {
                        WriteDebug(string.Format("Attempting to stop Channel Resource..."));
                        WriteObject(greports.channels.Stop(new Data.Channel(){
                            Id = Id,
                            ResourceId = ResourceId
                        }));
                        WriteVerbose(string.Format("Channel Resource stopped without error."));
                    }
                    catch (Exception e)
                    {
                        WriteError(new ErrorRecord(e, e.GetBaseException().ToString(), ErrorCategory.InvalidData, "Stop-GRepChannel"));
                    }
                }
                else
                {
                    WriteError(new ErrorRecord(new Exception("Stopping of Channel Resource failed"),
                        "", ErrorCategory.InvalidData, "Stop-GRepChannel"));
                }
            }
        }
    }
}
