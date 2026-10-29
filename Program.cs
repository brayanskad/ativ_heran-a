using System;
using System.Collections.Generic;

public abstract class Material
{
	public string Codigo { get; set; }
	public string Titulo { get; set; }
	public int AnoPublicacao { get; set; }
	public bool Disponivel { get; private set; } = true;
	public string UsuarioEmprestimo { get; private set; }
	public DateTime? DataEmprestimo { get; private set; }

	public Material(string codigo, string titulo, int anoPublicacao)
	{
		Codigo = codigo;
		Titulo = titulo;
		AnoPublicacao = anoPublicacao;
	}

	public virtual void Emprestar(string usuario)
	{
		if (!Disponivel)
		{
			Console.WriteLine("Material indisponível para empréstimo.");
			return;
		}

		Disponivel = false;
		UsuarioEmprestimo = usuario;
		DataEmprestimo = DateTime.Now;
		Console.WriteLine($"Material '{Titulo}' emprestado para {usuario}.");
	}

	public virtual void Devolver()
	{
		if (Disponivel)
		{
			Console.WriteLine("Material já está disponível na biblioteca.");
			return;
		}

		Disponivel = true;
		VerificarAtraso();
		UsuarioEmprestimo = null;
		DataEmprestimo = null;
		Console.WriteLine($"Material '{Titulo}' devolvido com sucesso.");
	}

	public abstract int CalcularPrazoDias();

	private void VerificarAtraso()
	{
		if (DataEmprestimo.HasValue && (DateTime.Now - DataEmprestimo.Value).Days > CalcularPrazoDias())
		{
			Console.WriteLine("A devolução está atrasada!");
		}
	}
}

public class Livro : Material
{
	public Livro(string codigo, string titulo, int anoPublicacao)
		: base(codigo, titulo, anoPublicacao) { }

	public override int CalcularPrazoDias() => 14;
}

public class Revista : Material
{
	public Revista(string codigo, string titulo, int anoPublicacao)
		: base(codigo, titulo, anoPublicacao) { }

	public override int CalcularPrazoDias() => 7;
}

public class DVD : Material
{
	public DVD(string codigo, string titulo, int anoPublicacao)
		: base(codigo, titulo, anoPublicacao) { }

	public override int CalcularPrazoDias() => 3;
}

public class Biblioteca
{
	private List<Material> materiais = new List<Material>();

	public void CadastrarMaterial(Material material)
	{
		materiais.Add(material);
		Console.WriteLine($"Material '{material.Titulo}' cadastrado com sucesso.");
	}

	public void ConsultarMateriais()
	{
		Console.WriteLine("=== Materiais na Biblioteca ===");
		if (materiais.Count == 0)
		{
			Console.WriteLine("Nenhum material cadastrado.");
			return;
		}

		foreach (var material in materiais)
		{
			Console.WriteLine($"Código: {material.Codigo}, Título: {material.Titulo}, Ano: {material.AnoPublicacao}, Prazo: {material.CalcularPrazoDias()} dias, Disponível: {material.Disponivel}");
		}
	}

	public Material BuscarMaterial(string codigo)
	{
		return materiais.Find(material => material.Codigo.Equals(codigo, StringComparison.OrdinalIgnoreCase));
	}
}

public class Program
{
	public static void Main()
	{
		var biblioteca = new Biblioteca();
		bool executando = true;

		while (executando)
		{
			Console.WriteLine("\n=== Sistema da Biblioteca ===");
			Console.WriteLine("1 - Cadastrar material");
			Console.WriteLine("2 - Consultar materiais");
			Console.WriteLine("3 - Emprestar material");
			Console.WriteLine("4 - Devolver material");
			Console.WriteLine("0 - Sair");
			Console.Write("Escolha uma opção: ");

			switch (Console.ReadLine())
			{
				case "1":
					CadastrarMaterial(biblioteca);
					break;
				case "2":
					biblioteca.ConsultarMateriais();
					break;
				case "3":
					EmprestarMaterial(biblioteca);
					break;
				case "4":
					DevolverMaterial(biblioteca);
					break;
				case "0":
					executando = false;
					Console.WriteLine("Sistema encerrado.");
					break;
				default:
					Console.WriteLine("Opção inválida.");
					break;
			}
		}
	}

	private static void CadastrarMaterial(Biblioteca biblioteca)
	{
		Console.WriteLine("Tipo: 1 - Livro | 2 - Revista | 3 - DVD");
		Console.Write("Escolha o tipo: ");
		string tipo = Console.ReadLine();

		if (tipo != "1" && tipo != "2" && tipo != "3")
		{
			Console.WriteLine("Tipo inválido.");
			return;
		}

		Console.Write("Código: ");
		string codigo = Console.ReadLine();
		if (biblioteca.BuscarMaterial(codigo) != null)
		{
			Console.WriteLine("Já existe um material com esse código.");
			return;
		}

		Console.Write("Título: ");
		string titulo = Console.ReadLine();
		Console.Write("Ano de publicação: ");
		if (!int.TryParse(Console.ReadLine(), out int ano))
		{
			Console.WriteLine("Ano inválido.");
			return;
		}

		Material material;
		switch (tipo)
		{
			case "1": material = new Livro(codigo, titulo, ano); break;
			case "2": material = new Revista(codigo, titulo, ano); break;
			default: material = new DVD(codigo, titulo, ano); break;
		}

		biblioteca.CadastrarMaterial(material);
	}

	private static void EmprestarMaterial(Biblioteca biblioteca)
	{
		Console.Write("Código do material: ");
		Material material = biblioteca.BuscarMaterial(Console.ReadLine());
		if (material == null)
		{
			Console.WriteLine("Material não encontrado.");
			return;
		}

		Console.Write("Nome do usuário: ");
		string usuario = Console.ReadLine();
		material.Emprestar(usuario);
	}

	private static void DevolverMaterial(Biblioteca biblioteca)
	{
		Console.Write("Código do material: ");
		Material material = biblioteca.BuscarMaterial(Console.ReadLine());
		if (material == null)
		{
			Console.WriteLine("Material não encontrado.");
			return;
		}

		material.Devolver();
	}
}
