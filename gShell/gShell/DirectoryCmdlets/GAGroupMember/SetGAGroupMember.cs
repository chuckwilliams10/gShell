﻿using System;
using System.Collections.Generic;
using System.Management.Automation;
using gShell.DirectoryCmdlets.GAGroup;
using Google.Apis.Admin.Directory.directory_v1;
using Google.Apis.Admin.Directory.directory_v1.Data;

namespace gShell.DirectoryCmdlets.GAGroupMember
{
    [Cmdlet(VerbsCommon.Set, "GAGroupMember",
          DefaultParameterSetName = "OneGroup",
          SupportsShouldProcess = true)]
    public class SetGAGroupMember : GetGAGroupBase
    {
        #region Properties

        public enum GroupMembershipRoles { MEMBER, MANAGER, OWNER };

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

        [Parameter(Position = 3,
            Mandatory = true,
            ParameterSetName = "OneGroup")]
        public GroupMembershipRoles Role { get; set; }
        #endregion

        protected override void ProcessRecord()
        {
            UpdateGroupMember();
        }

        private void UpdateGroupMember()
        {
            GroupName = GetFullEmailAddress(GroupName, Domain);
            UserName = GetFullEmailAddress(UserName, Domain);

            //Member member = directoryServiceDict[Domain].Members.Get(GroupName, UserName).Execute();
            
            Member member = new Member
            {
                Role = this.Role.ToString()
            };

            directoryServiceDict[Domain].Members.Update(member, GroupName, UserName).Execute();
        }
    }

}