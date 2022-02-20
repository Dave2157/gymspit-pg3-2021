using System;
using System.IO;
using System.Collections.Generic;


namespace rpgpg
{

	class Program
	{
		delegate int getDamageOfGivenType(Character a, Character d);
		delegate void waitAction(Character c);
		delegate string ControllerChooseAction(Character c, Character e);
		struct Attack //poškození útoku se dělí do 3 částí, na každou z nichž má poté Character určitou hodnotu obranného modifikátoru (armor brání physical, magic resist brání magic) ten výpočet na konečný damage je ve funkci ReceiveDamage
		{
			public string name;
			public int manaCost;

			public getDamageOfGivenType getPhysical;
			public getDamageOfGivenType getMagic;
			public getDamageOfGivenType getTrue;


			public Attack(string n, int mc, getDamageOfGivenType p, getDamageOfGivenType m, getDamageOfGivenType t)
			{
				name = n;
				manaCost = mc;

				getPhysical = p;
				getMagic = m;
				getTrue = t;

			}
		};
		static List<Attack> attacks = new List<Attack>();
		struct WaitAction
        {
			public string name;
			public waitAction action;
			public WaitAction(string n, waitAction a)
            {
				name = n;
				action = a;
            }
				

		}
		static List<WaitAction> waitActions = new List<WaitAction>();
		class Controller //z controlleru jsem musel udělat classu (bylo to interface), protože v interfacu jsem ChooseAction nemohl implemetovat pomocí function pointeru
        {
			public ControllerChooseAction ChooseAction;

			public Controller(ControllerChooseAction ca)
            {
				ChooseAction = ca;
            }

		}
		class Character
		{
			public const string TURN_CHOICE_WAIT = "wait";

			Controller controller;

			public string name;

			public int hp;
			public int maxHp;
			public int mana;
			public int maxMana;

			public float healthRegen;
			public float manaRegen;


			public int ad;
			public int ap;
			public int armor;
			public int magicResist;

			public List<Attack> equippedAttacks;
			public WaitAction equippedWaitAction;

			public bool Alive
			{
				get
				{
					return hp > 0;
				}
			}

			public Character(Controller controller, string name, int maxHp, int maxMana, float healthRegen, float manaRegen, int ad, int ap, int armor, int magicResist, List<string> ea, string ewa)
			{
				this.controller = controller;
				this.name = name;

				this.maxHp = maxHp;
				this.maxMana = maxMana;

				this.healthRegen = healthRegen;
				this.manaRegen = manaRegen;

				this.ad = ad;
				this.ap = ap;
				this.armor = armor;
				this.magicResist = magicResist;

				equippedAttacks = new List<Attack>();
				foreach (string an in ea)
                {
					equippedAttacks.Add(attacks.Find((Attack a) => { return a.name == an; }));
                }
				equippedWaitAction = waitActions.Find((WaitAction wa) => { return wa.name == ewa; });

				Reset();
			}


			public void TakeTurn(TextWriter output, Character enemy)
			{
				mana += (int)(Math.Round(manaRegen));
				mana = mana > maxMana ? maxMana : mana;

				hp += (int)(Math.Round(healthRegen));
				hp = hp > maxHp ? maxHp : hp;

				string action = controller.ChooseAction(this, enemy);

				// v případě útoku (útočná akce má předponu attack-) zahodí předponu a najde odpovídající útok
				if (action.Contains("attack"))
				{
					action = action.Remove(0, 7);
					AttackEnemy(output, enemy, equippedAttacks.Find((Attack a) => { return a.name == action; }));
				}
				else
					Wait(output);
			}


			public void Reset()
			{
				hp = maxHp;
				mana = maxMana;
			}


			private void AttackEnemy(TextWriter output, Character enemy, Attack attack)
			{
				output.WriteLine("{0} attacks {1} using {2}!", name, enemy.name, attack.name);
				
				if (attack.manaCost > mana)
                {
					output.WriteLine(name + " couldn't gather enough mana to finish " + attack.name + '!');
					return;
                }
				mana -= attack.manaCost;
				enemy.ReceiveDamage(output, attack.getPhysical(this, enemy), attack.getMagic(this, enemy), attack.getTrue(this, enemy));
			}


			private void ReceiveDamage(TextWriter output, int physical, int magic, int tru)
			{

				int damage = (int) (physical * (100.0f / (100 + armor)) + magic * (100.0f / (100 + magicResist)) + tru);

				if (damage > 0)
				{
					hp -= damage;
					output.WriteLine("{0} takes {1} damage!", name, damage);
				}
				else
				{
					output.WriteLine("{0} takes no damage!", name);
				}
			}


			private void Wait(TextWriter output)
			{
                equippedWaitAction.action(this);
				output.WriteLine("{0} waits...", name);
			}
		}
		class Player : Controller
		{
			private TextReader input;

			private TextWriter prompt;


			public Player(TextReader input, TextWriter prompt = null) : base((Character character, Character enemy) =>
			{
				while (true)
				{
					if (prompt != null)
					{
						prompt.WriteLine("Choose an action:");
						prompt.WriteLine("Attack");
						for (int i = 0; i < character.equippedAttacks.Count; i++)
							prompt.WriteLine('\t' + character.equippedAttacks[i].name + " (" + i + ")");
						prompt.WriteLine("(W)ait");
					}

					string choice = input.ReadLine();
					if (choice == null)
					{
						return null;
					}

					int nChoice;
					choice = choice.ToLower();
					if (int.TryParse(choice, out nChoice))
					{
						if (nChoice < character.equippedAttacks.Count)
							return new string("attack-" + character.equippedAttacks[nChoice].name);
					}
					else if (choice == "w" || choice == "wait")
						return Character.TURN_CHOICE_WAIT;

					if (prompt != null)
					{
						prompt.WriteLine("Invalid choice!");
					}
				}
				
			})
			{
				this.input = input;
				this.prompt = prompt;


			}
		}
		class Game
		{
			private Character characterOne;

