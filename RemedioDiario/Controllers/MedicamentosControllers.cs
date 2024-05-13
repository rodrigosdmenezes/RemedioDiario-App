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
                // Obtém todos os medicamentos cadastrados no banco de dados
                var medicamentos = _context.MedicamentosApp.ToList();

                // Retorna uma resposta com a lista de medicamentos
                return Ok(medicamentos);
            }
            catch (Exception ex)
            {
                // Em caso de erro, retorna uma resposta de erro com o detalhe do erro
                return StatusCode(500, $"Ocorreu um erro ao recuperar os medicamentos: {ex.Message}");
            }
        }

        // GET: api/Medicamentos/Ids
        [HttpGet("MedicamentosIds")]
        public IActionResult GetMedicamentoIds()
        {
            try
            {
                // Obtém apenas os IDs dos medicamentos
                var medicamentoIds = _context.MedicamentosApp.Select(m => m.Id).ToList();
                
                // Retorna os IDs dos medicamentos
                return Ok(medicamentoIds);
            }
            catch (Exception ex)
            {
                // Em caso de erro, retorna uma resposta de erro com o detalhe do erro
                return StatusCode(500, $"Ocorreu um erro ao obter os IDs dos medicamentos: {ex.Message}");
            }
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
                medicamento.Quantidade = medicamentoDto.Quantidade;
                medicamento.Tipo = medicamentoDto.Tipo;
                medicamento.Data = medicamentoDto.Data;
                medicamento.Hora = medicamentoDto.Hora;

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
        [HttpDelete("excluir/{id}")]
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
