using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Xml;

namespace Kitechan
{
    public class UserInfo
    {
        public struct ImagePath
        {
            public string ImageUrl { get; set; }
            public string CachedImagePath { get; set; }
        }

        public event EventHandler<ImageLoadedEventArgs> ImageLoadedEvent;

        public int Id { get; private set; }

        public string Name { get; set; }

        public string ImageUrl { get; private set; }

        public string CachedImagePath { get; set; }

        public List<string> Aliases { get; private set; }

        public List<ImagePath> PriorImages { get; private set; }

        public Image UserImage { get; private set; }

        private UserInfo()
        {
            this.Aliases = new List<string>();
            this.PriorImages = new List<ImagePath>();
        }

        public UserInfo(int id, string name, string imageUrl)
        {
            this.Id = id;
            this.Name = name;
            this.ImageUrl = imageUrl;
            this.CachedImagePath = null;
            this.UserImage = null;
            this.Aliases = new List<string>();
            this.PriorImages = new List<ImagePath>();
        }

        public void UpdateUser(string name, string imageUrl)
        {
            if (!this.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
            {
                this.Aliases.Add(this.Name);
                this.Name = name;
            }
            if (!this.ImageUrl.Equals(imageUrl, StringComparison.InvariantCultureIgnoreCase))
            {
                this.PriorImages.Add(new ImagePath() { ImageUrl = this.ImageUrl, CachedImagePath = this.CachedImagePath });
                this.ImageUrl = imageUrl;
                this.CachedImagePath = null;
                this.UserImage = null;
            }
        }

        public void LoadImage()
        {
            Task.Factory.StartNew(() => this.PerformLoadImage());
        }

        private void PerformLoadImage()
        {
            if (File.Exists(this.CachedImagePath))
            {
                this.UserImage = Image.FromFile(this.CachedImagePath);
                this.ImageLoadedEvent(this, new ImageLoadedEventArgs(this.Id, false));
            }
            else
            {
                using (WebClient webClient = new WebClient())
                {
                    try
                    {
                        Stream imageStream = webClient.OpenRead(this.ImageUrl);
                        Image image = Image.FromStream(imageStream);
                        imageStream.Close();
                        this.UserImage = image;
                        this.CachedImagePath = Path.Combine(Engine.ImageCacheDir, this.Id + "_" + DateTime.Now.ToString("yyMMddHHmmss"));
                        this.UserImage.Save(this.CachedImagePath);
                        this.ImageLoadedEvent(this, new ImageLoadedEventArgs(this.Id, true));
                    }
                    catch
                    {
                        this.UserImage = null;
                    }
                }
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("user");

            writer.WriteElementString("id", this.Id.ToString());
            writer.WriteElementString("name", this.Name);
            writer.WriteElementString("imageUrl", this.ImageUrl);
            writer.WriteElementString("cachedImagePath", this.CachedImagePath);

            foreach (string alias in this.Aliases)
            {
                writer.WriteElementString("alias", alias);
            }

            foreach (ImagePath image in this.PriorImages)
            {
                writer.WriteStartElement("priorImage");

                writer.WriteAttributeString("url", image.ImageUrl);
                writer.WriteAttributeString("cache", image.CachedImagePath);

                writer.WriteEndElement();
            }

            writer.WriteEndElement(); // user
        }

        public static UserInfo FromXml(XmlNode node)
        {
            UserInfo ret = new UserInfo();
            if (node.Name == "user")
            {
                foreach (XmlNode innerNode in node.ChildNodes)
                {
                    switch (innerNode.Name)
                    {
                        case "id":
                            ret.Id = int.Parse(innerNode.InnerText);
                            break;
                        case "name":
                            ret.Name = innerNode.InnerText;
                            break;
                        case "imageUrl":
                            ret.ImageUrl = innerNode.InnerText;
                            break;
                        case "cachedImagePath":
                            ret.CachedImagePath = innerNode.InnerText;
                            break;
                        case "alias":
                            ret.Aliases.Add(innerNode.InnerText);
                            break;
                        case "priorImage":
                            ret.PriorImages.Add(new ImagePath() { ImageUrl = innerNode.Attributes["url"].InnerText, CachedImagePath = innerNode.Attributes["cache"].InnerText });
                            break;
                    }
                }
            }
            return ret;
        }
    }
}
