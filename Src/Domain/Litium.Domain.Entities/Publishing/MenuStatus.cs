namespace Litium.Domain.Entities.Publishing
{
	/// <summary>
	/// 	Page menu status.
	/// </summary>
	public enum MenuStatus
	{
		/// <summary>
		/// 	Not in menu status
		/// </summary>
		NotInMenu = 0,

		/// <summary>
		/// 	Visible and enabled in menu status.
		/// </summary>
		VisibleEnabledInMenu = 1,

		/// <summary>
		/// 	Visible and disabled in menu.
		/// </summary>
		VisibleDisabledInMenu = 2
	}
}