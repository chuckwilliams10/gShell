﻿using System;
using System.Collections.Generic;
using directory_v1 = Google.Apis.Admin.Directory.directory_v1;
using Data = Google.Apis.Admin.Directory.directory_v1.Data;
using gShell.dotNet;
using gShell.dotNet.Utilities.OAuth2;

namespace gShell.dotNet
{
    /// <summary>
    /// A consumer of the Admin Directory API that includes gShell authentication.
    /// </summary>
    public class Directory : ServiceWrapper<directory_v1.DirectoryService>
    {
        #region Inherited Members

        /// <summary>
        /// Indicates if this set of services will work with Gmail (as opposed to Google Apps). 
        /// This will cause authentication to fail if false and the user attempts to authenticate with
        /// a gmail address.
        /// </summary>
        protected override bool worksWithGmail { get { return false; } }

        /// <summary>
        /// Initialize and return a new DirectoryService
        /// </summary>
        protected override directory_v1.DirectoryService CreateNewService(string domain)
        {
            return new directory_v1.DirectoryService(OAuth2Base.GetInitializer(domain));
        }

        public override string apiNameAndVersion { get { return "admin:directory_v1"; } }

        #endregion

        #region Properties

        public ChromeosDevices chromeosDevices = new ChromeosDevices();
        public Groups groups = new Groups();
        public Members members = new Members();
        public MobileDevices mobileDevices = new MobileDevices();
        public Orgunits orgunits = new Orgunits();
        public Users users = new Users();
        public Asps asps = new Asps();
        public Tokens tokens = new Tokens();
        public VerificationCodes verificationCodes = new VerificationCodes();
        public Notifications notifications = new Notifications();
        public Channels channels = new Channels();
        public Schemas schemas = new Schemas();
        
        #endregion

        #region Wrapped Methods

        //the following methods assume that the service has been authenticated first.

        #region Chromeosdevices

        public class ChromeosDevices
        {
            public class ChromeosDevicesListProperties
            {
                public int maxResults=100;
                public directory_v1.ChromeosdevicesResource.ListRequest.OrderByEnum? orderBy=null;
                public directory_v1.ChromeosdevicesResource.ListRequest.ProjectionEnum? projection=null;
                public directory_v1.ChromeosdevicesResource.ListRequest.SortOrderEnum? sortOrder = null;
                public Action<string, string> startProgressBar = null;
                public Action<int, int, string, string> updateProgressBar = null;
                public int totalResults = 0;
            }

            public Data.ChromeOsDevice Get(string customerId, string deviceId,
                directory_v1.ChromeosdevicesResource.GetRequest.ProjectionEnum? projection=null)
            {
                directory_v1.ChromeosdevicesResource.GetRequest request =
                 GetService().Chromeosdevices.Get(customerId, deviceId);
                request.Projection = projection;
                return request.Execute();
            }

