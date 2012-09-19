using System;
using System.Web.Mvc;
using Litium.Common.Validation;
using log4net;

namespace Site.Infrastuctures.Utility
{
	public class ActionHelper
	{
		public static bool TryExecute( Action action, ModelStateDictionary state )
		{
			try
			{
				action.Invoke();
			}
			catch (ValidationArgumentException ex)
			{

				foreach(var result in ex.ValidationResult)
				{
					state.AddModelError("ValidationException", result.Message);	
				}
				return false;
			}
			catch (Exception ex)
			{
				ILog log = LogManager.GetLogger(typeof(ActionHelper));
				log.Error(ex);
				state.AddModelError("Exception", ex.Message);
				return false;
			}
			return true;
		}
	}
}
