using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAS.Padrones.Tucuman
{
    internal class Parser
    {
        private Options _options { get; set; }

        public Parser(string[] args)
        {
            _options = new Options();

            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-p":
                        _options.ProvinceCode = args[++i];
                        break;

                    case "-a":
                        _options.AcreditanFilepath = args[++i];
                        break;

                    case "-c":
                        _options.CoeficientesFilepath = args[++i];
                        break;

                    case "-o":
                        _options.OutputFilepath = args[++i];
                        break;

                }
            }
        }

        public Options GetOptions()
        {
            return _options;
        }
    }
}