            public List<Data.ChromeOsDevice> List(string customerId, ChromeosDevicesListProperties properties = null)
            {
                List<Data.ChromeOsDevice> results = new List<Data.ChromeOsDevice>();

                directory_v1.ChromeosdevicesResource.ListRequest request = 
                 GetService().Chromeosdevices.List(customerId);

                if (properties != null)
                {
                    request.MaxResults = properties.maxResults;
                    request.OrderBy = properties.orderBy;
                    request.Projection = properties.projection;
                    request.SortOrder = properties.sortOrder;
                }

                string resultObjType = "Chrome OS Devices";

                if (null != properties.startProgressBar)
                {
                    properties.startProgressBar("Gathering " + resultObjType,
                        string.Format("-Collecting {0} {1} to {2}", resultObjType, "1", request.MaxResults.ToString()));
                }

                Data.ChromeOsDevices pagedResult = request.Execute();

                if (pagedResult.Chromeosdevices != null && pagedResult.Chromeosdevices.Count != 0)
                {
                    results.AddRange(pagedResult.Chromeosdevices);
                }

                while (!string.IsNullOrWhiteSpace(pagedResult.NextPageToken) &&
                    pagedResult.NextPageToken != request.PageToken &&
                (properties.totalResults == 0 || results.Count < properties.totalResults))
                {
                    request.PageToken = pagedResult.NextPageToken;
                    if (null != properties.updateProgressBar)
                    {
                        properties.updateProgressBar(5, 10, "Gathering " + resultObjType,
                                string.Format("-Collecting {0} {1} to {2}",
                                    resultObjType,
                                    (results.Count + 1).ToString(),
                                    (results.Count + request.MaxResults).ToString()));
                    }
                    pagedResult = request.Execute();

                    if (pagedResult.Chromeosdevices != null && pagedResult.Chromeosdevices.Count != 0)
                    {
                        results.AddRange(pagedResult.Chromeosdevices);
                    }
                }

                if (null != properties.updateProgressBar)
                {
                    properties.updateProgressBar(1, 2, "Gathering " + resultObjType,
                            string.Format("-Returning {0} results.", results.Count.ToString()));
                }

                return results;
            }

            public Data.ChromeOsDevice Patch(Data.ChromeOsDevice body, string customerId, string deviceId,
                directory_v1.ChromeosdevicesResource.PatchRequest.ProjectionEnum? projection=null)
            {
                directory_v1.ChromeosdevicesResource.PatchRequest request =
                 GetService().Chromeosdevices.Patch(body, customerId, deviceId);
                request.Projection = projection;
                return request.Execute();
            }

            public Data.ChromeOsDevice Update(Data.ChromeOsDevice body, string customerId, string deviceId,
                directory_v1.ChromeosdevicesResource.UpdateRequest.ProjectionEnum? projection = null)
            {
                directory_v1.ChromeosdevicesResource.UpdateRequest request =
                 GetService().Chromeosdevices.Update(body, customerId, deviceId);
                request.Projection = projection;
                return request.Execute();
            }
        }

        #endregion

        #region Groups

        public class Groups
        {
            public Aliases aliases = new Aliases();

            public class GroupsListProperties
            {
                public string customer = null;
                public string domain = null;
                public string userKey = null;
                public int maxResults = 200;
                public Action<string, string> startProgressBar = null;
                public Action<int, int, string, string> updateProgressBar = null;
                public int totalResults = 0;
            }

            public string Delete(string groupKey)
            {
                return GetService().Groups.Delete(groupKey).Execute();
            }

            public Data.Group Get(string groupKey)
            {
                return GetService().Groups.Get(groupKey).Execute();
            }

            public Data.Group Insert(Data.Group body)
            {
                return GetService().Groups.Insert(body).Execute();
            }

            public List<Data.Group> List(GroupsListProperties properties = null)
            {
                List<Data.Group> results = new List<Data.Group>();

                directory_v1.GroupsResource.ListRequest request =
                 GetService().Groups.List();

                if (null != properties)
                {
                    request.MaxResults = properties.maxResults;
                    request.Domain = properties.domain;
                    request.Customer = properties.customer;
                    request.UserKey = properties.userKey;
                }

                string resultObjType = "groups";

                if (null != properties.startProgressBar)
                {
                    properties.startProgressBar("Gathering " + resultObjType,
                        string.Format("-Collecting {0} {1} to {2}", resultObjType, "1", request.MaxResults.ToString()));
                }
                
                Data.Groups pagedResult = request.Execute();

                if (pagedResult.GroupsValue != null && pagedResult.GroupsValue.Count != 0)
                {
                    results.AddRange(pagedResult.GroupsValue);
                }

                while (!string.IsNullOrWhiteSpace(pagedResult.NextPageToken) &&
                    pagedResult.NextPageToken != request.PageToken &&
                (properties.totalResults == 0 || results.Count < properties.totalResults))
                {
                    request.PageToken = pagedResult.NextPageToken;
                    if (null != properties.updateProgressBar)
                    {
                        properties.updateProgressBar(5, 10, "Gathering " + resultObjType,
                                string.Format("-Collecting {0} {1} to {2}",
                                    resultObjType,
                                    (results.Count + 1).ToString(),
                                    (results.Count + request.MaxResults).ToString()));
                    }
                    pagedResult = request.Execute();

                    if (pagedResult.GroupsValue != null && pagedResult.GroupsValue.Count != 0)
                    {
                        results.AddRange(pagedResult.GroupsValue);
                    }
                }

                if (null != properties.updateProgressBar)
                {
                    properties.updateProgressBar(1, 2, "Gathering " + resultObjType,
                            string.Format("-Returning {0} results.", results.Count.ToString()));

                }
                return results;
            }

