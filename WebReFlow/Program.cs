// See https://aka.ms/new-console-template for more information
using NUglify;
using PuppeteerSharp;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Xml.Linq;
using WebReFlow;


var browserFetcher = new BrowserFetcher() { CacheDir = "../" };
await browserFetcher.DownloadAsync();
await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
await using var page = await browser.NewPageAsync();

var path = "file:///" + Path.Combine(Directory.GetCurrentDirectory(), "test.html");
await page.GoToAsync(path);

var tables = await page.ExtractTable();
var characters = await page.ExtractCharacter();

// 生成新的 HTML 文件
var sb = new StringBuilder();
sb.AppendLine("<!DOCTYPE html>");
sb.AppendLine("<html lang='zh'>");
sb.AppendLine("<head>");
sb.AppendLine("<meta charset='UTF-8'>");
sb.AppendLine("<title>Absolute Layout</title>");
sb.AppendLine("<link href=\"report.css\" rel=\"stylesheet\"/>");
sb.AppendLine("<style>");
sb.AppendLine("body { position: relative; }");
sb.AppendLine(@"
.watermark {
            position: absolute;
            font-size: 20px;
            color: rgba(0, 0, 0, 0.2);
            pointer-events: none;
            transform: rotate(-45deg);
            white-space: nowrap;
        }");
sb.AppendLine(".abs div { position: absolute; white-space: nowrap; }");
sb.AppendLine("div.tde { position: absolute; border:1px solid #000; border-collapse:collapse; background-color:whitesmoke}");
sb.AppendLine("div.the { position: absolute; border:1px solid #000; border-collapse:collapse; background-color:#deecfe}");
sb.AppendLine(@"table.tb { position: absolute; }");
sb.AppendLine("</style>");
sb.AppendLine(@"
 <script>
        function createWatermarks() {
            const watermarkText = '张三 13911223232';
            const watermarkSize = 200; // 水印之间的间距
            const pageWidth = window.innerWidth;
            const pageHeight = document.body.scrollHeight;

            // 计算需要多少行和列的水印
            const rows = Math.ceil(pageHeight / watermarkSize);
            const cols = Math.ceil(pageWidth / watermarkSize);

            for (let i = 0; i < rows; i++) {
                for (let j = 0; j < cols; j++) {
                    const watermark = document.createElement('div');
                    watermark.className = 'watermark';
                    watermark.textContent = watermarkText;
                    watermark.style.top = `${i * watermarkSize}px`;
                    watermark.style.left = `${j * watermarkSize}px`;
                    document.body.appendChild(watermark);
                }
            }
        }

        window.onload = createWatermarks;
        window.onresize = createWatermarks;
    </script>
");


StringBuilder body = new StringBuilder();
foreach (Table table in tables)
{
    //body.Append($@"<table class='tb' style='top:{top}px; left:{left}px; width:{width}px; height:{height}px;'>");
    foreach (TableCell th in table.ths)
    {
        body.Append($@"<div class='the' style='top:{th.y}px; left:{th.x}px; width:{th.w}px; height:{th.h}px;'></div>");
    }
    foreach (TableCell td in table.tds)
    {
        body.Append($@"<div class='tde' style='top:{td.y}px; left:{td.x}px; width:{td.w}px; height:{td.h}px;'></div>");
    }
    //body.Append("</table>");
}

Console.WriteLine(characters.Length);
characters.Shuffle();
// 解析 JSON 并写入 HTML
foreach (Character c in characters)
{
    body.Append($"<div style='left: {c.x}px; top: {c.y}px;'>{c.t}</div>");
}

sb.AppendLine("</head>");
sb.AppendLine("<body class='abs'>");

sb.AppendLine("</body>");
sb.AppendLine("<script>");

var script = (@"
const bodyHtml = `" + body.ToString() + @"`
//if (window.location.protocol !== 'file:') {
    document.body.innerHTML = bodyHtml;
//} 
");

var result = Uglify.Js(script, new NUglify.JavaScript.CodeSettings { });
sb.AppendLine(result.Code);
sb.AppendLine("</script>");
sb.AppendLine("</html>");

// 保存到新 HTML 文件
string outputPath = Path.Combine(Directory.GetCurrentDirectory(), "output.html");
File.WriteAllText(outputPath, sb.ToString(), Encoding.UTF8);

Console.WriteLine($"新 HTML 页面已生成: {outputPath}");