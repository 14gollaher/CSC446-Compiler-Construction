namespace CMinusMinusCompiler
{
    public class Program
    {
        static void Main(string[] arguments)
        {
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.Start(arguments);
        }
    }
}
