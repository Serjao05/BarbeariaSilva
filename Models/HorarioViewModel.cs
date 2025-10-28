namespace BarbeariaSilva.Models
{
    public class HorarioViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
