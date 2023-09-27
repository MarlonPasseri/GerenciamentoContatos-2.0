using GerenciamentoContatos.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GerenciamentoContatos.Data
{
    public class ContatoRepository : IContatoRepository
    {
        private List<Contato> contatos = new List<Contato>();

        public List<Contato> GetAll()
        {
            return contatos;
        }

        public Contato GetById(Guid id)
        {
            return contatos.FirstOrDefault(c => c.Id == id);
        }

        public void Add(Contato contato)
        {
            contato.Id = Guid.NewGuid();
            contatos.Add(contato);
        }

        public void Update(Contato contato)
        {
            var existingContato = contatos.FirstOrDefault(c => c.Id == contato.Id);
            if (existingContato != null)
            {
                existingContato.Nome = contato.Nome;
                existingContato.Email = contato.Email;
                existingContato.Telefone = contato.Telefone;
            }
        }

        public void Delete(Guid id)
        {
            var contatoToDelete = contatos.FirstOrDefault(c => c.Id == id);
            if (contatoToDelete != null)
            {
                contatos.Remove(contatoToDelete);
            }
        }

        public List<Contato> Search(string keyword)
        {
            return contatos
                .Where(c => c.Nome.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}
