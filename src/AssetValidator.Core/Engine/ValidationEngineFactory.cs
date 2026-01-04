using AssetValidator.Core.Abstractions;

namespace AssetValidator.Core.Engine;

internal static class ValidationEngineFactory
{
    private static IEnumerable<IValidationRule> _rules = [];
    private static bool _isInit;
    
    internal static ValidationEngine Create()
    {
        if (!IsInit())
        {
            Init();
        }
        
        return CreateEngine();
    }

    private static void Init()
    {
        IEnumerable<Type> allTypes = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(assembly =>
                {
                    try
                    {
                        return assembly.GetTypes();
                    }
                    catch
                    {
                        return [];
                    }
                }
            );
        IEnumerable<Type> ruleTypes = allTypes.Where(type =>
            type is { IsAbstract: false, IsInterface: false } &&
            typeof(IValidationRule).IsAssignableFrom(type)
        );
        
        _rules = ruleTypes.Select(ruleType => (IValidationRule)Activator.CreateInstance(ruleType)!);
        _isInit = true;
    }
    
    private static bool IsInit() => _isInit;

    private static ValidationEngine CreateEngine() => new(_rules);
}