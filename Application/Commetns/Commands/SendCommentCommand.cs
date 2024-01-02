using Application.Interfaces.Contexts;
using Domain.Catalogs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commetns.Commands
{

    public class CommentDto
    {
        public string Title { get; set; }
        public string Comment { get; set; }
        public string Email { get; set; }
        public int CatalogItemId { get; set; }
    }

    public class SendCommentCommand : IRequest<SendCommentResponseDto>
    {
        public SendCommentCommand(CommentDto commentDto)
        {
            Comment = commentDto;
        }
        public CommentDto Comment { get; set; }

    }

    public class SendCommentHandler : IRequestHandler<SendCommentCommand, SendCommentResponseDto>
    {

        private readonly IDataBaseContext context;
        public SendCommentHandler(IDataBaseContext context)
        {
            this.context = context;
        }

        public Task<SendCommentResponseDto> Handle(SendCommentCommand request, CancellationToken cancellationToken)
        {
            var catalogItem = context.CatalogItems.Find(request.Comment.CatalogItemId);
            CatalogItemComment comment = new CatalogItemComment
            {
                Comment = request.Comment.Comment,
                Email = request.Comment.Email,
                Title = request.Comment.Title,
                CatalogItem = catalogItem,
            };
            var entity = context.CatalogItemComments.Add(comment);
            context.SaveChanges();

            return Task.FromResult(new SendCommentResponseDto
            {
                Id = entity.Entity.Id
            });
        }
    }




    public class SendCommentResponseDto
    {
        public int Id { get; set; }
    }
}
