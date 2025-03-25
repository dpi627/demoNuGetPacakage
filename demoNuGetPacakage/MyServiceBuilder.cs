using Microsoft.Extensions.Logging;

namespace demoNuGetPacakage;

//Factory Pattern：透過 Init 和 Build 控制 MyService 的創建
//Builder Pattern：透過 SetParam1 和 SetParam2 逐步配置物件
public class MyServiceBuilder : IMyServiceBuilder
{
    private readonly IMyService _service;

    // 靜態 Init 方法提高了封裝性，MyServiceBuilder() 建構式是 private，外部無法直接創建
    // 透過 Init 方法，可以控制 MyService 的初始化，也可命名為 Create() 或 New()
    public static MyServiceBuilder Init(ILoggerFactory? loggerFactory = default) => new(loggerFactory);

    private MyServiceBuilder(ILoggerFactory? loggerFactory)
    {
        _service = MyService.Init(loggerFactory);
    }
    public IMyServiceBuilder SetParam1(string p1)
    {
        _service.SetParam1(p1);
        return this;
    }
    public IMyServiceBuilder SetParam2(int p2)
    {
        _service.SetParam2(p2);
        return this;
    }

    // Build 方法返回 MyService 實例，可在此檢查參數設定
    public IMyService Build() => _service;
}
