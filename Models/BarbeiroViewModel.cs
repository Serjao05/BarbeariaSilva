using System;
using System.Collections.Generic;   
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;   

namespace BarbeariaSilva.Models
{
    public class BarbeiroViewModel  
    {
        [Required(ErrorMessage = "O Nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O Nome não pode exceder 100 caracteres.")]

        public string? nome { get; set; }   

        [Required(ErrorMessage = "O E-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "O Formato do E-mail não é válido.")]

        public string? email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.")]
        [DataType(DataType.Password)]

        public string? senha { get; set; }

        [Compare("senha", ErrorMessage = "As senhas não coincidem.")]   
        [DataType(DataType.Password)]

        public string? confirmar_senha { get; set; }



    }
}
