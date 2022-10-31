using Api.Domain.Entities;
using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Services.User
{
	public interface ILoginService
	{
		Task<object> FindByLogin(LoginDTO user);
	}
}
