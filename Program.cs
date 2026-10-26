using System;

public class Animal
{
	public string Nome { get; set; }
	public int Idade { get; set; }

	public Animal(string nome, int idade)
	{
		Nome = nome;
		Idade = idade;
	}

	 public virtual void EmitirSom()
	{
		Console.WriteLine("O animal está emitindo um som.");
	}
}

public class Cachorro : Animal
{
	public Cachorro(string nome, int idade) : base(nome, idade)
	{
	}

	public override void EmitirSom()
	{
        base.EmitirSom();
		Console.WriteLine(" AI AI AI");
	}
}

public class Gato : Animal
{
	public Gato(string nome, int idade) : base(nome, idade)
	{
	}

	public override void EmitirSom()
	{
        base.EmitirSom();
		Console.WriteLine("MIAU (GROSSO)");
	}
}

public class Program
{
	public static void Main()
	{
		var cachorro = new Cachorro("Rex", 3);
		var gato = new Gato("Mimi", 2);

		Console.WriteLine("=== SISTEMA DE ANIMAIS ===");
		Console.WriteLine($"Animal: {cachorro.Nome}");
		Console.WriteLine($"Idade: {cachorro.Idade} anos");
		Console.Write("Som: ");
		cachorro.EmitirSom();

		Console.WriteLine();
		Console.WriteLine($"Animal: {gato.Nome}");
		Console.WriteLine($"Idade: {gato.Idade} anos");
		Console.Write("Som: ");
		gato.EmitirSom();

		Console.WriteLine("Pressione qualquer tecla para sair...");
		Console.ReadKey();
	}
}

