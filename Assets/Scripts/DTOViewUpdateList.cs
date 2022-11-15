using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class DTOViewUpdateList
    {
        public List<RobotMoveViewStep> viewSteps;      //todo: potentially use ViewStep as abstraction away. deserialization may be annoying
        public StartInfo startInfo;
    }
}
