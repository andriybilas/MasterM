namespace Litium.Domain.Entities.Publishing
{
	/// <summary>
	/// 	Sitmape change frequancy.
	/// </summary>
	public enum ChangeFrequency
	{
		/// <summary>
		/// 	Never.
		/// </summary>
		Never = -1,

		/// <summary>
		/// 	Always.
		/// </summary>
		Always = 0,

		/// <summary>
		/// 	Hourly.
		/// </summary>
		Hourly = 1,

		/// <summary>
		/// 	Daily.
		/// </summary>
		Daily = 2,

		/// <summary>
		/// 	Weekly.
		/// </summary>
		Weekly = 3,

		/// <summary>
		/// 	Monthly.
		/// </summary>
		Monthly = 4,

		/// <summary>
		/// 	Yearly.
		/// </summary>
		Yearly = 5
	}
}