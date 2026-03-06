using MSWMS.Models.DTO.Soap.Responses;

namespace MSWMS.Services.Interfaces;

public interface IDcxSoapService
{
    Task<List<DirectedPickGetHeadersResult>> GetDirectedPickHeaders(string locationCode);
}