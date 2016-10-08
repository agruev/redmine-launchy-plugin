using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using LaunchySharp;

namespace SiiRed
{
    public class SiiRedPlugin : IPlugin
    {
        private LaunchySharp.IPluginHost m_pluginHost = null;
        private LaunchySharp.ICatItemFactory m_catItemFactory = null;
        private uint m_id = 0;
        private string m_name = "SiiRed";

        private RedmineConnector rconn;
        private bool taskLoaded;

        public void init(LaunchySharp.IPluginHost pluginHost)
        {
            m_pluginHost = pluginHost;
            if (m_pluginHost != null)
            {
                m_catItemFactory = m_pluginHost.catItemFactory();
            }
            m_id = m_pluginHost.hash(m_name);
            rconn = new RedmineConnector();
        }

        public uint getID()
        {
            return m_id;
        }

        public string getName()
        {
            return m_name;
        }

        public void getLabels(List<IInputData> inputDataList)
        {
        }

        public void getResults(List<IInputData> inputDataList, List<ICatItem> resultsList)
        {
            String req = inputDataList[0].getText();
            if (req.StartsWith("rm"))
            {
                String[] parts = req.Split(new Char[]{' '}, 2);

                if (parts.Length > 1)
                {
                    Issue[] issues = rconn.getCacheIssues(parts[1]);
                    foreach (Issue i in issues)
                    {
                        resultsList.Add(m_catItemFactory.createCatItem(i.subject, i.id, getID(), getName()));

                    }
                }
            }
        }

        public void getCatalog(List<ICatItem> catalogItems)
        {
        }

        public void launchItem(List<IInputData> inputDataList, ICatItem item)
        {
            ICatItem catItem =
                inputDataList[inputDataList.Count - 1].getTopResult();
            try
            {
                System.Diagnostics.Process.Start("http://redmine.dreamix.eu/issues/"+item.getShortName());
            }
            catch
        (
         System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (System.Exception other)
            {
                MessageBox.Show(other.Message);
            }
            
        }

        public bool hasDialog()
        {
            return false;
        }

        public IntPtr doDialog()
        {
            return IntPtr.Zero;
        }

        public void endDialog(bool acceptedByUser)
        {
        }

        public void launchyShow()
        {
            rconn.issueCache = null;
        }

        public void launchyHide()
        {
            
        }

    }
}
