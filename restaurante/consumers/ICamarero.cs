namespace restauranteCsharp.restaurante.consumers
{
    internal interface ICamarero<T>
    {
        void TakePlatos();
        void SendPlatos(int Id);
    }
}
