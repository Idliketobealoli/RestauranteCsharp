namespace restauranteCsharp.restaurante.monitor
{
    internal interface IMonitor<T>
    {
        T? Get();

        void Put(T entity);
    }
}
