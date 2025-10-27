using Azure;
using Elympic_Games.Web.Data.Entities;
using Elympic_Games.Web.Helpers;
using Elympic_Games.Web.Models.TicketOrders;
using Microsoft.EntityFrameworkCore;
using Stripe.Checkout;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Net.Sockets;
using System.Text;

namespace Elympic_Games.Web.Data
{
    public class TicketOrderRepository : GenericRepository<TicketOrder>, ITicketOrderRepository
    {
        private readonly DataContext _context;
        private readonly IPdfGeneratorHelper _pdfGeneratorHelper;
        private readonly IUserHelper _userHelper;
        private readonly IEncryptHelper _encryptHelper;

        public TicketOrderRepository(
            DataContext context,
            IQrCodeHelper qrCodeHelper,
            IPdfGeneratorHelper pdfGeneratorHelper,
            IUserHelper userHelper,
            IEncryptHelper encryptHelper) : base(context)
        {
            _context = context;
            _pdfGeneratorHelper = pdfGeneratorHelper;
            _userHelper = userHelper;
            _encryptHelper = encryptHelper;
        }

        public async Task<IQueryable<TicketOrder>> GetTicketOrderAsync(string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);

            if (user == null)
            {
                return null;
            }
            else
            {
                if (await _userHelper.IsUserInRoleAsync(user, "Admin"))
                {
                    return _context.TicketOrders
                        .Include(o => o.User)
                        .Include(o => o.Items)
                        .ThenInclude(p => p.Ticket)
                        .ThenInclude(t => t.Event)
                        .ThenInclude(e => e.GameType)
                        .OrderByDescending(o => o.OrderDate);
                }


                return _context.TicketOrders
                        .Include(o => o.Items)
                        .ThenInclude(p => p.Ticket)
                        .ThenInclude(t => t.Event)
                        .ThenInclude(e => e.GameType)
                        .Where(o => o.User == user)
                        .OrderByDescending(o => o.OrderDate);
            }
        }

        public async Task AddItemToOrderAsync(AddItemViewModel model, string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);

            if (user == null)
            {
                return;
            }

            var ticket = await _context.Tickets.FindAsync(model.TicketId);

            if (ticket == null)
            {
                return;
            }

            var cart = new Cart
            {
                Price = ticket.Price,
                Ticket = ticket,
                UserId = user.Id
            };

            _context.Carts.Add(cart);

