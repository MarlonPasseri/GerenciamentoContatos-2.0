using GerenciamentoContatos.Entities;
using GerenciamentoContatos.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace GerenciamentoContatos
{
    class Program
    {
        private static List<Contato> contatos = new List<Contato>();
        private static StorageType storageType = StorageType.MemoryList; // Tipo de armazenamento padrão
        private static string txtFileName = "contatos.txt"; // Nome padrão do arquivo de texto

        static void Main(string[] args)
        {
            Console.WriteLine("Bem-vindo ao Gerenciamento de Contatos!");
            Console.WriteLine("======================================");

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Gerenciamento de Contatos");
                Console.WriteLine("=========================");

                List<Contato> contatos = GetAll();

                if (contatos.Count > 0)
                {
                    Console.WriteLine("Últimos Contatos Cadastrados:");
                    for (int i = Math.Max(0, contatos.Count - 5); i < contatos.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {contatos[i].Nome}");
                    }
                }
                else
                {
                    Console.WriteLine("Nenhum contato cadastrado.");
                }

                Console.WriteLine("\nMenu Principal:");
                Console.WriteLine("1. Incluir Contato");
                Console.WriteLine("2. Alterar Contato");
                Console.WriteLine("3. Excluir Contato");
                Console.WriteLine("4. Pesquisar Contato");
                Console.WriteLine("5. Salvar Contatos");
                Console.WriteLine("6. Carregar Contatos");
                Console.WriteLine("7. Escolher Tipo de Armazenamento");
                Console.WriteLine("8. Sair");

                Console.Write("Escolha uma opção: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AdicionarContato();
                        break;
                    case "2":
                        AtualizarContato();
                        break;
                    case "3":
                        ExcluirContato();
                        break;
                    case "4":
                        PesquisarContato();
                        break;
                    case "5":
                        if (storageType == StorageType.MemoryList)
                        {
                            SalvarContatosEmMemoria();
                        }
                        else if (storageType == StorageType.FileTxt)
                        {
                            SalvarContatosEmArquivo();
                        }
                        break;
                    case "6":
                        CarregarContatos();
                        break;
                    case "7":
                        EscolherTipoArmazenamento();
                        break;
                    case "8":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Opção inválida. Tente novamente.");
                        break;
                }
            }
        }

        static List<Contato> GetAll()
        {
            return contatos;
        }

        static void AdicionarContato()
        {
            Console.Clear();
            Console.WriteLine("Incluir Contato");
            Console.WriteLine("===============");

            Console.Write("Nome: ");
            string nome = Console.ReadLine();

            Console.Write("Email: ");
            string email = Console.ReadLine();

            Console.Write("Telefone: ");
            string telefone = Console.ReadLine();

            Contato novoContato = new Contato
            {
                Nome = nome,
                Email = email,
                Telefone = telefone,
                Id = Guid.NewGuid(),
                DataCadastro = DateTime.Now // Adiciona a data de cadastro ao novo contato
            };

            contatos.Add(novoContato);
            Console.WriteLine("Contato adicionado com sucesso!");
            Console.ReadLine();
        }

        static void AtualizarContato()
        {
            Console.Clear();
            Console.WriteLine("Alterar Contato");
            Console.WriteLine("===============");

            Console.Write("Informe o ID do contato que deseja atualizar: ");
            if (Guid.TryParse(Console.ReadLine(), out Guid id))
            {
                Contato contato = contatos.FirstOrDefault(c => c.Id == id);
                if (contato != null)
                {
                    Console.WriteLine("Dados atuais do contato:");
                    Console.WriteLine($"Nome: {contato.Nome}");
                    Console.WriteLine($"Email: {contato.Email}");
                    Console.WriteLine($"Telefone: {contato.Telefone}");

                    Console.WriteLine("\nInforme os novos dados:");

                    Console.Write("Nome: ");
                    string novoNome = Console.ReadLine();

                    Console.Write("Email: ");
                    string novoEmail = Console.ReadLine();

                    Console.Write("Telefone: ");
                    string novoTelefone = Console.ReadLine();

                    contato.Nome = novoNome;
                    contato.Email = novoEmail;
                    contato.Telefone = novoTelefone;

                    Console.WriteLine("Contato atualizado com sucesso!");
                }
                else
                {
                    Console.WriteLine("Contato não encontrado.");
                }
            }
            else
            {
                Console.WriteLine("ID inválido.");
            }

            Console.ReadLine();
        }

        static void ExcluirContato()
        {
            Console.Clear();
            Console.WriteLine("Excluir Contato");
            Console.WriteLine("===============");

            Console.Write("Informe o ID do contato que deseja excluir: ");
            if (Guid.TryParse(Console.ReadLine(), out Guid id))
            {
                Contato contato = contatos.FirstOrDefault(c => c.Id == id);
                if (contato != null)
                {
                    Console.WriteLine("Dados do contato:");
                    Console.WriteLine($"Nome: {contato.Nome}");
                    Console.WriteLine($"Email: {contato.Email}");
                    Console.WriteLine($"Telefone: {contato.Telefone}");

                    Console.Write("Deseja realmente excluir este contato? (S/N): ");
                    string confirmacao = Console.ReadLine();

                    if (confirmacao.Equals("S", StringComparison.OrdinalIgnoreCase))
                    {
                        contatos.Remove(contato);
                        Console.WriteLine("Contato excluído com sucesso!");
                    }
                    else
                    {
                        Console.WriteLine("Exclusão cancelada.");
                    }
                }
                else
                {
                    Console.WriteLine("Contato não encontrado.");
                }
            }
            else
            {
                Console.WriteLine("ID inválido.");
            }

            Console.ReadLine();
        }

        static void PesquisarContato()
        {
            Console.Clear();
            Console.WriteLine("Pesquisar Contato");
            Console.WriteLine("=================");

            Console.Write("Digite o termo de pesquisa (nome, email ou telefone): ");
            string termo = Console.ReadLine();

            List<Contato> resultados = contatos
                .Where(c => c.Nome.Contains(termo, StringComparison.OrdinalIgnoreCase) ||
                            c.Email.Contains(termo, StringComparison.OrdinalIgnoreCase) ||
                            c.Telefone.Contains(termo, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (resultados.Count > 0)
            {
                Console.WriteLine("Resultados da pesquisa:");
                for (int i = 0; i < resultados.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {resultados[i].Nome}");
                }

                Console.Write("Digite o número do contato para ver os detalhes (ou 0 para voltar): ");
                if (int.TryParse(Console.ReadLine(), out int escolha) && escolha >= 1 && escolha <= resultados.Count)
                {
                    MostrarDetalhesContato(resultados[escolha - 1]);
                }
            }
            else
            {
                Console.WriteLine("Nenhum contato encontrado.");
            }

            Console.ReadLine();
        }

        static void MostrarDetalhesContato(Contato contato)
        {
            Console.Clear();
            Console.WriteLine("Detalhes do Contato");
            Console.WriteLine("====================");
            Console.WriteLine($"Nome: {contato.Nome}");
            Console.WriteLine($"Email: {contato.Email}");
            Console.WriteLine($"Telefone: {contato.Telefone}");
            Console.WriteLine($"ID: {contato.Id}");
            Console.WriteLine($"Data de Cadastro: {contato.DataCadastro}");
            Console.ReadLine();
        }

        static void EscolherTipoArmazenamento()
        {
            Console.Clear();
            Console.WriteLine("Escolher Tipo de Armazenamento");
            Console.WriteLine("==============================");

            Console.WriteLine("Escolha o tipo de armazenamento:");
            Console.WriteLine("1. Memória (Lista)");
            Console.WriteLine("2. Arquivo de Texto (.txt)");

            Console.Write("Digite o número da opção desejada: ");
            if (int.TryParse(Console.ReadLine(), out int escolha))
            {
                if (escolha == 1)
                {
                    storageType = StorageType.MemoryList;
                    Console.WriteLine("Tipo de armazenamento definido como Memória (Lista).");
                }
                else if (escolha == 2)
                {
                    storageType = StorageType.FileTxt;
                    Console.Write("Digite o nome do arquivo de texto (.txt) para usar: ");
                    txtFileName = Console.ReadLine();
                    Console.WriteLine($"Tipo de armazenamento definido como Arquivo de Texto ({txtFileName}).");
                }
                else
                {
                    Console.WriteLine("Opção inválida. Voltando ao menu principal.");
                }
            }
            else
            {
                Console.WriteLine("Opção inválida. Voltando ao menu principal.");
            }

            Console.ReadLine();
        }

        static void SalvarContatosEmMemoria()
        {
            Console.Clear();
            Console.WriteLine("Salvar Contatos em Memória");
            Console.WriteLine("==========================");

            // Implemente aqui o código para salvar os contatos em memória (lista).
            // Neste caso, como os contatos já estão na lista, não é necessário fazer nada aqui.

            Console.WriteLine("Contatos salvos em memória com sucesso!");
            Console.ReadLine();
        }

        static void SalvarContatosEmArquivo()
        {
            Console.Clear();
            Console.WriteLine("Salvar Contatos em Arquivo de Texto");
            Console.WriteLine("==================================");

            Console.Write("Digite o nome do arquivo de texto (.txt) para salvar os contatos: ");
            string fileName = Console.ReadLine();

            try
            {
                // Obtém o diretório de trabalho atual (onde o executável está localizado)
                string diretorioAtual = Directory.GetCurrentDirectory();
                // Combina o diretório atual com o nome do arquivo para criar o caminho completo
                string caminhoArquivo = Path.Combine(diretorioAtual, fileName);

                using (StreamWriter sw = new StreamWriter(caminhoArquivo))
                {
                    foreach (Contato contato in contatos)
                    {
                        string contatoJson = JsonSerializer.Serialize(contato);
                        sw.WriteLine(contatoJson);
                    }
                }
                Console.WriteLine($"Contatos salvos com sucesso no arquivo {fileName}!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao salvar contatos: {ex.Message}");
            }

            Console.ReadLine();
        }

        static void CarregarContatos()
        {
            Console.Clear();
            Console.WriteLine("Carregar Contatos");
            Console.WriteLine("=================");

            if (storageType == StorageType.MemoryList)
            {
                Console.WriteLine("Os contatos já estão carregados em memória (lista).");
            }
            else if (storageType == StorageType.FileTxt)
            {
                Console.Write("Digite o nome do arquivo de texto (.txt) para carregar os contatos: ");
                string fileName = Console.ReadLine();

                try
                {
                    // Obtém o diretório de trabalho atual (onde o executável está localizado)
                    string diretorioAtual = Directory.GetCurrentDirectory();
                    // Combina o diretório atual com o nome do arquivo para criar o caminho completo
                    string caminhoArquivo = Path.Combine(diretorioAtual, fileName);

                    if (File.Exists(caminhoArquivo))
                    {
                        using (StreamReader sr = new StreamReader(caminhoArquivo))
                        {
                            contatos.Clear(); // Limpa a lista atual de contatos
                            string linha;
                            while ((linha = sr.ReadLine()) != null)
                            {
                                Contato contato = JsonSerializer.Deserialize<Contato>(linha);
                                contatos.Add(contato);
                            }
                        }
                        Console.WriteLine($"Contatos carregados com sucesso do arquivo {fileName}!");
                    }
                    else
                    {
                        Console.WriteLine($"O arquivo {fileName} não existe.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao carregar contatos: {ex.Message}");
                }
            }

            Console.ReadLine();
        }
    }
}
