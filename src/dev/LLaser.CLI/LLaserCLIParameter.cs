using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLaser.CLI
{
    class LLaserCLIParameter
    {
        private static LLaserCLIParameter _instance = null;
        public static LLaserCLIParameter Instance
        {
            get
            {
                _instance = _instance ?? new LLaserCLIParameter();
                return _instance;
            }
        }

        private LLaserCLIParameter()
        {

        }

        public string WorkLocation { get; set; }
    }
}
