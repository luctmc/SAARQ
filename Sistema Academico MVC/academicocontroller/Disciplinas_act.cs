using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using SAmodel;

namespace SAcontroller
{
    public class Disciplinas_act
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["SistemaAcademicoDB"].ConnectionString;
        private string caminho = Path.Combine(ConfigurationManager.AppSettings["caminhoBanco"], ConfigurationManager.AppSettings["nomeBancoDisciplinas"]);

        public void inserir()
        {
            Console.Write("Digite o ID da disciplina: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("Digite o nome da disciplina: ");
            string nome = Console.ReadLine();
            Console.Write("Digite a sigla da disciplina: ");
            string sigla = Console.ReadLine();
            Console.Write("Digite uma observação para a disciplina: ");
            string obs = Console.ReadLine();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Disciplinas (disid, disnome, dissig, disobs) VALUES (@id, @nome, @sigla, @obs)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@nome", nome);
                cmd.Parameters.AddWithValue("@sigla", sigla);
                cmd.Parameters.AddWithValue("@obs", obs);
                cmd.ExecuteNonQuery();
            }

            SalvarDisciplinasEmCsv(id, nome, sigla, obs);

            Console.WriteLine("Disciplina inserida com sucesso!");
            Console.ReadKey();
        }

        private void SalvarDisciplinasEmCsv(int id, string nome, string sigla, string obs)
        {
            try
            {
                bool existe = File.Exists(caminho);
                using (StreamWriter writer = new StreamWriter(caminho, true))
                {
                    if (!existe) writer.WriteLine("disid,disnome,dissig,disobs");
                    writer.WriteLine($"{id},{nome},{sigla},{obs}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao salvar no CSV: " + ex.Message);
            }
        }

        public void alterar()
        {
            Console.Write("Digite o ID da disciplina que deseja alterar: ");
            int id = int.Parse(Console.ReadLine());

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string querySelect = "SELECT * FROM Disciplinas WHERE disid = @id";
                SqlCommand cmdSelect = new SqlCommand(querySelect, conn);
                cmdSelect.Parameters.AddWithValue("@id", id);

                using (SqlDataReader reader = cmdSelect.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        Console.WriteLine("Disciplina não encontrada.");
                        Console.ReadKey();
                        return;
                    }
                }
            }

            Console.Write("Digite o novo nome da disciplina: ");
            string novoNome = Console.ReadLine();
            Console.Write("Digite a nova sigla da disciplina: ");
            string novaSigla = Console.ReadLine();
            Console.Write("Digite a nova observação para a disciplina: ");
            string novaObs = Console.ReadLine();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string queryUpdate = "UPDATE Disciplinas SET disnome = @nome, dissig = @sigla, disobs = @obs WHERE disid = @id";
                SqlCommand cmdUpdate = new SqlCommand(queryUpdate, conn);
                cmdUpdate.Parameters.AddWithValue("@id", id);
                cmdUpdate.Parameters.AddWithValue("@nome", novoNome);
                cmdUpdate.Parameters.AddWithValue("@sigla", novaSigla);
                cmdUpdate.Parameters.AddWithValue("@obs", novaObs);
                cmdUpdate.ExecuteNonQuery();
            }

            AtualizarDisciplinasCsv();

            Console.WriteLine("Disciplina alterada com sucesso!");
            Console.ReadKey();
        }

        private void AtualizarDisciplinasCsv()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM Disciplinas";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        using (StreamWriter writer = new StreamWriter(caminho))
                        {
                            writer.WriteLine("disid,disnome,dissig,disobs");
                            while (reader.Read())
                            {
                                writer.WriteLine($"{reader["disid"]},{reader["disnome"]},{reader["dissig"]},{reader["disobs"]}");
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
            Console.Write("Digite o ID da disciplina que deseja excluir: ");
            int id = int.Parse(Console.ReadLine());

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string queryDelete = "DELETE FROM Disciplinas WHERE disid = @id";
                SqlCommand cmdDelete = new SqlCommand(queryDelete, conn);
                cmdDelete.Parameters.AddWithValue("@id", id);

                int rowsAffected = cmdDelete.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Disciplina excluída com sucesso!");
                }
                else
                {
                    Console.WriteLine("Disciplina não encontrada.");
                }
            }

            AtualizarDisciplinasCsv();

            Console.ReadKey();
        }

        public void exibirTodos()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM Disciplinas";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["disid"]} - {reader["disnome"]} - {reader["dissig"]} - {reader["disobs"]}");
                        }
                    }
                }
                Console.WriteLine("Pressione qualquer tecla para continuar...");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocorreu um erro ao exibir as disciplinas: " + ex.Message);
            }
            finally
            {
                Console.ReadKey();
            }
        }
    }
}
