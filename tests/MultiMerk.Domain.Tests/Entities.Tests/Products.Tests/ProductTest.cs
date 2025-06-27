using Xunit;
using Domain.Entities.Products;
namespace MultiMerk.Domain.Tests.Entities.Tests.Products.Tests;

public class ProductTest
{
    [Fact]
    public void SettingTitle_ShouldSetAfterLastDashProperly()
    {
        // Arrange
        var sku = "SKU123";
        var title = "Cool-Product-Red";
        var expectedAfterLastDash = "Red";
        // Act        
        var product = new Product(sku)
        {
            Title = title
        };

        // Assert
        Assert.Equal(title, product.Title);
        Assert.Equal(expectedAfterLastDash, product.AfterLastDash);
    }

    [Fact]
    public void SettingTitle_WithSpace_ShouldSetAfterLastDashProperly()
    {
        // Arrange
        var sku = "SKU123";
        var title = "Cool - Product - Black";
        var expectedAfterLastDash = "Black";
        // Act        
        var product = new Product(sku)
        {
            Title = title
        };

        // Assert
        Assert.Equal(title, product.Title);
        Assert.Equal(expectedAfterLastDash, product.AfterLastDash);
    }

    [Fact]
    public void SettingTitle_WithoutDash_ShouldSetAfterLastDashToEmptyString()
    {
        // Arrange
        var sku = "SKU456";
        var title = "SimpleProduct";
        var expectedAfterLastDash = string.Empty;

        // Act
        var product = new Product(sku)
        {
            Title = title
        };

        // Assert
        Assert.Equal(title, product.Title);
        Assert.Equal(expectedAfterLastDash, product.AfterLastDash);
    }

    [Fact]
    public void ChangingTitleAfterInitialization_ShouldUpdateAfterLastDash()
    {
        // Arrange
        var sku = "SKU789";
        var product = new Product(sku);
        // Act
        product.Title = "Test-Product-Green";
        // Assert
        Assert.Equal("Green", product.AfterLastDash);
    }  

}
