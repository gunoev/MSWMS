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
    public class TransferShipmentHeader : ControllerBase
    {
        private readonly ExternalReadOnlyContext _context;

        public TransferShipmentHeader(ExternalReadOnlyContext context)
        {
            _context = context;
        }

        // GET: api/TransferShipmentHeader
        [HttpGet("page={page}")]
        public async Task<ActionResult<IEnumerable<object>>> GetMikesportCoSALTransferShipmentHeader(int page = 1, string transferToName = "")
        {
            var result = await _context.MikesportCoSALTransferShipmentHeader
                .AsSplitQuery()
                .Select(tsh => new
            {
                No = tsh.No,
                TransferFromCode = tsh.TransferFromCode,
                TransferFromName = tsh.TransferFromName,
                TransferToCode = tsh.TransferToCode,
                TransferToName = tsh.TransferToName,
                TransferOrderDate = tsh.TransferOrderDate,
                PostingDate = tsh.PostingDate,
                TransferOrderNo = tsh.TransferOrderNo,
                TransferFromContact = tsh.TransferFromContact,
                TransferToContact = tsh.TransferToContact,
                ExternalDocumentNo = tsh.ExternalDocumentNo
            })
                .Where(tsh => tsh.TransferOrderDate >= DateTime.Now.AddMonths(-1)
                && tsh.TransferFromCode == "W01"
                && (string.IsNullOrEmpty(transferToName) || tsh.TransferToName.Contains(transferToName)))
                .OrderByDescending(tsh => tsh.PostingDate)
                .Skip((page - 1) * 10)
            .Take(10)
            .ToListAsync();
            
            return Ok(result);
        }

        // GET: api/TransferShipmentHeader/25TS112656
        [HttpGet("details/{no}")]
        public async Task<ActionResult<object>> GetMikesportCoSALTransferShipmentHeaderDetails(string no)
        {
            var transferShipment =
                await _context.MikesportCoSALTransferShipmentHeader
                    .Include(tsh => tsh.Lines)
                    .Select(tsh => new 
                    {
                        No = tsh.No,
                        TransferFromCode = tsh.TransferFromCode,
                        TransferFromName = tsh.TransferFromName,
                        TransferToCode = tsh.TransferToCode,
                        TransferToName = tsh.TransferToName,
                        TransferOrderDate = tsh.TransferOrderDate,
                        PostingDate = tsh.PostingDate,
                        TransferOrderNo = tsh.TransferOrderNo,
                        TransferFromContact = tsh.TransferFromContact,
                        TransferToContact = tsh.TransferToContact,
                        ExternalDocumentNo = tsh.ExternalDocumentNo,
                        Lines = tsh.Lines.Select(l => new 
                        {
                            ItemNo = l.ItemNo,
                            VariantCode = l.VariantCode,
                            Quantity = l.Quantity
                        }).ToList()
                    })
                    .FirstOrDefaultAsync(tsh => tsh.No == no);

            if (transferShipment == null)
            {
                return NotFound();
            }

            return transferShipment;
        }

        private bool MikesportCoSALTransferShipmentHeaderExists(string id)
        {
            return _context.MikesportCoSALTransferShipmentHeader.Any(e => e.No == id);
        }
    }
}
