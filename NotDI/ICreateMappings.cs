using System;
using System.Collections.Generic;
using System.Text;

namespace NotDI
{
    public interface ICreateMappings
    {
        Mapper CreateMappings(Mapper mapper);
    }
}
