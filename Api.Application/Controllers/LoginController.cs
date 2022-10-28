using Api.Domain.Entities;
using Domain.Interfaces.Services.User;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace application.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LoginController : ControllerBase
	{
		[HttpPost]
		public async Task<object> Login([FromBody] UserEntity userEntity, [FromServices] ILoginService loginService)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);
			if (userEntity == null) return BadRequest();
			try
			{
				object result = await loginService.FindByLogin(userEntity);
				if (result != null)
				{
					return Ok(result);
				}
				else
				{
					return NotFound();
				}
			}
			catch(ArgumentException e)
			{
				return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
			}
		}
	}
}
