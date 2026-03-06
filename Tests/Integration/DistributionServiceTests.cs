using Moq;
using MSWMS.Entities.Distributions;
using MSWMS.Models.DTO.Requests;
using MSWMS.Repositories.Interfaces;
using MSWMS.Services;

namespace Tests.Integration;

public class DistributionServiceTests
{
    private readonly DistributionService _service;
    private readonly Mock<IDistributionRepository> _distributionRepository;
    private readonly Mock<ILocationRepository> _locationRepository;
    
    public DistributionServiceTests()
    {
        _distributionRepository = new Mock<IDistributionRepository>();
        _locationRepository = new Mock<ILocationRepository>();
        _service = new DistributionService(_distributionRepository.Object, _locationRepository.Object);
    }
    [Fact]
    public async Task CreateAsync_Should_Create_Entity_And_Save_It()
    {

        var request = new CreateDistributionRequest
        {
            Date = DateOnly.Parse("2026-01-01"), 
            Note = "Test"
        };
        
        Distribution entity = await _service.CreateDistributionAsync(request);
        
        Assert.NotNull(entity);
        
        Assert.IsType<Distribution>(entity);
        
        Assert.Equal(request.Date, entity.Date);
        Assert.Equal(request.Note, entity.Note);
        
        _distributionRepository.Verify(
            x => x.AddAsync(
                It.Is<Distribution>(d =>
                    d.Date == request.Date &&
                    d.Note == request.Note),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetDistributionByIdAsync_Should_Return_Distribution()
    {
        var result = await _service.GetDistributionByIdAsync(1);
        
        Assert.NotNull(result);
        Assert.IsType<Distribution>(result);
    }
    
    [Fact]
    public async Task GetDistributionByIdAsync_Should_Return_Null_If_Not_Found()
    {
        var result = await _service.GetDistributionByIdAsync(0);
        
        Assert.Null(result);
    }
    
    [Fact]
    public async Task DeleteDistribution_Should_Delete_Distribution()
    {
        await _service.DeleteDistributionAsync(1);
        
        var result = await _service.GetDistributionByIdAsync(1);
        
        Assert.Null(result);
    }
    
    [Fact]
    public async Task UpdateNote_Should_Update_Note()
    {
        var distribution = await _service.GetByIdAsync(1);
        
        Assert.NotNull(distribution);
        
        await _service.UpdateNoteAsync(distribution.Id, "Updated Note");
        
        Assert.NotEqual("Test", distribution.Note);
        Assert.Equal("Updated Note", distribution.Note);
    }

    [Fact]
    public async Task UpdateNote_Should_Return_Null_If_Distribution_Not_Found()
    {
        var result = await _service.UpdateNoteAsync(0, "Updated Note");
        
        Assert.Null(result);
    }
    
    [Fact]
    public async Task AddDocuments_Should_Add_And_Assign_Documents_To_Distribution()
    {
        var documentsNumber = new List<string>(["20WPAE087891", "20WPAE087892", "20WPAE087893"]);
        
        await _service.AddDocuments(1,  documentsNumber);
        
        var distribution = await _service.GetByIdAsync(1);
        
        Assert.NotNull(distribution);
        
        Assert.NotEmpty(distribution.Documents);
    }

    [Fact]
    public async Task GetDocumentsAsync_Should_Return_All_Distribution_Documents()
    {
        throw new NotImplementedException();
    }
    
    [Fact]
    public async Task GetDocumentsAsync_Should_Return_Empty_List_If_No_Documents()
    {
        throw new NotImplementedException();
    }
    
    [Fact]
    public async Task GetDocumentByIdAsync_Should_Return_Document()
    {
        throw new NotImplementedException();
    }
    
    [Fact]
    public async Task GetDocumentByIdAsync_Should_Return_Null_If_Document_Not_Found()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public async Task GetDocumentByIdAsync_Should_Return_Null_If_Distribution_Not_Found()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public async Task GetDistributionItemsAsync_Should_Return_All_Distribution_Items()
    {
        throw new NotImplementedException();
    }
    
    [Fact]
    public async Task GetScansAsync_Should_Return_All_Distribution_Scans()
    {
        throw new NotImplementedException();
    }
    
    [Fact]
    public async Task GetScansAsync_Should_Return_Empty_List_If_No_Scans()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public async Task GetScanByIdAsync_Should_Return_Scan()
    {
        throw new NotImplementedException();
    }
    
    
    
    [Fact]
    public async Task ProceedScan_Should_Return_ScanDto_With_Proceed_Status()
    {
        throw new NotImplementedException();
    }
    
    [Fact]
    public async Task DeleteScan_Should_Delete_Scan()
    {
        throw new NotImplementedException();
    }

}