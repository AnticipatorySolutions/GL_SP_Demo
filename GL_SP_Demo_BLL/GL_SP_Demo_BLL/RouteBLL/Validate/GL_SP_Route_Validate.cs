using System.Linq;
using GL_SP_Demo_BLL.Abstracts.Validate;

namespace GL_SP_Demo_BLL.Route
{
    public interface IRouteValidationHandler:IValidationHandler<bool,GL_SP_Route_Validator_Del,string>
    {
        bool Run(GL_SP_Route_Validator_Del validator, string src, string dst);
    }

    public interface IRouteValidatorStore:IValidatorStore<bool,string>
    {
        bool Match(string src, string dst);
        bool Code3Validation(string src, string data);
    }

    public delegate bool GL_SP_Route_Validator_Del(string src, string dst);

    public class RouteValidationHandler : GL_SP_Abstract_ValidattionHandler<bool,GL_SP_Route_Validator_Del,string>,IRouteValidationHandler
    {
        public bool Run(GL_SP_Route_Validator_Del validator, string src, string dst)
        {
            return validator(src, dst);
        }
    }


    public class RouteValidatorStore : GL_SP_Abstract_ValidatorStore<bool,string>,IRouteValidatorStore
    {
        public bool Match(string src, string dst)
        {
            if (src == dst) { return true; }
            return false;
        }

        public bool Code3Validation(string src, string dst)
        {
            if (src == null) return false;
            if (dst == null) return false;

            bool srcTest = src.Length == 3;
            bool dstTest = dst.Length == 3;

            if (!srcTest || !dstTest)
            {
                return false;
            }

            srcTest = src.All(char.IsLetter);
            dstTest = dst.All(char.IsLetter);

            if (!srcTest || !dstTest)
            {
                return false;
            }
            return true;
        }

    }
}
