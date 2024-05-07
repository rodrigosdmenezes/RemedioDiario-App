namespace RemedioDiario.Entitys
{
    public class MedicamentosApp
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public DateTime HoraTomar { get; set; }
        public bool Comprimido { get; set; }
        public bool Gotas { get; set; }

    }
}
