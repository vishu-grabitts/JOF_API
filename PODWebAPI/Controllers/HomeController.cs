using System;
using System.IO;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace PODWebAPI.Controllers 
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/Home")]
    public class HomeController : ApiController
    {
        //[HttpGet] 
        //[Route("Index")]
        //public HttpResponseMessage Index()
        //{
        //    //var path = System.Web.HttpContext.Current.Server.MapPath("~/Content/VoucherReportInvoice.html");

        //    //// Create HTML load options
        //    //HtmlLoadOptions htmloptions = new HtmlLoadOptions();
        //    //// Load HTML file
        //    //Document doc = new Document(path, htmloptions);
        //    //// Convert HTML file to PDF
        //    //doc.Save(System.Web.HttpContext.Current.Server.MapPath("~/Content/"+DateTime.Now.Ticks+"_.html"));


        //    var path = System.Web.HttpContext.Current.Server.MapPath("~/Content/");

        //    var txt = File.ReadAllText(System.IO.Path.Combine(path, "VoucherReportInvoice.html"));

        //    var footerHtml = "<div style='text-align:center;width: 100%;'><div style='font-size:16px;'><b>Thank you for your order</b></div>" +
        //                     "<div style='font-size:14px;'> An opportunity to report damage / shortages was given on receipt of your items if present.			</div> </div>";

        //    UrlToPDF(txt, path, DateTime.Now.Ticks + ".pdf", true, footerHtml);
        //    return null;
        //}

        //public string UrlToPDF(string htmlString, string savePath, string fileName = "attachment.pdf", bool isDisplayFooter = true, string footerHtml = null)
        //{

        //    //read parameters from the webpage
        //    // string htmlString = File.ReadAllText(path);// "TxtBaseUrl.Text";
        //    string pdf_page_size = "A4";
        //    string pdf_orientation = "Portrait";//Landscape

        //    PdfPageSize pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize), pdf_page_size, true);
        //    PdfPageOrientation pdfOrientation = (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation), pdf_orientation, true);

        //    int webPageWidth = 1024;
        //    try
        //    {
        //        webPageWidth = 1024;
        //    }
        //    catch { }

        //    int webPageHeight = 0;
        //    try
        //    {
        //        // webPageHeight = Convert.ToInt32(TxtHeight.Text);
        //    }
        //    catch { }

        //    // instantiate a html to pdf converter object
        //    HtmlToPdf converter = new HtmlToPdf();

        //    // set converter options
        //    converter.Options.PdfPageSize = pageSize;
        //    converter.Options.PdfPageOrientation = pdfOrientation;
        //    converter.Options.WebPageWidth = webPageWidth;
        //    converter.Options.WebPageHeight = webPageHeight;
        //    converter.Options.MinPageLoadTime = 2;
        //    converter.Options.MaxPageLoadTime = 60000;



        //    if (isDisplayFooter)
        //    {

        //        // footer settings
        //        converter.Options.DisplayFooter = true;
        //        converter.Footer.DisplayOnFirstPage = true;
        //        converter.Footer.DisplayOnOddPages = true;
        //        converter.Footer.DisplayOnEvenPages = true;
        //        //converter.Footer.Height = model.FooterHeight;


        //        //if (!string.IsNullOrEmpty(model.FooterUrl))
        //        //{
        //        //    PdfHtmlSection footerHtml = new PdfHtmlSection(new HttpHelper<string>(baseUrl, "application/html").GetHTML(model.FooterUrl), baseUrl);
        //        //    footerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
        //        //    converter.Footer.Add(footerHtml);
        //        //}

        //        if (!string.IsNullOrEmpty(footerHtml))
        //        {
        //            PdfHtmlSection _footer = new PdfHtmlSection(footerHtml, null); //, baseUrl
        //            _footer.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
        //            converter.Footer.Add(_footer);
        //        }
        //    }



        //    // create a new pdf document converting an url
        //    PdfDocument doc = converter.ConvertHtmlString(htmlString);  //baseUrl
        //                                                                //PdfDocument doc = converter.ConvertUrl(baseUrl + "" + url);// converter.ConvertHtmlString(htmlString, Helper.GetDomainName());

        //    // save pdf document
        //    //  doc.Save(System.Web.HttpContext.Current.Response, false, "Sample.pdf");

        //    //   doc.Save(System.Web.HttpContext.Current.Server.MapPath("~"), false, "Sample.pdf");
        //    // var path2 = System.Web.HttpContext.Current.Server.MapPath("~/content");
        //    if (!Directory.Exists(savePath))
        //    {
        //        Directory.CreateDirectory(savePath);
        //    }

        //    string pdfFullPath = Path.Combine(savePath, fileName);
        //    doc.Save(pdfFullPath);

        //    // close pdf document
        //    doc.Close();
        //    return pdfFullPath;
        //}





    }
}
