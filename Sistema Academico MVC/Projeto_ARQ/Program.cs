using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using SAview;

namespace SA
{
    internal class Program
    {
        static void Main(string[] args)
        {
            menuPrincipal menu = new menuPrincipal();
            menu.ExibirMenu(); 
        }
    }
}