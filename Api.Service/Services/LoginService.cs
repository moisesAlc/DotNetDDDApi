﻿using Api.Domain.Entities;
using Domain.DTOs;
using Domain.Interfaces.Services.User;
using Domain.Repository;
using Domain.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Service.Services
{
	public class LoginService : ILoginService
	{
		private IUserRepository _userRepository;

		private SigningConfigurations _signinConfigurations;

		private TokenConfigurations _tokenConfigurations;

		private IConfiguration _configuration { get; }

		public LoginService(IUserRepository userRepository, SigningConfigurations signinConfigurations, TokenConfigurations tokenConfigurations, IConfiguration configuration)
		{
			_userRepository = userRepository;
			_signinConfigurations = signinConfigurations;
			_tokenConfigurations = tokenConfigurations;
			_configuration = configuration;
		}

		public async Task<object> FindByLogin(LoginDTO user)
		{
			UserEntity baseUser = new UserEntity();
			if (user != null && !string.IsNullOrWhiteSpace(user.Email))
			{
				baseUser = await _userRepository.FindByLogin(user.Email);
				if (baseUser == null)
				{
					return new
					{
						authenticated = false,
						message = "Falha ao autenticar"
					};
				}
				else
				{
					ClaimsIdentity identity = new ClaimsIdentity(
						new GenericIdentity(baseUser.Email),
						new[]
						{
							new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
							new Claim(JwtRegisteredClaimNames.UniqueName, user.Email),
						}
					);

					DateTime createDate = DateTime.Now;
					DateTime expirationDate = createDate + TimeSpan.FromSeconds(_tokenConfigurations.Seconds);

					JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

					string token = CreateToken(identity, createDate, expirationDate, handler);

					return SuccessObject(createDate, expirationDate, token, baseUser);
				}
			}
			else
			{
				return new
				{
					authenticated = false,
					message = "Falha ao autenticar"
				};
			}
		}

		private object SuccessObject(DateTime createDate, DateTime expirationDate, string token, UserEntity user)
		{
			return new
			{
				authenticated = true,
				created = createDate.ToString("yyyy-MM-dd HH:mm:ss"),
				expires = expirationDate.ToString("yyyy-MM-dd HH:mm:ss"),
				accessToken = token,
				userName = user.Email,
				name = user.Name,
				message = "Usuário Logado com Sucesso"
			};
		}

		private string CreateToken(ClaimsIdentity identity, DateTime createDate, DateTime expirationDate, JwtSecurityTokenHandler handler)
		{
			SecurityToken securityToken = handler.CreateToken(new SecurityTokenDescriptor
			{
				Issuer = _tokenConfigurations.Issuer,
				Audience = _tokenConfigurations.Audience,
				SigningCredentials = _signinConfigurations.SigningCredentials,
				Subject = identity,
				NotBefore = createDate,
				Expires = expirationDate,
			});

			string token = handler.WriteToken(securityToken);
			return token;
		}
	}
}
