namespace AlexPortfolio.Models
{
    /// <summary>
    /// IContactModel is used to share
    /// behavior between MVC and Razor Pages.
    /// This allows us to strongly-type the
    /// view, while leaning on the attributes of the
    /// viewmodel or razor page
    ///
    /// We could optionally yell YOLO and make our
    /// views dynamic, but now we're living on the edge
    /// and could have runtime exceptions.
    /// </summary>
    public interface IContactModel
    {
        string Name { get; }
        string Phone { get; }
        string Email { get; }
        string Message { get; }
        bool HasSuccessMessage { get; }
        string SuccessMessage { get; }
    }
}