using FluentAssertions;
using FamilySafety.Domain.Entities;
using FamilySafety.Domain.ValueObjects;
using FamilySafety.Domain.Enums;

namespace FamilySafety.UnitTests;

public class UserTests
{
    [Fact]
    public void CreateUser_WithValidData_ShouldCreateUser()
    {
        // Arrange
        var externalId = "ext-123";
        var email = "test@example.com";
        var firstName = "John";
        var lastName = "Doe";

        // Act
        var user = User.Create(externalId, email, firstName, lastName);

        // Assert
        user.Email.Should().Be(email);
        user.FirstName.Should().Be(firstName);
        user.LastName.Should().Be(lastName);
        user.Status.Should().Be(UserStatus.Active);
    }

    [Fact]
    public void User_FullName_ShouldConcatenateFirstAndLastName()
    {
        // Arrange
        var user = User.Create("ext-123", "test@example.com", "John", "Doe");

        // Act
        var fullName = user.FullName;

        // Assert
        fullName.Should().Be("John Doe");
    }
}

public class GeoLocationTests
{
    [Fact]
    public void CreateGeoLocation_WithValidCoordinates_ShouldSucceed()
    {
        // Arrange & Act
        var location = GeoLocation.Create(40.7128, -74.0060);

        // Assert
        location.Latitude.Should().Be(40.7128);
        location.Longitude.Should().Be(-74.0060);
    }

    [Fact]
    public void CalculateDistance_BetweenTwoPoints_ShouldReturnCorrectDistance()
    {
        // Arrange
        var newYork = GeoLocation.Create(40.7128, -74.0060);
        var losAngeles = GeoLocation.Create(34.0522, -118.2437);

        // Act
        var distance = newYork.DistanceTo(losAngeles);

        // Assert - Distance should be approximately 3944 km (3944000 meters)
        distance.Should().BeApproximately(3944000, 50000); // Allow 50km tolerance in meters
    }
}
