using Api.Domain.Entities;
using Domain.Interfaces.Services.User;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
	public class LoginService : ILoginService
	{
		private IUserRepository _userRepository;

		public LoginService(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		public async Task<object> FindByLogin(UserEntity user)
		{
			if (user != null && !string.IsNullOrWhiteSpace(user.Email))
			{
				return await _userRepository.FindByLogin(user.Email);
			}
			else
			{
				return null;
			}
		}
	}
}
