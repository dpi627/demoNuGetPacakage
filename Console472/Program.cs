using demoNuGetPacakage;

namespace Console472
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var service = MyService.Init())
            {
                service.DoWork();
            }
        }
    }
}
