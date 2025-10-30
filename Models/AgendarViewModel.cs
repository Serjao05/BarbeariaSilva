namespace BarbeariaSilva.Models
{
    public class AgendarViewModel
    {
        public int? id_agendamento { get; set; }

        public string? data { get; set; }

        public int? id_barbeiro { get; set; }

        public int? id_cliente { get; set; }

        public int? id_servico { get; set; }
        
        public string? status { get; set; }

    }
}
