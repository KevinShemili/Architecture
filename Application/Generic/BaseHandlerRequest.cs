using Application.Contracts.Persistence;
using AutoMapper;
using MediatR;

namespace Application.Generic {
	public abstract class BaseHandlerRequest<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
		where TRequest : IRequest<TResponse> {

		protected readonly ICoreDbContext _dbContext;
		protected readonly IMapper _mapper;

		protected BaseHandlerRequest(BaseHandlerService service) {
			_dbContext = service.coreDbContext;
			_mapper = service.mapper;
		}

		public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
	}
}
