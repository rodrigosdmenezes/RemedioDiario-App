using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RemedioDiario.Data;
using RemedioDiario.Entitys;


namespace LonginControllers.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class LoginController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public LoginController(IConfiguration config, ApplicationDbContext context)
        {
            _config = config;

            _context = context;
        }


        // POST: api/registrar
        [HttpPost("registrar")]
        [AllowAnonymous]
        public IActionResult Register(RegisterDto registerDto)
        {
            try
            {
                if (_context.RegistrarApp.Any(u => u.Email == registerDto.Email))
                {
                    return Conflict("O email informado já está em uso.");
                }

                var newUser = new RegistrarApp
                {
                    Nome = registerDto.Nome,
                    Email = registerDto.Email,
                    Password = registerDto.Password
                };

                _context.RegistrarApp.Add(newUser);
                _context.SaveChanges();

                return Ok(newUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro ao criar o usuário: {ex.Message}");
            }
        }


        // POST: api/login
        [HttpPost("login")]
        [Authorize]
        public async Task<IActionResult> Login([FromBody] LoginApp model)
        {
            try
            {
                var user = await _context.RegistrarApp.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);

                if (user == null)
                {
                    return Unauthorized("Credenciais inválidas.");
                }

                // Se chegou aqui, o usuário está autenticado com sucesso
                // Faça o que for necessário e retorne uma resposta apropriada

                return Ok("Usuário autenticado com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro ao fazer login: {ex.Message}");
            }
        }

        [AllowAnonymous]
        // POST: api/resetpassword
        [HttpPost("resetpassword")]
        public IActionResult ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = _context.RegistrarApp.FirstOrDefault(u => u.Email == resetPasswordDto.Email);

                if (user == null)
                {
                    return NotFound("Usuário não encontrado.");
                }

                user.Password = resetPasswordDto.NewPassword;

                _context.SaveChanges();

                return Ok("Senha resetada com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro ao redefinir a senha: {ex.Message}");
            }
        }
        private RegistrarApp GetUserById(Guid id)
        {
            return _context.RegistrarApp.FirstOrDefault(u => u.Id == id);
        }

    }
}
