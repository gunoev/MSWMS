using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSWMS.Entities.External;

namespace MSWMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesShipmentHeader : ControllerBase
    {
        private readonly ExternalReadOnlyContext _context;

        public SalesShipmentHeader(ExternalReadOnlyContext context)
        {
            _context = context;
        }

        // GET: api/SalesShipmentHeader
        [HttpGet("page={page}")]
        public async Task<ActionResult<IEnumerable<object>>> GetMikesportCoSALSalesShipmentHeader(int page = 1, string shipToName = "")
        {
            return await _context.MikesportCoSALSalesShipmentHeader
                .AsSplitQuery()
                .Select(ss => new
                {
                    No = ss.No,
                    OrderNo = ss.OrderNo,
                    SellToCustomerNo = ss.SellToCustomerNo,
                    BillToCustomerNo = ss.BillToCustomerNo,
                    BillToName = ss.BillToName,
                    BillToAddress = ss.BillToAddress,
                    ShipToName = ss.ShipToName,
                    ShipToAddress = ss.ShipToAddress,
                    OrderDate = ss.OrderDate,
                    PostingDate = ss.PostingDate,
                    ShipmentDate = ss.ShipmentDate,
                    PostingDescription = ss.PostingDescription,
                    LocationCode = ss.LocationCode,
                    UserId = ss.UserId,
                })
                .Where(ss => ss.OrderDate >= DateTime.Now.AddMonths(-1) && 
                             ss.LocationCode == "W01" &&
                             (string.IsNullOrEmpty(shipToName) || ss.ShipToName.ToLower().Contains(shipToName.ToLower())))
                .OrderByDescending(ss => ss.PostingDate)
                .Skip((page - 1) * 10)
                .Take(10)
                .ToListAsync();

        }

        // GET: api/SalesShipmentHeader/5
        [HttpGet("details/{no}")]
        public async Task<ActionResult<object>> GetMikesportCoSALSalesShipmentHeaderDetails(string no)
        {
            var salesShipmentHeader =
                await _context.MikesportCoSALSalesShipmentHeader
                    .Include(ss => ss.Lines)
                    .Select(ss => new 
                    {
                        No = ss.No,
                        OrderNo = ss.OrderNo,
                        SellToCustomerNo = ss.SellToCustomerNo,
                        BillToCustomerNo = ss.BillToCustomerNo,
                        BillToName = ss.BillToName,
                        BillToAddress = ss.BillToAddress,
                        ShipToName = ss.ShipToName,
                        ShipToAddress = ss.ShipToAddress,
                        OrderDate = ss.OrderDate,
                        PostingDate = ss.PostingDate,
                        ShipmentDate = ss.ShipmentDate,
                        PostingDescription = ss.PostingDescription,
                        LocationCode = ss.LocationCode,
                        UserId = ss.UserId,
                        Lines = ss.Lines
                            .Where(l => 
                                    l.GenProdPostingGroup != "SERVICES" 
                                        && l.Quantity > 0
                                    )
                            .Select(l => new 
                        {
                            No = l.No,
                            VariantCode = l.VariantCode,
                            Quantity = l.Quantity,
                        }).ToList()
                    })
                    .FirstOrDefaultAsync(ss => ss.No == no);
            
            if (salesShipmentHeader == null)
            {
                return NotFound();
            }

            return salesShipmentHeader;
        }

        private bool MikesportCoSALSalesShipmentHeaderExists(string id)
        {
            return _context.MikesportCoSALSalesShipmentHeader.Any(e => e.No == id);
        }
    }
}
