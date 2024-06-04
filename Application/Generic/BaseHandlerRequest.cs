using Application.Contracts.Persistence;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.Generic {
	public abstract class BaseHandlerRequest<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
		where TRequest : IRequest<TResponse> {

		protected readonly ICoreDbContext _dbContext;
		protected readonly IMapper _mapper;
        protected readonly IValidator<TRequest> _validator;

        protected BaseHandlerRequest(BaseHandlerService<TRequest> service)
        {
            _dbContext = service.coreDbContext;
            _mapper = service.mapper;
            _validator = service.validator;
        }

        public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
	}
}
