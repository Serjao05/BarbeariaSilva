namespace BarbeariaSilva.Models
{
    public class AgendarViewModel
    {
        public int? id_agendamento { get; set; }

        public DateTime? data { get; set; }

        public string? nome { get; set; }

        public int? id_barbeiro { get; set; }

        public int? id_cliente { get; set; }

        public int? id_servico { get; set; }
        
        public string? status { get; set; }

    }
}
