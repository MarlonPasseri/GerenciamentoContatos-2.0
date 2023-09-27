using GerenciamentoContatos.Entities;
using System;
using System.Collections.Generic;

namespace GerenciamentoContatos.Data
{
    public interface IContatoRepository
    {
        List<Contato> GetAll();
        Contato GetById(Guid id);
        void Add(Contato contato);
        void Update(Contato contato);
        void Delete(Guid id);
        List<Contato> Search(string keyword);
    }
}
