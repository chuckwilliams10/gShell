﻿using System;
using System.Collections.Generic;
using System.Management.Automation;
using Data = Google.Apis.Admin.Directory.directory_v1.Data;

namespace gShell.Cmdlets.Directory.GAUserAlias
{
    [Cmdlet(VerbsCommon.New, "GAUserAlias",
          DefaultParameterSetName = "PasswordGenerated",
          SupportsShouldProcess = true,
          HelpUri = @"https://github.com/squid808/gShell/wiki/New-GAUserAlias")]
    public class NewGAUserAlias : DirectoryBase
    {
        #region Properties

        [Parameter(Position = 0,
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "The user's main username")]
        [ValidateNotNullOrEmpty]
        public string UserName { get; set; }

        //Domain position = 1

        [Parameter(Position = 2,
            Mandatory = true)]
        public string Alias { get; set; }

        #endregion

        protected override void ProcessRecord()
        {
            if (ShouldProcess(UserName, "New-GAUserAlias"))
            {
                CreateUserAlias();
            }
        }

        private void CreateUserAlias()
        {
            Data.Alias aliasBody = new Data.Alias()
            {
                AliasValue = GetFullEmailAddress(Alias, Domain)
            };

            users.aliases.Insert(aliasBody, UserName, Domain);
        }

    }
}
