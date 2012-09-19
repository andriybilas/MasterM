using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using Litium.Common;
using Litium.Common.Configurations;
using Telerik.Web.Mvc;
using Litium.Domain.Entities;
using Litium.Domain.Entities.Media;
using Litium.Domain.Utilities;
using Litium.Resources;
using LFile = Litium.Domain.Entities.Media.File;

namespace Site.Infrastuctures.ModelHelpers.File
{
	public class ImageUploadHelper
	{
		private static ImageUploadHelper _instance;

		public static ImageUploadHelper Helper
		{
			get
			{
				if(_instance == null)
					_instance = new ImageUploadHelper();
				return _instance;
			}
		}

		private bool CheckIfImageForProductExist( IEnumerable<Litium.Domain.Entities.Media.File> images, String fileName )
		{
			return images.FirstOrDefault (x => x.Name.Equals (fileName, StringComparison.InvariantCultureIgnoreCase)) != null;
		}

		private LFile CreateImageEntity( string fileName, HttpPostedFileBase file1 )
		{
			var image = new Litium.Domain.Entities.Media.File ();
			image.Name = fileName;

			Stream copiedFileStream = new MemoryStream ();
			file1.InputStream.CopyTo (copiedFileStream);

			image.FileStream = new Lazy<Stream> (() => copiedFileStream);
			image.DisplayName = file1.FileName;

			image.StoragePath = HttpContext
				.Current.Request.MapPath (LitiumConfigs.Data.FilesStorage)
				.Replace (@"Shared\", String.Empty);

			image.ContentType = FileType.image;
			image.Size = file1.ContentLength;
			image.ResizedTo = ResizedVersion.Origin;
			return image;
		}

		private LFile CreateScallableImageEnity( HttpPostedFileBase file1, LFile image, ResizedVersion resizedVersion )
		{
			var scallableImage = (LFile)image.Clone ();
			
			int width = 100;
			int height = 125;
			if (resizedVersion == ResizedVersion.To100x125) { width = 100; height = 125; }
			if (resizedVersion == ResizedVersion.To620x195) { width = 620; height = 195; }

			Stream imageStream = ScalableImageUtility.GetResizedVersionStream (width, height, file1.InputStream, ImageFormat.Jpeg, true);
			scallableImage.FileStream = new Lazy<Stream> (() => imageStream);
			scallableImage.ResizedTo = resizedVersion;
			scallableImage.Name = String.Format ("Resized{0}_{1}", resizedVersion, image.Name);
			return scallableImage;
		}
		
        public string UploadProductImageAsync<T>( IEnumerable<HttpPostedFileBase> attachments, string entityInput, ResizedVersion resizedVersion ) where T : IImage
		{
			Guid entityId;
			if (Guid.TryParse (entityInput, out entityId))
			{
				foreach (var file in attachments)
				{
					var fileName = Path.GetFileName (file.FileName);

					//if (CheckIfImageForProductExist (Repository.Data.Get<T> (entityId).Image, fileName))
						//return String.Empty;

					FileType fileType;
					Enum.TryParse (file.ContentType.Replace ("image/", String.Empty), out fileType);

					if (fileType != FileType.other && fileType < FileType.gif)
					{
						HttpPostedFileBase file1 = file;
						LFile image = CreateImageEntity (fileName, file1);
						LFile scallableImage = CreateScallableImageEnity (file1, image, resizedVersion);

						//Repository.Data.Save (image);
						scallableImage.EntityId = entityId;
						Repository.Data.Save (scallableImage);

						var entity = Repository.Data.Get<T>(entityId);
						entity.HasImage = true;

						//entity.Image.Add (image);
						Repository.Data.Save (entity);
					}
					else
					{
						return StoreResourceStrings.OnlyImagesIsAllovedToUpload;
					}
				}
			}
			return String.Empty;
		}

	    public string GetImageUrl(Guid productEntityId, EntityType type)
        {
            string imageUrl = String.Empty;
            LFile image = null;

            switch (type)
            {
                case EntityType.Product:
                    {
                        image =
                            Repository.Data.Get<LFile>().Where(
                                x => x.EntityId == productEntityId && x.ResizedTo == ResizedVersion.To100x125).FirstOrDefault().
                                Value;
                        break;
                    }
                default:
                    {
                        image =
                            Repository.Data.Get<LFile>().Where(
                                x => x.EntityId == productEntityId && x.ResizedTo == ResizedVersion.To620x195).FirstOrDefault().
                                Value;
                        break;                        
                    }
            }

            if (image != null)
                imageUrl = String.Format("/{0}/{1}", LitiumConfigs.Data.FilesStorage.Replace("\\", "/"), image.Name);

            return imageUrl;
        }
	}
}
