using System;
using SAcontroller;

namespace SAview
{
    public class menuPrincipal
    {
        private Cursos_act cursosController = new Cursos_act();
        private Disciplinas_act disciplinasController = new Disciplinas_act();
        private Periodos_act periodosController = new Periodos_act();

        public void ExibirMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Sistema Acadêmico - Menu Principal");
                Console.WriteLine("1. Operações de Cursos");
                Console.WriteLine("2. Operações de Disciplinas");
                Console.WriteLine("3. Operações de Períodos");
                Console.WriteLine("4. Sair");
                Console.Write("Escolha uma opção: ");

                string opcao = Console.ReadLine();
                switch (opcao)
                {
                    case "1":
                        MenuCursos();
                        break;
                    case "2":
                        MenuDisciplinas();
                        break;
                    case "3":
                        MenuPeriodos();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Opção inválida!");
                        break;
                }
            }
        }

        private void MenuCursos()
        {
            Console.Clear();
            Console.WriteLine("Operações de Cursos");
            Console.WriteLine("1. Inserir Curso");
            Console.WriteLine("2. Exibir Todos os Cursos");
            Console.WriteLine("3. Alterar Curso");
            Console.WriteLine("4. Excluir Curso");
            Console.WriteLine("5. Voltar");
            Console.Write("Escolha uma opção: ");

            string opcao = Console.ReadLine();
            switch (opcao)
            {
                case "1":
                    cursosController.inserir(); // Chama o método de inserção no controlador
                    break;
                case "2":
                    cursosController.exibirTodos(); // Chama o método de exibição no controlador
                    Console.WriteLine("Pressione qualquer tecla para continuar...");
                    Console.ReadKey();
                    break;
                case "3":
                    cursosController.alterar(); // Chama o método de alteração no controlador
                    break;
                case "4":
                    cursosController.excluir(); // Chama o método de exclusão no controlador
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Opção inválida!");
                    break;
            }
        }

        private void MenuDisciplinas()
        {
            Console.Clear();
            Console.WriteLine("Operações de Disciplinas");
            Console.WriteLine("1. Inserir Disciplina");
            Console.WriteLine("2. Exibir Todas as Disciplinas");
            Console.WriteLine("3. Alterar Disciplina");
            Console.WriteLine("4. Excluir Disciplina");
            Console.WriteLine("5. Voltar");
            Console.Write("Escolha uma opção: ");

            string opcao = Console.ReadLine();
            switch (opcao)
            {
                case "1":
                    disciplinasController.inserir(); // Chama o método de inserção no controlador
                    break;
                case "2":
                    disciplinasController.exibirTodos(); // Chama o método de exibição no controlador
                    Console.WriteLine("Pressione qualquer tecla para continuar...");
                    Console.ReadKey();
                    break;
                case "3":
                    disciplinasController.alterar(); // Chama o método de alteração no controlador
                    break;
                case "4":
                    disciplinasController.excluir(); // Chama o método de exclusão no controlador
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Opção inválida!");
                    break;
            }
        }

        private void MenuPeriodos()
        {
            Console.Clear();
            Console.WriteLine("Operações de Períodos");
            Console.WriteLine("1. Inserir Período");
            Console.WriteLine("2. Exibir Todos os Períodos");
            Console.WriteLine("3. Alterar Período");
            Console.WriteLine("4. Excluir Período");
            Console.WriteLine("5. Voltar");
            Console.Write("Escolha uma opção: ");

            string opcao = Console.ReadLine();
            switch (opcao)
            {
                case "1":
                    periodosController.inserir(); // Chama o método de inserção no controlador
                    break;
                case "2":
                    periodosController.exibirTodos(); // Chama o método de exibição no controlador
                    Console.WriteLine("Pressione qualquer tecla para continuar...");
                    Console.ReadKey();
                    break;
                case "3":
                    periodosController.alterar(); // Chama o método de alteração no controlador
                    break;
                case "4":
                    periodosController.excluir(); // Chama o método de exclusão no controlador
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Opção inválida!");
                    break;
            }
        }
    }
}
