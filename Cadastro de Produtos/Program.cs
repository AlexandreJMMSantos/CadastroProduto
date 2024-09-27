using System;
using System.Collections.Generic;

namespace CadastroProduto
{
    class Program
    {
        enum TipoProduto
        {
            Final,
            Intermediario,
            Consumo,
            MateriaPrima
        }

        class Produto
        {
            public string Descricao { get; set; }
            public double ValorVenda { get; set; }
            public double ValorCompra { get; set; }
            public TipoProduto Tipo { get; set; }
            public DateTime DataCriacao { get; set; }
            public double MargemLucro { get; set; }
        }

        static void Main(string[] args)
        {
            Console.ResetColor();
            List<Produto> produtos = new List<Produto>();

            while (true)
            {
                ClearConsole();

                Console.WriteLine(@"
                     ____ ____ ____ ____ ____ ____ ____ ____ 
                    ||P |||r |||o |||d |||u |||t |||o |||s ||
                    ||__|||__|||__|||__|||__|||__|||__|||__||
                    |/__\|/__\|/__\|/__\|/__\|/__\|/__\|/__\|
                ");

                Console.WriteLine(Environment.NewLine + "Informe novo produto:");

                string descricao = string.Empty;
                while (true)
                {
                    Console.Write("* Descrição: ");
                    string input = Console.ReadLine();

                    if (string.IsNullOrEmpty(input) || input.Length < 5)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("####### Descrição deve ter no mínimo 5 caracteres #######" + Environment.NewLine);
                        Console.ResetColor();
                        continue;
                    }
                    else
                    {
                        descricao = input;
                        break;
                    }
                }

                double valorVenda = 0;
                while (true)
                {
                    Console.Write("* Val. Venda: ");
                    string input = Console.ReadLine();

                    if (double.TryParse(input, out valorVenda))
                    {
                        if (valorVenda <= 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("####### Valor precisa ser maior que zero #######" + Environment.NewLine);
                            Console.ResetColor();
                            continue;
                        }
                        else
                        {
                            if (valorVenda > double.MaxValue)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("####### Valor precisa ser menor que " + double.MaxValue.ToString() + " #######" + Environment.NewLine);
                                Console.ResetColor();
                                continue;
                            }

                            break;
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("####### Somente valor numérico #######" + Environment.NewLine);
                        Console.ResetColor();
                    }
                }

                double valorCompra = 0;
                while (true)
                {
                    Console.Write("* Val. Compra: ");
                    string input = Console.ReadLine();

                    if (double.TryParse(input, out valorCompra))
                    {
                        if (valorCompra >= valorVenda)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("####### Val. Compra deve ser menor que Val. Venda #######" + Environment.NewLine);
                            Console.ResetColor();
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("####### Somente valor numérico #######" + Environment.NewLine);
                        Console.ResetColor();
                        continue;
                    }
                }


                TipoProduto tipo = TipoProduto.Final;
                while (true)
                {
                    Console.Write("* Tipo (F | I | C | M): ");
                    string input = Console.ReadLine().ToUpper();

                    if (input != "F" && input != "I" && input != "C" && input != "M")
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("####### Informe F ou I ou C ou M #######" + Environment.NewLine);
                        Console.ResetColor();
                        continue;
                    }

                    if (input == "I")
                    {
                        tipo = TipoProduto.Intermediario;
                    }
                    else if (input == "C")
                    {
                        tipo = TipoProduto.Consumo;
                    }
                    else if (input == "M")
                    {
                        tipo = TipoProduto.MateriaPrima;
                    }

                    break;
                }

                DateTime dataCriacao = new DateTime();
                while (true)
                {
                    Console.Write("* Data Criação (dd/MM/yyyy): ");
                    string input = Console.ReadLine();

                    if (DateTime.TryParse(input, out dataCriacao))
                    {
                        if (dataCriacao < new DateTime(2024, 1, 1) || dataCriacao > DateTime.Now)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("####### Data deve ser maior que 01/01/2024 e menor que " + DateTime.Today.ToString("dd/MM/yyyy") + " #######" + Environment.NewLine);
                            Console.ResetColor();
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("####### Informe uma data válida #######" + Environment.NewLine);
                        Console.ResetColor();
                        continue;
                    }
                }

                produtos.Add(new Produto
                {
                    Descricao = descricao,
                    ValorVenda = valorVenda,
                    ValorCompra = valorCompra,
                    Tipo = tipo,
                    DataCriacao = dataCriacao,
                    MargemLucro = (valorVenda - valorCompra)
                });

                Console.WriteLine(Environment.NewLine + "-=-=-=-=-=-=-= PRODUTOS (ORD. TIPO) -=-=-=-=-=-=-=-=-=");

                produtos = produtos.OrderBy(x => x.Tipo).ToList();
                foreach (var produto in produtos)
                {
                    Console.WriteLine($"\nDescrição: {produto.Descricao}");
                    Console.WriteLine($"Preço: {produto.ValorVenda.ToString("F2")}");
                    Console.WriteLine($"Quantidade: {produto.ValorCompra.ToString("F2")}");
                    Console.WriteLine($"Categoria: {produto.Tipo}");
                    Console.WriteLine($"Fabricante: {produto.DataCriacao}");
                    Console.WriteLine(Environment.NewLine + "-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=");
                }

                ProdutosMaisLucrativos(produtos);
                ListarSegundoTrimestre(produtos);

                Console.Write(Environment.NewLine + "Pressione ENTER para adicionar outro produto... ");

                string enter = Console.ReadLine();
            }
        }

        private static void ListarSegundoTrimestre(List<Produto> produtos)
        {
            var produtos2Trim = produtos.Where(x => x.DataCriacao.Month == 4 || 
                                                    x.DataCriacao.Month == 5 || 
                                                    x.DataCriacao.Month == 6).ToList();

            if (produtos2Trim.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- PRODUTOS 2º TRIMESTE");

                foreach (var item in produtos2Trim)
                {
                    Console.Write(Environment.NewLine + "Desc. Produto: " + item.Descricao);
                }

                Console.Write(Environment.NewLine + "-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=" + Environment.NewLine);
                Console.ResetColor();
            }
        }

        private static void ProdutosMaisLucrativos(List<Produto> produtos)
        { 
            var produtosMaisLucrativos = produtos.OrderByDescending(p => p.MargemLucro).Take(3).ToList();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-= TOP 3 LUCRO");

            foreach (var pml in produtosMaisLucrativos)
            {
                Console.Write(Environment.NewLine + "Produto: " + pml.Descricao + " >> Lucro: R$ " + pml.MargemLucro.ToString("F2"));
            }

            Console.Write(Environment.NewLine + "-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=" + Environment.NewLine);
            Console.ResetColor();
        }
    }
}
