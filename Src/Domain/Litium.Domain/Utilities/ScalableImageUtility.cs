using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using Encoder = System.Drawing.Imaging.Encoder;
using FileEntity = Litium.Domain.Entities.Media.File;

namespace Litium.Domain.Utilities
{
	public class ScalableImageUtility
	{
		public static Stream GetResizedVersionStream( int width, int height, FileEntity file, ImageFormat outputFormat, bool keepAspectRatio )
		{
			Stream fileStream = FileStorageUtilities.LoadFile (file);
		    return GetResizedVersionStream(width, height, fileStream, outputFormat, keepAspectRatio);
		}

        public static Stream GetResizedVersionStream( int width, int height, Stream fileStream, ImageFormat outputFormat, bool keepAspectRatio )
        {
            if (width < 1)
                width = 1;

            if (height < 1)
                height = 1;

            byte[] result = Resize(new Size(width, height), keepAspectRatio, GetBitmap(fileStream), outputFormat);

            if (result != null && result.Length > 0)
                using (MemoryStream s = new MemoryStream(result))

                    if (result != null)
                        return new MemoryStream(result);
            return null;        		    
        }

		private static Bitmap GetBitmap( Stream fileStream )
		{
			try
			{
				using (Stream stream = fileStream)
				{
					if (stream != null && stream.CanRead)
						return new Bitmap (stream);
					return null;
				}
			}
			catch (Exception e)
			{
				throw;
			}
		}

		private static byte[] Resize( Size newSize, bool keepAspectRatio, Image image, ImageFormat outputFormat )
		{
			if (image == null)
				return null;

			Size cropSize = newSize;
			Size orginalSize = image.Size;

			//Calculate scale/Crop ratios
			if (!keepAspectRatio)
			{
				float scaleWidth = (float)newSize.Width / orginalSize.Width;
				float scaleHeight = (float)newSize.Height / orginalSize.Height;

				//Not proportional scale
				if (scaleWidth != scaleHeight)
				{
					if (scaleHeight > scaleWidth)
					{
						newSize.Width = (int)(scaleHeight * orginalSize.Width);
					}
					else
					{
						newSize.Height = (int)(scaleWidth * orginalSize.Height);
					}
				}
				else
				{
					keepAspectRatio = true;
				}
			}

			try
			{
				using (MemoryStream ms = new MemoryStream ())
				{
					using (Bitmap bmPhoto = new Bitmap (newSize.Width, newSize.Height, PixelFormat.Format24bppRgb))
					{
						bmPhoto.SetResolution (image.HorizontalResolution, image.VerticalResolution);

						if (outputFormat.Equals (ImageFormat.Png))
							bmPhoto.MakeTransparent ();

						using (Graphics grPhoto = Graphics.FromImage (bmPhoto))
						{
							grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
							grPhoto.SmoothingMode = SmoothingMode.HighQuality;
							grPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;
							grPhoto.CompositingQuality = CompositingQuality.HighQuality;

							if (outputFormat.Equals (ImageFormat.Png))
								grPhoto.FillRectangle (Brushes.Transparent, 0, 0, newSize.Width, newSize.Height);
							else
								grPhoto.FillRectangle (Brushes.White, 0, 0, newSize.Width, newSize.Height);

							// Use wrap mode in order to get rid of the unwanted border around the image
							using (ImageAttributes wrapMode = new ImageAttributes ())
							{
								wrapMode.SetWrapMode (WrapMode.TileFlipXY);
								Rectangle rectangle = new Rectangle (0, 0, newSize.Width, newSize.Height);
								grPhoto.DrawImage (image, rectangle, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
							}
						}

						if (!keepAspectRatio && !cropSize.Equals (newSize))
						{
							int widthToCrop = Math.Max (0, (newSize.Width - cropSize.Width) / 2);
							int heigthToCrop = Math.Max (0, (newSize.Height - cropSize.Height) / 2);

							using (Bitmap bmpCrop = new Bitmap (cropSize.Width, cropSize.Height, PixelFormat.Format24bppRgb))
							{
								bmPhoto.SetResolution (image.HorizontalResolution, image.VerticalResolution);

								if (outputFormat.Equals (ImageFormat.Png))
									bmpCrop.MakeTransparent ();

								using (Graphics gfx = Graphics.FromImage (bmpCrop))
								{
									Rectangle source = new Rectangle (widthToCrop, heigthToCrop, cropSize.Width, cropSize.Height);
									Rectangle target = new Rectangle (0, 0, bmpCrop.Width, bmpCrop.Height);

									if (outputFormat.Equals (ImageFormat.Png))
										gfx.FillRectangle (Brushes.Transparent, 0, 0, newSize.Width, newSize.Height);
									else
										gfx.FillRectangle (Brushes.White, 0, 0, newSize.Width, newSize.Height);

									gfx.DrawImage (bmPhoto, target, source, GraphicsUnit.Pixel);
									GetImageStream (outputFormat, bmpCrop, ms);
								}
							}
						}
						else
						{
							GetImageStream (outputFormat, bmPhoto, ms);
						}
					}
					return ms.ToArray ();
				}
			}
			catch (Exception e)
			{
				throw;
			}
		}

		private static void GetImageStream( ImageFormat outputFormat, Bitmap bitmap, MemoryStream ms )
		{
			var imageEncoders = ImageCodecInfo.GetImageEncoders ();
			var encoderParameters = new EncoderParameters (1);
			encoderParameters.Param[0] = new EncoderParameter (Encoder.Quality, 100L);

			if (ImageFormat.Jpeg.Equals (outputFormat))
			{
				bitmap.Save (ms, imageEncoders[1], encoderParameters);
			}
			else if (ImageFormat.Png.Equals (outputFormat))
			{
				bitmap.Save (ms, imageEncoders[4], encoderParameters);
			}
			else if (ImageFormat.Gif.Equals (outputFormat))
			{
				var quantizer = new OctreeQuantizer (255, 8);
				using (var quantized = quantizer.Quantize (bitmap))
				{
					quantized.Save (ms, imageEncoders[2], encoderParameters);
				}
			}
			else if (ImageFormat.Bmp.Equals (outputFormat))
			{
				bitmap.Save (ms, imageEncoders[0], encoderParameters);
			}
			else
			{
				bitmap.Save (ms, outputFormat);
			}
		}
	}
}
