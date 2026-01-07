using FluentAssertions;
using Moq;
using FamilySafety.Application.Common.Interfaces;
using FamilySafety.Application.Queries;
using FamilySafety.Application.QueryHandlers;
using FamilySafety.Domain.Entities;

namespace FamilySafety.UnitTests.QueryHandlers;

public class GetLocationHistoryQueryHandlerTests
{
    private readonly Mock<IRepository<LocationHistory>> _mockLocationRepository;
    private readonly GetLocationHistoryQueryHandler _handler;

    public GetLocationHistoryQueryHandlerTests()
    {
        _mockLocationRepository = new Mock<IRepository<LocationHistory>>();
        _handler = new GetLocationHistoryQueryHandler(_mockLocationRepository.Object);
    }

    [Fact]
    public async Task Handle_WithNoDateFilters_ShouldReturnAllLocationsForUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();
        var query = new GetLocationHistoryQuery { UserId = userId };

        var locationHistories = new List<LocationHistory>
        {
            LocationHistory.Create(userId, 40.7128, -74.0060, batteryLevel: 80),
            LocationHistory.Create(userId, 40.7100, -74.0050, batteryLevel: 75),
            LocationHistory.Create(otherUserId, 34.0522, -118.2437, batteryLevel: 90) // Different user
        };

        _mockLocationRepository
            .Setup(r => r.Query())
            .Returns(locationHistories.AsQueryable());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().HaveCount(2);
        result.Data.Should().OnlyContain(l => l.UserId == userId);
    }

    [Fact]
    public async Task Handle_WithFromDate_ShouldReturnLocationsAfterDate()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var fromDate = DateTime.UtcNow.AddHours(-1);
        var query = new GetLocationHistoryQuery 
        { 
            UserId = userId,
            From = fromDate
        };

        var oldLocation = LocationHistory.Create(userId, 40.7128, -74.0060, batteryLevel: 80);
        await Task.Delay(10); // Ensure different timestamps
        var newLocation = LocationHistory.Create(userId, 40.7100, -74.0050, batteryLevel: 75);

        var locationHistories = new List<LocationHistory> { oldLocation, newLocation };

        _mockLocationRepository
            .Setup(r => r.Query())
            .Returns(locationHistories.AsQueryable());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeEmpty();
        result.Data.Should().OnlyContain(l => l.RecordedAt >= fromDate);
    }

    [Fact]
    public async Task Handle_WithToDate_ShouldReturnLocationsBeforeDate()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var toDate = DateTime.UtcNow.AddHours(1);
        var query = new GetLocationHistoryQuery 
        { 
            UserId = userId,
            To = toDate
        };

        var locationHistories = new List<LocationHistory>
        {
            LocationHistory.Create(userId, 40.7128, -74.0060, batteryLevel: 80),
            LocationHistory.Create(userId, 40.7100, -74.0050, batteryLevel: 75)
        };

        _mockLocationRepository
            .Setup(r => r.Query())
            .Returns(locationHistories.AsQueryable());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeEmpty();
        result.Data.Should().OnlyContain(l => l.RecordedAt <= toDate);
    }

    [Fact]
    public async Task Handle_WithFromAndToDate_ShouldReturnLocationsInRange()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var fromDate = DateTime.UtcNow.AddHours(-2);
        var toDate = DateTime.UtcNow.AddHours(2);
        var query = new GetLocationHistoryQuery 
        { 
            UserId = userId,
            From = fromDate,
            To = toDate
        };

        var locationHistories = new List<LocationHistory>
        {
            LocationHistory.Create(userId, 40.7128, -74.0060, batteryLevel: 80),
            LocationHistory.Create(userId, 40.7100, -74.0050, batteryLevel: 75),
            LocationHistory.Create(userId, 40.7090, -74.0040, batteryLevel: 70)
        };

        _mockLocationRepository
            .Setup(r => r.Query())
            .Returns(locationHistories.AsQueryable());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeEmpty();
        result.Data.Should().OnlyContain(l => l.RecordedAt >= fromDate && l.RecordedAt <= toDate);
    }

    [Fact]
    public async Task Handle_ShouldReturnLocationsOrderedByRecordedAtDescending()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var query = new GetLocationHistoryQuery { UserId = userId };

        var location1 = LocationHistory.Create(userId, 40.7128, -74.0060, batteryLevel: 80);
        await Task.Delay(10);
        var location2 = LocationHistory.Create(userId, 40.7100, -74.0050, batteryLevel: 75);
        await Task.Delay(10);
        var location3 = LocationHistory.Create(userId, 40.7090, -74.0040, batteryLevel: 70);

        var locationHistories = new List<LocationHistory> { location1, location2, location3 };

        _mockLocationRepository
            .Setup(r => r.Query())
            .Returns(locationHistories.AsQueryable());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeEmpty();
        
        var locationList = result.Data.ToList();
        for (int i = 0; i < locationList.Count - 1; i++)
        {
            locationList[i].RecordedAt.Should().BeOnOrAfter(locationList[i + 1].RecordedAt);
        }
    }
}
