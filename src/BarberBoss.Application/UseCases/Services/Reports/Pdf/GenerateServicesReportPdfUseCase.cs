using BarberBoss.Application.UseCases.Services.Reports.Pdf.Colors;
using BarberBoss.Application.UseCases.Services.Reports.Pdf.Fonts;
using BarberBoss.Domain.Extensions;
using BarberBoss.Domain.Reports;
using BarberBoss.Domain.Repositories.Services;
using BarberBoss.Domain.Services.LoggedUser;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Fonts;
using System.Globalization;
using System.Reflection;
using Document = MigraDoc.DocumentObjectModel.Document;


namespace BarberBoss.Application.UseCases.Services.Reports.Pdf
{
    public class GenerateServicesReportPdfUseCase : IGenerateServicesReportPdfUseCase
    {

        private const string CURRENCY_SYMBOL = "R$";
        private const int HEIGHT_ROW_SERVICE_TABLE = 25;

        private readonly IServicesReadOnlyRepository _repository;
        private readonly ILoggedUser _loggedUser;

        public GenerateServicesReportPdfUseCase(IServicesReadOnlyRepository repository, ILoggedUser loggedUser)
        {
            _repository = repository;
            _loggedUser = loggedUser;

            GlobalFontSettings.FontResolver = new ServicesReportFontResolver();

        }
        public async Task<byte[]> Execute(DateOnly week)
        {
            var loggedUser = await _loggedUser.Get();

            var services = await _repository.FilterByWeek(loggedUser, week);
            if (services.Count == 0)
            {
                return [];
            }

            var document = CreateDocument(week);
            var page = CreatePage(document);

            CreateHeaderWithProfilePhotoAndName(page);

            var totalServices = services.Sum(services => services.Amount);
            totalServices = Math.Round(totalServices, 2);
            CreateTotalSpentSection(page, week, totalServices);

            foreach (var service in services)
            {
                var table = CreateServiceTable(page);

                var row = table.AddRow();
                row.Height = HEIGHT_ROW_SERVICE_TABLE;

                AddServiceType(row.Cells[0], service.ServiceType.ServiceTypeToString());
                AddHeaderAmount(row.Cells[3]);

                row = table.AddRow();
                row.Height = HEIGHT_ROW_SERVICE_TABLE;

                row.Cells[0].AddParagraph(service.Date.ToString("D"));
                SetStyleBaseForServiceInformation(row.Cells[0]);
                row.Cells[0].Format.LeftIndent = 9;

                row.Cells[1].AddParagraph(service.Date.ToString("t"));
                SetStyleBaseForServiceInformation(row.Cells[1]);

                row.Cells[2].AddParagraph(service.PaymentType.PaymentTypeToString());
                SetStyleBaseForServiceInformation(row.Cells[2]);

                AddAmountForService(row.Cells[3], service.Amount);

                if(string.IsNullOrWhiteSpace(service.Description) == false)
                {
                    var descriptionRow = table.AddRow();
                    descriptionRow.Height = HEIGHT_ROW_SERVICE_TABLE;

                    descriptionRow.Cells[0].AddParagraph(service.Description);
                    descriptionRow.Cells[0].Format.Font = new Font { Name = FontHelper.ROBOTO_REGULAR, Size = 9, Color = ColorsHelper.DARK_GREY };
                    descriptionRow.Cells[0].Shading.Color = ColorsHelper.LIGHT_GREY;
                    descriptionRow.Cells[0].VerticalAlignment = VerticalAlignment.Center;
                    descriptionRow.Cells[0].MergeRight = 2;
                    descriptionRow.Cells[0].Format.LeftIndent = 7;

                    row.Cells[3].MergeDown = 1;
                }

                AddWhiteSpace(table);
            }
            return RenderDocument(document);
        }

        private Document CreateDocument(DateOnly week)
        {

            var document = new Document();
            document.Info.Title = $"{ResourceReportGenerationMessages.GAINS_FOR} {week:Y}";
            document.Info.Author = "João Pedro";

            var style = document.Styles["Normal"];
            style!.Font.Name = FontHelper.ROBOTO_REGULAR;

            return document;
        }

        private Section CreatePage(Document document)
        {
            var section = document.AddSection();
            section.PageSetup = document.DefaultPageSetup.Clone();

            section.PageSetup.PageFormat = PageFormat.A4;

            section.PageSetup.TopMargin = 53;
            section.PageSetup.LeftMargin = 40;
            section.PageSetup.BottomMargin = 40;
            section.PageSetup.RightMargin = 35;

            return section;
        }

