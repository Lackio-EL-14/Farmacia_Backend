
using Farmacia.Api.Controllers;
using Farmacia.Application.Services;
using Farmacia.Core.DTOs;
using Farmacia.Core.Pagination;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class VentaControllerTests
{
    [Fact]
    public async Task GetVentas_ReturnsPagedApiResponse()
    {
        var mockService = new Mock<IVentaDapperRepository>();
        var filter = new VentaFilterDto { Page = 1, PageSize = 2 };
        var paged = new PagedList<VentaDtos>(new List<VentaDtos> { new VentaDtos { Id = 1, Total = 100, Fecha = DateTime.UtcNow } }, 1, 1, 2);
        mockService.Setup(x => x.GetPagedAsync(It.IsAny<VentaFilterDto>())).ReturnsAsync(paged);

        var controller = new VentaController(mockService.Object);

    }
}