            public Data.Group Patch(Data.Group body, string groupKey)
            {
                return GetService().Groups.Patch(body, groupKey).Execute();
            }

            public Data.Group Update(Data.Group body, string groupKey)
            {
                return GetService().Groups.Update(body, groupKey).Execute();
            }

            #region Groups.aliases

            public class Aliases
            {
                public string Delete(string groupKey, string alias)
                {
                    return GetService().Groups.Aliases.Delete(groupKey, alias).Execute();
                }

                public Data.Alias Insert(Data.Alias body, string groupKey)
                {
                    return GetService().Groups.Aliases.Insert(body, groupKey).Execute();
                }

                public List<Data.Alias> List(string groupKey)
                {
                    List<Data.Alias> results = new List<Data.Alias>();

                    directory_v1.GroupsResource.AliasesResource.ListRequest request =
                     GetService().Groups.Aliases.List(groupKey);

                    Data.Aliases pagedResult = request.Execute();

                    if (pagedResult.AliasesValue != null && pagedResult.AliasesValue.Count != 0)
                    {
                        results.AddRange(pagedResult.AliasesValue);
                    }

                    return results;
                }
            }

            #endregion

        }

        #endregion

        #region Members

        public class Members
        {
            public class MembersListProperties
            {
                public string roles = null;
                public int maxResults = 200;
                public Action<string, string> startProgressBar = null;
                public Action<int, int, string, string> updateProgressBar = null;
                public int totalResults = 0;
            }

            public string Delete(string groupKey, string memberKey)
            {
                return GetService().Members.Delete(groupKey, memberKey).Execute();
            }

            public Data.Member Get(string groupKey, string memberKey)
            {
                return GetService().Members.Get(groupKey, memberKey).Execute();
            }

            public Data.Member Insert(Data.Member body, string groupKey)
            {
                return GetService().Members.Insert(body, groupKey).Execute();
            }

            public List<Data.Member> List(string groupKey, MembersListProperties properties = null)
            {
                List<Data.Member> results = new List<Data.Member>();
                
                directory_v1.MembersResource.ListRequest request =
                 GetService().Members.List(groupKey);

                if (null != properties)
                {
                    request.Roles = properties.roles;
                    request.MaxResults = properties.maxResults;
                }

                string resultObjType = "group members";

                if (null != properties.startProgressBar)
                {
                    properties.startProgressBar("Gathering " + resultObjType,
                        string.Format("-Collecting {0} {1} to {2}", resultObjType, "1", request.MaxResults.ToString()));
                }

                Data.Members pagedResult = request.Execute();

                if (pagedResult.MembersValue != null && pagedResult.MembersValue.Count != 0)
                {
                    results.AddRange(pagedResult.MembersValue);
                }

                while (!string.IsNullOrWhiteSpace(pagedResult.NextPageToken) &&
                    pagedResult.NextPageToken != request.PageToken &&
                (properties.totalResults == 0 || results.Count < properties.totalResults))
                {
                    request.PageToken = pagedResult.NextPageToken;
                    if (null != properties.updateProgressBar)
                    {
                        properties.updateProgressBar(5, 10, "Gathering " + resultObjType,
                                string.Format("-Collecting {0} {1} to {2}",
                                    resultObjType,
                                    (results.Count + 1).ToString(),
                                    (results.Count + request.MaxResults).ToString()));
                    }

                    pagedResult = request.Execute();

                    if (pagedResult.MembersValue != null && pagedResult.MembersValue.Count != 0)
                    {
                        results.AddRange(pagedResult.MembersValue);
                    }
                }

                if (null != properties.updateProgressBar)
                {
                    properties.updateProgressBar(1, 2, "Gathering " + resultObjType,
                            string.Format("-Returning {0} results.", results.Count.ToString()));
                }

                return results;
            }

