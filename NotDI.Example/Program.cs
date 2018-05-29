using System;
using NotDI;

namespace NotDI.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            Resolver.ResolveIt(out IA test1, out IB test2);
            test1.PrintIt();
            test2.PrintIt();
            Console.WriteLine();

            Resolver.ResolveIt(out IA test3, out IB test4);
            test3.PrintIt();
            test4.PrintIt();
            Console.WriteLine();

            Console.ReadLine();
        }
    }

    public class MappingConfig : ICreateMappings
    {
        public Mapper CreateMappings(Mapper mapper)
        {
            mapper.AddTransient<IA,A_Transient>();
            mapper.AddSessionScoped<IB,B_Scoped>();
            mapper.AddSingleton<IC,C_Singleton>();
            return mapper;
        }
    }

    interface IA
    {
        void PrintIt();
    }

    class A_Transient : IA
    {
        DateTime _now;

        public A_Transient()
        {
            _now = DateTime.Now;
            Resolver.ResolveIt(out IB b, out IC c);
            b.PrintIt();
            c.PrintIt();
        }

        public void PrintIt()
        {
            Console.WriteLine("From A, Date " + _now.ToString("HH:mm:ss.ffffff"));
        }
    }

    interface IB
    {
        void PrintIt();
    }

    class B_Scoped : IB
    {
        DateTime _now;

        public B_Scoped()
        {
            _now = DateTime.Now;
        }

        public void PrintIt()
        {
            Console.WriteLine("From B, Date " + _now.ToString("HH:mm:ss.ffffff"));
        }
    }

    interface IC
    {
        void PrintIt();
    }

    class C_Singleton : IC
    {
        DateTime _now;

        public C_Singleton()
        {
            _now = DateTime.Now;
        }

        public void PrintIt()
        {
            Console.WriteLine("From C, Date " + _now.ToString("HH:mm:ss.ffffff"));
        }
    }
}
