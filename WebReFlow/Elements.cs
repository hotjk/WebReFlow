using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebReFlow;

public record Character(string t, int x, int y, int w, int h);

public record TableCell(int x, int y, int w, int h);

public record Table(int x, int y, int w, int h, TableCell[] ths, TableCell[] tds);