			private Character characterTwo;


			public Game(Character characterOne, Character characterTwo)
			{
				this.characterOne = characterOne;
				this.characterTwo = characterTwo;
			}


			public void Run(TextWriter output)
			{
				Console.WriteLine("Let the games begin!");

				characterOne.Reset();
				characterTwo.Reset();

				Character active = characterOne;
				Character nonActive = characterTwo;

				PrintStatus(output, characterOne);
				PrintStatus(output, characterTwo);
				Console.WriteLine();

				while (characterOne.Alive && characterTwo.Alive)
				{
					Console.WriteLine("{0}'s turn:", active.name);
					active.TakeTurn(output, nonActive);
					Console.WriteLine();

					PrintStatus(output, characterOne);
					PrintStatus(output, characterTwo);
					Console.WriteLine();

					Character tmp = active;
					active = nonActive;
					nonActive = tmp;
				}

				Console.WriteLine("GAME OVER!");
				if (characterOne.Alive || characterTwo.Alive)
				{
					Character winner = characterOne.Alive ?
						characterOne :
						characterTwo;
					Console.WriteLine("The winner is {0}!", winner.name);
				}
				else
				{
					Console.WriteLine("Let's call it a draw!");
				}
			}


			private void PrintStatus(TextWriter output, Character character)
			{
				output.WriteLine(
					"{0}: {1}, {2} / {3} HP",
					character.name,
					character.Alive ? "alive" : "dead",
					character.hp,
					character.maxHp
				);
			}
		}
		static void Main(string[] args) //definice různých útoků, první lambda vrací udělený physical damage, druhá magical, třetí true
		{
			attacks.Add(new Attack(new string("punch"), 10, (Character c, Character e) => { return 20; }, (Character c, Character e) => { return 0; }, (Character c, Character e) => { return 0; }));
			attacks.Add(new Attack(new string("ray"), 70, (Character c, Character e) => { return 0; }, (Character c, Character e) => { return 40; }, (Character c, Character e) => { return 0; }));
			attacks.Add(new Attack(new string("ram"), 20, (Character c, Character e) => { return 20; }, (Character c, Character e) => { return 0; }, (Character c, Character e) => { return 0; }));
			attacks.Add(new Attack(new string("lightning bolt"), 100, (Character c, Character e) => { return 15; }, (Character c, Character e) => { return 55; }, (Character c, Character e) => { return 0; }));
			attacks.Add(new Attack(new string("EMP"), 100, (Character c, Character e) => { return 0; }, (Character c, Character e) => { return e.maxHp / 2; }, (Character c, Character e) => { return 0; }));
			attacks.Add(new Attack(new string("lunge"), 20, (Character c, Character e) => { return 10; }, (Character c, Character e) => { return 0; }, (Character c, Character e) => { return 40 * (e.maxHp - e.hp) / e.maxHp; }));
			attacks.Add(new Attack(new string("force push"), 30, (Character c, Character e) => { return 15; }, (Character c, Character e) => { return 15; }, (Character c, Character e) => { return 10; }));
			waitActions.Add(new WaitAction(new string("heal"), (Character c) => { c.hp += ((c.maxHp - c.hp) / 5); }));
			waitActions.Add(new WaitAction(new string("repair & reinforce"), (Character c) => { c.armor += 10; c.magicResist += 10; }));
			Character c3PO = new Character(new Controller(
			(Character c, Character e) => {										//v těhle lambdách se zadává chování AI

				Attack punch = c.equippedAttacks[0];
				Attack ray = c.equippedAttacks[1];
				if (c.hp < c.maxHp / 5)
					return "wait";
				if (c.mana > ray.manaCost)
					return "attack-ray";
				return "attack-punch";
			}), "C-3PO", 150, 100, 3, 5, 50, 20, 20, 50, new List<string>() { new string("punch"), new string ("ray") }, new string("heal"));

			Character r2D2 = new Character(new Controller(
			(Character c, Character e) =>
			{
				Attack ram = c.equippedAttacks[0];
				Attack lb = c.equippedAttacks[1];
				Attack emp = c.equippedAttacks[2];
				Random rnd = new Random(54);

				if (e.magicResist > 80)
                {		
					return "attack-ram";
				}
				if (c.mana < emp.manaCost)
					return "wait";
				if (e.maxHp > 150)
					return "attack-EMP";
				return "attack-lightning bolt";

			}), "R2-D2", 75, 200, 3, 20, 20, 50, 10, 70, new List<string>() { new string("ram"), new string("lightning bolt"), new string("EMP") }, new string("repair & reinforce"));

			Character luke = new Character(new Player(Console.In, Console.Out), "Luke", 120, 75, 3, 10, 50, 50, 60, 20, new List<string>() { new string("lunge"), new string("force push") }, "heal");

			Game game = new Game(c3PO, r2D2);
			game.Run(Console.Out);
			Console.WriteLine();

			Game game2 = new Game(c3PO, luke);
			game2.Run(Console.Out);
			Console.WriteLine();

			Game game3 = new Game(r2D2, luke);
			game3.Run(Console.Out);
			Console.WriteLine();

			Console.WriteLine("Press any key to quit...");
			Console.ReadKey();
		}
    }
}
