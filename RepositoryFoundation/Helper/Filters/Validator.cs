using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperFoundation.Validator
{
    public static class ParameterValidator
    {
        public static void CheckParametersAreNull(params object[] parameterList)
        {
            if (parameterList.Length <= 0)
                return;
            foreach (var parameter in parameterList)
            {
                if (parameter == null || parameter.ToString().Length == 0)
                    throw new ArgumentNullException(nameof(parameter), "Arguments cannot be null");
            };
        }
    }
}
