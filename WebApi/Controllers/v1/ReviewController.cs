using Application.Features.ReviewFeatures.Commands.AddEditReview;
using Application.Features.ReviewFeatures.Commands.DeleteReview;
using Application.Features.ReviewFeatures.Queries.GetReviewByUserId;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : BaseApiController
    {
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Create(AddEditReviewCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Update(int id)
        {
            return Ok(await Mediator.Send(new DeleteReviewCommand { Id = id }));
        }

        [HttpGet("user")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetByUserId([FromQuery] GetReviewByUserIdParametter model)
        {
            return Ok(await Mediator.Send(new GetReviewByUserIdQuery
            {
                OrderBy = model.OrderBy,
                Rate = model.Rate,
                PageNumber = model.PageNumber,
                PageSize = model.PageSize,
                ProductName = model.ProductName,
                
            }));
        }
    }
}