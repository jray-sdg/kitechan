using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Xml;

namespace Kitechan
{
    public class UserInfo
    {
        public event EventHandler<ImageLoadedEventArgs> ImageLoadedEvent;

        public int Id { get; private set; }

        public string Name { get; private set; }

        public string ImageUrl { get; private set; }

        public string CachedImagePath { get; set; }

        public Image UserImage { get; private set; }

        private WebClient webClient;

        private UserInfo()
        {
        }

        public UserInfo(int id, string name, string imageUrl)
        {
            this.Id = id;
            this.Name = name;
            this.ImageUrl = imageUrl;
            this.CachedImagePath = null;
            this.UserImage = null;
        }

        private void webClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                if (!e.Cancelled && !(e.Error is Exception))
                {
                    Image image;
                    using (e.Result)
                    {
                        image = Image.FromStream(e.Result);
                    }
                    this.UserImage = image;
                    this.ImageLoadedEvent(this, new ImageLoadedEventArgs(this.Id, true));
                }
            }
            catch
            {
                this.UserImage = null;
            }
            finally
            {
                if (this.webClient != null)
                {
                    this.webClient.Dispose();
                    this.webClient = null;
                }
            }
        }

        public void LoadImage()
        {
            if (File.Exists(this.CachedImagePath))
            {
                this.UserImage = Image.FromFile(this.CachedImagePath);
                this.ImageLoadedEvent(this, new ImageLoadedEventArgs(this.Id, false));
            }
            else
            {
                this.webClient = new WebClient();
                this.webClient.OpenReadCompleted += webClient_OpenReadCompleted;

                try
                {
                    this.webClient.OpenReadAsync(new Uri(this.ImageUrl));
                }
                catch
                {
                }
            }
        }

        public static UserInfo FromUrl(string url)
        {
            using (WebClient client = new WebClient())
            {
                string userInfo = client.DownloadString(url);
                UserJson json = UserJson.Parse(userInfo);
                return new UserInfo(int.Parse(json.Id), json.UserName, json.ProfileImageUrl);
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("user");

            writer.WriteElementString("id", this.Id.ToString());
            writer.WriteElementString("name", this.Name);
            writer.WriteElementString("imageUrl", this.ImageUrl);
            writer.WriteElementString("cachedImagePath", this.CachedImagePath);

            writer.WriteEndElement(); // user
        }

        public static UserInfo FromXml(XmlNode node)
        {
            UserInfo ret = new UserInfo();
            if (node.Name == "user")
            {
                if (node["id"] != null)
                {
                    ret.Id = int.Parse(node["id"].InnerText);
                }
                if (node["name"] != null)
                {
                    ret.Name = node["name"].InnerText;
                }
                if (node["imageUrl"] != null)
                {
                    ret.ImageUrl = node["imageUrl"].InnerText;
                }
                if (node["cachedImagePath"] != null)
                {
                    ret.CachedImagePath = node["cachedImagePath"].InnerText;
                }
            }
            return ret;
        }
    }
}
