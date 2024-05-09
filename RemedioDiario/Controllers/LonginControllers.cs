using Microsoft.AspNetCore.Mvc;
using RemedioDiario.Data;
using RemedioDiario.Entitys;


namespace LonginControllers.Controllers
{
    [Route("api")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/registrar
        [HttpPost("registrar")]
        public IActionResult Register(RegisterDto registerDto)
        {
            try
            {
                // Verifica se já existe um usuário com o mesmo email
                if (_context.RegistrarApp.Any(u => u.Email == registerDto.Email))
                {
                    return Conflict("O email informado já está em uso.");
                }

                // Cria um novo usuário
                var newUser = new RegistrarApp
                {
                    Nome = registerDto.Nome,
                    Email = registerDto.Email,
                    Password = registerDto.Password
                };

                _context.RegistrarApp.Add(newUser);
                _context.SaveChanges();

                // Retorna o novo usuário criado
                return Ok(newUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro ao criar o usuário: {ex.Message}");
            }
        }

        // POST: api/login
        [HttpPost("login")]
        public IActionResult Login(LoginApp login)
        {
            try
            {
                var user = _context.RegistrarApp.FirstOrDefault(u => u.Email == login.Email);

                if (user == null || user.Password != login.Password)
                {
                    return Unauthorized("Credenciais inválidas.");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro ao fazer login: {ex.Message}");
            }
        }

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

                // Atualiza a senha do usuário com a nova senha fornecida
                user.Password = resetPasswordDto.NewPassword;

                // Salva as mudanças no banco de dados
                _context.SaveChanges();

                return Ok("Senha resetada com sucesso.");
            }
            catch (Exception ex)
            {
                // Em caso de erro, retorna uma resposta de erro
                return StatusCode(500, $"Ocorreu um erro ao redefinir a senha: {ex.Message}");
            }
        }
        // Método auxiliar para obter um usuário por ID
        private RegistrarApp GetUserById(Guid id)
        {
            return _context.RegistrarApp.FirstOrDefault(u => u.Id == id);
        }
    }
}
