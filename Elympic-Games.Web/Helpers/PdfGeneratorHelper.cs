using Elympic_Games.Web.Data.Entities;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Elympic_Games.Web.Helpers
{
    public class PdfGeneratorHelper : IPdfGeneratorHelper
    {
        private readonly IWebHostEnvironment _env;
        private readonly IQrCodeHelper _qrCodeHelper;
        private readonly IEncryptHelper _encryptHelper;

        public PdfGeneratorHelper(
            IWebHostEnvironment env,
            IQrCodeHelper qrCodeHelper,
            IEncryptHelper encryptHelper)
        {
            _env = env;
            _qrCodeHelper = qrCodeHelper;
            _encryptHelper = encryptHelper;
        }

        public async Task<byte[]> FillPdflMultipleTickets(List<TicketOrderDetail> tickets)
        {
            string templatePath = Path.Combine(_env.WebRootPath, "pdfs", "Elympic_Tickets_Template.pdf");

            MemoryStream finalStream = new MemoryStream();
            Document finalDoc = new Document();
            PdfCopy finalWriter = new PdfCopy(finalDoc, finalStream)
            {
                CloseStream = false
            };

            try
            {
                finalDoc.Open();

                foreach (var ticket in tickets)
                {
                    MemoryStream ms = new MemoryStream();
                    PdfReader reader = new PdfReader(templatePath);
                    PdfStamper stamper = new PdfStamper(reader, ms);

                    try
                    {
                        var form = stamper.AcroFields;
                        form.SetField("tb_event", ticket.Ticket.Event.Name);
                        form.SetField("tb_game", ticket.Ticket.Event.GameType.Name);
                        form.SetField("tb_type", ticket.Ticket.TicketType);

                        string imagePath = Path.Combine(_env.WebRootPath, "images", "ticketpng.png");
                        Image staticImage = Image.GetInstance(imagePath);

                        var positions = form.GetFieldPositions("gameImage_af_image");
                        if (positions != null && positions.Count > 0)
                        {
                            var pos = positions[0].position;
                            int page = positions[0].page;

                            var pb = new PushbuttonField(stamper.Writer, pos, "gameImage_af_image")
                            {
                                Layout = PushbuttonField.LAYOUT_ICON_ONLY,
                                Image = staticImage,
                                ScaleIcon = PushbuttonField.SCALE_ICON_ALWAYS
                            };

                            stamper.AddAnnotation(pb.Field, page);
                        }

                        byte[] qrBytes = await _qrCodeHelper.GenerateQrAsync(_encryptHelper.EncryptString(ticket.Id.ToString()));
                        Image qrImage = Image.GetInstance(qrBytes);

                        positions = form.GetFieldPositions("qrcodeImage_af_image");
                        if (positions != null && positions.Count > 0)
                        {
                            var pos = positions[0].position;
                            int page = positions[0].page;

                            var pb = new PushbuttonField(stamper.Writer, pos, "qrcodeImage_af_image")
                            {
                                Layout = PushbuttonField.LAYOUT_ICON_ONLY,
                                Image = qrImage,
                                ScaleIcon = PushbuttonField.SCALE_ICON_ALWAYS
                            };

                            stamper.AddAnnotation(pb.Field, page);
                        }

                        stamper.FormFlattening = true;
                    }
                    finally
                    {
                        stamper.Close();
                        reader.Close();
                    }

                    PdfReader ticketReader = new PdfReader(ms.ToArray());
                    try
                    {
                        for (int i = 1; i <= ticketReader.NumberOfPages; i++)
                        {
                            finalWriter.AddPage(finalWriter.GetImportedPage(ticketReader, i));
                        }
                    }
                    finally
                    {
                        ticketReader.Close();
                        ms.Dispose();
                    }
                }
            }
            finally
            {
                finalDoc.Close();
                finalWriter.Close();
            }

            finalStream.Position = 0;
            return finalStream.ToArray();
        }
    }
}
