namespace Litium.Domain.Entities.Publishing
{
	/// <summary>
	/// 	Page status.
	/// </summary>
	public enum PageStatus
	{
		/// <summary>
		/// 	Published.
		/// </summary>
		Published = 0,

		/// <summary>
		/// 	Not published.
		/// </summary>
		NotPublished = 1,

		/// <summary>
		/// 	Not published and delayed publish.
		/// </summary>
		NotPublishedDelayedPublish = 2,

		/// <summary>
		/// 	Not published but ready to publish.
		/// </summary>
		NotPublishedReadyToPublish = 3,

		/// <summary>
		/// 	In archive.
		/// </summary>
		InArchive = 4,

		/// <summary>
		/// 	In trashcan.
		/// </summary>
		InTrashcan = 5
	}
}