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

                // GET: api/Medicamentos
        [HttpGet("Medicamentos")]
        public IActionResult GetMedicamentos()
        {
            try
            {
                var medicamentos = _context.MedicamentosApp.ToList();

                return Ok(medicamentos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro ao recuperar os medicamentos: {ex.Message}");
            }
        }       
        
        [HttpGet("MedicamentosIds")]
        public IActionResult GetMedicamentoIds()
        {
            try
            {
                var medicamentoIds = _context.MedicamentosApp.Select(m => m.Id).ToList();

                return Ok(medicamentoIds);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro ao obter os IDs dos medicamentos: {ex.Message}");
            }
        }

        // POST: api/Medicamentos
        [HttpPost("Medicamentos")]
        public IActionResult PostMedicamento(MedicamentosApp medicamentosApp)
        {
            try
            {
                _context.MedicamentosApp.Add(medicamentosApp);

                _context.SaveChanges();

                return Ok(medicamentosApp);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro ao adicionar o medicamento: {ex.Message}");
            }
        }

        // PUT: api/Medicamentos/{id}
        [HttpPut("Medicamentos/{id}")]
        public IActionResult PutMedicamento(int id, MedicamentoDto medicamentoDto)
        {
            try
            {
                var medicamento = _context.MedicamentosApp.FirstOrDefault(m => m.Id == id);
                if (medicamento == null)
                {
                    return NotFound("Medicamento não encontrado.");
                }

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
        public IActionResult DeleteMedicamento(int id)
        {
            try
            {
                var medicamentoParaExcluir = _context.MedicamentosApp.FirstOrDefault(m => m.Id == id);

                if (medicamentoParaExcluir == null)
                {
                    return NotFound($"Medicamento com ID {id} não encontrado.");
                }

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