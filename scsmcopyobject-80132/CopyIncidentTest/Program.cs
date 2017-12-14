using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CopyObject;
using Microsoft.Win32;
using Microsoft.EnterpriseManagement;

namespace TestHarness.CopyIncident
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string strIncidentIDToCopy = args[0];
                //string strIncidentIDToCopy = "TIR38671"; //For debugging purposes only
                Incident incident = new Incident();
                incident.IDToCopy = strIncidentIDToCopy;
                incident.EMG = Common.GetManagementGroupConnectionFromRegistry();
                string strWorkItemID = incident.Copy();
                if (strWorkItemID != null)
                {
                    Console.WriteLine(String.Format("{0} copied to {1}", strIncidentIDToCopy, strWorkItemID));
                    Console.ReadLine();
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException.Message);
            }
        }
    }
}
