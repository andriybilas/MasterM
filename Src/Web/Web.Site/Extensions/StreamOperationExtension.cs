﻿using System;
using System.IO;

namespace Web.Site.Extensions
{
    public static  class StreamOperationExtension
    {
        public static void StreamToStreamCopy(this Stream src, Stream dest)
        {
            int size = (src.CanSeek) ? Math.Min((int)(src.Length - src.Position), 0x2000) : 0x2000;
            byte[] buffer = new byte[size];
            int n;
            do
            {
                n = src.Read(buffer, 0, buffer.Length);
                dest.Write(buffer, 0, n);
            } while (n != 0);
        }

        public static void MemToStreamCopy(this MemoryStream src, Stream dest)
        {
            dest.Write(src.GetBuffer(), (int)src.Position, (int)(src.Length - src.Position));
        }

        public static void StreamToMemCopy(this Stream src, MemoryStream dest)
        {
            if (src.CanSeek)
            {
                int pos = (int)dest.Position;
                int length = (int)(src.Length - src.Position) + pos;
                dest.SetLength(length);

                while (pos < length)
                    pos += src.Read(dest.GetBuffer(), pos, length - pos);
            }
            else
                src.CopyTo((Stream)dest);

            dest.Seek(0, SeekOrigin.Begin);
        } 

    }
}