namespace BarbeariaSilva.Models
{
    public class ClubeViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
