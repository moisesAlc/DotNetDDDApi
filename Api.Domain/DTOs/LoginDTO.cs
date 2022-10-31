using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.DTOs
{
	public class LoginDTO
	{
		[Required(ErrorMessage = "Email é um campo obrigatório para o login")]
		[EmailAddress(ErrorMessage = "Email em formato inválido")]
		[StringLength(100, ErrorMessage = "Email deve ter no máximo {1} caracteres")]
		public string Email { get; set; }
	}
}
