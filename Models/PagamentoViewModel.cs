namespace BarbeariaSilva.Models
{
    public class PagamentoViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
