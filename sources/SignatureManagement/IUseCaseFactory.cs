using System.Windows.Input;

namespace DustInTheWind.SignatureManagement;

public interface IUseCaseFactory
{
    T Create<T>()
        where T: ICommand;
}