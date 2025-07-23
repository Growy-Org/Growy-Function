using System.Net;
using Growy.Function.Models;
using Growy.Function.Models.Dtos;
using Growy.Function.Services.Interfaces;
using Growy.Function.Utils;
using Microsoft.Azure.Functions.Worker;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;
using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

namespace Growy.Function.Controllers;

public class AssignmentController(
    IAssignmentService assignmentService,
    IParentService parentService,
    IChildService childService,
    IAuthService authService)
{
    # region Assignments

    // Read
    [Function("GetAssignmentCount")]
    [OpenApiOperation(operationId: "GetAssignmentCount", tags: new[] { "Assignment" },
        Summary = "Get Assignment Count",
        Description =
            "Retrieve the count of Assignments for a specific Home, optionally filtered by Parent ID, Child ID, or incomplete status.")]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string),
        Summary = "Home ID",
        Description = "The unique identifier of the home")]
    [OpenApiParameter(name: "parentId", In = ParameterLocation.Query, Required = false, Type = typeof(string),
        Summary = "Parent ID filter",
        Description = "Optional parent ID to filter the assignments")]
    [OpenApiParameter(name: "childId", In = ParameterLocation.Query, Required = false, Type = typeof(string),
        Summary = "Child ID filter",
        Description = "Optional child ID to filter the assignments")]
    [OpenApiParameter(name: "showOnlyIncomplete", In = ParameterLocation.Query, Required = false, Type = typeof(string),
        Summary = "Incomplete filter",
        Description = "Optional flag to show only incomplete assignments (e.g., 'true')")]
    [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer,
        BearerFormat = "JWT")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json",
        bodyType: typeof(int),
        Summary = "Assignments count",
        Description = "Returns the count of Assignments matching the filter criteria")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Invalid request")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Unauthorized,
        Description = "Not authorized to perform this action")]
    public async Task<IActionResult> GetAssignmentCount(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "home/{id}/assignments/count")]
        HttpRequest req, string id, [FromQuery] string? parentId, [FromQuery] string? childId,
        [FromQuery] string? showOnlyIncomplete)
    {
        var (err, homeId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        if (!bool.TryParse(showOnlyIncomplete, out var showOnlyIncompleteBool))
        {
            showOnlyIncompleteBool = false;
        }

        // Parent Id Validation
        var (parentIdErr, parentIdGuid) = await parentId.VerifyIdFromHome(homeId, parentService.GetHomeIdByParentId);
        if (parentIdErr != string.Empty) return new BadRequestObjectResult(parentIdErr);

        // Child Id Validation
        var (childIdErr, childIdGuid) = await childId.VerifyIdFromHome(homeId, childService.GetHomeIdByChildId);
        if (childIdErr != string.Empty) return new BadRequestObjectResult(childIdErr);

        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await assignmentService.GetAssignmentsCount(homeId, parentIdGuid, childIdGuid,
                showOnlyIncompleteBool);
            return new OkObjectResult(res);
        });
    }

    [Function("GetAllAssignments")]
    [OpenApiOperation(operationId: "GetAllAssignments", tags: new[] { "Assignment" },
        Summary = "Get All Assignments",
        Description =
            "Retrieve all Assignment records for a specific Home, optionally filtered by Parent ID, Child ID, and incomplete status.")]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string),
        Summary = "Home ID",
        Description = "The unique identifier of the Home")]
    [OpenApiParameter(name: "pageNumber", In = ParameterLocation.Query, Required = false, Type = typeof(int),
        Summary = "Page Number",
        Description = "Optional page number for pagination")]
    [OpenApiParameter(name: "pageSize", In = ParameterLocation.Query, Required = false, Type = typeof(int),
        Summary = "Page Size",
        Description = "Optional page size for pagination")]
    [OpenApiParameter(name: "parentId", In = ParameterLocation.Query, Required = false, Type = typeof(string),
        Summary = "Parent ID Filter",
        Description = "Optional Parent ID to filter the Assignments")]
    [OpenApiParameter(name: "childId", In = ParameterLocation.Query, Required = false, Type = typeof(string),
        Summary = "Child ID Filter",
        Description = "Optional Child ID to filter the Assignments")]
    [OpenApiParameter(name: "showOnlyIncomplete", In = ParameterLocation.Query, Required = false, Type = typeof(string),
        Summary = "Incomplete Filter",
        Description = "Optional flag to return only incomplete Assignments (e.g., 'true')")]
    [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer,
        BearerFormat = "JWT")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json",
        bodyType: typeof(List<Assignment>),
        Summary = "Assignment List Response",
        Description = "Returns the list of Assignments matching the specified filters")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Invalid request")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Unauthorized,
        Description = "Not authorized to perform this action")]
    public async Task<IActionResult> GetAllAssignments(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "home/{id}/assignments")]
        HttpRequest req, string id, [FromQuery] int? pageNumber, [FromQuery] int? pageSize,
        [FromQuery] string? parentId, [FromQuery] string? childId, [FromQuery] string? showOnlyIncomplete)
    {
        var (err, homeId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        if (!bool.TryParse(showOnlyIncomplete, out var showOnlyIncompleteBool))
        {
            showOnlyIncompleteBool = false;
        }

        // Parent Id Validation
        var (parentIdErr, parentIdGuid) = await parentId.VerifyIdFromHome(homeId, parentService.GetHomeIdByParentId);
        if (parentIdErr != string.Empty) return new BadRequestObjectResult(parentIdErr);

        // Child Id Validation
        var (childIdErr, childIdGuid) = await childId.VerifyIdFromHome(homeId, childService.GetHomeIdByChildId);
        if (childIdErr != string.Empty) return new BadRequestObjectResult(childIdErr);

        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await assignmentService.GetAllAssignments(homeId,
                pageNumber ?? Constants.DEFAULT_PAGE_NUMBER,
                pageSize ?? Constants.DEFAULT_PAGE_SIZE, parentIdGuid, childIdGuid
                , showOnlyIncompleteBool);
            return new OkObjectResult(res);
        });
    }

    // Create
    [Function("CreateAssignment")]
    [OpenApiOperation(operationId: "CreateAssignment", tags: new[] { "Assignment" },
        Summary = "Create Assignment",
        Description = "Create a new Assignment record for a specific Home.")]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string),
        Summary = "Home ID",
        Description = "The unique identifier of the Home")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(AssignmentRequest), Required = true,
        Description = "The Assignment object to be created")]
    [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer,
        BearerFormat = "JWT")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json",
        bodyType: typeof(Guid),
        Summary = "Assignment Created",
        Description = "Returns the ID of the created Assignment")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Invalid request")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Unauthorized, Description = "Not authorized to perform this action")]
    public async Task<IActionResult> CreateAssignment(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "home/{id}/assignment")]
        HttpRequest req, string id, [FromBody] AssignmentRequest assignmentRequest)
    {
        var (err, homeId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await assignmentService.CreateAssignment(homeId, assignmentRequest);
            return new OkObjectResult(res);
        });
    }

    // Update
    [Function("EditAssignment")]
    [OpenApiOperation(operationId: "EditAssignment", tags: new[] { "Assignment" },
        Summary = "Edit Assignment",
        Description = "Update an existing Assignment record by its ID.")]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string),
        Summary = "Assignment ID",
        Description = "The unique identifier of the Assignment to update")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(AssignmentRequest), Required = true,
        Description = "The updated Assignment object")]
    [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer,
        BearerFormat = "JWT")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NoContent,
        Summary = "Assignment Updated",
        Description = "The Assignment was successfully updated")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Invalid request")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Description = "Assignment not found")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Unauthorized, Description = "Not authorized to perform this action")]
    public async Task<IActionResult> EditAssignment(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "assignment/{id}")]
        HttpRequest req, string id, [FromBody] AssignmentRequest request)
    {
        var (err, assignmentId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        var homeId = await assignmentService.GetHomeIdByAssignmentId(assignmentId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await assignmentService.EditAssignment(assignmentId, request);
            return new OkObjectResult(res);
        });
    }

    [Function("CompleteAssignment")]
    [OpenApiOperation(operationId: "CompleteAssignment", tags: new[] { "Assignment" },
        Summary = "Complete Assignment",
        Description = "Mark an Assignment as complete by its ID.")]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string),
        Summary = "Assignment ID",
        Description = "The unique identifier of the Assignment to complete")]
    [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer,
        BearerFormat = "JWT")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NoContent,
        Summary = "Assignment Completed",
        Description = "The Assignment was successfully marked as complete")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Invalid request")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Description = "Assignment not found")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Unauthorized, Description = "Not authorized to perform this action")]
    public async Task<IActionResult> CompleteAssignment(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "assignment/{id}/complete")]
        HttpRequest req, string id)
    {
        var (err, assignmentId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        var homeId = await assignmentService.GetHomeIdByAssignmentId(assignmentId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await assignmentService.EditAssignmentCompleteStatus(assignmentId, true);
            return new OkObjectResult(res);
        });
    }

    [Function("UnCompleteAssignment")]
    [OpenApiOperation(operationId: "UnCompleteAssignment", tags: new[] { "Assignment" },
        Summary = "Mark Assignment as Incomplete",
        Description = "Mark an Assignment as incomplete by its ID.")]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string),
        Summary = "Assignment ID",
        Description = "The unique identifier of the Assignment to mark as incomplete")]
    [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer,
        BearerFormat = "JWT")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NoContent,
        Summary = "Assignment Marked Incomplete",
        Description = "The Assignment was successfully marked as incomplete")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Invalid request")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Description = "Assignment not found")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Unauthorized, Description = "Not authorized to perform this action")]
    public async Task<IActionResult> UnCompleteAssignment(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "assignment/{id}/incomplete")]
        HttpRequest req, string id)
    {
        var (err, assignmentId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        var homeId = await assignmentService.GetHomeIdByAssignmentId(assignmentId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await assignmentService.EditAssignmentCompleteStatus(assignmentId, false);
            return new OkObjectResult(res);
        });
    }

    // Delete
    [Function("DeleteAssignment")]
    [OpenApiOperation(operationId: "DeleteAssignment", tags: new[] { "Assignment" },
        Summary = "Delete Assignment",
        Description = "Delete an Assignment by its ID.")]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string),
        Summary = "Assignment ID",
        Description = "The unique identifier of the Assignment to delete")]
    [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer,
        BearerFormat = "JWT")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NoContent,
        Summary = "Assignment Deleted",
        Description = "The Assignment was successfully deleted")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Invalid request")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Description = "Assignment not found")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Unauthorized, Description = "Not authorized to perform this action")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Conflict,
        Description = "Assignment record could not be deleted due to related records not deleted")]
    public async Task<IActionResult> DeleteAssignment(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "assignment/{id}")]
        HttpRequest req, string id)
    {
        var (err, assignmentId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        var homeId = await assignmentService.GetHomeIdByAssignmentId(assignmentId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            await assignmentService.DeleteAssignment(assignmentId);
            return new NoContentResult();
        });
    }

    #endregion

    #region Steps

    // Create
    [Function("CreateStep")]
    [OpenApiOperation(operationId: "CreateStep", tags: new[] { "Step" , "Assignment" },
        Summary = "Create Step",
        Description = "Create a new Step for a specific Assignment.")]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string),
        Summary = "Assignment ID",
        Description = "The unique identifier of the Assignment to attach the Step to")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(StepRequest), Required = true,
        Description = "The Step object to be created")]
    [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer,
        BearerFormat = "JWT")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json",
        bodyType: typeof(Guid),
        Summary = "Step Created",
        Description = "Returns the ID of the created Step")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Invalid request")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Unauthorized, Description = "Not authorized to perform this action")]
    public async Task<IActionResult> CreateStep(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "assignment/{id}/step")]
        HttpRequest req, string id, [FromBody] StepRequest stepRequest)
    {
        var (err, assignmentId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        var homeId = await assignmentService.GetHomeIdByAssignmentId(assignmentId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await assignmentService.CreateStepToAssignment(assignmentId, stepRequest);
            return new OkObjectResult(res);
        });
    }

    // Update
    [Function("EditStep")]
    [OpenApiOperation(operationId: "EditStep", tags: new[] { "Step", "Assignment" },
        Summary = "Edit Step",
        Description = "Update an existing Step associated with an Assignment.")]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string),
        Summary = "Step ID",
        Description = "The unique identifier of the Step to update")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(StepRequest), Required = true,
        Description = "The updated Step object")]
    [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer,
        BearerFormat = "JWT")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NoContent,
        Summary = "Step Updated",
        Description = "The Step was successfully updated")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Invalid request")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Description = "Step not found")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Unauthorized, Description = "Not authorized to perform this action")]
    public async Task<IActionResult> EditStep(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "step/{id}")]
        HttpRequest req, string id, [FromBody] StepRequest request)
    {
        var (err, stepId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        var homeId = await assignmentService.GetHomeIdByStepId(stepId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await assignmentService.EditStep(stepId, request);
            return new OkObjectResult(res);
        });
    }

    [Function("CompleteStep")]
    [OpenApiOperation(operationId: "CompleteStep", tags: new[] { "Step", "Assignment" },
        Summary = "Complete Step",
        Description = "Mark a Step as complete by its ID.")]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string),
        Summary = "Step ID",
        Description = "The unique identifier of the Step to complete")]
    [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer,
        BearerFormat = "JWT")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NoContent,
        Summary = "Step Completed",
        Description = "The Step was successfully marked as complete")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Invalid request")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Description = "Step not found")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Unauthorized, Description = "Not authorized to perform this action")]
    public async Task<IActionResult> CompleteStep(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "step/{id}/complete")]
        HttpRequest req, string id)
    {
        var (err, stepId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        var homeId = await assignmentService.GetHomeIdByStepId(stepId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await assignmentService.EditStepCompleteStatus(stepId, true);
            return new OkObjectResult(res);
        });
    }

    [Function("UnCompleteStep")]
    [OpenApiOperation(operationId: "UnCompleteStep", tags: new[] { "Step", "Assignment" },
        Summary = "Mark Step as Incomplete",
        Description = "Mark a Step as incomplete by its ID.")]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string),
        Summary = "Step ID",
        Description = "The unique identifier of the Step to mark as incomplete")]
    [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer,
        BearerFormat = "JWT")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NoContent,
        Summary = "Step Marked Incomplete",
        Description = "The Step was successfully marked as incomplete")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Invalid request")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Description = "Step not found")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Unauthorized, Description = "Not authorized to perform this action")]
    public async Task<IActionResult> UnCompleteStep(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "step/{id}/incomplete")]
        HttpRequest req, string id)
    {
        var (err, stepId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        var homeId = await assignmentService.GetHomeIdByStepId(stepId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await assignmentService.EditStepCompleteStatus(stepId, false);
            return new OkObjectResult(res);
        });
    }

    // Delete
    [Function("DeleteStep")]
    [OpenApiOperation(operationId: "DeleteStep", tags: new[] { "Step", "Assignment" },
        Summary = "Delete Step",
        Description = "Delete a Step by its ID.")]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string),
        Summary = "Step ID",
        Description = "The unique identifier of the Step to delete")]
    [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer,
        BearerFormat = "JWT")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NoContent,
        Summary = "Step Deleted",
        Description = "The Step was successfully deleted")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Invalid request")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Description = "Step not found")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Unauthorized, Description = "Not authorized to perform this action")]
    public async Task<IActionResult> DeleteStep(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "step/{id}")]
        HttpRequest req, string id)
    {
        var (err, stepId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        var homeId = await assignmentService.GetHomeIdByStepId(stepId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            await assignmentService.DeleteStep(stepId);
            return new NoContentResult();
        });
    }

    #endregion
}