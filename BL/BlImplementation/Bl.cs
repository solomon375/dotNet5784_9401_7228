using BlApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlImplementation;

internal class Bl : IBl
{
    public ITask task => new TaskImplementation();

    public IEngineer engineer => new EngineerImplementation();
    public void InitializeDB() => DalTest.Initialization.Do();
}
