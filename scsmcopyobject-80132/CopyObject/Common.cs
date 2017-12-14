using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using Microsoft.EnterpriseManagement;
using Microsoft.EnterpriseManagement.Common;
using Microsoft.EnterpriseManagement.Configuration;

namespace CopyObject
{
    public class Common
    {
        public static CreatableEnterpriseManagementObject CreateNewObjectFromExistingObject(EnterpriseManagementObject emoToBeCopied, ManagementPackClass mpc, string[] strPropertiesToExclude, EnterpriseManagementGroup emg)
        {
            //Create a new object to copy property values into
            CreatableEnterpriseManagementObject cemo = new CreatableEnterpriseManagementObject(emg, mpc);
            //For each property copy the property value into the new object
            foreach (ManagementPackProperty property in emoToBeCopied.GetProperties())
            {
                //.. unless it is in the list of properties to NOT copy over
                if (Array.IndexOf(strPropertiesToExclude, property.Name) == -1)
                {
                    cemo[property.Id].Value = emoToBeCopied[property.Id].Value;
                }
            }
            return cemo;
        }

        public static EnterpriseManagementObjectProjection CreateNewObjectProjectionFromExistingObjectProjection(EnterpriseManagementObjectProjection emopToBeCopied, ManagementPackClass mpc, string[] strPropertiesToExclude, EnterpriseManagementGroup emg)
        {
            //Create a new  projection that everything will be copied into
            EnterpriseManagementObjectProjection emop = new EnterpriseManagementObjectProjection(emg, mpc);

            //For each of the properties on the seed object itself, copy the value into the new seed object
            IList<ManagementPackProperty> properties = emopToBeCopied.Object.GetProperties();
            foreach (ManagementPackProperty property in properties)
            {
                //...except for the list of properties to NOT copy over
                if (Array.IndexOf(strPropertiesToExclude, property.Name) == -1)
                {
                    emop.Object[property.Id].Value = emopToBeCopied.Object[property.Id].Value;
                }
            }
            return emop;
        }

        public static EnterpriseManagementObject GetLoggedInUserAsObject(EnterpriseManagementGroup emg)
        {
            EnterpriseManagementObject emoUserToReturn = null;

            string strUserName = System.Environment.UserName;
            string strDomain = System.Environment.UserDomainName;
            string strUserByUserNameAndDomainCriteria = string.Format("{0} = '{1}' AND {2} = '{3}'", Constants.strPropertyUserName, strUserName, Constants.strPropertyDomain ,strDomain);

            ManagementPackClassCriteria mpccUser = new ManagementPackClassCriteria(String.Format("{0} = '{1}'", Constants.strMPAttributeName, Constants.strClassUser));
            ManagementPackClass mpcUser = GetManagementPackClassByName(Constants.strClassUser, Constants.strManagementPackSystemLibrary, emg);
            EnterpriseManagementObjectCriteria emocUserByUserNameAndDomain = new EnterpriseManagementObjectCriteria(strUserByUserNameAndDomainCriteria, mpcUser);
            IObjectReader<EnterpriseManagementObject> emoUsers = emg.EntityObjects.GetObjectReader<EnterpriseManagementObject>(emocUserByUserNameAndDomain, ObjectQueryOptions.Default);
            foreach (EnterpriseManagementObject emoUser in emoUsers)
            {
                //There will be only one if any
                emoUserToReturn = emoUser;
            }
            return emoUserToReturn;
        }

        public static ManagementPack GetManagementPackByName(string strManagementPackName, EnterpriseManagementGroup emg)
        {
            ManagementPack mpToReturn = null;
            ManagementPackCriteria mpc = new ManagementPackCriteria(String.Format("Name = '{0}'", strManagementPackName));
            foreach (ManagementPack mp in emg.ManagementPacks.GetManagementPacks(mpc))
            {
                mpToReturn = mp;
            }
            return mpToReturn;
        }

        public static ManagementPackClass GetManagementPackClassByName(string strClassName, string strManagementPackName, EnterpriseManagementGroup emg)
        {
            ManagementPackClass mpcToReturn = null;
            ManagementPackClassCriteria mpcc = new ManagementPackClassCriteria(String.Format("Name = '{0}'", strClassName));
            foreach(ManagementPackClass mpc in emg.EntityTypes.GetClasses(mpcc))
            {
                if(mpc.GetManagementPack().Name == strManagementPackName)
                    mpcToReturn = mpc;
            }
            return mpcToReturn;
        }

        public static ManagementPackRelationship GetManagementPackRelationshipByName(string strRelationshipName, string strManagementPackName, EnterpriseManagementGroup emg)
        {
            ManagementPackRelationship mprToReturn = null;
            ManagementPackRelationshipCriteria mprc = new ManagementPackRelationshipCriteria(String.Format("Name = '{0}'", strRelationshipName));
            foreach (ManagementPackRelationship mpr in emg.EntityTypes.GetRelationshipClasses(mprc))
            {
                if (mpr.GetManagementPack().Name == strManagementPackName)
                    mprToReturn = mpr;
            }
            return mprToReturn;
        }

        public static ManagementPackTypeProjection GetManagementPackTypeProjectionByName(string strTypeProjectionName, string strManagementPackName, EnterpriseManagementGroup emg)
        {
            ManagementPackTypeProjection mptpToReturn = null;
            ManagementPackTypeProjectionCriteria mptpc = new ManagementPackTypeProjectionCriteria(String.Format("Name = '{0}'", strTypeProjectionName));
            foreach (ManagementPackTypeProjection mptp in emg.EntityTypes.GetTypeProjections(mptpc))
            {
                if (mptp.GetManagementPack().Name == strManagementPackName)
                    mptpToReturn = mptp;
            }
            return mptpToReturn;
        }

        public static ManagementPackEnumeration GetManagementPackEnummerationByName(string strManagementPackEnumerationName, string strManagementPackName, EnterpriseManagementGroup emg)
        {
            ManagementPackEnumeration mpeToReturn = null;
            ManagementPackEnumerationCriteria mpec = new ManagementPackEnumerationCriteria(String.Format("Name = '{0}'", strManagementPackEnumerationName));
            foreach (ManagementPackEnumeration mpe in emg.EntityTypes.GetEnumerations(mpec))
            {
                if (mpe.GetManagementPack().Name == strManagementPackName)
                    mpeToReturn = mpe;
            }
            return mpeToReturn;
        }

        public static EnterpriseManagementGroup GetManagementGroupConnectionFromRegistry()
        {
            String strServerName = Registry.GetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\System Center\\2010\\Service Manager\\Console\\User Settings", "SDKServiceMachine", "localhost").ToString();
            EnterpriseManagementGroup emg = new EnterpriseManagementGroup(strServerName);
            return emg;
        }

        public static void CopyRelationships(EnterpriseManagementObjectProjection emopToBeCopiedFrom, ref EnterpriseManagementObjectProjection emopToBeCopiedTo, ManagementPackTypeProjection mptp, string[] strAliasesToExclude)
        {
            foreach (ManagementPackTypeProjectionComponent mptpc in mptp.ComponentCollection)
            {
                if (Array.IndexOf(strAliasesToExclude, mptpc.Alias) == -1)
                {
                    foreach (IComposableProjection icp in emopToBeCopiedFrom[mptpc.TargetEndpoint])
                    {
                        emopToBeCopiedTo.Add(icp.Object, mptpc.TargetEndpoint);
                    }
                }
            }
        }
    }
}
