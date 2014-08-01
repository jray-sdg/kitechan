using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Kitechan.Types
{
    public class ClientSettings
    {
        public List<int> MutedUsers { get; set; }

        public bool LoadChatHistory { get; set; }

        public ClientSettings()
        {
            this.MutedUsers = new List<int>();
            this.LoadChatHistory = true;
        }

        public ClientSettings(ClientSettings other)
        {
            this.MutedUsers = new List<int>(other.MutedUsers);
            this.LoadChatHistory = other.LoadChatHistory;
        }

        public ClientSettings Clone()
        {
            return new ClientSettings(this);
        }

        public static ClientSettings FromXml(XmlNode xmlNode)
        {
            ClientSettings ret = new ClientSettings();

            if (xmlNode.Name == "clientSettings")
            {
                foreach (XmlNode childNode in xmlNode.ChildNodes)
                {
                    switch (childNode.Name)
                    {
                        case "mutedUser":
                            ret.MutedUsers.Add(int.Parse(childNode.InnerText));
                            break;
                        case "loadHistory":
                            ret.LoadChatHistory = bool.Parse(childNode.InnerText);
                            break;
                    }
                }
            }

            return ret;
        }

        public void WriteXml(XmlWriter xmlWriter)
        {
            xmlWriter.WriteStartElement("clientSettings");

            foreach (int mutedUser in this.MutedUsers)
            {
                xmlWriter.WriteElementString("mutedUser", mutedUser.ToString());
            }

            xmlWriter.WriteElementString("loadHistory", this.LoadChatHistory.ToString());

            xmlWriter.WriteEndElement(); // clientSettings
        }
    }
}
