using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CopyObject
{
    class Constants
    {
        public const string strManagementPackSystemLibrary = "System.Library";
        public const string strManagementPackIncidentManagementLibrary = "ServiceManager.IncidentManagement.Library";
        public const string strManagementPackIncidentLibrary = "System.WorkItem.Incident.Library";
        public const string strManagementPackWorkItemLibrary = "System.WorkItem.Library";

        public const string strTypeProjectionIncident = "System.WorkItem.Incident.ProjectionType";

        public const string strClassIncident = "System.WorkItem.Incident";
        public const string strClassIncidentSettings = "System.WorkItem.Incident.GeneralSetting";
        public const string strClassActionLog = "System.WorkItem.TroubleTicket.ActionLog";
        public const string strClassAnalystComment = "System.WorkItem.TroubleTicket.AnalystCommentLog";
        public const string strClassAuditComment = "System.WorkItem.TroubleTicket.AuditCommentLog";
        public const string strClassUserComment = "System.WorkItem.TroubleTicket.UserCommentLog";
        public const string strClassNotificationLog = "System.WorkItem.TroubleTicket.NotificationLog";
        public const string strClassUser = "System.Domain.User";

        public const string strRelationshipWorkItemRelatesToWorkItem = "System.WorkItemRelatesToWorkItem";
        public const string strRelationshipCreatedByUser = "System.WorkItemCreatedByUser";
        /*
        public const string strRelationshipAffectedUser = "System.WorkItemAffectedUser";
        public const string strRelationshipAssignedToUser = "System.WorkItemAssignedToUser";
        
        public const string strRelationshipPrimaryOwnerUser = "System.WorkItem.IncidentPrimaryOwner";
        public const string strRelationshipWorkItemAboutConfigItem = "System.WorkItemAboutConfigItem";
        public const string strRelationshipActionLog = "System.WorkItem.TroubleTicketHasActionLog";
        public const string strRelationshipAnalystComment = "System.WorkItem.TroubleTicketHasAnalystComment";
        public const string strRelationshipAuditComment= "System.WorkItem.TroubleTicketHasAuditComment";
        public const string strRelationshipUserComment = "System.WorkItem.TroubleTicketHasUserComment";
        public const string strRelationshipNotificationLog = "System.WorkItem.TroubleTicketHasNotificationLog";
         * */
        
        public const string strPropertyPrefixForID = "PrefixForId";
        public const string strPropertyId = "Id";
        public const string strPropertyCreatedDate = "CreatedDate";
        public const string strPropertyUserName = "UserName";
        public const string strPropertyDomain = "Domain";
        public const string strPropertyStatus = "Status";
        public const string strPropertyTargetResolutionTime = "TargetResolutionTime";
        public const string strPropertyResolutionCategory = "ResolutionCategory";
        public const string strPropertyResolutionDescription = "ResolutionDescription";
        public const string strPropertyClosedDate = "ClosedDate";
        public const string strPropertyDisplayName = "DisplayName";
        public const string strPropertyTitle = "Title";

        public const string strMPAttributeName = "Name";

        public const string strEnumerationIncidentStatusActive = "IncidentStatusEnum.Active";

        public const string strAliasCreatedByUser = "CreatedByUser";
        public const string strAliasClosedByUser = "ClosedByUser";
        public const string strAliasResolvedByUser = "ResolvedByUser";
        public const string strAliasActionLogs = "ActionLogs";
        public const string strAliasUserComments = "UserComments";
        public const string strAliasAnalystComments = "AnalystComments";
        public const string strAliasSMTPNotifications = "SMTPNotifications";
        public const string strAliasActivities = "Activities";
        public const string strAliasFileAttachments = "FileAttachments";

    }
}
