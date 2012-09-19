using System;
using Litium.Domain.Entities.Publishing;

namespace Litium.Domain.Services
{
	public interface IPublishingService
	{
		Page CopyPaste(Guid pageToCopyId, Guid pageToPasteInId, bool recursive);
        Page CopyPasteAsTop(Guid pageToCopyId, Guid webSiteToPasteInId, bool recursive);
		Page CutAndPaste(Guid pageToCutId, Guid pageToPasteInId);
		Page CutAndPasteAsTop(Guid pageToCutId, Guid webSiteToPasteInId);
	}
}