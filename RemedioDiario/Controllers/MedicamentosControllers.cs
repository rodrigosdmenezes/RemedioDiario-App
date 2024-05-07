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

        // POST: api/Medicamentos
        [HttpPost("Medicamentos")]
        public IActionResult PostMedicamento(MedicamentosApp medicamentosApp)
        {
            try
            {
                // Adiciona o novo medicamento ao contexto do banco de dados
                _context.MedicamentosApp.Add(medicamentosApp);

                // Salva as alterações no banco de dados
                _context.SaveChanges();

                // Retorna uma resposta de sucesso com o novo medicamento criado
                return Ok(medicamentosApp);
            }
            catch (Exception ex)
            {
                // Em caso de erro, retorna uma resposta de erro com o detalhe do erro
                return StatusCode(500, $"Ocorreu um erro ao adicionar o medicamento: {ex.Message}");
            }
        }

        // PUT: api/Medicamentos/{id}
        [HttpPut("Medicamentos/{id}")]
        public IActionResult PutMedicamento(int id, MedicamentoDto medicamentoDto)
        {
            try
            {
                // Verifica se o ID do medicamento fornecido corresponde a algum medicamento existente
                var medicamento = _context.MedicamentosApp.FirstOrDefault(m => m.Id == id);
                if (medicamento == null)
                {
                    return NotFound("Medicamento não encontrado.");
                }

                // Atualiza as propriedades do medicamento com base nos dados fornecidos no DTO
                medicamento.Nome = medicamentoDto.Nome;
                medicamento.Descricao = medicamentoDto.Descricao;
                medicamento.HoraTomar = medicamentoDto.HoraTomar;
                medicamento.Comprimido = medicamentoDto.Comprimido;
                medicamento.Gotas = medicamentoDto.Gotas;

                // Salva as alterações no banco de dados
                _context.SaveChanges();

                // Retorna uma resposta de sucesso com o medicamento atualizado
                return Ok(medicamento);
            }
            catch (Exception ex)
            {
                // Em caso de erro, retorna uma resposta de erro com o detalhe do erro
                return StatusCode(500, $"Ocorreu um erro ao atualizar o medicamento: {ex.Message}");
            }
        }
        // DELETE: api/Medicamentos/{id}
        [HttpDelete("Medicamentos/{id}")]
        public IActionResult DeleteMedicamento(int id)
        {
            try
            {
                // Busca o medicamento pelo ID no banco de dados
                var medicamentoParaExcluir = _context.MedicamentosApp.FirstOrDefault(m => m.Id == id);

                // Se o medicamento não for encontrado, retorna um erro 404 Not Found
                if (medicamentoParaExcluir == null)
                {
                    return NotFound($"Medicamento com ID {id} não encontrado.");
                }

                // Remove o medicamento do contexto do banco de dados
                _context.MedicamentosApp.Remove(medicamentoParaExcluir);

                // Salva as alterações no banco de dados
                _context.SaveChanges();

                // Retorna uma resposta de sucesso indicando que o medicamento foi excluído
                return Ok($"Medicamento com ID {id} foi excluído com sucesso.");
            }
            catch (Exception ex)
            {
                // Em caso de erro, retorna uma resposta de erro com o detalhe do erro
                return StatusCode(500, $"Ocorreu um erro ao excluir o medicamento: {ex.Message}");
            }
        }

    }

}
