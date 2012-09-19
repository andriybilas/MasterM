namespace Litium.Common.Lifecycle
{
	/// <summary>
	/// 	Interface for managing conversation per unit of work
	/// </summary>
	public interface IConversation
	{
		/// <summary>
		/// 	Closes conversation
		/// </summary>
		void Close();

		/// <summary>
		/// 	Opens conversation
		/// </summary>
		void Open();
	}
}