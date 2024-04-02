using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi;
public interface IBl
{
    public ITask task { get; }
    public IEngineer engineer { get; }
    public Clock clock { get; }
    public void InitializeDB() => DalTest.Initialization.Do();
    public void ResetDB() => DalTest.Initialization.Resat();
    void UpdateTasksStatus();

    #region Now

    DateTime Now { get; }

    void AdvanceTimeByMonth();
    void AdvanceTimeByDay();
    void AdvanceTimeByHour();
    void InitializeClock();

    #endregion Now
}
