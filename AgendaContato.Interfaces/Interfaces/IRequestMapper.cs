namespace AgendaContato.Interfaces.Interfaces;

public interface IRequestMapper<VM, M> 
{
    M ToModel(VM request);
}