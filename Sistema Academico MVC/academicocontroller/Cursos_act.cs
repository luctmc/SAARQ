using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using SAmodel;

namespace SAcontroller
{
    public class Cursos_act
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["SistemaAcademicoDB"].ConnectionString;
        private string caminho = Path.Combine(ConfigurationManager.AppSettings["caminhoBanco"], ConfigurationManager.AppSettings["nomeBancoCursos"]);

        public void inserir()
        {
            Console.Write("Digite o ID do curso: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("Digite o nome do curso: ");
            string nome = Console.ReadLine();
            Console.Write("Digite a sigla do curso: ");
            string sigla = Console.ReadLine();
            Console.Write("Digite uma observação para o curso: ");
            string obs = Console.ReadLine();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Cursos (cursoid, cursonome, cursosig, cursoobs) VALUES (@id, @nome, @sigla, @obs)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@nome", nome);
                cmd.Parameters.AddWithValue("@sigla", sigla);
                cmd.Parameters.AddWithValue("@obs", obs);
                cmd.ExecuteNonQuery();
            }

            // Salvar em CSV
            SalvarCursosEmCsv(id, nome, sigla, obs);

            Console.WriteLine("Curso inserido com sucesso!");
            Console.ReadKey();
        }

        private void SalvarCursosEmCsv(int id, string nome, string sigla, string obs)
        {
            try
            {
                bool existe = File.Exists(caminho);
                using (StreamWriter writer = new StreamWriter(caminho, true))
                {
                    if (!existe) writer.WriteLine("cursoid,cursonome,cursosig,cursoobs"); // Cabeçalho do CSV
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
            Console.Write("Digite o ID do curso que deseja alterar: ");
            int id = int.Parse(Console.ReadLine());

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string querySelect = "SELECT * FROM Cursos WHERE cursoid = @id";
                SqlCommand cmdSelect = new SqlCommand(querySelect, conn);
                cmdSelect.Parameters.AddWithValue("@id", id);

                using (SqlDataReader reader = cmdSelect.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        Console.WriteLine("Curso não encontrado.");
                        Console.ReadKey();
                        return;
                    }
                }
            }

            Console.Write("Digite o novo nome do curso: ");
            string novoNome = Console.ReadLine();
            Console.Write("Digite a nova sigla do curso: ");
            string novaSigla = Console.ReadLine();
            Console.Write("Digite a nova observação para o curso: ");
            string novaObs = Console.ReadLine();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string queryUpdate = "UPDATE Cursos SET cursonome = @nome, cursosig = @sigla, cursoobs = @obs WHERE cursoid = @id";
                SqlCommand cmdUpdate = new SqlCommand(queryUpdate, conn);
                cmdUpdate.Parameters.AddWithValue("@id", id);
                cmdUpdate.Parameters.AddWithValue("@nome", novoNome);
                cmdUpdate.Parameters.AddWithValue("@sigla", novaSigla);
                cmdUpdate.Parameters.AddWithValue("@obs", novaObs);
                cmdUpdate.ExecuteNonQuery();
            }

            // Atualizar CSV
            AtualizarCursosCsv();

            Console.WriteLine("Curso alterado com sucesso!");
            Console.ReadKey();
        }

        private void AtualizarCursosCsv()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM Cursos";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        using (StreamWriter writer = new StreamWriter(caminho))
                        {
                            writer.WriteLine("cursoid,cursonome,cursosig,cursoobs");
                            while (reader.Read())
                            {
                                writer.WriteLine($"{reader["cursoid"]},{reader["cursonome"]},{reader["cursosig"]},{reader["cursoobs"]}");
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
            Console.Write("Digite o ID do curso que deseja excluir: ");
            int id = int.Parse(Console.ReadLine());

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string queryDelete = "DELETE FROM Cursos WHERE cursoid = @id";
                SqlCommand cmdDelete = new SqlCommand(queryDelete, conn);
                cmdDelete.Parameters.AddWithValue("@id", id);

                int rowsAffected = cmdDelete.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Curso excluído com sucesso!");
                }
                else
                {
                    Console.WriteLine("Curso não encontrado.");
                }
            }

            // Atualizar CSV
            AtualizarCursosCsv();

            Console.ReadKey();
        }

        public void exibirTodos()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM Cursos";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["cursoid"]} - {reader["cursonome"]} - {reader["cursosig"]} - {reader["cursoobs"]}");
                        }
                    }
                }
                Console.WriteLine("Pressione qualquer tecla para continuar...");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocorreu um erro ao exibir os cursos: " + ex.Message);
            }
            finally
            {
                Console.ReadKey(); // Aguarda o usuário pressionar uma tecla
            }
        }
    }
}
