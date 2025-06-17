namespace AgendaContato.Interfaces.Interfaces;

public interface IResponseMapper<VM, M>
{
    VM ToViewModel(M model);
}