            public Data.Member Patch(Data.Member body, string groupKey, string memberKey)
            {
                return GetService().Members.Patch(body, groupKey, memberKey).Execute();
            }

            public Data.Member Update(Data.Member body, string groupKey, string memberKey)
            {
                return GetService().Members.Update(body, groupKey, memberKey).Execute();
            }
        }

        #endregion

        #region MobileDevices

        public class MobileDevices
        {
            public class MobileDevicesPropertiesList
            {
                public int maxResults = 100;
                public directory_v1.MobiledevicesResource.ListRequest.OrderByEnum? orderBy = null;
                public directory_v1.MobiledevicesResource.ListRequest.ProjectionEnum? projection = null;
                public directory_v1.MobiledevicesResource.ListRequest.SortOrderEnum? sortOrder = null;
                public Action<string, string> startProgressBar = null;
                public Action<int, int, string, string> updateProgressBar = null;
                public int totalResults = 0;
            }

            public enum MobileDeviceAction
            {
                admin_remote_wipe, admin_account_wipe, approve, block, cancel_remote_wipe_then_activate, cancel_remote_wipe_then_block
            }

            public string Action(Data.MobileDeviceAction body, string customerId, string resourceId)
            {
                return GetService().Mobiledevices.Action(body, customerId, resourceId).Execute();
            }

            public string Delete(string customerId, string resourceId)
            {
                return GetService().Mobiledevices.Delete(customerId, resourceId).Execute();
            }

            public Data.MobileDevice Get(string customerId, string resourceId,
                directory_v1.MobiledevicesResource.GetRequest.ProjectionEnum? projection = null)
            {
                directory_v1.MobiledevicesResource.GetRequest request = 
                    GetService().Mobiledevices.Get(customerId, resourceId);
                request.Projection = projection;
                return request.Execute();
            }

            public List<Data.MobileDevice> List(string customerId,MobileDevicesPropertiesList properties = null)
            {
                List<Data.MobileDevice> results = new List<Data.MobileDevice>();

                directory_v1.MobiledevicesResource.ListRequest request = 
                    GetService().Mobiledevices.List(customerId);

                if (null != properties)
                {
                    request.MaxResults = properties.maxResults;
                    request.OrderBy = properties.orderBy;
                    request.Projection = properties.projection;
                    request.SortOrder = properties.sortOrder;
                }

                string resultObjType = "mobile devices";

                if (null != properties.startProgressBar)
                {
                    properties.startProgressBar("Gathering " + resultObjType,
                        string.Format("-Collecting {0} {1} to {2}", resultObjType, "1", request.MaxResults.ToString()));
                }

                Data.MobileDevices pagedResult = request.Execute();

                if (pagedResult.Mobiledevices != null && pagedResult.Mobiledevices.Count != 0)
                {
                    results.AddRange(pagedResult.Mobiledevices);
                }

                while (!string.IsNullOrWhiteSpace(pagedResult.NextPageToken) &&
                    pagedResult.NextPageToken != request.PageToken &&
                (properties.totalResults == 0 || results.Count < properties.totalResults))
                {
                    request.PageToken = pagedResult.NextPageToken;
                    if (null != properties.updateProgressBar)
                    {
                        properties.updateProgressBar(5, 10, "Gathering " + resultObjType,
                                string.Format("-Collecting {0} {1} to {2}",
                                    resultObjType,
                                    (results.Count + 1).ToString(),
                                    (results.Count + request.MaxResults).ToString()));
                    }
                    pagedResult = request.Execute();
                    if (pagedResult.Mobiledevices != null && pagedResult.Mobiledevices.Count != 0)
                    {
                        results.AddRange(pagedResult.Mobiledevices);
                    }
                }

                if (null != properties.updateProgressBar)
                {
                    properties.updateProgressBar(1, 2, "Gathering " + resultObjType,
                            string.Format("-Returning {0} results.", results.Count.ToString()));
                }

                return results;
            }
        }

