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

	public override int CalcularPrazoDias() => 14; // Prazo de 14 dias para livros
}

public class Revista : Material
{
	public Revista(string codigo, string titulo, int anoPublicacao)
		: base(codigo, titulo, anoPublicacao) { }

	public override int CalcularPrazoDias() => 7; // Prazo de 7 dias para revistas
}

public class DVD : Material
{
	public DVD(string codigo, string titulo, int anoPublicacao)
		: base(codigo, titulo, anoPublicacao) { }

	public override int CalcularPrazoDias() => 3; // Prazo de 3 dias para DVDs
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
		foreach (var material in materiais)
		{
			Console.WriteLine($"Código: {material.Codigo}, Título: {material.Titulo}, Disponível: {material.Disponivel}");
		}
	}
}

public class Program
{
	public static void Main()
	{
		var biblioteca = new Biblioteca();

		var livro = new Livro("L001", "O Senhor dos Anéis", 1954);
		var revista = new Revista("R001", "O Príncipe", 1532);
		var dvd = new DVD("D001", "Nineteen Eighty-Fourx", 1949);

		biblioteca.CadastrarMaterial(livro);
		biblioteca.CadastrarMaterial(revista);
		biblioteca.CadastrarMaterial(dvd);

		biblioteca.ConsultarMateriais();

		livro.Emprestar("João");
		revista.Emprestar("Maria");
		dvd.Emprestar("Pedro");

		biblioteca.ConsultarMateriais();

		livro.Devolver();
		dvd.Devolver();

		biblioteca.ConsultarMateriais();
	}
}
