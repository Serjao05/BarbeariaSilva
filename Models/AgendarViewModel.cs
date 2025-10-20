namespace BarbeariaSilva.Models
{
    public class AgendarViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
