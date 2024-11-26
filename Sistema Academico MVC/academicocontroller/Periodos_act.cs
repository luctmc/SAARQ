using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using SAomodel;

namespace SAcontroller
{
    public class Periodos_act
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["SistemaAcademicoDB"].ConnectionString;
        private string caminho = Path.Combine(ConfigurationManager.AppSettings["caminhoBanco"], ConfigurationManager.AppSettings["nomeBancoPeriodos"]);

        public void inserir()
        {
            Console.Write("Digite o ID do período: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("Digite o nome do período: ");
            string nome = Console.ReadLine();
            Console.Write("Digite a sigla do período: ");
            string sigla = Console.ReadLine();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Periodos (perid, pernome, persigla) VALUES (@id, @nome, @sigla)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@nome", nome);
                cmd.Parameters.AddWithValue("@sigla", sigla);
                cmd.ExecuteNonQuery();
            }

            SalvarPeriodosEmCsv(id, nome, sigla);

            Console.WriteLine("Período inserido com sucesso!");
            Console.ReadKey();
        }

        private void SalvarPeriodosEmCsv(int id, string nome, string sigla)
        {
            try
            {
                bool existe = File.Exists(caminho);
                using (StreamWriter writer = new StreamWriter(caminho, true))
                {
                    if (!existe) writer.WriteLine("perid,pernome,persigla");
                    writer.WriteLine($"{id},{nome},{sigla}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao salvar no CSV: " + ex.Message);
            }
        }

        public void alterar()
        {
            Console.Write("Digite o ID do período que deseja alterar: ");
            int id = int.Parse(Console.ReadLine());

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string querySelect = "SELECT * FROM Periodos WHERE perid = @id";
                SqlCommand cmdSelect = new SqlCommand(querySelect, conn);
                cmdSelect.Parameters.AddWithValue("@id", id);

                using (SqlDataReader reader = cmdSelect.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        Console.WriteLine("Período não encontrado.");
                        Console.ReadKey();
                        return;
                    }
                }
            }

            Console.Write("Digite o novo nome do período: ");
            string novoNome = Console.ReadLine();
            Console.Write("Digite a nova sigla do período: ");
            string novaSigla = Console.ReadLine();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string queryUpdate = "UPDATE Periodos SET pernome = @nome, persigla = @sigla WHERE perid = @id";
                SqlCommand cmdUpdate = new SqlCommand(queryUpdate, conn);
                cmdUpdate.Parameters.AddWithValue("@id", id);
                cmdUpdate.Parameters.AddWithValue("@nome", novoNome);
                cmdUpdate.Parameters.AddWithValue("@sigla", novaSigla);
                cmdUpdate.ExecuteNonQuery();
            }

            AtualizarPeriodosCsv();

            Console.WriteLine("Período alterado com sucesso!");
            Console.ReadKey();
        }

        private void AtualizarPeriodosCsv()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM Periodos";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        using (StreamWriter writer = new StreamWriter(caminho))
                        {
                            writer.WriteLine("perid,pernome,persigla");
                            while (reader.Read())
                            {
                                writer.WriteLine($"{reader["perid"]},{reader["pernome"]},{reader["persigla"]}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao atualizar o CSV: " + ex.Message);
            }
        }

        public void excluir()
        {
            Console.Write("Digite o ID do período que deseja excluir: ");
            int id = int.Parse(Console.ReadLine());

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string queryDelete = "DELETE FROM Periodos WHERE perid = @id";
                SqlCommand cmdDelete = new SqlCommand(queryDelete, conn);
                cmdDelete.Parameters.AddWithValue("@id", id);

                int rowsAffected = cmdDelete.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Período excluído com sucesso!");
                }
                else
                {
                    Console.WriteLine("Período não encontrado.");
                }
            }

            AtualizarPeriodosCsv();

            Console.ReadKey();
        }

        public void exibirTodos()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM Periodos";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["perid"]} - {reader["pernome"]} - {reader["persigla"]}");
                        }
                    }
                }
                Console.WriteLine("Pressione qualquer tecla para continuar...");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocorreu um erro ao exibir os períodos: " + ex.Message);
            }
            finally
            {
                Console.ReadKey();
            }
        }
    }
}
