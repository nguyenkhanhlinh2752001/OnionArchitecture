using Application.Features.CategoryFeatures.Commands.CreateCategoryCommand;
using Application.Features.CategoryFeatures.Commands.DeleteCategoryByIdCommand;
using Application.Features.CategoryFeatures.Commands.UpdateCategoryCommand;
using Application.Features.CategoryFeatures.Queries.GetAllCategoriesQuery;
using Application.Features.CategoryFeatures.Queries.GetProductsByCategoryIdQuery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence.Constants;
using WebApi.Attributes;

namespace WebApi.Controllers.v1
{
    public class CategoryController : BaseApiController
    {
        /// <summary>
        /// Create category
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [CustomAuthorizeAtrtibute(ConstantsAtr.CategoryPermission, ConstantsAtr.Add)]
        public async Task<IActionResult> Create(CreateCategoryCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// delete category
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [CustomAuthorizeAtrtibute(ConstantsAtr.CategoryPermission, ConstantsAtr.Delete)]
        public async Task<IActionResult> Delete(DeleteCategoryByIdCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// update category
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [CustomAuthorizeAtrtibute(ConstantsAtr.CategoryPermission, ConstantsAtr.Update)]
        public async Task<IActionResult> Update(UpdateCategoryCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        ///  get all categories
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllCategoriesQueryParemeter query)
        {
            return Ok(await Mediator.Send(new GetAllCategoriesQuery
            {
                Name = query.Name,
                CreatedBy = query.CreatedBy,
                FromDate = query.FromDate,
                ToDate = query.ToDate,
                SortBy = query.SortBy,
                Order = query.Order,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            }));
        }

        /// <summary>
        /// get products by category id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/products")]
        public async Task<IActionResult> GetProductsByCategoryId(int id)
        {
            return Ok(await Mediator.Send(new GetProductsByCategoryIdQuery { Id = id }));
        }
    }
}