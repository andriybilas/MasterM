namespace Litium.Common.Lifecycle
{
	public static class ConversationHelper
	{
		/// <summary>
		/// 	Close conversations.
		/// </summary>
		public static void Close()
		{
			var conversations = IoC.ResolveAll<IConversation>();
			foreach (var conversation in conversations)
			{
				conversation.Close();
			}
		}

		/// <summary>
		/// 	Open conversations.
		/// </summary>
		public static void Open()
		{
			var conversations = IoC.ResolveAll<IConversation>();
			foreach (var conversation in conversations)
			{
				conversation.Open();
			}
		}

		/// <summary>
		/// 	Reopen conversations.
		/// </summary>
		public static void ReOpen()
		{
			Close();
			Open();
		}
	}
}