        #endregion

        #region Orgunits

        public class Orgunits
        {
            public class OrgunitsListProperties
            {
                public string orgUnitPath=null;
                public directory_v1.OrgunitsResource.ListRequest.TypeEnum? type=null;
            }
            public string Delete(string customerId, Google.Apis.Util.Repeatable<string> orgUnitPath)
            {
                return GetService().Orgunits.Delete(customerId, orgUnitPath).Execute();
            }

            public Data.OrgUnit Get(string customerId, Google.Apis.Util.Repeatable<string> orgUnitPath)
            {
                return GetService().Orgunits.Get(customerId, orgUnitPath).Execute();
            }

            public Data.OrgUnit Insert(Data.OrgUnit body, string customerId)
            {
                return GetService().Orgunits.Insert(body, customerId).Execute();
            }

            public List<Data.OrgUnit> List(string customerId, OrgunitsListProperties properties = null)
            {
                List<Data.OrgUnit> results = new List<Data.OrgUnit>();

                directory_v1.OrgunitsResource.ListRequest request =
                    GetService().Orgunits.List(customerId);

                if (null != properties)
                {
                    request.OrgUnitPath = properties.orgUnitPath;
                    request.Type = properties.type;
                }

                Data.OrgUnits pagedResult = request.Execute();

                if (pagedResult.OrganizationUnits != null && pagedResult.OrganizationUnits.Count != 0)
                {
                    results.AddRange(pagedResult.OrganizationUnits);
                }

                return results;
            }

            public Data.OrgUnit Patch(Data.OrgUnit body, string customerId, Google.Apis.Util.Repeatable<string> orgUnitPath)
            {
                return GetService().Orgunits.Patch(body, customerId, orgUnitPath).Execute();
            }

            public Data.OrgUnit Update(Data.OrgUnit body, string customerId, Google.Apis.Util.Repeatable<string> orgUnitPath)
            {
                return GetService().Orgunits.Update(body, customerId, orgUnitPath).Execute();
            }
        }

        #endregion

        #region Users

        public class Users
        {
            public Photos photos = new Photos();
            public Aliases aliases = new Aliases();

            public class UsersListProperties
            {
                public string customer = null;
                public string domain = null;
                public string fields = null;
                public int maxResults = 100;
                public string query = null;
                public bool showDeleted = false;
                public directory_v1.UsersResource.ListRequest.OrderByEnum? orderBy = null;
                public directory_v1.UsersResource.ListRequest.SortOrderEnum? sortOrder = null;
                public directory_v1.UsersResource.ListRequest.ViewTypeEnum? viewType = null;
                public directory_v1.UsersResource.ListRequest.ProjectionEnum? projection = null;
                public string customFieldMask = null;
                public Action<string, string> startProgressBar = null;
                public Action<int, int, string, string> updateProgressBar = null;
                public int totalResults = 0;
            }

            public string Delete(string userKey)
            {
                return GetService().Users.Delete(userKey).Execute();
            }

