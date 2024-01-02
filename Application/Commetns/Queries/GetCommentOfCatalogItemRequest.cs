using Application.Interfaces.Contexts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commetns.Queries
{
    public class GetCommentOfCatalogItemRequest : IRequest<List<GetCommentDto>>
    {
        public int CataLogItemId { get; set; }

    }

    public class GetCommentOfCatalogItemQuery : IRequestHandler<GetCommentOfCatalogItemRequest, List<GetCommentDto>>
    {
        private readonly IDataBaseContext context;

        public GetCommentOfCatalogItemQuery(IDataBaseContext context)
        {
            this.context = context;
        }

        public Task<List<GetCommentDto>> Handle(GetCommentOfCatalogItemRequest request, CancellationToken cancellationToken)
        {
            var comments = context.CatalogItemComments.Where(p => p.CatalogItemId == request.CataLogItemId)
   .Select(p => new GetCommentDto
   {
       Comment = p.Comment,
       Id = p.Id,
       Title = p.Title,
   }).ToList();

            return Task.FromResult(comments);

        }
    }
    public class GetCommentDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Comment { get; set; }
    }
}
