using Application.DTOs.Queries;
using Application.DTOs.Requests;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoanController(LoanService loanService) : ControllerBase
{
    [HttpPost("borrow")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Borrow([FromBody] CreateLoanRequest request)
    {
        await loanService.BorrowAsync(request);
        
        return Created();
    }

    [HttpPatch("return/{loanId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Return([FromRoute] Guid loanId)
    {
        await loanService.ReturnAsync(loanId);
        
        return NoContent();
    }
    
    [HttpGet("{loanId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetById(Guid loanId)
    {
        var loan = await loanService.GetByIdAsync(loanId);
        return Ok(loan);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetAll([FromQuery] LoanQueryParameters queryParameters)
    {
        var loans = await loanService.GetActiveLoansAsync(queryParameters);
        return Ok(loans);
    }
    
    [HttpGet("archived")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetArchived([FromQuery] LoanQueryParameters queryParameters)
    {
        var loans = await loanService.GetArchivedLoansAsync(queryParameters);
        return Ok(loans);
    }
}