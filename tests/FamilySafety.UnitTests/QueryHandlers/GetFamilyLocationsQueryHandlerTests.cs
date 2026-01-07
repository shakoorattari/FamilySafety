using FluentAssertions;
using Moq;
using FamilySafety.Application.Common.Interfaces;
using FamilySafety.Application.Queries;
using FamilySafety.Application.QueryHandlers;
using FamilySafety.Domain.Entities;
using FamilySafety.Domain.Enums;

namespace FamilySafety.UnitTests.QueryHandlers;

public class GetFamilyLocationsQueryHandlerTests
{
    private readonly Mock<IRepository<FamilyMember>> _mockFamilyMemberRepository;
    private readonly Mock<IRepository<LocationHistory>> _mockLocationRepository;
    private readonly GetFamilyLocationsQueryHandler _handler;

    public GetFamilyLocationsQueryHandlerTests()
    {
        _mockFamilyMemberRepository = new Mock<IRepository<FamilyMember>>();
        _mockLocationRepository = new Mock<IRepository<LocationHistory>>();
        
        // Setup default behavior for ToListAsync
        _mockLocationRepository
            .Setup(r => r.ToListAsync(It.IsAny<IQueryable<LocationHistory>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((IQueryable<LocationHistory> q, CancellationToken ct) => q != null ? q.ToList() : new List<LocationHistory>());
        
        _handler = new GetFamilyLocationsQueryHandler(
            _mockFamilyMemberRepository.Object,
            _mockLocationRepository.Object);
    }

    [Fact]
    public async Task Handle_WhenFamilyHasNoMembers_ShouldReturnEmptyList()
    {
        // Arrange
        var familyGroupId = Guid.NewGuid();
        var query = new GetFamilyLocationsQuery { FamilyGroupId = familyGroupId };

        _mockFamilyMemberRepository
            .Setup(r => r.Query())
            .Returns(new List<FamilyMember>().AsQueryable());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_WhenFamilyHasMembersWithLocationSharing_ShouldReturnLatestLocations()
    {
        // Arrange
        var familyGroupId = Guid.NewGuid();
        var userId1 = Guid.NewGuid();
        var userId2 = Guid.NewGuid();
        var query = new GetFamilyLocationsQuery { FamilyGroupId = familyGroupId };

        var familyMembers = new List<FamilyMember>
        {
            FamilyMember.Create(userId1, familyGroupId, FamilyRole.Admin),
            FamilyMember.Create(userId2, familyGroupId, FamilyRole.Member)
        };

        var locationHistories = new List<LocationHistory>
        {
            LocationHistory.Create(userId1, 40.7128, -74.0060, batteryLevel: 80),
            LocationHistory.Create(userId1, 40.7100, -74.0050, batteryLevel: 75), // Older location
            LocationHistory.Create(userId2, 34.0522, -118.2437, batteryLevel: 90)
        };

        // Delay to ensure different timestamps
        await Task.Delay(10);
        locationHistories[0] = LocationHistory.Create(userId1, 40.7128, -74.0060, batteryLevel: 80);

        _mockFamilyMemberRepository
            .Setup(r => r.Query())
            .Returns(familyMembers.AsQueryable());

        _mockLocationRepository
            .Setup(r => r.Query())
            .Returns(locationHistories.AsQueryable());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().HaveCount(2);
        result.Data.Should().Contain(l => l.UserId == userId1 && l.Latitude == 40.7128);
        result.Data.Should().Contain(l => l.UserId == userId2 && l.Latitude == 34.0522);
    }

    [Fact]
    public async Task Handle_WhenMemberDisabledLocationSharing_ShouldNotIncludeTheirLocation()
    {
        // Arrange
        var familyGroupId = Guid.NewGuid();
        var userId1 = Guid.NewGuid();
        var userId2 = Guid.NewGuid();
        var query = new GetFamilyLocationsQuery { FamilyGroupId = familyGroupId };

        var member1 = FamilyMember.Create(userId1, familyGroupId, FamilyRole.Admin);
        var member2 = FamilyMember.Create(userId2, familyGroupId, FamilyRole.Member);
        member2.ToggleLocationSharing(false); // Disable location sharing

        var familyMembers = new List<FamilyMember> { member1, member2 };

        var locationHistories = new List<LocationHistory>
        {
            LocationHistory.Create(userId1, 40.7128, -74.0060, batteryLevel: 80),
            LocationHistory.Create(userId2, 34.0522, -118.2437, batteryLevel: 90)
        };

        _mockFamilyMemberRepository
            .Setup(r => r.Query())
            .Returns(familyMembers.AsQueryable());

        _mockLocationRepository
            .Setup(r => r.Query())
            .Returns(locationHistories.AsQueryable());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().HaveCount(1);
        result.Data.Should().Contain(l => l.UserId == userId1);
        result.Data.Should().NotContain(l => l.UserId == userId2);
    }
}
