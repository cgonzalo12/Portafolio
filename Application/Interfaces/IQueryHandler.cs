using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IQueryHandler<TRequest, TResponse>
    {
        Task<TResponse> HandleAsync(TRequest request);
    }
}
