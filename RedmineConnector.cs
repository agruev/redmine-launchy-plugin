using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Net;
using System.IO;


namespace SiiRed
{
    public class RedmineConnector
    {
        public String redmineURL = "http://redmine.dreamix.eu";

        public Dictionary<String, Issue> issueCache = null;

        public Issue[] getCacheIssues(String filter) {
            filter = filter.ToLower();
            if (issueCache == null)
                initIssueCache();

            List<Issue> res = new List<Issue>();
            foreach (String key in issueCache.Keys){
                if (key.Contains(filter) || filter == null)
                {
                    res.Add(issueCache[key]);
                }
            }
            return res.ToArray();
        }

        public void initIssueCache()
        {
            issueCache = new Dictionary<String, Issue>();
            Issue[] issues = getAllIssues();
            foreach (Issue i in issues ){
                issueCache.Add(i.id +":"+ i.subject.ToLower()+":"+i.author.ToLower(), i);
            }
        }

        public Issue[] getAllIssues(String filter = null) {

            WebRequest request = WebRequest.Create(redmineURL+"/issues.xml");
            request.Credentials = new NetworkCredential("acho", "krechetalo");
            Console.WriteLine("new credentials");
            HttpWebResponse response = (HttpWebResponse) request.GetResponse();
            Stream dataStream = response.GetResponseStream();

            XmlDocument a = new XmlDocument();
            a.Load(dataStream);
            XmlNodeList issues = a.GetElementsByTagName("issue");
            List<Issue> res = new List<Issue>();
            foreach (XmlNode node in issues)
            {
                XmlSerializer xs = new XmlSerializer(typeof(Issue));
                Issue s = (Issue)xs.Deserialize(new XmlNodeReader(node));
                if (filter == null || s.subject.Contains(filter))
                    res.Add(s);
            }

            return res.ToArray(); ;
        }

        

        static void Main(string[] args)
        {
            Issue[] res = new RedmineConnector().getCacheIssues("FAKT");
            foreach (Issue i in res) {
                Console.WriteLine(i.subject);
            }
        }
    }
}
