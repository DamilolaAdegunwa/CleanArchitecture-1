﻿using Ardalis.ApiEndpoints;
using TodoApp.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace TodoApp.Web.Endpoints.ProjectEndpoints;

public class ListIncomplete : BaseAsyncEndpoint
    .WithRequest<ListIncompleteRequest>
    .WithResponse<ListIncompleteResponse>
{
  private readonly IToDoItemSearchService _searchService;

  public ListIncomplete(IToDoItemSearchService searchService)
  {
    _searchService = searchService;
  }

  [HttpGet("/Projects/{ProjectId}/IncompleteItems")]
  [SwaggerOperation(
      Summary = "Gets a list of a project's incomplete items",
      Description = "Gets a list of a project's incomplete items",
      OperationId = "Project.ListIncomplete",
      Tags = new[] { "ProjectEndpoints" })
  ]
  public override async Task<ActionResult<ListIncompleteResponse>> HandleAsync([FromQuery] ListIncompleteRequest request, CancellationToken cancellationToken)
  {
    if (request.SearchString == null)
    {
      return BadRequest();
    }
    var response = new ListIncompleteResponse(0, new List<ToDoItemRecord>());
    var result = await _searchService.GetAllIncompleteItemsAsync(request.ProjectId, request.SearchString);

    if (result.Status == Ardalis.Result.ResultStatus.Ok)
    {
      response.ProjectId = request.ProjectId;
      response.IncompleteItems = new List<ToDoItemRecord>(
              result.Value.Select(
                  item => new ToDoItemRecord(item.Id,
                  item.Title,
                  item.Description,
                  item.IsDone)));
    }
    else if (result.Status == Ardalis.Result.ResultStatus.Invalid)
    {
      return BadRequest(result.ValidationErrors);
    }
    else if (result.Status == Ardalis.Result.ResultStatus.NotFound)
    {
      return NotFound();
    }

    return Ok(response);
  }
}
