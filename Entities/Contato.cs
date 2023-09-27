using System;

namespace GerenciamentoContatos.Entities
{
    public class Contato
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }

        public DateTime DataCadastro { get; set; }
    }
}
