﻿
using Business.BusinessAspects;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Business.Handlers.Storages.ValidationRules;
using MimeKit.Encodings;
using System;
using Business.Handlers.WareHouses.Queries;
using Business.Handlers.Products.Queries;
using Elasticsearch.Net;
using static Business.Handlers.Products.Queries.GetSizeQuery;

namespace Business.Handlers.Storages.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateStorageCommand : IRequest<IResult>
    {

        public int CreatedUserId { get; set; }
     
        public int LastUpdatedUserId { get; set; }
        
        public bool Status { get; set; }
        public bool isDeleted { get; set; }
        public int ProductId { get; set; }
        public int UnitsInStock { get; set; }
        public bool IsReady { get; set; }

        public string Size { get; set; }



        public class CreateStorageCommandHandler : IRequestHandler<CreateStorageCommand, IResult>
        {
            private readonly IStorageRepository _storageRepository;
            private readonly IMediator _mediator;
            public CreateStorageCommandHandler(IStorageRepository storageRepository, IMediator mediator)
            {
                _storageRepository = storageRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateStorageValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateStorageCommand request, CancellationToken cancellationToken)
            {
                var getSize = await _mediator.Send(new GetSizeQuery { ProductId = request.ProductId,Size=request.Size});
                if (getSize.Data == true)
                {
                    return new ErrorResult("Depoya eklemek istediğiniz ürünün size  bulunmamaktadır ");
                }

                var isThereStorageRecord = _storageRepository.Query().Any(u => u.ProductId==request.ProductId);

                if (isThereStorageRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedStorage = new Storage
                {
                    CreatedUserId = request.CreatedUserId,
                    CreatedDate = DateTime.Now,
                    LastUpdatedUserId = request.CreatedUserId,
                    LastUpdatedDate = DateTime.Now,
                    Status = request.Status,
                    isDeleted = request.isDeleted,
                    ProductId = request.ProductId,
                    UnitsInStock = request.UnitsInStock,
                    IsReady = request.IsReady,
                    Size = request.Size,

                };

                _storageRepository.Add(addedStorage);
                await _storageRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}