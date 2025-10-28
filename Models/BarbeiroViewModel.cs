using System;
using System.Collections.Generic;   
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;   

namespace BarbeariaSilva.Models
{
    public class BarbeiroViewModel  
    {
        [Required(ErrorMessage = "O Nome � obrigat�rio.")]
        [StringLength(100, ErrorMessage = "O Nome n�o pode exceder 100 caracteres.")]

        public string? nome { get; set; }   

        [Required(ErrorMessage = "O E-mail � obrigat�rio.")]
        [EmailAddress(ErrorMessage = "O Formato do E-mail n�o � v�lido.")]

        public string? email { get; set; }

        [Required(ErrorMessage = "A senha � obrigat�ria.")]
        [MinLength(6, ErrorMessage = "A senha deve ter no m�nimo 6 caracteres.")]
        [DataType(DataType.Password)]

        public string? senha { get; set; }

        [Compare("senha", ErrorMessage = "As senhas n�o coincidem.")]   
        [DataType(DataType.Password)]

        public string? confirmar_senha { get; set; }



    }
}
