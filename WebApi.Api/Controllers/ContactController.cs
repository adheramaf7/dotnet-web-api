using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Application.DTOs.Request.Contact;
using WebApi.Application.Interfaces.Service;

namespace WebApi.Api.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/v1/contacts")]
    public class ContactController(IContactService contactService) : BaseApiController
    {
        private readonly IContactService contactService = contactService;

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllPaginated([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var userId = GetLoggedInUserId()!;

            var result = await contactService.GetAllAsync(
                userId: userId,
                pageNumber: pageNumber,
                pageSize: pageSize
            );

            return Success(result);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = GetLoggedInUserId()!;

            var result = await contactService.GetByIdAsync(userId, id);

            return Success(result);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create([FromForm] SaveContactRequest request)
        {
            var userId = GetLoggedInUserId()!;

            var result = await contactService.CreateAsync(userId: userId, request: request);

            return Success(result, "Create Success");
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] SaveContactRequest request)
        {
            var userId = GetLoggedInUserId()!;

            var result = await contactService.UpdateAsync(id: id, userId: userId, request: request);

            return Success(result, "Update Success");
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetLoggedInUserId()!;

            await contactService.DeleteAsync(id: id, userId: userId);

            return Success<object>(null, "Delete Success");
        }
    }
}