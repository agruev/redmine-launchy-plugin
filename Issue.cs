using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace SiiRed
{
    [XmlRootAttribute("issue", IsNullable = false)]
    public class Issue
    {
        public String id;
        public String project;
        public String tracker;
        public String status;
        public String priority;
        public String author;
        public String assigned_to;
        public String category;
        public String subject;
        public String description;
        public String start_date;
        public String due_date;
        [XmlElement(IsNullable=true, DataType="int")]
        public Nullable<Int32> done_ratio;
        public String estimated_hours;
        public String created_on;
        public String updated_on;

    }
}
