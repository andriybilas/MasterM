namespace Litium.Domain.Entities.Publishing
{
	/// <summary>
	/// 	Working copy status.
	/// </summary>
	public enum WorkingCopyStatus
	{
		/// <summary>
		/// 	working copy is not published.
		/// </summary>
		NotPublished = 1,

		/// <summary>
		/// 	Working copy is delayed published.
		/// </summary>
		DelayedPublish = 2,

		/// <summary>
		/// 	Working copy is ready to be published.
		/// </summary>
		ReadyToPublish = 3
	}
}