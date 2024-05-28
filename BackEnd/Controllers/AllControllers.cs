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
        
        // POST: api/Medicamentos
        [Authorize]
        [HttpPost("Medicamentos")]
        public IActionResult PostMedicamento(MedicamentosApp medicamentosApp)
        {
            try
            {
                // Recuperar o ID do usuário autenticado a partir do token JWT
                var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                // Verificar se o valor recuperado não é nulo e se pode ser convertido para Guid
                if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
                {
                    // Se não for possível recuperar o ID do usuário ou se a conversão falhar, retornar um erro de autorização
                    return Unauthorized("Usuário não autenticado.");
                }

                // Criar uma instância de MedicamentoApp e atribuir os valores do DTO
                var medicamentoApp = new MedicamentosApp
                {
                    Nome = medicamentosApp.Nome,
                    Descricao = medicamentosApp.Descricao,
                    Quantidade = medicamentosApp.Quantidade,
                    Tipo = medicamentosApp.Tipo,
                    Data = medicamentosApp.Data,
                    Hora = medicamentosApp.Hora,
                    UserId = userId // Associar o ID do usuário ao medicamento
                };

                // Adicionar o medicamento ao contexto e salvar as alterações
                _context.MedicamentosApp.Add(medicamentoApp);
                _context.SaveChanges();

                return Ok(medicamentoApp);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro ao adicionar o medicamento: {ex.Message}");
            }
        }

        // GET: api/Medicamentos
        [Authorize]
        [HttpGet("Medicamentos")]        
        public IActionResult GetMedicamentos()
        {
            try
            {
                // Recuperar o ID do usuário autenticado a partir do token JWT
                var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                // Verificar se o valor recuperado não é nulo e se pode ser convertido para Guid
                if (!Guid.TryParse(userIdString, out var userId))
                {
                    // Se não for possível recuperar o ID do usuário, retornar um erro de autorização
                    return Unauthorized("Usuário não autenticado.");
                }

                // Recuperar os medicamentos associados a esse usuário
                var medicamentos = _context.MedicamentosApp.Where(m => m.UserId == userId).ToList();

                return Ok(medicamentos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro ao recuperar os medicamentos: {ex.Message}");
            }
        }

        // PUT: api/Medicamentos/{id}
        [HttpPut("Medicamentos/{id}")]
        [Authorize]
        public IActionResult PutMedicamento(int id, MedicamentoDto medicamentoDto)
        {
            try
            {
                // Recuperar o ID do usuário autenticado a partir do token JWT
                var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                // Verificar se o valor recuperado não é nulo e se pode ser convertido para Guid
                if (!Guid.TryParse(userIdString, out var userId))
                {
                    // Se não for possível recuperar o ID do usuário, retornar um erro de autorização
                    return Unauthorized("Usuário não autenticado.");
                }

                // Verificar se o medicamento com o ID fornecido pertence ao usuário autenticado
                var medicamento = _context.MedicamentosApp.FirstOrDefault(m => m.Id == id && m.UserId == userId);
                if (medicamento == null)
                {
                    return NotFound("Medicamento não encontrado ou não pertence ao usuário autenticado.");
                }

                // Atualizar os detalhes do medicamento
                medicamento.Nome = medicamentoDto.Nome;
                medicamento.Descricao = medicamentoDto.Descricao;
                medicamento.Quantidade = medicamentoDto.Quantidade;
                medicamento.Tipo = medicamentoDto.Tipo;
                medicamento.Data = medicamentoDto.Data;
                medicamento.Hora = medicamentoDto.Hora;

                _context.SaveChanges();

                return Ok(medicamento);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro ao atualizar o medicamento: {ex.Message}");
            }
        }

        // DELETE: api/Medicamentos/{id}
        [HttpDelete("Medicamentos/{id}")]
        [Authorize]
        public IActionResult DeleteMedicamento(int id)
        {
            try
            {
                // Recuperar o ID do usuário autenticado a partir do token JWT
                var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                // Verificar se o valor recuperado não é nulo e se pode ser convertido para Guid
                if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
                {
                    // Se não for possível recuperar o ID do usuário ou se a conversão falhar, retornar um erro de autorização
                    return Unauthorized("Usuário não autenticado.");
                }

                // Verificar se o medicamento pertence ao usuário autenticado
                var medicamentoParaExcluir = _context.MedicamentosApp.FirstOrDefault(m => m.Id == id && m.UserId == userId);
                if (medicamentoParaExcluir == null)
                {
                    return NotFound($"Medicamento com ID {id} não encontrado ou não pertence ao usuário autenticado.");
                }

                // Excluir o medicamento
                _context.MedicamentosApp.Remove(medicamentoParaExcluir);
                _context.SaveChanges();

                return Ok($"Medicamento com ID {id} foi excluído com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro ao excluir o medicamento: {ex.Message}");
            }
        }

    }
}