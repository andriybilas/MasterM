using System;

namespace Litium.Domain.Entities.Media
{
	[Flags]
	public enum FileType
	{
		other = 0,
		image = 2,
		png = 4,
		jpeg = 8,
        gif = 16,
		txt = 32,
		docx = 64,
		xml = 128
	}
}
