using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Resources;
using System.Text;
using System.Threading;
using Litium.Resources;

namespace Litium.Common.Validation
{
	public sealed class ValidationArgumentException : Exception
	{
		private static ICollection<ValidationConsequence> _validationResult;

		public ICollection<ValidationConsequence> ValidationResult
		{
			get { return _validationResult; }
		}

		public ValidationArgumentException(object target, params string[] messagesResourceKeys)
			: base(Initialize(target, typeof (DomainNotification), messagesResourceKeys))
		{
		}

		public ValidationArgumentException(object target, Type messagesResourceType, params string[] messagesResourceKeys)
			: base(Initialize(target, messagesResourceType, messagesResourceKeys))
		{
		}

		public ValidationArgumentException(object target, IEnumerable<ValidationResult> validationResults)
			: base(Initialize(target, validationResults))
		{
		}

		private static string Initialize(object target, Type resourceType, params string[] resourceKeys)
		{
			var manager = new ResourceManager(resourceType);
			_validationResult = new List<ValidationConsequence>();
			foreach (var resourceKey in resourceKeys)
			{
				var message = manager.GetString(resourceKey, Thread.CurrentThread.CurrentUICulture);
				var consequence = new ValidationConsequence(target.GetType(), target.GetType().FullName, message);
				_validationResult.Add(consequence);
			}

			return InitializeErrorMessage();
		}

		private static string Initialize(object target, IEnumerable<ValidationResult> validationResults)
		{
			_validationResult = new List<ValidationConsequence>();
			foreach (var result in validationResults)
			{
				var consequence = new ValidationConsequence(target.GetType(), target.GetType().FullName, result.ErrorMessage);
				_validationResult.Add(consequence);
			}

			return InitializeErrorMessage();
		}

		private static string InitializeErrorMessage()
		{
			var message = new StringBuilder();
			foreach (var validationConsequence in _validationResult)
			{
				message.AppendLine(string.Format("{0} - {1}",
				                                 validationConsequence.TargetFullTypeName,
				                                 validationConsequence.Message));
			}
			return message.ToString();
		}
	}
}