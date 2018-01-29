﻿namespace Chinook.Data.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Chinook.Domain.Entities;
    using Chinook.Domain.Repositories;

    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The invoice repository.
    /// </summary>
    public class InvoiceRepository : IInvoiceRepository
    {
        /// <summary>
        /// The _context.
        /// </summary>
        private readonly ChinookContext _context;

        private readonly IInvoiceLineRepository _invoiceLineRepo;
        private readonly ICustomerRepository _customerRepo;

        public InvoiceRepository(ChinookContext context, IInvoiceLineRepository invoiceLineRepo,
            ICustomerRepository customerRepo)
        {
            _context = context;
            _invoiceLineRepo = invoiceLineRepo;
            _customerRepo = customerRepo;
        }

        private async Task<bool> InvoiceExists(int id, CancellationToken ct = default(CancellationToken))
        {
            return await GetByIdAsync(id, ct) != null;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<List<Invoice>> GetAllAsync(CancellationToken ct = default(CancellationToken))
        {
            IList<Invoice> list = new List<Invoice>();
            var invoices = await _context.Invoice.ToListAsync(cancellationToken: ct);
            foreach (var i in invoices)
            {
                var customer = await _customerRepo.GetByIdAsync(i.CustomerId, ct);
                var invoiceLines = await _invoiceLineRepo.GetByInvoiceIdAsync(i.InvoiceId, ct);
                var invoice = new Invoice
                {
                    InvoiceId = i.InvoiceId,
                    CustomerId = i.CustomerId,
                    CustomerName = customer.LastName + ", " + customer.FirstName,
                    InvoiceDate = i.InvoiceDate,
                    BillingAddress = i.BillingAddress,
                    BillingCity = i.BillingCity,
                    BillingState = i.BillingState,
                    BillingCountry = i.BillingCountry,
                    BillingPostalCode = i.BillingPostalCode,
                    Total = i.Total,
                    InvoiceLines = invoiceLines,
                    Customer = customer
                };
                list.Add(invoice);
            }

            return list.ToList();
        }

        public async Task<Invoice> GetByIdAsync(int id, CancellationToken ct = default(CancellationToken))
        {
            var old = await _context.Invoice.FindAsync(id);
            var customer = await _customerRepo.GetByIdAsync(old.CustomerId, ct);
            var invoiceLines = await _invoiceLineRepo.GetByInvoiceIdAsync(old.InvoiceId, ct);
            var invoice = new Invoice
            {
                InvoiceId = old.InvoiceId,
                CustomerId = old.CustomerId,
                CustomerName = customer.LastName + ", " + customer.FirstName,
                InvoiceDate = old.InvoiceDate,
                BillingAddress = old.BillingAddress,
                BillingCity = old.BillingCity,
                BillingState = old.BillingState,
                BillingCountry = old.BillingCountry,
                BillingPostalCode = old.BillingPostalCode,
                Total = old.Total,
                InvoiceLines = invoiceLines,
                Customer = customer
            };
            return invoice;
        }

        public async Task<Invoice> AddAsync(Invoice newInvoice, CancellationToken ct = default(CancellationToken))
        {
            var invoice = new DataModels.Invoice
            {
                CustomerId = newInvoice.CustomerId,
                InvoiceDate = newInvoice.InvoiceDate,
                BillingAddress = newInvoice.BillingAddress,
                BillingCity = newInvoice.BillingCity,
                BillingState = newInvoice.BillingState,
                BillingCountry = newInvoice.BillingCountry,
                BillingPostalCode = newInvoice.BillingPostalCode,
                Total = newInvoice.Total
            };


            _context.Invoice.Add(invoice);
            await _context.SaveChangesAsync(ct);
            newInvoice.InvoiceId = invoice.InvoiceId;
            return newInvoice;
        }

        public async Task<bool> UpdateAsync(Invoice invoice, CancellationToken ct = default(CancellationToken))
        {
            if (!await InvoiceExists(invoice.InvoiceId, ct))
                return false;
            var changing = await _context.Invoice.FindAsync(invoice.InvoiceId);
            _context.Invoice.Update(changing);

            changing.InvoiceId = invoice.InvoiceId;
            changing.CustomerId = invoice.CustomerId;
            changing.InvoiceDate = invoice.InvoiceDate;
            changing.BillingAddress = invoice.BillingAddress;
            changing.BillingCity = invoice.BillingCity;
            changing.BillingState = invoice.BillingState;
            changing.BillingCountry = invoice.BillingCountry;
            changing.BillingPostalCode = invoice.BillingPostalCode;
            changing.Total = invoice.Total;

            await _context.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default(CancellationToken))
        {
            if (!await InvoiceExists(id, ct))
                return false;
            var toRemove = _context.Invoice.Find(id);
            _context.Invoice.Remove(toRemove);
            await _context.SaveChangesAsync(ct);
            return true;
        }

        public async Task<List<Invoice>> GetByCustomerIdAsync(int id, CancellationToken ct = default(CancellationToken))
        {
            IList<Invoice> list = new List<Invoice>();
            var current = await _context.Invoice.Where(a => a.InvoiceId == id).ToListAsync(cancellationToken: ct);
            foreach (var i in current)
            {
                var customer = await _customerRepo.GetByIdAsync(i.CustomerId, ct);
                var invoiceLines = await _invoiceLineRepo.GetByInvoiceIdAsync(i.InvoiceId, ct);
                var newisd = new Invoice
                {
                    InvoiceId = i.InvoiceId,
                    CustomerId = i.CustomerId,
                    CustomerName = customer.LastName + ", " + customer.FirstName,
                    InvoiceDate = i.InvoiceDate,
                    BillingAddress = i.BillingAddress,
                    BillingCity = i.BillingCity,
                    BillingState = i.BillingState,
                    BillingCountry = i.BillingCountry,
                    BillingPostalCode = i.BillingPostalCode,
                    Total = i.Total,
                    InvoiceLines = invoiceLines,
                    Customer = customer
                };
                list.Add(newisd);
            }
            return list.ToList();
        }
    }
}
