﻿using System;
using System.Management.Automation;
using System.Collections.Generic;
using gShell.OAuth2;

namespace gShell.UtilityCmdlets.ServiceAccount
{
    [Cmdlet(VerbsCommon.Set, "GAServiceAccount",
          SupportsShouldProcess = true)]
    public class SetGAServiceAccount : PSCmdlet
    {
        #region Properties

        [Parameter(Position = 0,
            Mandatory = true,
            ValueFromPipeline = true)]
        [ValidateNotNullOrEmpty]
        public string Email { get; set; }

        [Parameter(Position = 1,
            Mandatory = true,
            ValueFromPipeline = true)]
        [ValidateNotNullOrEmpty]
        public string CertificatePath { get; set; }

        #endregion

        protected override void ProcessRecord()
        {
            SavedFile.SetServiceAccountInfo(Email, CertificatePath);
        }

    }
}