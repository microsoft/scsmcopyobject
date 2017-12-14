using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EnterpriseManagement;
using Microsoft.EnterpriseManagement.Common;
using Microsoft.EnterpriseManagement.Configuration;
using Microsoft.EnterpriseManagement.Packaging;

namespace CopyObject
{
    public class Incident
    {
        public string IDToCopy = null;
        public EnterpriseManagementGroup EMG = null;
        public string[] PropertiesToExclude = new string[] { };
        public string[] RelationshipAliasesToExclude = new string[] { };

        public String Copy()
        {
            string strWorkItemID = null;

            #region MPVariables
            ManagementPack mpIncidentLibrary = Common.GetManagementPackByName(Constants.strManagementPackIncidentLibrary, this.EMG);
            ManagementPackTypeProjection mptpIncident = Common.GetManagementPackTypeProjectionByName(Constants.strTypeProjectionIncident, Constants.strManagementPackIncidentManagementLibrary, this.EMG);
            ManagementPackClass mpcIncident = Common.GetManagementPackClassByName(Constants.strClassIncident, Constants.strManagementPackIncidentLibrary, this.EMG);
            ManagementPackRelationship mprCreatedByUser = Common.GetManagementPackRelationshipByName(Constants.strRelationshipCreatedByUser, Constants.strManagementPackWorkItemLibrary, this.EMG);
            ManagementPackEnumeration mpeIncidentStatusActive = Common.GetManagementPackEnummerationByName(Constants.strEnumerationIncidentStatusActive, Constants.strManagementPackIncidentLibrary, this.EMG);
            ManagementPackRelationship mprWorkItemRelatesToWorkItem = Common.GetManagementPackRelationshipByName(Constants.strRelationshipWorkItemRelatesToWorkItem, Constants.strManagementPackWorkItemLibrary, this.EMG);
            #endregion

            string strIncidentIDPrefix = GetIncidentIDPrefix();

            //Create the criteria to get the object by the Work Item ID passed in
            String strIncidentByIDCriteria =
                String.Format(@"<Criteria xmlns=""http://Microsoft.EnterpriseManagement.Core.Criteria/"">" +
                                  "<Expression>" +
                                    "<SimpleExpression>" +
                                      "<ValueExpressionLeft>" +
                                        "<Property>$Target/Property[Type='System.WorkItem.Incident']/Id$</Property>" +
                                      "</ValueExpressionLeft>" +
                                      "<Operator>Equal</Operator>" +
                                      "<ValueExpressionRight>" +
                                        "<Value>{0}</Value>" +
                                      "</ValueExpressionRight>" +
                                    "</SimpleExpression>" +
                                  "</Expression>" +
                                "</Criteria>", this.IDToCopy);
            ObjectProjectionCriteria opcIncidentByID = new ObjectProjectionCriteria(strIncidentByIDCriteria, mptpIncident, mpIncidentLibrary, this.EMG);

            //Get the incident type projection by ID
            IObjectProjectionReader<EnterpriseManagementObject> emopIncidents = this.EMG.EntityObjects.GetObjectProjectionReader<EnterpriseManagementObject>(opcIncidentByID, ObjectQueryOptions.Default);
            foreach (EnterpriseManagementObjectProjection emopIncident in emopIncidents)
            {
                //Note: We are using foreach here but there will be only one since we are searching by the work item ID

                if (this.PropertiesToExclude.Length == 0)
                {
                    //A list of Properties to exclude was not passed in so we are going to go with a default list.
                    this.PropertiesToExclude = new string[] { 
                                                                    Constants.strPropertyId, 
                                                                    Constants.strPropertyCreatedDate,
                                                                    Constants.strPropertyStatus,
                                                                    Constants.strPropertyTargetResolutionTime,
                                                                    Constants.strPropertyResolutionCategory,
                                                                    Constants.strPropertyResolutionDescription,
                                                                    Constants.strPropertyClosedDate,
                                                                    Constants.strPropertyDisplayName
                                                                    };
                }

                //Copy all the properties (including extended properties, except for those specified
                EnterpriseManagementObjectProjection emopNewIncident = Common.CreateNewObjectProjectionFromExistingObjectProjection(emopIncident, mpcIncident, this.PropertiesToExclude, this.EMG);
                
                //Set the ID, DisplayName, Status, and CreatedDate properties
                emopNewIncident.Object[mpcIncident, Constants.strPropertyStatus].Value = mpeIncidentStatusActive;
                emopNewIncident.Object[mpcIncident, Constants.strPropertyId].Value = String.Format("{0}{1}",strIncidentIDPrefix, "{0}");
                emopNewIncident.Object[mpcIncident, Constants.strPropertyCreatedDate].Value = DateTime.Now.ToUniversalTime();
                emopNewIncident.Object[mpcIncident, Constants.strPropertyDisplayName].Value = String.Format("{0} - {1}", emopNewIncident.Object[mpcIncident, Constants.strPropertyId].Value, emopNewIncident.Object[mpcIncident, Constants.strPropertyTitle].Value);

                if (this.RelationshipAliasesToExclude.Length == 0)
                {
                    //A list of Relationships to exclude was not passed in so we are going to go with a default list.
                    this.RelationshipAliasesToExclude = new string[] {
                                                                    Constants.strAliasCreatedByUser,
                                                                    Constants.strAliasClosedByUser,
                                                                    Constants.strAliasResolvedByUser,
                                                                    Constants.strAliasActionLogs,
                                                                    Constants.strAliasUserComments,
                                                                    Constants.strAliasAnalystComments,
                                                                    Constants.strAliasSMTPNotifications,
                                                                    Constants.strAliasActivities,
                                                                    Constants.strAliasFileAttachments
                                                                };
                }

                //Copy all the relationships defined in the type projection, except for those specified
                Common.CopyRelationships(emopIncident, ref emopNewIncident, mptpIncident, this.RelationshipAliasesToExclude);

                //Set CreatedByUser to be the user that is logged in
                EnterpriseManagementObject emoCreatedByUser = Common.GetLoggedInUserAsObject(this.EMG);
                if (emoCreatedByUser != null)
                {
                    emopNewIncident.Add(emoCreatedByUser, mprCreatedByUser.Target);
                }

                //Relate the original incident to the new one
                emopNewIncident.Add(emopIncident.Object, mprWorkItemRelatesToWorkItem.Target);

                //And finally submit...
                emopNewIncident.Commit();
                strWorkItemID = emopNewIncident.Object[mpcIncident, Constants.strPropertyId].Value.ToString();
            }
            return strWorkItemID;
        }

        public string GetIncidentIDPrefix()
        {
            string strIncidentIDPrefixToReturn = null;
            ManagementPackClass mpcIncidentSettings = Common.GetManagementPackClassByName(Constants.strClassIncidentSettings, Constants.strManagementPackIncidentManagementLibrary, this.EMG);
            //Get the Incident ID prefix
            IObjectReader<EnterpriseManagementObject> emopIncidentSettings = this.EMG.EntityObjects.GetObjectReader<EnterpriseManagementObject>(mpcIncidentSettings, ObjectQueryOptions.Default);
            
            foreach(EnterpriseManagementObject emopIncidentSetting in emopIncidentSettings)
            {
                strIncidentIDPrefixToReturn = emopIncidentSetting[mpcIncidentSettings, Constants.strPropertyPrefixForID].Value.ToString();
            }
            return strIncidentIDPrefixToReturn;
        }
    }
}