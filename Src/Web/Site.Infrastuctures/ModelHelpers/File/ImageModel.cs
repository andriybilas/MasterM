using Litium.Domain.Entities.Media;

namespace Site.Infrastuctures.ModelHelpers.File
{
    public class ImageModel
    {
        public virtual FileType ContentType { get; set; }

        public virtual string DisplayName { get; set; }

        public virtual int? Size { get; set; }

        public virtual string Name { get; set; }

        public virtual string ImagePath { get; set; }

        public virtual ResizedVersion ResizedTo { get; set; }

        //public virtual Lazy<Stream> FileStream { get; set; }
    }
}
