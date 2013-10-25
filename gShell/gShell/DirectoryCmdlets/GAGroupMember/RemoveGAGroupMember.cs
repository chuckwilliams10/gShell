﻿using System;
using System.Collections.Generic;
using System.Management.Automation;
using gShell.DirectoryCmdlets.GAGroup;
using Google.Apis.Admin.Directory.directory_v1;
using Google.Apis.Admin.Directory.directory_v1.Data;

namespace gShell.DirectoryCmdlets.GAGroupMember
{
    [Cmdlet(VerbsCommon.Remove, "GAGroupMember",
          DefaultParameterSetName = "OneGroup",
          SupportsShouldProcess = true)]
    public class RemoveGAGroupMember : GetGAGroupBase
    {
        #region Properties

        [Parameter(Position = 0,
            ParameterSetName = "OneGroup",
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Help Text")]
        [ValidateNotNullOrEmpty]
        public string GroupName { get; set; }

        //Domain position = 1

        [Parameter(Position = 2,
            Mandatory = true,
            ParameterSetName = "OneGroup")]
        public string UserName { get; set; }

        [Parameter(Position = 3)]
        public SwitchParameter Force { get; set; }
        #endregion

        protected override void ProcessRecord()
        {
            if (ShouldProcess(GroupName, "Remove-GAGroupMember"))
            {
                if (Force || ShouldContinue((String.Format("Group member {0} will be removed from the {1}@{2} group.\nContinue?",
                    UserName, GroupName, Domain)), "Confirm Google Apps Group Member Removal"))
                {
                    try
                    {
                        WriteDebug(string.Format("Attempting to remove member {0} from group {1}@{2}...",
                            UserName, GroupName, Domain));
                        RemoveGroupMember();
                        WriteVerbose(string.Format("Removal of {0} from {1}@{2} completed without error.",
                            UserName, GroupName, Domain));
                    }
                    catch (Exception e)
                    {
                        WriteError(new ErrorRecord(e, e.GetBaseException().ToString(), ErrorCategory.InvalidData, GroupName));
                    }
                }
                else
                {
                    WriteError(new ErrorRecord(new Exception("Group member removal not confirmed"),
                        "", ErrorCategory.InvalidData, GroupName));
                }
            }
        }

        private void RemoveGroupMember()
        {
            GroupName = GetFullEmailAddress(GroupName, Domain);

            UserName = GetFullEmailAddress(UserName, Domain);

            directoryServiceDict[Domain].Members.Delete(GroupName, UserName).Execute();
        }
    }

}