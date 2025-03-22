using AutoMapper;
using Back.Api.Endpoints.Requests.MediaContent;
using Back.Api.Endpoints.Responses;
using Back.Api.Endpoints.v1.MediaContent;
using Back.Api.Infrastructure.Mappers;
using Back.Api.Infrastructure.Services.Core;
using Back.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Back.Tests;

public class MediaContentControllerTests
{
    private readonly MediaContentController _mediaContentController;
    private readonly Mock<IMediaContentService> _mediaContentService;
    
    public MediaContentControllerTests()
    {
        _mediaContentService = new Mock<IMediaContentService>();
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        
        _mediaContentController = new MediaContentController(_mediaContentService.Object, config.CreateMapper());
    }

    [Fact]
    public async Task CreateMediaContent_Returns_SuccessResult()
    {
        // Arrange
        var request = new CreateMediaContentRequest
        {
            Title = "title",
            Description = "",
            Rating = 4m,
            UserId = 1
        };
        
        // Act
        var result = await _mediaContentController.Create(request);
        
        // Assert
        var createdResult = result as NoContentResult;
        Assert.NotNull(createdResult);
        Assert.Equal(204, createdResult.StatusCode);
    }
    
    [Fact]
    public async Task GetRandomMediaContent_Returns_SuccessResult()
    {
        // Arrange
        _mediaContentService.Setup(x => x.GetRandom(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new MediaContent
            {
                Title = "test",
                Description = "desc",
                UserId = 1,
                Rating = 4m,
                Id = 1
            });
        
        // Act
        var response = await _mediaContentController.GetRandom();
        
        // Assert
        Assert.NotNull(response.Result);
        var result  = ((OkObjectResult)response.Result).Value as MediaContentResponse;
        Assert.NotNull(result);
        Assert.Multiple(() =>
        {
            Assert.Equal("test", result.Title);
            Assert.Equal("desc", result.Description);
            Assert.Equal(4m, result.Rating);
            Assert.Equal(1, result.UserId);
        });
    }
}