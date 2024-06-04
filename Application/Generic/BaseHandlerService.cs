using Application.Contracts.Persistence;
using AutoMapper;
using FluentValidation;

namespace Application.Generic {
	public class BaseHandlerService<TRequest> {

		public readonly ICoreDbContext coreDbContext;
		public readonly IMapper mapper;
		public readonly IValidator<TRequest> validator;

        public BaseHandlerService(ICoreDbContext coreDbContext, IMapper mapper, IValidator<TRequest> validator)
        {
            this.coreDbContext = coreDbContext;
            this.mapper = mapper;
            this.validator = validator;
        }
    }
}
