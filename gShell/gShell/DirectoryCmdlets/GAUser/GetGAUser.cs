﻿using System.Collections.Generic;
using System.Management.Automation;
using Google.Apis.Admin.Directory.directory_v1;
using Google.Apis.Admin.Directory.directory_v1.Data;

namespace gShell.DirectoryCmdlets.GAUser
{
    [Cmdlet(VerbsCommon.Get, "GAUser",
          DefaultParameterSetName = "OneUser",
          SupportsShouldProcess = true)]
    public class GetGAUser : GetGAUserBase
    {
        #region Properties

        [Parameter(Position = 0,
            ParameterSetName = "OneUser",
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Help Text")]
        [ValidateNotNullOrEmpty]
        public string UserName { get; set; }

        //Domain position = 1

        [Parameter(Position = 2,
            ParameterSetName = "AllUsers")]
        public SwitchParameter All { get; set; }

        [Parameter(Position = 3,
            ParameterSetName = "AllUsers")]
        public SwitchParameter Cache { get; set; }

        [Parameter(Position = 4,
            ParameterSetName = "AllUsers")]
        public SwitchParameter ForceCacheReload { get; set; }

        #endregion

        protected override void ProcessRecord()
        {
            switch (ParameterSetName)
            {
                case "OneUser":
                    WriteObject(GetOneCustomUser());
                    break;

                case "AllUsers":
                    if (Cache)
                    {
                        WriteObject(GetAllCustomCachedUsers());
                    } else {
                        WriteObject(GetAllCustomUsers());
                    }
                    break;
            }
        }

        private CustomGAUserObject GetOneCustomUser()
        {
            return (new CustomGAUserObject(GetOneUser(UserName)));
        }

        private List<CustomGAUserObject> GetAllCustomUsers()
        {
            return (CustomGAUserObject.ConvertList(GetAllUsers()));
        }

        private List<CustomGAUserObject> GetAllCustomCachedUsers()
        {
            return (CustomGAUserObject.ConvertList(RetrieveCachedUsers(ForceCacheReload)));
        }
    }

    public class GetGAUserBase : DirectoryBase
    {
        /// <summary>
        /// Retrieve a list of all users from the cache or, if it doesn't exist, the internet.
        /// </summary>
        /// <returns></returns>
        protected List<User> RetrieveCachedUsers(bool forcedReload=false)
        {
            List<User> usersList = new List<User>();

            if (cachedDomainUsers.ContainsKey(Domain) && !forcedReload)
            {
                usersList = cachedDomainUsers[Domain];
            }
            else
            {
                usersList = GetAllUsers();
                cachedDomainUsers[Domain] = usersList;
            }

            return usersList;
        }

        protected User GetOneUser(string UserName)
        {
            string fullEmail = GetFullEmailAddress(UserName, Domain);

            User returnedUser = directoryServiceDict[Domain].
                        Users.Get(fullEmail).Execute();

            return (returnedUser);
        }

        protected List<User> GetAllUsers()
        {
            //TODO: Figure out multi-domain accounts
            
            UsersResource.ListRequest request = directoryServiceDict[Domain].Users.List();

            request.Domain = Domain;
            request.MaxResults = 500;

            StartProgressBar("Gathering accounts",
                "-Collecting accounts 1 to " + request.MaxResults.ToString());

            UpdateProgressBar(1, 2, "Gathering accounts", 
                "-Collecting accounts 1 to " + request.MaxResults.ToString());

            Users execution = request.Execute();

            List<User> returnedList = new List<User>();

            returnedList.AddRange(execution.UsersValue);

            while (!string.IsNullOrWhiteSpace(execution.NextPageToken))
            {
                request.PageToken = execution.NextPageToken;
                UpdateProgressBar(5, 10,
                    "Gathering accounts",
                    string.Format("-Collecting users {0} to {1}",
                     (returnedList.Count + 1 ).ToString(),
                     (returnedList.Count + request.MaxResults).ToString()));
                execution = request.Execute();
                returnedList.AddRange(execution.UsersValue);
            }

            UpdateProgressBar(1, 2, "Gathering accounts",
                "-Returning " + returnedList.Count.ToString() + " results.");

            return (returnedList);
        }
    }

    /// <summary>
    /// A custom class to more easily expose common attribtues for viewing
    /// </summary>
    public class CustomGAUserObject
    {
        public string GivenName;
        public string FamilyName;
        public string PrimaryEmail;
        public List<string> Aliases;
        public bool Suspended;
        public string OrgUnitPath;
        public string LastLoginTime;

        public User userObject;

        public CustomGAUserObject(User userObj)
        {
            Aliases = new List<string>();

            FamilyName = userObj.Name.FamilyName;
            GivenName = userObj.Name.GivenName;
            PrimaryEmail = userObj.PrimaryEmail;
            if (null != userObj.Aliases)
            {
                foreach (string alias in userObj.Aliases)
                {
                    Aliases.Add(alias);
                }
            }
            if (userObj.Suspended.HasValue)
            {
                Suspended = userObj.Suspended.Value;
            }
            OrgUnitPath = userObj.OrgUnitPath;
            LastLoginTime = userObj.LastLoginTime;

            userObject = userObj;
        }

        public static List<CustomGAUserObject> ConvertList(List<User> userList) {
            List<CustomGAUserObject> customList = new List<CustomGAUserObject>();

            foreach (User user in userList)
            {
                customList.Add(new CustomGAUserObject(user));
            }

            return (customList);
        }
    }
}