            public Data.User Get(string userKey,
                directory_v1.UsersResource.GetRequest.ProjectionEnum? projection = null,
                directory_v1.UsersResource.GetRequest.ViewTypeEnum? viewType = null)
            {
                directory_v1.UsersResource.GetRequest request = 
                    GetService().Users.Get(userKey);

                request.Projection = projection;
                request.ViewType = viewType;

                return request.Execute();
            }

            public Data.User Insert(Data.User body)
            {
                return GetService().Users.Insert(body).Execute();
            }

            public List<Data.User> List(UsersListProperties properties = null)
            {
                List<Data.User> results = new List<Data.User>();

                directory_v1.UsersResource.ListRequest request =
                    GetService().Users.List();

                if (null != properties)
                {
                    if (null != properties.projection) { request.CustomFieldMask = properties.customFieldMask; }
                    request.Customer = properties.customer;
                    request.Domain = properties.domain;
                    request.Fields = properties.fields;
                    request.MaxResults = properties.maxResults;
                    request.OrderBy = properties.orderBy;
                    request.Projection = properties.projection;
                    request.Query = properties.query;
                    request.ShowDeleted = properties.showDeleted.ToString().ToLower();
                    request.SortOrder = properties.sortOrder;
                    request.ViewType = properties.viewType;
                }

                string resultObjType = "users";

                if (null != properties.startProgressBar)
                {
                    properties.startProgressBar("Gathering " + resultObjType,
                        string.Format("-Collecting {0} {1} to {2}", resultObjType, "1", request.MaxResults.ToString()));
                }

                Data.Users pagedResult = request.Execute();

                if (pagedResult.UsersValue != null && pagedResult.UsersValue.Count != 0)
                {
                    results.AddRange(pagedResult.UsersValue);
                }

                while (!string.IsNullOrWhiteSpace(pagedResult.NextPageToken) &&
                    pagedResult.NextPageToken != request.PageToken &&
                (properties.totalResults == 0 || results.Count < properties.totalResults))
                {
                    request.PageToken = pagedResult.NextPageToken;
                    if (null != properties.updateProgressBar)
                    {
                        properties.updateProgressBar(5, 10, "Gathering " + resultObjType,
                                string.Format("-Collecting {0} {1} to {2}",
                                    resultObjType,
                                    (results.Count + 1).ToString(),
                                    (results.Count + request.MaxResults).ToString()));
                    }
                    pagedResult = request.Execute();

                    if (pagedResult.UsersValue != null && pagedResult.UsersValue.Count != 0)
                    {
                        results.AddRange(pagedResult.UsersValue);
                    }
                }

                if (null != properties.updateProgressBar)
                {
                    properties.updateProgressBar(1, 2, "Gathering " + resultObjType,
                        string.Format("-Returning {0} results.", results.Count.ToString()));
                }

                return results;
            }

            public string MakeAdmin(Data.UserMakeAdmin body, string userKey)
            {
                return GetService().Users.MakeAdmin(body, userKey).Execute();
            }

            public Data.User Patch(Data.User body, string userKey)
            {
                return GetService().Users.Patch(body, userKey).Execute();
            }

            public string Undelete(Data.UserUndelete body, string userKey)
            {
                return GetService().Users.Undelete(body, userKey).Execute();
            }

            public Data.User Update(Data.User body, string userKey)
            {
                return GetService().Users.Update(body, userKey).Execute();
            }

            public Data.Channel Watch(Data.Channel body)
            {
                return GetService().Users.Watch(body).Execute();
            }

            #region Users.aliases
            public class Aliases
            {
                public string Delete(string userKey, string alias)
                {
                    return GetService().Users.Aliases.Delete(userKey, alias).Execute();
                }

                public Data.Alias Insert(Data.Alias body, string userKey)
                {
                    return GetService().Users.Aliases.Insert(body, userKey).Execute();
                }

