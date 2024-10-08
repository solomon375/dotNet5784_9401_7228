﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi;

public interface ITask
{
    public IEnumerable<BO.Task?> ReadAll(Func<DO.Task, bool>? filter = null);
    public BO.Task? Read(int id);
    public int Create(BO.Task item);
    public void Delete(int id);
    public void Update(BO.Task item);
    public void UpdataOrAddDate(int id, DateTime date);
    public void addDependecy(BO.Task item, int task);
    public List<int> choiceDependecy(BO.Task item);
    public void updateTasksEngineer(int taskId, int engineerId);
}
