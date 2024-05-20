using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RemedioDiario.Data;

namespace RemedioDiario.Controllers
{
    [Route("api")]
    [ApiController]
    public class MedicamentosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MedicamentosController(ApplicationDbContext context)
        {
            _context = context;
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

        // GET: api/Medicamentos/Ids
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
