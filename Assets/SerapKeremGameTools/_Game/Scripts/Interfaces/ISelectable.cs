namespace SerapKeremGameTools.Game._Interfaces
{
    /// <summary>
    /// Interface for selectable objects.
    /// </summary>
    public interface ISelectable
    {
        /// <summary>
        /// Method to select the object.
        /// </summary>
        void Select();

        /// <summary>
        /// Method to deselect the object.
        /// </summary>
        void DeSelect();
    }
}
