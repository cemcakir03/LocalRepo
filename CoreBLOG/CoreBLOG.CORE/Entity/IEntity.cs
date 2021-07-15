using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBLOG.CORE.Entity
{
    public interface IEntity<T>
    {
         T ID { get; set; }
    }
}
