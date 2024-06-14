using Application.Contracts.Persistence;
using AutoMapper;

namespace Application.Generic
{
    public class BaseHandlerService {

		public readonly ICoreDbContext coreDbContext;
		public readonly IMapper mapper;

        public BaseHandlerService(ICoreDbContext coreDbContext, IMapper mapper)
        {
            this.coreDbContext = coreDbContext;
            this.mapper = mapper;
        }
    }
}
