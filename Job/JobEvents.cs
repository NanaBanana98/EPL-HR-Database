using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPL_HR_Database
{
    /// <summary>
    /// Actions that can be applied to event values 
    /// </summary>
    public interface IJobEvents
    {
    }

    public record UpdateJob(int ID, string Title, string Description, DateTime Updated, string Comments) : IJobEvents;
    public record DecommisionJob(int Id, string Title, string Description, DateTime Removed, string Comments): IJobEvents;
}
