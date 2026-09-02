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
    public async Task<IActionResult> Borrow([FromBody] CreateLoanRequest request)
    {
        await loanService.BorrowAsync(request);
        
        return Created();
    }

    [HttpPatch("return/{loanId}")]
    public async Task<IActionResult> Return([FromRoute] Guid loanId)
    {
        await loanService.ReturnAsync(loanId);
        
        return NoContent();
    }
    
    [HttpGet("{loanId:guid}")]
    public async Task<ActionResult> GetById(Guid loanId)
    {
        var loan = await loanService.GetByIdAsync(loanId);
        return Ok(loan);
    }

    [HttpGet]
    public async Task<ActionResult> GetAll([FromQuery] LoanQueryParameters queryParameters)
    {
        var loans = await loanService.GetAllAsync(queryParameters);
        return Ok(loans);
    }
}