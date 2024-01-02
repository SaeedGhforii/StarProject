using Application.Catalogs.CatalogItems.GetCatalogIItemPLP;
using Application.Catalogs.CatalogItems.GetCatalogItemPDP;
using Application.Commetns.Commands;
using Application.Commetns.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.EndPoint.Controllers
{
    public class ProductController : Controller
    {
        private readonly IGetCatalogIItemPLPService getCatalogIItemPLPService;
        private readonly IGetCatalogItemPDPService getCatalogItemPDPService;
        private readonly IMediator mediator;

        public ProductController(IGetCatalogIItemPLPService
            getCatalogIItemPLPService
            , IGetCatalogItemPDPService getCatalogItemPDPService
            , IMediator mediator)
        {
            this.getCatalogIItemPLPService = getCatalogIItemPLPService;
            this.getCatalogItemPDPService = getCatalogItemPDPService;
            this.mediator = mediator;
        }
        public IActionResult Index(CatlogPLPRequestDto catlogPLPRequestDto)
        {
            var data = getCatalogIItemPLPService.Execute(catlogPLPRequestDto);
            return View(data);
        }

        public IActionResult Details(string Slug)
        {
            var data = getCatalogItemPDPService.Execute(Slug);
            GetCommentOfCatalogItemRequest itemDto = new GetCommentOfCatalogItemRequest()
            {
                CataLogItemId = data.Id,
            };
            var result =  mediator.Send(itemDto).Result;

            return View(data);
        }

        public IActionResult SendComment(CommentDto commentDto,string Slug)
        {
            SendCommentCommand sendComment = new SendCommentCommand(commentDto);
             var result= mediator.Send(sendComment).Result;
            return RedirectToAction(nameof(Details), new { Slug = Slug });

        }
    }
}
