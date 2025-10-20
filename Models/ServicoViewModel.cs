namespace BarbeariaSilva.Models
{
    public class ServicoViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
