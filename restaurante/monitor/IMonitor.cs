using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace restauranteCsharp.restaurante.monitor
{
    internal interface IMonitor<T>
    {
        T Get();

        void Put(T entity);
    }
}
