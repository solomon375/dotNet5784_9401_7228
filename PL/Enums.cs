using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
    internal class EngineerCollection : IEnumerable
    {
        static readonly IEnumerable<BO.EngineerExperience> s_enums =
            (Enum.GetValues(typeof(BO.EngineerExperience)) as IEnumerable<BO.EngineerExperience>)!;

        public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
    }

    internal class TaskCollection : IEnumerable
    {
        static readonly IEnumerable<BO.Status> s_enums =
            (Enum.GetValues(typeof(BO.Status)) as IEnumerable<BO.Status>)!;

        public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
    }

    internal class ComplexityCollection : IEnumerable
    {
        static readonly IEnumerable<DO.EngineerExperience> s_enums =
            (Enum.GetValues(typeof(DO.EngineerExperience)) as IEnumerable<DO.EngineerExperience>)!;

        public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
    }
}