                public List<Data.Alias> List(string userKey)
                {
                    List<Data.Alias> results = new List<Data.Alias>();

                    directory_v1.UsersResource.AliasesResource.ListRequest request =
                        GetService().Users.Aliases.List(userKey);

                    Data.Aliases pagedResult = request.Execute();

                    if (pagedResult.AliasesValue != null && pagedResult.AliasesValue.Count != 0)
                    {
                        results.AddRange(pagedResult.AliasesValue);
                    }

                    return results;
                }

                public Data.Channel Watch(Data.Channel body, string userKey)
                {
                    return GetService().Users.Aliases.Watch(body, userKey).Execute();
                }
            }
            #endregion

            #region Users.photos
            public class Photos
            {
                public string Delete(string userKey)
                {
                    return GetService().Users.Photos.Delete(userKey).Execute();
                }

                public Data.UserPhoto Get(string userKey)
                {
                    return GetService().Users.Photos.Get(userKey).Execute();
                }

                public Data.UserPhoto Patch(Data.UserPhoto body, string userKey)
                {
                    return GetService().Users.Photos.Patch(body, userKey).Execute();
                }

                public Data.UserPhoto Update(Data.UserPhoto body, string userKey)
                {
                    return GetService().Users.Photos.Update(body, userKey).Execute();
                }
            }
            #endregion
        }

        #endregion

        #region Asps

        public class Asps
        {
            public string Delete(string userKey, int codeId)
            {
                return GetService().Asps.Delete(userKey, codeId).Execute();
            }

            public Data.Asp Get(string userKey, int codeId)
            {
                return GetService().Asps.Get(userKey, codeId).Execute();
            }

            public List<Data.Asp> List(string userKey)
            {
                List<Data.Asp> results = new List<Data.Asp>();

                directory_v1.AspsResource.ListRequest request =
                    GetService().Asps.List(userKey);

                Data.Asps pagedResult = request.Execute();

                if (pagedResult.Items != null && pagedResult.Items.Count != 0)
                {
                    results.AddRange(pagedResult.Items);
                }

                return results;
            }
        }

        #endregion

        #region Tokens

        public class Tokens
        {
            public string Delete(string userKey, string clientId)
            {
                return GetService().Tokens.Delete(userKey, clientId).Execute();
            }

            public Data.Token Get(string userKey, string clientId)
            {
                return GetService().Tokens.Get(userKey, clientId).Execute();
            }

            public List<Data.Token> List(string userKey)
            {
                List<Data.Token> results = new List<Data.Token>();

                directory_v1.TokensResource.ListRequest request =
                    GetService().Tokens.List(userKey);

                Data.Tokens pagedResult = request.Execute();

                if (pagedResult.Items != null && pagedResult.Items.Count != 0)
                {
                    results.AddRange(pagedResult.Items);
                }

                return results;
            }
        }

        #endregion

        #region VerificationCodes

        public class VerificationCodes
        {
            public string Generate(string userKey)
            {
                return GetService().VerificationCodes.Generate(userKey).Execute();
            }

            public string Invalidate(string userKey)
            {
                return GetService().VerificationCodes.Invalidate(userKey).Execute();
            }

            public List<Data.VerificationCode> List(string userKey)
            {
                List<Data.VerificationCode> results = new List<Data.VerificationCode>();

                directory_v1.VerificationCodesResource.ListRequest request =
                    GetService().VerificationCodes.List(userKey);

                Data.VerificationCodes pagedResult = request.Execute();

                if (pagedResult.Items != null && pagedResult.Items.Count != 0)
                {
                    results.AddRange(pagedResult.Items);
                }

                return results;
            }
        }

        #endregion

        #region Notifications

        public class Notifications
        {
            public class NotificationsListProperties
            {
                public int maxResults = 100;
                public string language = null;
                public Action<string, string> startProgressBar = null;
                public Action<int, int, string, string> updateProgressBar = null;
                public int totalResults = 0;
            }

            public string Delete(string customer, string notificationId)
            {
                return GetService().Notifications.Delete(customer, notificationId).Execute();
            }

