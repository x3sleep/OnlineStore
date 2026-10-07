using Microsoft.AspNetCore.Mvc;
using OnlineStore.API.Contracts;
using OnlineStore.BLL.DTOs;
using OnlineStore.BLL.Services;

namespace OnlineStore.API.Controllers;

[ApiController]
[Route("user")]
[Produces("application/json")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<UserOperationResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationErrorResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<MessageResponse>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(UserRequest request, CancellationToken cancellationToken)
    {
        var user = await userService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, new UserOperationResponse("Пользователь создан", user));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType<UserResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<MessageResponse>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken) =>
        Ok(await userService.GetByIdAsync(id, cancellationToken));

    [HttpPut("{id:int}")]
    [ProducesResponseType<UserOperationResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<MessageResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<MessageResponse>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(int id, UserRequest request, CancellationToken cancellationToken)
    {
        var user = await userService.UpdateAsync(id, request, cancellationToken);
        return Ok(new UserOperationResponse("Пользователь обновлен", user));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType<MessageResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<MessageResponse>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await userService.DeleteAsync(id, cancellationToken);
        return Ok(new MessageResponse($"Пользователь с Id = {id} удален"));
    }
}
