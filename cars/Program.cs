using System;

namespace cars
{

	class Engine
	{
		private double kmPerHour;
		private double litersPerKm;


		public Engine(double kmPerHour, double litersPerKm)
		{
			this.kmPerHour = kmPerHour;
			this.litersPerKm = litersPerKm;
		}


		public double Run(Car car, double distance, GasTank tank)
		{
			double litersPerKm = this.litersPerKm + car.LitersPerKm;
			double needed = distance * litersPerKm;
			double used = tank.Use(needed);
			return used / litersPerKm;
		}


		public double Time(double distance)
		{
			return distance / kmPerHour;
		}
	}
	class GasTank
	{
		private double amount = 0;
		private double capacity;


		public GasTank(double capacity)
		{
			this.capacity = capacity;
		}


		public double Amount
		{
			get
			{
				return amount;
			}
		}


		public void Add(double amount)
		{
			this.amount = Math.Min(this.amount + amount, capacity);
		}


		public double Use(double amount)
		{
			if (amount > this.amount)
			{
				double current = this.amount;
				this.amount = 0;
				return current;
			}
			this.amount = this.amount - amount;
			return amount;
		}
	}
	class Car
	{
		// protected je viditelné i v podtřídě
		protected Engine engine;

		protected GasTank gasTank;

		protected string SPZ;
		public Car(Engine engine, GasTank gasTank, string spz)
		{
			this.engine = engine;
			this.gasTank = gasTank;
			SPZ = spz;
		}


		virtual public double LitersPerKm
		{
			get
			{
				return 2.0 / 100;
			}
		}

		virtual public void Honk()
		{
			Console.Beep(440, 1000);
		}

		public void Tank(double amount)
		{
			gasTank.Add(amount);
		}


		public virtual void Go(double distance)
		{
			double realDistance = engine.Run(this, distance, gasTank);
			double time = engine.Time(realDistance);
			Console.WriteLine("SPZ " + SPZ + ' ');
			Console.WriteLine("Went {0} km in {1} hours. {2} liters of gas left.", realDistance, time, gasTank.Amount);
		}
	}
	class Truck : Car
	{
		private double loadAmount = 0;
		private double capacity;


		public Truck(Engine engine, GasTank gasTank, double capacity, string spz) :
			base(engine, gasTank, spz)
		{
			this.capacity = capacity;
		}


		override public void Honk() 
		{
			Console.Beep(220, 1000);
		}
		public double LoadAmount
		{
			get
			{
				return loadAmount;
			}
		}


		public override double LitersPerKm
		{
			get
			{
				return 3.0 / 100 + loadAmount / 1000;
			}
		}


		public override void Go(double distance)
		{
			base.Go(distance);
			Console.WriteLine("Transported {0} stuff.", loadAmount);
		}


		public void Load(double amount)
		{
			if (amount + loadAmount > capacity)
			{
				throw new ArgumentException();
			}
			loadAmount += amount;
		}


		public void Unload(double amount)
		{
			if (amount > loadAmount)
			{
				throw new ArgumentException();
			}
			loadAmount -= amount;
		}
		public double UnloadAll()
        {
			double ret = loadAmount;
			loadAmount = 0;
			return ret;
        }
	}
	class Formula : Car
    {
		private bool nitro;
		private double nitroCoeficient;


		public Formula(Engine engine, GasTank gasTank, double nc, string spz) :
			base(engine, gasTank, spz)
		{
			nitro = false;
			this.nitroCoeficient = nc;
		}


		override public void Honk()
		{
			Console.Beep(880, 1000);
		}
		public bool gNitro
		{
			get
			{
				return nitro;
			}
		}
		public double gNC
		{
			get
			{
				return nitroCoeficient;
			}
		}


		public override double LitersPerKm
		{
			get
			{
				return 3.0 / 100;
			}
		}


		public override void Go(double distance)
		{
			base.Go(distance * nitroCoeficient);
			Console.WriteLine("Went " + distance * (nitroCoeficient - 1) + " extra meters thanks to nitro");
		}


		public void ToggleNitro()
		{
			nitro = !nitro;
		}
	}

	class Program
    {
        static void Main(string[] args)
        {
			Car car = new Car(new Engine(150, 5.0 / 100), new GasTank(40.0), "2AR4468");
			car.Tank(40.0);
			car.Go(500);

			Truck truck = new Truck(
				new Engine(100, 10.0 / 100),
				new GasTank(100.0),
				20.0, "4AZ 8595"
			) ;
			truck.Load(20.0);
			truck.Tank(100.0);
			truck.Go(250);
			truck.Unload(10.0);
			truck.Go(250);
			Console.WriteLine("Truck unloaded: " + truck.UnloadAll());


			Car[] fleet = { car, truck };

			foreach (Car fleetCar in fleet)
			{
				// fleetCar.load(20);
				if (fleetCar is Truck fleetTruck)
				{
					fleetTruck.Load(20);
				}
				fleetCar.Tank(60.0);
				fleetCar.Go(250);
			}

			Formula f1 = new Formula(new Engine(100, 10.0 / 100),
				new GasTank(100.0),
				3, "lulul lul");
			f1.Go(1000);
			f1.ToggleNitro();
			f1.Go(1000);

			Console.ReadKey();
			f1.Honk();
			car.Honk();
			truck.Honk();
		}
	}
}
