using System.ServiceModel;
using MSWMS.Infrastructure.Helpers;
using MSWMS.Models.DTO.Soap.Responses;
using MSWMS.NAVService;

namespace MSWMS.Services.Soap;

public class DcxSoapService 
{
    private NAVServiceClient CreateClient()
    {
        var binding = new BasicHttpBinding();
        
        binding.MaxReceivedMessageSize = 20000000;
        binding.ReaderQuotas.MaxStringContentLength = 20000000;
        
        var client = new NAVServiceClient(binding, new EndpointAddress("http://192.168.51.10:92/NAVService.svc"));
        
        return client;
    }

    public async Task<List<DirectedPickGetHeadersResult>> GetDirectedPickHeaders(string locationCode)
    {
        var client = CreateClient();
        
        try
        {
            var result = await client.DirectedPickGetHeadersAsync(locationCode);
            
            var headers = JsonParser.Parse<List<DirectedPickGetHeadersResult>>(result);
            
            return headers;
        }
        finally
        {
            await CloseClientAsync(client);
        }
    }
    
    private static async Task CloseClientAsync(NAVServiceClient client)
    {
        try
        {
            if (client.State != CommunicationState.Faulted)
            {
                await client.CloseAsync();
            }
            else
            {
                client.Abort();
            }
        }
        catch
        {
            client.Abort();
        }
    }
}