        private void CreateHeaderWithProfilePhotoAndName(Section page)
        {
            var table = page.AddTable();
            table.AddColumn();
            table.AddColumn("300");

            var row = table.AddRow();

            var assembly = Assembly.GetExecutingAssembly();
            var directoryName = Path.GetDirectoryName(assembly.Location);
            var pathFile = Path.Combine(directoryName!, "Logo", "foto-linkedin-62x62.png");

            row.Cells[0].AddImage(pathFile);

            row.Cells[1].AddParagraph($"BARBEARIA DO JOÃO");
            row.Cells[1].Format.Font = new Font { Name = FontHelper.BEBASNEUE_REGULAR, Size = 25 };
            row.Cells[1].VerticalAlignment = VerticalAlignment.Center;
        }

        private void CreateTotalSpentSection(Section page, DateOnly week, decimal totalServices)
        {
            var paragraph = page.AddParagraph();
            paragraph.Format.SpaceBefore = "40";
            paragraph.Format.SpaceAfter = "40";

            var dateTime = week.ToDateTime(TimeOnly.MinValue);
            var weekNumber = ISOWeek.GetWeekOfYear(dateTime);
            var year = ISOWeek.GetYear(dateTime);

            var weekInfo =  string.Format(ResourceReportGenerationMessages.WEEK_YEAR, weekNumber, year);

            var title = string.Format(ResourceReportGenerationMessages.TOTAL_SPENT_IN, weekInfo);

            paragraph.AddFormattedText(title, new Font { Name = FontHelper.ROBOTO_MEDIUM, Size = 15 });

            paragraph.AddLineBreak();

            paragraph.AddFormattedText($"{CURRENCY_SYMBOL}{totalServices}",
                new Font { Name = FontHelper.BEBASNEUE_REGULAR, Size = 50 });

        }

        private Table CreateServiceTable(Section page)
        {
            var table = page.AddTable();

            table.AddColumn("195").Format.Alignment = ParagraphAlignment.Left;
            table.AddColumn("80").Format.Alignment = ParagraphAlignment.Center;
            table.AddColumn("120").Format.Alignment = ParagraphAlignment.Center;
            table.AddColumn("120").Format.Alignment = ParagraphAlignment.Right;

            return table;
        }

        private void AddServiceType(Cell cell, string serviceTitle)
        {

            cell.AddParagraph(serviceTitle);
            cell.Format.Font = new Font { Name = FontHelper.BEBASNEUE_REGULAR, Size = 15, Color = ColorsHelper.WHITE };
            cell.Shading.Color = ColorsHelper.NAVY_BLUE;
            cell.VerticalAlignment = VerticalAlignment.Center;
            cell.MergeRight = 2;
            cell.Format.LeftIndent = 7;
        }

        private void AddHeaderAmount(Cell cell)
        {
            cell.AddParagraph(ResourceReportGenerationMessages.AMOUNT);
            cell.Format.Font = new Font { Name = FontHelper.BEBASNEUE_REGULAR, Size = 15, Color = ColorsHelper.WHITE };
            cell.Shading.Color = ColorsHelper.PRUSIA_BLUE;
            cell.VerticalAlignment = VerticalAlignment.Center;
        }

        private void SetStyleBaseForServiceInformation(Cell cell)
        {
            cell.Format.Font = new Font { Name = FontHelper.ROBOTO_REGULAR, Size = 10, Color = ColorsHelper.BLACK };
            cell.Shading.Color = ColorsHelper.MEDIUM_GREY;
            cell.VerticalAlignment = VerticalAlignment.Center;

        }

        private void AddAmountForService(Cell cell, decimal amount)
        {
            cell.AddParagraph($"{CURRENCY_SYMBOL}{amount:F2}");
            cell.Format.Font = new Font { Name = FontHelper.ROBOTO_REGULAR, Size = 10, Color = ColorsHelper.BLACK };
            cell.Shading.Color = ColorsHelper.WHITE;
            cell.VerticalAlignment = VerticalAlignment.Center;
        }

        private void AddWhiteSpace(Table table)
        {
            var row = table.AddRow();
            row.Height = 16;
            row.Borders.Visible = false;
        }

        private byte[] RenderDocument(Document document)
        {
            var renderer = new PdfDocumentRenderer
            {
                Document = document,
            };

            renderer.RenderDocument();

            using var file = new MemoryStream();
            renderer.PdfDocument.Save(file);

            return file.ToArray();
        }
    }
}
