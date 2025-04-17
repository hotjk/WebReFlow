using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using PuppeteerSharp;

namespace WebReFlow;
public static class PuppeteerExtentions
{
    private static readonly string ExtractTableJs;
    private static readonly string ExtractTextJs;
    private static readonly string ExtractCharacterJs;

    static PuppeteerExtentions()
    {
        ExtractTableJs = File.ReadAllText("ExtractTable.js");
        ExtractTextJs = File.ReadAllText("ExtractText.js");
        ExtractCharacterJs = File.ReadAllText("ExtractCharacter.js");
    }
    public async static Task<Table[]> ExtractTable(this IPage page)
    {
        return await page.EvaluateFunctionAsync<Table[]>(ExtractTableJs);
    }

    public async static Task<Character[]> ExtractCharacter(this IPage page)
    {
        return await page.EvaluateFunctionAsync<Character[]>(ExtractCharacterJs);
    }

    public async static Task<Character[]> ExtractText(this IPage page)
    {
        return await page.EvaluateFunctionAsync<Character[]>(ExtractTextJs);
    }
}
