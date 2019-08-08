using System;
using System.Collections.Generic;
using System.Text;

namespace GL_SP_Demo_BLL.Abstracts.Validate
{
    public interface IValidationHandler<J, K, L> { }

    public interface IValidatorStore<J, K> { }

    public abstract class GL_SP_Abstract_ValidattionHandler<J,K,L>:IValidationHandler<J,K,L>
    {
    }

    public abstract class GL_SP_Abstract_ValidatorStore<J, K>
    {
    }
}
