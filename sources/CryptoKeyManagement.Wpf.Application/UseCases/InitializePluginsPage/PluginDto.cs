namespace DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.InitializePluginsPage;

public class PluginDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string AssemblyName { get; set; }
    
    public Version Version { get; set; }
  
    public bool IsDefault { get; set; }
}