            public Data.Notification Get(string customer, string notificationId)
            {
                return GetService().Notifications.Get(customer, notificationId).Execute();
            }

            public List<Data.Notification> List(string customer, NotificationsListProperties properties = null)
            {
                List<Data.Notification> results = new List<Data.Notification>();

                directory_v1.NotificationsResource.ListRequest request =
                    GetService().Notifications.List(customer);

                if (null != properties)
                {
                    request.Language = properties.language;
                    request.MaxResults = properties.maxResults;
                }

                string resultObjType = "notifications";

                if (null != properties.startProgressBar)
                {
                    properties.startProgressBar("Gathering " + resultObjType,
                        string.Format("-Collecting {0} {1} to {2}", resultObjType, "1", request.MaxResults.ToString()));
                }

                Data.Notifications pagedResult = request.Execute();

                if (pagedResult.Items != null && pagedResult.Items.Count != 0)
                {
                    results.AddRange(pagedResult.Items);
                }

                while (!string.IsNullOrWhiteSpace(pagedResult.NextPageToken) &&
                    pagedResult.NextPageToken != request.PageToken &&
                (properties.totalResults == 0 || results.Count < properties.totalResults))
                {
                    request.PageToken = pagedResult.NextPageToken;
                    if (null != properties.updateProgressBar)
                    {
                        properties.updateProgressBar(5, 10, "Gathering " + resultObjType,
                                string.Format("-Collecting {0} {1} to {2}",
                                    resultObjType,
                                    (results.Count + 1).ToString(),
                                    (results.Count + request.MaxResults).ToString()));
                    }
                    pagedResult = request.Execute();

                    if (pagedResult.Items != null && pagedResult.Items.Count != 0)
                    {
                        results.AddRange(pagedResult.Items);
                    }
                }

                if (null != properties.updateProgressBar)
                {
                    properties.updateProgressBar(1, 2, "Gathering " + resultObjType,
                            string.Format("-Returning {0} results.", results.Count.ToString()));
                }

                return results;
            }

            public Data.Notification Patch(Data.Notification body, string customer, string notificationId)
            {
                return GetService().Notifications.Patch(body, customer, notificationId).Execute();
            }

            public Data.Notification Update(Data.Notification body, string customer, string notificationId)
            {
                return GetService().Notifications.Update(body, customer, notificationId).Execute();
            }
        }

        #endregion

        #region Channels

        public class Channels
        {
            public string Stop(Data.Channel body)
            {
                return GetService().Channels.Stop(body).Execute();
            }
        }

        #endregion

        #region Schemas

        public class Schemas
        {
            public string Delete(string customerId, string schemaKey)
            {
                return GetService().Schemas.Delete(customerId, schemaKey).Execute();
            }

            public Data.Schema Get(string customerId, string schemaKey)
            {
                return GetService().Schemas.Get(customerId, schemaKey).Execute();
            }

            public Data.Schema Insert(Data.Schema body, string customerId)
            {
                return GetService().Schemas.Insert(body, customerId).Execute();
            }

            public List<Data.Schema> List(string customerId)
            {
                List<Data.Schema> results = new List<Data.Schema>();

                directory_v1.SchemasResource.ListRequest request =
                    GetService().Schemas.List(customerId);

                Data.Schemas pagedResult = request.Execute();

                if (pagedResult.SchemasValue != null && pagedResult.SchemasValue.Count != 0)
                {
                    results.AddRange(pagedResult.SchemasValue);
                }

                return results;
            }

            public Data.Schema Patch(Data.Schema body, string customerId, string schemaKey)
            {
                return GetService().Schemas.Patch(body, customerId, schemaKey).Execute();
            }

            public Data.Schema Update(Data.Schema body, string customerId, string schemaKey)
            {
                return GetService().Schemas.Update(body, customerId, schemaKey).Execute();
            }
        }

        #endregion

        //end of wrapped methods
        #endregion
    }
}
