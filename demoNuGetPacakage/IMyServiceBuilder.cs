namespace demoNuGetPacakage;

public interface IMyServiceBuilder
{
    IMyServiceBuilder SetParam1(string p1);
    IMyServiceBuilder SetParam2(int p2);
    IMyService Build();
}
