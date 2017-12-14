using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using Microsoft.EnterpriseManagement;
using Microsoft.EnterpriseManagement.Common;
using Microsoft.EnterpriseManagement.Configuration;
using Microsoft.EnterpriseManagement.ConnectorFramework;
using Microsoft.EnterpriseManagement.UI.SdkDataAccess;
using Microsoft.EnterpriseManagement.UI.DataModel;
using Microsoft.EnterpriseManagement.UI.SdkDataAccess.DataAdapters;
using Microsoft.EnterpriseManagement.ConsoleFramework;

namespace CopyObject
{
    public class ConsoleTask : ConsoleCommand
    {
        public override void ExecuteCommand(IList<NavigationModelNodeBase> nodes, NavigationModelNodeTask task, ICollection<string> parameters)
        {
            foreach (NavigationModelNodeBase node in nodes)
            {
                if(parameters.Contains("Incident"))
                {
                    Incident incident = new Incident();
                    incident.IDToCopy= node["Id"].ToString();
                    incident.EMG = incident.EMG = Common.GetManagementGroupConnectionFromRegistry();
                    string strWorkItemID = incident.Copy();
                }
            }
        }
    }
}
