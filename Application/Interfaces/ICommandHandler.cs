using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICommandHandler<TRequest, TResponse>
    {
        Task<TResponse> HandleAsync(TRequest request);
    }
}
