using System.Windows.Input;

namespace DustInTheWind.CryptoKeyManagement;

public interface IUseCaseFactory
{
    T Create<T>()
        where T: ICommand;
}