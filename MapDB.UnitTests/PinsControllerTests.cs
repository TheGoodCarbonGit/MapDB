namespace MapDB.UnitTests;
using Microsoft.AspNetCore.Mvc;
using MapDB.Api.Controllers;
using MapDB.Api.Repositories;
using MapDB.Api.Entities;
using Moq;
using FluentAssertions;
using MapDB.Api.DTOs;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components.Routing;

public class PinsControllerTests
{
    private readonly Mock<IPinsRepository> repositoryStub = new();
    private readonly Random rand = new();


    [Fact] // used to declare a test method
    // test method naming convention: UnitOfWork_StateUnderTest_ExpectedBehaviour
    public async Task GetPinAsync_WithNonexistentPin_ReturnNotFound()
    {
        // Arrange: sets up inputs/variables/etc. for the test
        repositoryStub.Setup(repo => repo.GetPinAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Pin)null);

        var controller = new PinsController(repositoryStub.Object);

        // Act: executes the test
        var result = await controller.GetPinAsync(Guid.NewGuid());

        // Assert: whatever is found from the execution
        Assert.IsType<NotFoundResult>(result.Result);
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetPinAsync_WithExistingPin_ReturnsExpectedPin()
    {
        // Arrange
        Pin expectedPin = CreateRandomPin();
        repositoryStub.Setup(repo => repo.GetPinAsync(It.IsAny<Guid>()))
            .ReturnsAsync(expectedPin);

        var controller = new PinsController(repositoryStub.Object);

        // Act
        var result = await controller.GetPinAsync(Guid.NewGuid());

        // Assert
        result.Value.Should().BeEquivalentTo(expectedPin);
    }

    [Fact]
    public async Task GetPinsAsync_WithExistingPins_ReturnsAllPins()
    {
        // Arrange
        var expectedPins = new[]{CreateRandomPin(), CreateRandomPin(), CreateRandomPin()};

        repositoryStub.Setup(repo => repo.GetPinsAsync())
            .ReturnsAsync(expectedPins);

        var controller = new PinsController(repositoryStub.Object);
        
        // Act
        var actualPins = await controller.GetPinsAsync();

        // Assert
        actualPins.Should().BeEquivalentTo(expectedPins);        
    }

    [Fact]
    public async Task GetPinsAsync_WithMatchingPins_ReturnsMatchingPins()
    {
        // Arrange
        var allPins = new[] {
            new Pin(){Name="Striped pants"},
            new Pin(){Name="White shirt"},
            new Pin(){Name="Black shirt"}
        };

        var nameToMatch = "Shirt";

        repositoryStub.Setup(repo => repo.GetPinsAsync())
            .ReturnsAsync(allPins);

        var controller = new PinsController(repositoryStub.Object);
        
        // Act
        IEnumerable<PinDTO> foundPins = await controller.GetPinsAsync(nameToMatch);

        // Assert
        foundPins.Should().OnlyContain(
            Pin => Pin.Name == allPins[1].Name || Pin.Name == allPins[2].Name
        );       
    }

    [Fact]
    public async Task CreatePinAsync_WithPinToCreate_ReturnsCreatedPin()
    {
        // Arrange
        var PinToCreate = new CreatePinDTO(Guid.NewGuid().ToString(), // name
            Guid.NewGuid().ToString(), // location
            Guid.NewGuid().ToString(), // category
            Guid.NewGuid().ToString()); // description

        var controller = new PinsController(repositoryStub.Object);

        // Act
        var result = await controller.CreatePinAsync(PinToCreate);

        // Assert
        var createdPin = (result.Result as CreatedAtActionResult).Value as PinDTO;
        PinToCreate.Should().BeEquivalentTo(
            createdPin,
            options => options.ComparingByMembers<PinDTO>().ExcludingMissingMembers() // ignores that there are less values in one compared object
        );
        createdPin.ID.Should().NotBeEmpty();
        createdPin.CreationDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromMilliseconds(1000));
    }

    [Fact]
    public async Task UpdatePinAsync_WithExistingPin_ReturnsNoContent()
    {
        // Arrange
        Pin existingPin = CreateRandomPin();
        repositoryStub.Setup(repo => repo.GetPinAsync(It.IsAny<Guid>()))
            .ReturnsAsync(existingPin);

        var PinID = existingPin.ID;
        var PinToUpdate = new UpdatePinDTO(Guid.NewGuid().ToString(), // name
            Guid.NewGuid().ToString(), // location
            Guid.NewGuid().ToString(), // category
            Guid.NewGuid().ToString() // description
            );

        var controller = new PinsController(repositoryStub.Object);

        // Act
        var result = await controller.UpdatePinAsync(PinID, PinToUpdate);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task DeletePinAsync_WithExistingPin_ReturnsNoContent()
    {
        // Arrange
        Pin existingPin = CreateRandomPin();
        repositoryStub.Setup(repo => repo.GetPinAsync(It.IsAny<Guid>()))
            .ReturnsAsync(existingPin);

        var controller = new PinsController(repositoryStub.Object);

        // Act
        var result = await controller.DeletePinAsync(existingPin.ID);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    // Helper method for creating a random Pin
    private Pin CreateRandomPin(){
        return new(){
            ID = Guid.NewGuid(),
            Name = Guid.NewGuid().ToString(),
            Category = Guid.NewGuid().ToString(),
            Location = Guid.NewGuid().ToString(),
            CreationDate = DateTimeOffset.UtcNow
        };
    }
}
