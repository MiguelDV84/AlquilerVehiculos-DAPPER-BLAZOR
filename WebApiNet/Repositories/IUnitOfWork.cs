namespace WebApiNet.Repositories
{
    public interface IUnitOfWork
    {
        IVehiculoRepository Vehiculo { get; }
    }
}
