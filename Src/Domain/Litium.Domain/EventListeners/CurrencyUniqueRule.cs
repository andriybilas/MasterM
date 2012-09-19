using System;
using Litium.Common;
using Litium.Common.Events;
using Litium.Common.Validation;
using Litium.Domain.Entities;

namespace Litium.Domain.EventListeners
{
	internal sealed class CurrencyUniqueRule : IEventListener<Currency, ValidateEvent>
	{
		public void HandleEvent(EntityEventArgs<Currency> eventArgs)
		{
			CheckForSameCurrency(eventArgs.Entity);
		}

		private void CheckForSameCurrency(Currency currency)
		{
			if (currency == null)
				throw new ArgumentNullException("currency");

			var sameCurrency = Repository.Data.Get<Currency>()
				.Where(x => x.Id != currency.Id && x.Code == currency.Code)
				.FirstOrDefault()
				.Value;

			if (sameCurrency != null)
				throw new ValidationArgumentException(currency, "CurrencyExistsException");
		}
	}
}