            await _context.SaveChangesAsync();
        }

        public async Task<IQueryable<Cart>> GetCartAsync(string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);

            if (user == null)
            {
                return null;
            }

            return _context.Carts
                        .Include(p => p.Ticket)
                        .ThenInclude(t => t.Event)
                        .ThenInclude(e => e.GameType)
                        .Where(o => o.User == user)
                        .OrderBy(o => o.Ticket.Event.Name)
                        .ThenBy(o => o.Ticket.Event.GameType.Name);
        }

        public async Task<IQueryable<TicketOrderDetail>> GetTicketOrderDetails(int id)
        {
            return _context.TicketOrderDetails
                        .Include(p => p.Ticket)
                        .ThenInclude(t => t.Event)
                        .ThenInclude(e => e.GameType)
                        .Where(tod => tod.TicketOrderId == id)
                        .OrderBy(o => o.Ticket.Event.Name)
                        .ThenBy(o => o.Ticket.Event.GameType.Name);
        }

        public async Task<TicketOrderResult> ConfirmOrderAsync(string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);

            if (user == null)
            {
                return new TicketOrderResult
                {
                    Success = false,
                    Message = "User Not Found"
                };
            }

            var cart = await _context.Carts
                .Include(o => o.Ticket)
                .ThenInclude(t => t.Event)
                .ThenInclude(t => t.GameType)
                .Where(o => o.User == user)
                .ToListAsync();

            if (cart == null || cart.Count == 0)
            {
                return new TicketOrderResult
                {
                    Success = false,
                    Message = "Cart is Empty"
                };
            }

            foreach (var item in cart)
            {
                var eventObj = item.Ticket.Event;

                if (item.Ticket.TicketType == "Normal")
                {
                    if (eventObj.AvailableSeats <= 0)
                    {
                        return new TicketOrderResult
                        {
                            Success = false,
                            FailedEvent = eventObj,
                            Message = $"The Event '{eventObj.Name} + {eventObj.GameType.Name}' dont have more available seats."
                        };
                    }

                }
                else if (item.Ticket.TicketType == "Accessible")
                {
                    if (eventObj.AvailableAccessibleSeats <= 0)
                    {
                        return new TicketOrderResult
                        {
                            Success = false,
                            FailedEvent = eventObj,
                            Message = $"The Event '{eventObj.Name} + {eventObj.GameType.Name}' dont have more accessible seats."
                        };
                    }
                }

                _context.Events.Update(eventObj);
            }

            var details = cart.Select(o => new TicketOrderDetail
            {
                Price = o.Price,
                Ticket = o.Ticket,
            }).ToList();

            var domain = "https://localhost:44387/";

            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + $"TicketOrders/FinishOrder",
                CancelUrl = domain + $"TicketOrders/Cart",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                CustomerEmail = userName
            };

            foreach (var item in details)
            {
                var sessionListItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Price * 100),
                        Currency = "eur",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Ticket " + item.Ticket.Event.Name + " - " + item.Ticket.Event.GameType.Name 
                        }
                    },
                    Quantity = 1
                };

                options.LineItems.Add(sessionListItem);
            }

            var service = new SessionService();
            Session session = service.Create(options);

            return new TicketOrderResult
            {
                Success = true,
                Message = session.Id,
                StripeUrl = session.Url
            };
        }

        public async Task<TicketOrderResult> FinishOrder(string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);

            if (user == null)
            {
                return new TicketOrderResult
                {
                    Success = false,
                    Message = "User Not Found"
                };
            }

            var cart = await _context.Carts
                .Include(o => o.Ticket)
                .ThenInclude(t => t.Event)
                .ThenInclude(t => t.GameType)
                .Where(o => o.User == user)
                .ToListAsync();

            if (cart == null || cart.Count == 0)
            {
                return new TicketOrderResult
                {
                    Success = false,
                    Message = "Cart is Empty"
                };
            }

            foreach (var item in cart)
            {
                var eventObj = item.Ticket.Event;

                if (item.Ticket.TicketType == "Normal")
                {
                    if (eventObj.AvailableSeats <= 0)
                    {
                        return new TicketOrderResult
                        {
                            Success = false,
                            FailedEvent = eventObj,
                            Message = $"The Event '{eventObj.Name} + {eventObj.GameType.Name}' dont have more available seats."
                        };
                    }

                    eventObj.AvailableSeats--;
                }
                else if (item.Ticket.TicketType == "Accessible")
                {
                    if (eventObj.AvailableAccessibleSeats <= 0)
                    {
                        return new TicketOrderResult
                        {
                            Success = false,
                            FailedEvent = eventObj,
                            Message = $"The Event '{eventObj.Name} + {eventObj.GameType.Name}' dont have more accessible seats."
                        };
                    }

                    eventObj.AvailableAccessibleSeats--;
                }

                _context.Events.Update(eventObj);
            }

            var details = cart.Select(o => new TicketOrderDetail
            {
                Price = o.Price,
                Ticket = o.Ticket,
            }).ToList();

            var order = new TicketOrder
            {
                OrderDate = DateTime.UtcNow,
                User = user,
                Items = details
            };

            var pdfBytes = await _pdfGeneratorHelper.FillPdflMultipleTickets(details);

            MailMessage email = new MailMessage();
            email.From = new MailAddress("schoolmanagerpws@gmail.com");
            email.To.Add(user.Email);
            email.Subject = "Order Confirmation";
            email.IsBodyHtml = true;

            var bodyBuilder = new StringBuilder();
            bodyBuilder.AppendLine($"<h1>Your Tickets From Your Order - {DateTime.Now}</h1>");

            foreach (var detail in cart)
            {
                bodyBuilder.AppendLine($@"
                    <h2>Event: {detail.Ticket.Event.Name}</h2>
                    Game: {detail.Ticket.Event.GameType.Name}<br>
                    Start Date: {detail.Ticket.Event.StartDate}<br>
                    End Date: {detail.Ticket.Event.EndDate}<br>
                    Seat Type: {detail.Ticket.TicketType}<br>");
            }

            bodyBuilder.AppendLine("<br><b>Qr Codes to Join the Event are on the Pdf</b><br>" +
                "---------------------------------------------------------<br><h1><b>WARNING: DO NOT LOSE THIS EMAIL</b></h1>");

            var htmlView = AlternateView.CreateAlternateViewFromString(bodyBuilder.ToString(), null, MediaTypeNames.Text.Html);

            email.AlternateViews.Add(htmlView);

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("schoolmanagerpws@gmail.com", "lzqf lrqa jywi agkj"),
                EnableSsl = true
            };
            var attachmentStream = new MemoryStream(pdfBytes);
            var attachment = new Attachment(attachmentStream, "Tickets_Elympic.pdf", "application/pdf");
            email.Attachments.Add(attachment);
            smtp.Send(email);

            await CreateAsync(order);
            _context.Carts.RemoveRange(cart);
            await _context.SaveChangesAsync();
            return new TicketOrderResult
            {
                Success = true,
                Message = "Purchased"
            };
        }

        public async Task<int> CheckTicket(string ticketId, int eventId)
        {
            var ticket = await _context.TicketOrderDetails
                .FirstOrDefaultAsync(t => t.TicketId == Convert.ToInt64(_encryptHelper.DecryptString(ticketId)) && t.Ticket.EventId == eventId);

            if (ticket == null)
            {
                return 0;
            }

            return 1;
        }

        public async Task DeleteTicketFromCartAsync(int id)
        {
            var cart = await _context.Carts.FindAsync(id);

            if (cart == null)
            {
                return;
            }

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
        }
    }
}
